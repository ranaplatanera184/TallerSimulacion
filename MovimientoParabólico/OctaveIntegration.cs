using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MovimientoParabólico
{
    /// <summary>
    /// Puente entre la aplicación en C# y GNU Octave.
    ///
    /// Toda la física del movimiento parabólico y toda la generación de valores
    /// aleatorios (dirección y fuerza del viento) se resuelven en Octave.
    /// Esta clase sólo se encarga de:
    ///   1. localizar el ejecutable de Octave,
    ///   2. invocarlo como proceso externo con los parámetros del usuario,
    ///   3. leer los archivos CSV que el script genera.
    /// </summary>
    public static class OctaveIntegration
    {
        /// <summary>Nombre de la función de Octave (coincide con simular.m).</summary>
        private const string NOMBRE_SCRIPT = "simular.m";

        /// <summary>Carpeta de trabajo del script, relativa al ejecutable de la app.</summary>
        private const string CARPETA_SCRIPT = "Simulacion";

        /// <summary>Archivos que escribe el script.</summary>
        private const string ARCHIVO_RESULTADOS = "resultados.csv";
        private const string ARCHIVO_RESUMEN = "resumen.csv";

        /// <summary>Tiempo máximo de espera de Octave antes de abortar.</summary>
        private const int TIMEOUT_MS = 30000;

        /// <summary>Orden de búsqueda de los ejecutables candidatos.</summary>
        private static readonly string[] EjecutablesCandidatos = new string[]
        {
            "octave-cli.exe",
            "octave-cli-w.exe",
            "octave-cli",
            "octave.exe",
            "octave"
        };

        #region Ubicación de Octave

        /// <summary>
        /// Devuelve la ruta completa de un ejecutable de Octave usable, o null
        /// si no se encontró ninguno.
        ///
        /// Orden de búsqueda:
        ///   1. Ruta configurada manualmente por el usuario en la interfaz.
        ///   2. Variable de entorno OCTAVE_EXECUTABLE (estándar de facto).
        ///   3. Directorios listados en la variable PATH.
        ///   4. Rutas típicas de instalación en Windows.
        /// </summary>
        public static string BuscarOctave(string rutaConfigurada)
        {
            // 1) Ruta escrita por el usuario.
            if (!string.IsNullOrWhiteSpace(rutaConfigurada))
            {
                string ruta = rutaConfigurada.Trim().Trim('"');
                if (File.Exists(ruta))
                {
                    return ruta;
                }
                if (Directory.Exists(ruta))
                {
                    string encontrada = BuscarEnDirectorio(ruta);
                    if (encontrada != null)
                    {
                        return encontrada;
                    }
                }
            }

            // 2) Variable de entorno estándar.
            string rutaEnv = Environment.GetEnvironmentVariable("OCTAVE_EXECUTABLE");
            if (!string.IsNullOrWhiteSpace(rutaEnv) && File.Exists(rutaEnv))
            {
                return rutaEnv;
            }

            // 3) PATH del sistema.
            string path = Environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrEmpty(path))
            {
                foreach (string carpeta in path.Split(Path.PathSeparator))
                {
                    if (string.IsNullOrWhiteSpace(carpeta))
                    {
                        continue;
                    }
                    string encontrada = BuscarEnDirectorio(carpeta.Trim());
                    if (encontrada != null)
                    {
                        return encontrada;
                    }
                }
            }

            // 4) Rutas típicas de instalación de GNU Octave en Windows.
            foreach (string raiz in RutasTipicas())
            {
                if (!Directory.Exists(raiz))
                {
                    continue;
                }

                // El instalador crea una subcarpeta con la versión.
                string[] subcarpetas;
                try
                {
                    subcarpetas = Directory.GetDirectories(raiz, "Octave*");
                }
                catch (IOException)
                {
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }

                foreach (string subcarpeta in subcarpetas)
                {
                    string encontrada = BuscarEnDirectorio(subcarpeta);
                    if (encontrada != null)
                    {
                        return encontrada;
                    }

                    // Algunas instalaciones anidan los binarios en mingw64\bin.
                    string mingw = Path.Combine(subcarpeta, "mingw64", "bin");
                    encontrada = BuscarEnDirectorio(mingw);
                    if (encontrada != null)
                    {
                        return encontrada;
                    }
                }
            }

            return null;
        }

        /// <summary>Devuelve las raíces típicas donde se instala Octave en Windows.</summary>
        private static IEnumerable<string> RutasTipicas()
        {
            yield return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "GNU Octave");
            yield return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "GNU Octave");
            yield return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Programs", "GNU Octave");
        }

        /// <summary>Busca un ejecutable de Octave dentro de una carpeta concreta.</summary>
        private static string BuscarEnDirectorio(string carpeta)
        {
            if (string.IsNullOrWhiteSpace(carpeta) || !Directory.Exists(carpeta))
            {
                return null;
            }

            foreach (string nombre in EjecutablesCandidatos)
            {
                try
                {
                    string ruta = Path.Combine(carpeta, nombre);
                    if (File.Exists(ruta))
                    {
                        return ruta;
                    }
                }
                catch (ArgumentException)
                {
                    // Ruta mal formada: se ignora y se continúa con la siguiente.
                }
            }
            return null;
        }

        #endregion

        #region Invocación de Octave

        /// <summary>
        /// Invoca a Octave con los parámetros del usuario y devuelve las dos
        /// tablas de información más la evaluación del objetivo.
        /// </summary>
        public static ResultadoOctave Ejecutar(ParametrosSimulacion parametros, string rutaConfigurada)
        {
            ResultadoOctave resultado = new ResultadoOctave();

            string directorioScript = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, CARPETA_SCRIPT);

            string script = Path.Combine(directorioScript, NOMBRE_SCRIPT);
            if (!File.Exists(script))
            {
                resultado.MensajeError =
                    "No se encontró el script de Octave:\n" + script +
                    "\n\nVerifique que la carpeta '" + CARPETA_SCRIPT +
                    "' se haya copiado junto al ejecutable.";
                return resultado;
            }

            string ejecutable = BuscarOctave(rutaConfigurada);
            if (ejecutable == null)
            {
                resultado.MensajeError =
                    "No se encontró GNU Octave.\n\n" +
                    "Opciones:\n" +
                    "  1) Instale Octave desde https://octave.org/windows\n" +
                    "  2) Defina la variable de entorno OCTAVE_EXECUTABLE apuntando a octave-cli.exe\n" +
                    "  3) Escriba la ruta completa de octave-cli.exe en el campo indicado.";
                return resultado;
            }

            resultado.RutaEjecutable = ejecutable;

            // Borra los resultados de la corrida anterior para que no se lea
            // un archivo viejo si Octave falla.
            BorrarSiExiste(Path.Combine(directorioScript, ARCHIVO_RESULTADOS));
            BorrarSiExiste(Path.Combine(directorioScript, ARCHIVO_RESUMEN));

            // Se construye la llamada con cultura invariante: si se usara la
            // configuración regional del equipo, la coma decimal rompería la
            // sintaxis de Octave.
            string llamada = string.Format(
                CultureInfo.InvariantCulture,
                "simular({0},{1},{2},{3},{4},{5},{6},{7},{8})",
                parametros.Angulo,
                parametros.Fuerza,
                parametros.DireccionMin,
                parametros.DireccionMax,
                parametros.FuerzaVientoMin,
                parametros.FuerzaVientoMax,
                parametros.DistanciaHoyo,
                parametros.Tolerancia,
                parametros.Semilla);

            // Se normaliza la ruta con barras diagonales y se escapan las
            // comillas simples del lenguaje Octave.
            string directorioOctave = directorioScript
                .Replace('\\', '/')
                .Replace("'", "''");

            string argumentos = string.Format(
                CultureInfo.InvariantCulture,
                "--norc --quiet --no-window-system --eval \"addpath('{0}'); {1}\"",
                directorioOctave,
                llamada);

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = ejecutable;
            info.Arguments = argumentos;
            info.WorkingDirectory = directorioScript;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            try
            {
                using (Process proceso = new Process())
                {
                    proceso.StartInfo = info;
                    proceso.Start();

                    // Se leen las salidas en hilos aparte para evitar que la
                    // tubería se llene y el proceso se bloquee.
                    Task<string> tareaSalida = Task.Run(
                        delegate () { return proceso.StandardOutput.ReadToEnd(); });
                    Task<string> tareaError = Task.Run(
                        delegate () { return proceso.StandardError.ReadToEnd(); });

                    bool terminoATiempo = proceso.WaitForExit(TIMEOUT_MS);

                    if (!terminoATiempo)
                    {
                        try { proceso.Kill(); }
                        catch (InvalidOperationException) { /* ya terminó */ }

                        try { proceso.WaitForExit(3000); }
                        catch (InvalidOperationException) { /* ya terminó */ }

                        resultado.CodigoSalida = -1;
                        resultado.MensajeError =
                            "Octave tardó demasiado en responder (más de " +
                            (TIMEOUT_MS / 1000) + " segundos) y fue interrumpido.";
                        return resultado;
                    }

                    resultado.CodigoSalida = proceso.ExitCode;
                    resultado.SalidaEstandar = tareaSalida.Result;
                    resultado.SalidaError = tareaError.Result;
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                resultado.MensajeError =
                    "No se pudo ejecutar Octave:\n" + ejecutable + "\n\n" + ex.Message;
                return resultado;
            }
            catch (IOException ex)
            {
                resultado.MensajeError = "Error de E/S al invocar a Octave:\n" + ex.Message;
                return resultado;
            }

            if (resultado.CodigoSalida != 0)
            {
                resultado.MensajeError =
                    "Octave terminó con el código de salida " +
                    resultado.CodigoSalida + "." +
                    (string.IsNullOrWhiteSpace(resultado.SalidaError)
                        ? string.Empty
                        : "\n\nDetalle:\n" + resultado.SalidaError.Trim());
                return resultado;
            }

            return LeerResultados(directorioScript, resultado);
        }

        private static void BorrarSiExiste(string ruta)
        {
            try
            {
                if (File.Exists(ruta))
                {
                    File.Delete(ruta);
                }
            }
            catch (IOException) { /* se ignora: se comprobará al leer */ }
            catch (UnauthorizedAccessException) { /* se ignora */ }
        }

        #endregion

        #region Lectura de los archivos generados por Octave

        /// <summary>
        /// Lee resultados.csv (recorrido + viento) y resumen.csv (objetivo).
        /// </summary>
        private static ResultadoOctave LeerResultados(string directorioScript, ResultadoOctave resultado)
        {
            string rutaResultados = Path.Combine(directorioScript, ARCHIVO_RESULTADOS);
            string rutaResumen = Path.Combine(directorioScript, ARCHIVO_RESUMEN);

            if (!File.Exists(rutaResultados) || !File.Exists(rutaResumen))
            {
                resultado.MensajeError =
                    "Octave terminó pero no generó los archivos de resultado." +
                    (string.IsNullOrWhiteSpace(resultado.SalidaError)
                        ? string.Empty
                        : "\n\nDetalle:\n" + resultado.SalidaError.Trim());
                return resultado;
            }

            try
            {
                ResultadoSimulacion datos = new ResultadoSimulacion();
                LeerRecorrido(rutaResultados, datos);
                LeerResumen(rutaResumen, datos);

                if (datos.Recorrido.Count == 0)
                {
                    resultado.MensajeError = "El archivo de recorrido está vacío.";
                    return resultado;
                }

                datos.Recorrido.Sort(delegate (FilaRecorrido a, FilaRecorrido b)
                {
                    return a.Tiempo.CompareTo(b.Tiempo);
                });
                datos.Viento.Sort(delegate (FilaViento a, FilaViento b)
                {
                    return a.Tiempo.CompareTo(b.Tiempo);
                });

                resultado.Datos = datos;
                resultado.Exitoso = true;
                return resultado;
            }
            catch (FormatException ex)
            {
                resultado.MensajeError =
                    "Se encontró un valor no numérico en los archivos de Octave:\n" +
                    ex.Message;
                return resultado;
            }
            catch (IOException ex)
            {
                resultado.MensajeError = "Error al leer los archivos de Octave:\n" + ex.Message;
                return resultado;
            }
        }

        /// <summary>
        /// Lee la tabla de recorrido y la de agentes aleatorios desde un único
        /// archivo: t,x,y,vx,vy,magnitud,dir_viento,fuerza_viento,ax_viento
        /// </summary>
        private static void LeerRecorrido(string ruta, ResultadoSimulacion datos)
        {
            string[] lineas = File.ReadAllLines(ruta);

            for (int i = 0; i < lineas.Length; i++)
            {
                string linea = lineas[i].Trim();
                if (linea.Length == 0)
                {
                    continue;
                }

                string[] columnas = linea.Split(',');
                if (columnas.Length < 9)
                {
                    if (i == 0)
                    {
                        continue; // encabezado
                    }
                    throw new FormatException("Fila " + (i + 1) + " incompleta.");
                }

                if (i == 0 && !EsNumero(columnas[0]))
                {
                    continue; // se omite la fila de encabezado
                }

                double t = Parsear(columnas[0]);
                double x = Parsear(columnas[1]);
                double y = Parsear(columnas[2]);
                double vx = Parsear(columnas[3]);
                double vy = Parsear(columnas[4]);
                double mag = Parsear(columnas[5]);
                double dir = Parsear(columnas[6]);
                double fuerza = Parsear(columnas[7]);
                double ax = Parsear(columnas[8]);

                FilaRecorrido fila = new FilaRecorrido();
                fila.Tiempo = t;
                fila.X = x;
                fila.Y = y;
                fila.Vx = vx;
                fila.Vy = vy;
                fila.Magnitud = mag;
                datos.Recorrido.Add(fila);

                FilaViento viento = new FilaViento();
                viento.Tiempo = t;
                viento.Direccion = dir;
                viento.Fuerza = fuerza;
                viento.Ax = ax;
                datos.Viento.Add(viento);
            }
        }

        /// <summary>
        /// Lee el resumen: alcanzo,distancia_impacto,tolerancia,tiempo_vuelo,x_impacto,y_max,impacto
        /// </summary>
        private static void LeerResumen(string ruta, ResultadoSimulacion datos)
        {
            string[] lineas = File.ReadAllLines(ruta);
            for (int i = 0; i < lineas.Length; i++)
            {
                string linea = lineas[i].Trim();
                if (linea.Length == 0)
                {
                    continue;
                }

                string[] columnas = linea.Split(',');
                if (columnas.Length < 7)
                {
                    if (i == 0)
                    {
                        continue;
                    }
                    throw new FormatException("Fila de resumen incompleta.");
                }

                if (i == 0 && !EsNumero(columnas[0]))
                {
                    continue;
                }

                ResumenSimulacion resumen = new ResumenSimulacion();
                resumen.AlcanzoObjetivo = Parsear(columnas[0]) != 0;
                resumen.DistanciaImpacto = Parsear(columnas[1]);
                resumen.Tolerancia = Parsear(columnas[2]);
                resumen.TiempoVuelo = Parsear(columnas[3]);
                resumen.XImpacto = Parsear(columnas[4]);
                resumen.YMax = Parsear(columnas[5]);
                resumen.Impacto = Parsear(columnas[6]) != 0;
                datos.Resumen = resumen;
                return;
            }
        }

        private static double Parsear(string valor)
        {
            return double.Parse(valor.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        private static bool EsNumero(string valor)
        {
            double ignorado;
            return double.TryParse(
                valor.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out ignorado);
        }

        #endregion
    }
}
