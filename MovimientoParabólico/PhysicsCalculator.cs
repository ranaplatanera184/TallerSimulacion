using System;
using System.Globalization;

namespace MovimientoParabólico
{
    /// <summary>
    /// Clase estática con las utilidades de conversión de unidades y formateo
    /// que utiliza la interfaz.
    ///
    /// IMPORTANTE: la física del movimiento parabólico y la generación aleatoria
    /// del viento NO se calculan aquí. Todo ese cálculo lo realiza GNU Octave
    /// (ver Simulacion/simular.m). Esta clase sólo traduce metros a píxeles
    /// para poder dibujar y da formato a los valores que vienen de Octave.
    /// </summary>
    public static class PhysicsCalculator
    {
        /// <summary>Formato numérico con dos decimales, usado en toda la interfaz.</summary>
        private const string FORMATO_DOS_DECIMALES = "F2";

        #region Conversión de Unidades

        /// <summary>
        /// Convierte metros a píxeles según la escala de dibujo actual.
        /// La escala se recalcula en cada corrida para que la trayectoria
        /// siempre quepa dentro del área de juego.
        /// </summary>
        public static double MetersToPixels(double metros, double escala)
        {
            return metros * escala;
        }

        /// <summary>
        /// Convierte píxeles a metros según la escala de dibujo actual.
        /// </summary>
        public static double PixelsToMeters(double pixeles, double escala)
        {
            if (escala <= 0)
            {
                return 0;
            }
            return pixeles / escala;
        }

        /// <summary>
        /// Calcula la escala de dibujo (píxeles por metro) de forma UNIFORME para
        /// ambos ejes. Usar la misma escala en X e Y es lo que garantiza que la
        /// trayectoria dibujada sea realmente una parábola y no una figura
        /// deformada.
        /// </summary>
        /// <param name="anchoPixeles">Ancho disponible del área de juego.</param>
        /// <param name="altoPixeles">Alto disponible del área de juego.</param>
        /// <param name="rangoXMetros">Longitud horizontal que debe caber (metros).</param>
        /// <param name="rangoYMetros">Altura vertical que debe caber (metros).</param>
        public static double CalcularEscala(int anchoPixeles, int altoPixeles,
            double rangoXMetros, double rangoYMetros)
        {
            if (anchoPixeles <= 0 || altoPixeles <= 0)
            {
                return 1.0;
            }

            double escalaX = rangoXMetros > 0 ? anchoPixeles / rangoXMetros : double.MaxValue;
            double escalaY = rangoYMetros > 0 ? altoPixeles / rangoYMetros : double.MaxValue;

            double escala = Math.Min(escalaX, escalaY);

            if (double.IsNaN(escala) || double.IsInfinity(escala) || escala <= 0)
            {
                escala = 1.0;
            }

            return escala;
        }

        /// <summary>
        /// Posición en píxeles del eje X para una coordenada del mundo en metros.
        /// </summary>
        public static int PosicionPixelesX(double xMetros, double escala, int origenXPixeles)
        {
            return origenXPixeles + (int)Math.Round(MetersToPixels(xMetros, escala));
        }

        /// <summary>
        /// Posición en píxeles del eje Y para una coordenada del mundo en metros.
        /// En pantalla el eje Y crece hacia abajo, por lo que se invierte respecto
        /// de la línea base (el suelo).
        /// </summary>
        public static int PosicionPixelesY(double yMetros, double escala, int baseYPixeles)
        {
            return baseYPixeles - (int)Math.Round(MetersToPixels(yMetros, escala));
        }

        #endregion

        #region Formateo de Valores

        /// <summary>Formatea un número con dos decimales usando punto decimal.</summary>
        public static string Formatear(double valor)
        {
            return valor.ToString(FORMATO_DOS_DECIMALES, CultureInfo.InvariantCulture);
        }

        /// <summary>Formatea un ángulo con el símbolo de grados.</summary>
        public static string FormatearAngulo(double angulo)
        {
            return Formatear(angulo) + "°";
        }

        /// <summary>Formatea una velocidad con sus unidades.</summary>
        public static string FormatearVelocidad(double velocidad)
        {
            return Formatear(velocidad) + " m/s";
        }

        /// <summary>Formatea una aceleración con sus unidades.</summary>
        public static string FormatearAceleracion(double aceleracion)
        {
            return Formatear(aceleracion) + " m/s²";
        }

        /// <summary>Formatea una posición con sus unidades.</summary>
        public static string FormatearPosicion(double posicion)
        {
            return Formatear(posicion) + " m";
        }

        /// <summary>Formatea un tiempo con sus unidades.</summary>
        public static string FormatearTiempo(double tiempo)
        {
            return Formatear(tiempo) + " s";
        }

        #endregion
    }
}
