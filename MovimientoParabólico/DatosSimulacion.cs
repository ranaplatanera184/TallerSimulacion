using System;
using System.Collections.Generic;

namespace MovimientoParabólico
{
    /// <summary>
    /// Estados posibles de la simulación.
    /// </summary>
    public enum EstadoSimulacion
    {
        Esperando,   // Esperando que el usuario cargue los datos y lance
        Calculando,  // Octave está calculando la trayectoria
        Animando,    // Reproduciendo los fotogramas (0.5 s por fotograma)
        Finalizado   // Simulación terminada y objetivo evaluado
    }

    /// <summary>
    /// Parámetros que el usuario carga en la interfaz y que se envían a Octave.
    /// </summary>
    public class ParametrosSimulacion
    {
        /// <summary>Ángulo de salida en grados.</summary>
        public double Angulo { get; set; }

        /// <summary>Velocidad de salida en m/s (la "fuerza" del golpe).</summary>
        public double Fuerza { get; set; }

        /// <summary>Límite inferior del ángulo del viento en grados.</summary>
        public double DireccionMin { get; set; }

        /// <summary>Límite superior del ángulo del viento en grados.</summary>
        public double DireccionMax { get; set; }

        /// <summary>Límite inferior de la fuerza del viento en m/s².</summary>
        public double FuerzaVientoMin { get; set; }

        /// <summary>Límite superior de la fuerza del viento en m/s².</summary>
        public double FuerzaVientoMax { get; set; }

        /// <summary>Distancia de la meta (hoyo) desde la salida, en metros.</summary>
        public double DistanciaHoyo { get; set; }

        /// <summary>Margen de tolerancia para dar por alcanzada la meta, en metros.</summary>
        public double Tolerancia { get; set; }

        /// <summary>Semilla del generador aleatorio (corrida reproducible).</summary>
        public double Semilla { get; set; }
    }

    /// <summary>
    /// Una fila de la tabla de recorrido (un fotograma de 0.5 s).
    /// Todos los valores provienen directamente de Octave.
    /// </summary>
    public class FilaRecorrido
    {
        public double Tiempo { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Vx { get; set; }
        public double Vy { get; set; }
        public double Magnitud { get; set; }
    }

    /// <summary>
    /// Una fila de la tabla de agentes aleatorios: el viento sorteado por Octave
    /// para ese fotograma.
    /// </summary>
    public class FilaViento
    {
        public double Tiempo { get; set; }
        public double Direccion { get; set; }
        public double Fuerza { get; set; }
        public double Ax { get; set; }
    }

    /// <summary>
    /// Resultado global de la corrida, calculado íntegramente por Octave.
    /// </summary>
    public class ResumenSimulacion
    {
        /// <summary>Verdadero si el proyectil cayó dentro de la tolerancia del hoyo.</summary>
        public bool AlcanzoObjetivo { get; set; }

        /// <summary>Distancia entre el punto de impacto y el hoyo, en metros.</summary>
        public double DistanciaImpacto { get; set; }

        /// <summary>Tolerancia usada para la evaluación, en metros.</summary>
        public double Tolerancia { get; set; }

        /// <summary>Tiempo total de vuelo en segundos.</summary>
        public double TiempoVuelo { get; set; }

        /// <summary>Posición horizontal del impacto, en metros.</summary>
        public double XImpacto { get; set; }

        /// <summary>Altura máxima alcanzada, en metros.</summary>
        public double YMax { get; set; }

        /// <summary>Indica si el proyectil llegó a tocar el suelo.</summary>
        public bool Impacto { get; set; }
    }

    /// <summary>
    /// Resultado completo devuelto por la integración con Octave: las dos tablas
    /// de información más el resumen del objetivo.
    /// </summary>
    public class ResultadoSimulacion
    {
        public List<FilaRecorrido> Recorrido { get; private set; }
        public List<FilaViento> Viento { get; private set; }
        public ResumenSimulacion Resumen { get; set; }

        public ResultadoSimulacion()
        {
            Recorrido = new List<FilaRecorrido>();
            Viento = new List<FilaViento>();
            Resumen = new ResumenSimulacion();
        }
    }

    /// <summary>
    /// Envuelve el resultado de invocar a Octave: éxito o error con detalle.
    /// </summary>
    public class ResultadoOctave
    {
        public bool Exitoso { get; set; }
        public string MensajeError { get; set; }
        public string RutaEjecutable { get; set; }
        public int CodigoSalida { get; set; }
        public string SalidaEstandar { get; set; }
        public string SalidaError { get; set; }
        public ResultadoSimulacion Datos { get; set; }

        public ResultadoOctave()
        {
            MensajeError = string.Empty;
            SalidaEstandar = string.Empty;
            SalidaError = string.Empty;
            Datos = new ResultadoSimulacion();
        }
    }
}
