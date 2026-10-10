using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MovimientoParabólico
{
    /// <summary>
    /// Ventana principal del juego de golf.
    ///
    /// Esta clase NO calcula ninguna fórmula física ni ningún valor aleatorio:
    /// sólo carga los parámetros, los pasa a GNU Octave, recibe los dos CSV que
    /// Octave genera y los va mostrando fotograma a fotograma (uno cada 0.5 s).
    /// Lo único que hace con números es convertir metros a píxeles para dibujar.
    /// </summary>
    public partial class ProcessForm : Form
    {
        #region Campos

        /// <summary>
        /// Margen izquierdo del área de dibujo (donde comienza el tiro).
        /// Tiene que dejar libre la derecha del golfista, que ocupa
        /// x = 14 … 70, para que la pelota no despegue encima de él.
        /// </summary>
        private const int MARGEN_IZQUIERDO = 90;

        /// <summary>Margen derecho del área de dibujo.</summary>
        private const int MARGEN_DERECHO = 30;

        /// <summary>Margen superior del área de dibujo (para que la parábola respire).</summary>
        private const int MARGEN_SUPERIOR = 40;

        /// <summary>Margen inferior: franja de suelo bajo la línea base.</summary>
        private const int MARGEN_INFERIOR = 26;

        /// <summary>Resultados devueltos por Octave.</summary>
        private ResultadoSimulacion datos;

        /// <summary>Estado actual de la simulación.</summary>
        private EstadoSimulacion estado;

        /// <summary>Índice del fotograma que se está mostrando en pantalla.</summary>
        private int indiceFotograma;

        /// <summary>Escala de dibujo actual (píxeles por metro).</summary>
        private double escala;

        /// <summary>Coordenada en píxeles del origen del tiro (x = 0 m).</summary>
        private int origenXPixeles;

        /// <summary>Coordenada en píxeles de la línea base del suelo (y = 0 m).</summary>
        private int baseYPixeles;

        /// <summary>Distancia al hoyo y tolerancia vigentes (metros).</summary>
        private double distanciaHoyoMetros;
        private double toleranciaMetros;

        #endregion

        #region Constructores

        public ProcessForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Carga y cierre

        private void ProcessForm_Load(object sender, EventArgs e)
        {
            // NO se puede fijar en el diseñador: MinimumSize infla la ventana
            // cuando se crea el handle y, si eso ocurre antes de que se
            // calculen los márgenes de anclaje, tabControl1 queda con unos
            // márgenes distintos a los del diseñador y las dos columnas se
            // solapan al encoger. Se fija aquí, cuando el layout ya está tomado.
            this.MinimumSize = new Size(1310, 720);

            AplicarEstiloTabla(dgvRecorrido);
            AplicarEstiloTabla(dgvViento);

            distanciaHoyoMetros = Decimal.ToDouble(numDistancia.Value);
            toleranciaMetros = Decimal.ToDouble(numTolerancia.Value);

            CambiarEstado(EstadoSimulacion.Esperando, "Cargue los datos y pulse Lanzar");
            PrepararEscena();
        }

        private void ProcessForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerAnimacion.Stop();
        }

        #endregion

        #region Botones

        /// <summary>
        /// Lee los campos, invoca a Octave y, si todo salió bien, arranca la
        /// animación de los fotogramas.
        /// </summary>
        private void btnLanzar_Click(object sender, EventArgs e)
        {
            if (estado == EstadoSimulacion.Calculando ||
                estado == EstadoSimulacion.Animando)
            {
                return;
            }

            ParametrosSimulacion parametros = LeerParametros();
            if (parametros == null)
            {
                return;
            }

            distanciaHoyoMetros = parametros.DistanciaHoyo;
            toleranciaMetros = parametros.Tolerancia;

            CambiarEstado(EstadoSimulacion.Calculando,
                "Calculando la trayectoria en GNU Octave...");
            Cursor = Cursors.WaitCursor;
            Refresh();

            ResultadoOctave salida;
            try
            {
                salida = OctaveIntegration.Ejecutar(parametros, txtOctave.Text);
            }
            catch (Exception ex)
            {
                salida = new ResultadoOctave();
                salida.MensajeError =
                    "Error inesperado al invocar a Octave:\n" + ex.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            if (!salida.Exitoso)
            {
                CambiarEstado(EstadoSimulacion.Esperando,
                    "No se pudo ejecutar la simulación");
                MessageBox.Show(this, salida.MensajeError, "Error de simulación",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            datos = salida.Datos;
            PrepararEscena();

            // La animación se ve en la pestaña del campo; las tablas se van
            // llenando en paralelo en la otra pestaña.
            tabControl1.SelectedTab = tabPartida;
            IniciarAnimacion();
        }

        /// <summary>Detiene la animación y deja la pantalla como al inicio.</summary>
        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            timerAnimacion.Stop();

            dgvRecorrido.Rows.Clear();
            dgvViento.Rows.Clear();

            datos = null;
            indiceFotograma = 0;

            lblResultado.Text = "Objetivo: sin evaluar";
            lblResultado.ForeColor = Color.White;
            lblResultado.BackColor = Color.FromArgb(40, 40, 40);

            CambiarEstado(EstadoSimulacion.Esperando, "Cargue los datos y pulse Lanzar");
            PrepararEscena();
        }

        #endregion

        #region Lectura de los parámetros de entrada

        /// <summary>
        /// Convierte los controles de la interfaz en los parámetros que entiende
        /// Octave. Devuelve null (y avisa) si hay algún rango inválido.
        /// </summary>
        private ParametrosSimulacion LeerParametros()
        {
            ParametrosSimulacion p = new ParametrosSimulacion();
            p.Angulo = Decimal.ToDouble(numAngulo.Value);
            p.Fuerza = Decimal.ToDouble(numFuerza.Value);
            p.DireccionMin = Decimal.ToDouble(numDirMin.Value);
            p.DireccionMax = Decimal.ToDouble(numDirMax.Value);
            p.FuerzaVientoMin = Decimal.ToDouble(numFuerzaVientoMin.Value);
            p.FuerzaVientoMax = Decimal.ToDouble(numFuerzaVientoMax.Value);
            p.DistanciaHoyo = Decimal.ToDouble(numDistancia.Value);
            p.Tolerancia = Decimal.ToDouble(numTolerancia.Value);
            p.Semilla = Decimal.ToDouble(numSemilla.Value);

            // Los límites se normalizan aquí para que la interfaz quede
            // consistente con lo que Octave recibe (Octave también los
            // reordena por seguridad).
            if (p.DireccionMin > p.DireccionMax)
            {
                double t = p.DireccionMin;
                p.DireccionMin = p.DireccionMax;
                p.DireccionMax = t;
                numDirMin.Value = (decimal)p.DireccionMin;
                numDirMax.Value = (decimal)p.DireccionMax;
            }

            if (p.FuerzaVientoMin > p.FuerzaVientoMax)
            {
                double t = p.FuerzaVientoMin;
                p.FuerzaVientoMin = p.FuerzaVientoMax;
                p.FuerzaVientoMax = t;
                numFuerzaVientoMin.Value = (decimal)p.FuerzaVientoMin;
                numFuerzaVientoMax.Value = (decimal)p.FuerzaVientoMax;
            }

            if (p.FuerzaVientoMin < 0)
            {
                MessageBox.Show(this,
                    "La fuerza mínima del viento no puede ser negativa.",
                    "Parámetros inválidos", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return null;
            }

            return p;
        }

        #endregion

        #region Preparación del escenario

        /// <summary>
        /// Ubica el golfista, el hoyo y la pelota dentro del área de dibujo.
        /// La escala se deriva de los datos reales de Octave cuando existen;
        /// si aún no hay corrida se usa una proporción de dibujo provisional.
        /// </summary>
        private void PrepararEscena()
        {
            origenXPixeles = MARGEN_IZQUIERDO;
            baseYPixeles = picCampo.ClientSize.Height - MARGEN_INFERIOR;

            int anchoUtil = picCampo.ClientSize.Width - MARGEN_IZQUIERDO - MARGEN_DERECHO;
            int altoUtil = baseYPixeles - MARGEN_SUPERIOR;

            double rangoX;
            double rangoY;

            if (datos != null && datos.Recorrido.Count > 0)
            {
                // Con los resultados de Octave ya se sabe exactamente cuánto
                // se recorrió y cuánto se subió, así la parábola siempre cabe.
                double maxX = 0;
                double maxY = 0;
                foreach (FilaRecorrido fila in datos.Recorrido)
                {
                    if (fila.X > maxX) maxX = fila.X;
                    if (fila.Y > maxY) maxY = fila.Y;
                }

                rangoX = Math.Max(maxX, distanciaHoyoMetros) * 1.05;
                rangoY = maxY * 1.10;
                if (rangoY < 10) rangoY = 10;
            }
            else
            {
                // Proporción de dibujo provisional: no es física, sólo sirve
                // para colocar el hoyo antes de la primera corrida.
                rangoX = Math.Max(distanciaHoyoMetros, 10) * 1.15;
                rangoY = rangoX * 0.45;
            }

            if (rangoX <= 0) rangoX = 1;

            // Escala UNIFORME en ambos ejes: es lo que hace que la trayectoria
            // dibujada sea realmente una parábola.
            escala = PhysicsCalculator.CalcularEscala(
                anchoUtil, altoUtil, rangoX, rangoY);

            int xHoyo = PhysicsCalculator.PosicionPixelesX(
                distanciaHoyoMetros, escala, origenXPixeles);

            picGolfista.Location = new Point(14, baseYPixeles - picGolfista.Height);
            picHoyo.Location = new Point(
                xHoyo - picHoyo.Width / 2,
                baseYPixeles + 8 - picHoyo.Height);

            ColocarPelota(0, 0);
        }

        /// <summary>Ubica la pelota en la posición indicada por los metros dados.</summary>
        private void ColocarPelota(double xMetros, double yMetros)
        {
            int px = PhysicsCalculator.PosicionPixelesX(xMetros, escala, origenXPixeles);
            int py = PhysicsCalculator.PosicionPixelesY(yMetros, escala, baseYPixeles);

            picPelota.Location = new Point(
                px - picPelota.Width / 2,
                py - picPelota.Height / 2);
        }

        /// <summary>
        /// El área de juego cambia de tamaño al maximizar la ventana, por lo
        /// que hay que recalcular la escala y volver a dibujar el escenario y
        /// el fotograma que se esté mostrando.
        /// </summary>
        private void picCampo_Resize(object sender, EventArgs e)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            PrepararEscena();

            if (datos != null && indiceFotograma > 0 &&
                indiceFotograma <= datos.Recorrido.Count)
            {
                FilaRecorrido actual = datos.Recorrido[indiceFotograma - 1];
                ColocarPelota(actual.X, actual.Y);
            }
        }

        #endregion

        #region Animación de los fotogramas

        /// <summary>Prepara ambas tablas y arranca el reloj de 0.5 s.</summary>
        private void IniciarAnimacion()
        {
            if (datos == null || datos.Recorrido.Count == 0)
            {
                CambiarEstado(EstadoSimulacion.Esperando,
                    "Sin fotogramas que animar");
                return;
            }

            dgvRecorrido.Rows.Clear();
            dgvViento.Rows.Clear();
            indiceFotograma = 0;

            CambiarEstado(EstadoSimulacion.Animando, "Animando...");
            timerAnimacion.Start();
        }

        /// <summary>
        /// Un disparo del reloj = un fotograma: se agrega una fila a cada una de
        /// las dos tablas y se actualiza la posición de la pelota.
        /// </summary>
        private void timerAnimacion_Tick(object sender, EventArgs e)
        {
            if (datos == null || indiceFotograma >= datos.Recorrido.Count)
            {
                FinalizarAnimacion();
                return;
            }

            FilaRecorrido recorrido = datos.Recorrido[indiceFotograma];

            dgvRecorrido.Rows.Add(
                PhysicsCalculator.Formatear(recorrido.Tiempo),
                PhysicsCalculator.Formatear(recorrido.X),
                PhysicsCalculator.Formatear(recorrido.Y),
                PhysicsCalculator.Formatear(recorrido.Vx),
                PhysicsCalculator.Formatear(recorrido.Vy),
                PhysicsCalculator.Formatear(recorrido.Magnitud));

            if (indiceFotograma < datos.Viento.Count)
            {
                FilaViento viento = datos.Viento[indiceFotograma];
                dgvViento.Rows.Add(
                    PhysicsCalculator.Formatear(viento.Tiempo),
                    PhysicsCalculator.FormatearAngulo(viento.Direccion),
                    PhysicsCalculator.FormatearAceleracion(viento.Fuerza),
                    PhysicsCalculator.FormatearAceleracion(viento.Ax));
            }

            ColocarPelota(recorrido.X, recorrido.Y);

            indiceFotograma++;

            lblEstado.Text = string.Format(CultureInfo.InvariantCulture,
                "Fotograma {0} de {1}   t = {2} s",
                indiceFotograma, datos.Recorrido.Count,
                PhysicsCalculator.Formatear(recorrido.Tiempo));

            ÚltimaFilaVisible(dgvRecorrido);
            ÚltimaFilaVisible(dgvViento);

            if (indiceFotograma >= datos.Recorrido.Count)
            {
                FinalizarAnimacion();
            }
        }

        /// <summary>Mantiene siempre visible la fila más reciente.</summary>
        private void ÚltimaFilaVisible(DataGridView dgv)
        {
            if (dgv.Rows.Count > 0)
            {
                dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
            }
        }

        /// <summary>Detiene el reloj y muestra el resultado de la corrida.</summary>
        private void FinalizarAnimacion()
        {
            timerAnimacion.Stop();

            if (datos == null || datos.Resumen == null)
            {
                CambiarEstado(EstadoSimulacion.Esperando,
                    "La simulación terminó sin resultado");
                return;
            }

            ResumenSimulacion resumen = datos.Resumen;

            string detalle = string.Format(CultureInfo.InvariantCulture,
                "Distancia al hoyo: {0} m (tolerancia {1} m)\n" +
                "Tiempo de vuelo: {2} s\n" +
                "Altura máxima: {3} m\n" +
                "Punto de impacto: {4} m",
                PhysicsCalculator.FormatearPosicion(resumen.DistanciaImpacto),
                PhysicsCalculator.FormatearPosicion(resumen.Tolerancia),
                PhysicsCalculator.FormatearTiempo(resumen.TiempoVuelo),
                PhysicsCalculator.FormatearPosicion(resumen.YMax),
                PhysicsCalculator.FormatearPosicion(resumen.XImpacto));

            if (resumen.AlcanzoObjetivo)
            {
                CambiarEstado(EstadoSimulacion.Finalizado, "¡Objetivo alcanzado!");
                lblResultado.Text = "OBJETIVO ALCANZADO\n" +
                    PhysicsCalculator.FormatearPosicion(resumen.DistanciaImpacto) +
                    " del hoyo";
                lblResultado.ForeColor = Color.LimeGreen;

                MessageBox.Show(this,
                    "¡La pelota cayó dentro de la tolerancia del hoyo!\n\n" +
                    detalle,
                    "Objetivo alcanzado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                CambiarEstado(EstadoSimulacion.Finalizado,
                    "Objetivo no alcanzado");
                lblResultado.Text = "NO ALCANZADO\na " +
                    PhysicsCalculator.FormatearPosicion(resumen.DistanciaImpacto) +
                    " del hoyo";
                lblResultado.ForeColor = Color.OrangeRed;

                MessageBox.Show(this,
                    "La pelota no llegó al hoyo.\n\n" + detalle +
                    "\n\nAjuste el ángulo o la fuerza e intente de nuevo.",
                    "Objetivo no alcanzado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            lblResultado.BackColor = Color.FromArgb(40, 40, 40);
            lblEstado.Text += "   ·   Vea la pestaña «Tablas de información»";
        }

        #endregion

        #region Utilidades de interfaz

        /// <summary>Cambia el estado general y el texto de la barra de estado.</summary>
        private void CambiarEstado(EstadoSimulacion nuevo, string mensaje)
        {
            estado = nuevo;
            lblEstado.Text = mensaje;

            btnLanzar.Enabled = (nuevo != EstadoSimulacion.Calculando &&
                                 nuevo != EstadoSimulacion.Animando);
            btnReiniciar.Enabled = (nuevo != EstadoSimulacion.Calculando);

            if (nuevo == EstadoSimulacion.Finalizado)
            {
                lblEstado.ForeColor = Color.LimeGreen;
            }
            else if (nuevo == EstadoSimulacion.Calculando ||
                     nuevo == EstadoSimulacion.Animando)
            {
                lblEstado.ForeColor = Color.Gold;
            }
            else
            {
                lblEstado.ForeColor = Color.Gold;
            }
        }

        /// <summary>Aplica un tema oscuro uniforme a una tabla de información.</summary>
        private void AplicarEstiloTabla(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 24;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 30;

            dgv.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 130, 180);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
        }

        #endregion
    }
}
