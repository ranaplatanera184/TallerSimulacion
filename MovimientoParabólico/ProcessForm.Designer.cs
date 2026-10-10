namespace MovimientoParabólico
{
    partial class ProcessForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.timerAnimacion = new System.Windows.Forms.Timer(this.components);

            // Encabezado
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();

            // Pestañas: "Partida" (campo + entradas) y "Tablas de información"
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPartida = new System.Windows.Forms.TabPage();
            this.tabTablas = new System.Windows.Forms.TabPage();

            // Campo de golf
            this.picCampo = new System.Windows.Forms.PictureBox();
            this.picGolfista = new System.Windows.Forms.PictureBox();
            this.picHoyo = new System.Windows.Forms.PictureBox();
            this.picPelota = new System.Windows.Forms.PictureBox();

            // Panel de entradas
            this.panelEntradas = new System.Windows.Forms.Panel();
            this.grpEntradas = new System.Windows.Forms.GroupBox();
            this.btnLanzar = new System.Windows.Forms.Button();
            this.btnReiniciar = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();

            // Etiquetas de entrada
            this.lblAngulo = new System.Windows.Forms.Label();
            this.lblFuerza = new System.Windows.Forms.Label();
            this.lblDirMin = new System.Windows.Forms.Label();
            this.lblDirMax = new System.Windows.Forms.Label();
            this.lblFuerzaVientoMin = new System.Windows.Forms.Label();
            this.lblFuerzaVientoMax = new System.Windows.Forms.Label();
            this.lblDistancia = new System.Windows.Forms.Label();
            this.lblTolerancia = new System.Windows.Forms.Label();
            this.lblSemilla = new System.Windows.Forms.Label();
            this.lblOctave = new System.Windows.Forms.Label();

            // Campos numéricos de entrada
            this.numAngulo = new System.Windows.Forms.NumericUpDown();
            this.numFuerza = new System.Windows.Forms.NumericUpDown();
            this.numDirMin = new System.Windows.Forms.NumericUpDown();
            this.numDirMax = new System.Windows.Forms.NumericUpDown();
            this.numFuerzaVientoMin = new System.Windows.Forms.NumericUpDown();
            this.numFuerzaVientoMax = new System.Windows.Forms.NumericUpDown();
            this.numDistancia = new System.Windows.Forms.NumericUpDown();
            this.numTolerancia = new System.Windows.Forms.NumericUpDown();
            this.numSemilla = new System.Windows.Forms.NumericUpDown();
            this.txtOctave = new System.Windows.Forms.TextBox();

            // Tablas de información
            this.grpRecorrido = new System.Windows.Forms.GroupBox();
            this.dgvRecorrido = new System.Windows.Forms.DataGridView();
            this.colT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVMag = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.grpViento = new System.Windows.Forms.GroupBox();
            this.dgvViento = new System.Windows.Forms.DataGridView();
            this.colTViento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDireccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFuerzaViento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAx = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // Inicialización de contenedores y controles compuestos
            this.SuspendLayout();

            // El tamaño del formulario se fija ANTES de crear los
            // controles anclados: el anclaje mide sus márgenes contra
            // el área cliente del formulario.
            this.ClientSize = new System.Drawing.Size(1280, 670);
            this.tabControl1.SuspendLayout();
            this.tabPartida.SuspendLayout();
            this.tabTablas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCampo)).BeginInit();
            this.picCampo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGolfista)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHoyo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPelota)).BeginInit();
            this.panelEntradas.SuspendLayout();
            this.grpEntradas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAngulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFuerza)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDirMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDirMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFuerzaVientoMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFuerzaVientoMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDistancia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerancia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSemilla)).BeginInit();
            this.grpRecorrido.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecorrido)).BeginInit();
            this.grpViento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvViento)).BeginInit();

            // 
            // timerAnimacion
            // Un fotograma cada 0.5 segundos (requisito del taller)
            // 
            this.timerAnimacion.Interval = 500;
            this.timerAnimacion.Tick += new System.EventHandler(this.timerAnimacion_Tick);

            //
            // tabControl1
            //
            // El control queda anclado a los cuatro lados: si la ventana se
            // maximiza, crece sobre todo el área de las tablas y se ven más
            // filas sin necesidad de desplazarse.
            //
            // IMPORTANTE: las páginas DEBEN llevar Size explícito. El anclaje
            // de sus hijos se mide contra el tamaño que la página tiene en ese
            // momento; si se deja en (0,0) los controles se desbordan al
            // primer reajuste de la ventana.
            this.tabControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.tabControl1.ForeColor = System.Drawing.Color.White;
            this.tabControl1.Location = new System.Drawing.Point(12, 64);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1256, 596);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPartida);
            this.tabControl1.Controls.Add(this.tabTablas);

            //
            // tabPartida
            //
            this.tabPartida.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tabPartida.Location = new System.Drawing.Point(2, 26);
            this.tabPartida.Name = "tabPartida";
            this.tabPartida.Padding = new System.Windows.Forms.Padding(3);
            this.tabPartida.Size = new System.Drawing.Size(1252, 568);
            this.tabPartida.TabIndex = 0;
            this.tabPartida.Text = "Partida";
            this.tabPartida.UseVisualStyleBackColor = false;
            this.tabPartida.Controls.Add(this.picCampo);
            this.tabPartida.Controls.Add(this.panelEntradas);

            //
            // tabTablas
            //
            this.tabTablas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tabTablas.Location = new System.Drawing.Point(2, 26);
            this.tabTablas.Name = "tabTablas";
            this.tabTablas.Padding = new System.Windows.Forms.Padding(3);
            this.tabTablas.Size = new System.Drawing.Size(1252, 568);
            this.tabTablas.TabIndex = 1;
            this.tabTablas.Text = "Tablas de información";
            this.tabTablas.UseVisualStyleBackColor = false;
            this.tabTablas.Controls.Add(this.grpRecorrido);
            this.tabTablas.Controls.Add(this.grpViento);

            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(12, 8);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(560, 28);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Simulador de Movimiento Parabólico - Golf";

            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.BackColor = System.Drawing.Color.Transparent;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEstado.ForeColor = System.Drawing.Color.Gold;
            this.lblEstado.Location = new System.Drawing.Point(14, 44);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(330, 19);
            this.lblEstado.TabIndex = 1;
            this.lblEstado.Text = "Cargue los datos y pulse Lanzar";

            // 
            // picCampo
            // 
            this.picCampo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(90)))), ((int)(((byte)(40)))));
            this.picCampo.BackgroundImage = global::MovimientoParabólico.Properties.Resources.Fondo;
            this.picCampo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picCampo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCampo.Location = new System.Drawing.Point(8, 8);
            this.picCampo.Name = "picCampo";
            this.picCampo.Size = new System.Drawing.Size(940, 546);
            this.picCampo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.picCampo.TabIndex = 2;
            this.picCampo.TabStop = false;
            this.picCampo.Resize += new System.EventHandler(this.picCampo_Resize);

            // 
            // picGolfista
            // 
            this.picGolfista.BackColor = System.Drawing.Color.Transparent;
            this.picGolfista.Image = global::MovimientoParabólico.Properties.Resources.Golfista;
            this.picGolfista.Location = new System.Drawing.Point(14, 290);
            this.picGolfista.Name = "picGolfista";
            this.picGolfista.Size = new System.Drawing.Size(80, 126);
            this.picGolfista.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGolfista.TabIndex = 3;
            this.picGolfista.TabStop = false;

            // 
            // picHoyo
            // 
            this.picHoyo.BackColor = System.Drawing.Color.Transparent;
            this.picHoyo.Image = global::MovimientoParabólico.Properties.Resources.Hoyo;
            this.picHoyo.Location = new System.Drawing.Point(760, 344);
            this.picHoyo.Name = "picHoyo";
            this.picHoyo.Size = new System.Drawing.Size(72, 72);
            this.picHoyo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHoyo.TabIndex = 4;
            this.picHoyo.TabStop = false;

            // 
            // picPelota
            // 
            this.picPelota.BackColor = System.Drawing.Color.Transparent;
            this.picPelota.Image = global::MovimientoParabólico.Properties.Resources.Pelota;
            this.picPelota.Location = new System.Drawing.Point(70, 394);
            this.picPelota.Name = "picPelota";
            this.picPelota.Size = new System.Drawing.Size(20, 20);
            this.picPelota.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPelota.TabIndex = 5;
            this.picPelota.TabStop = false;

            // Los gráficos del escenario viven dentro de picCampo (coordenadas
            // relativas al área de dibujo). El índice 0 queda arriba del todo,
            // por lo que la pelota se agrega primero para que nunca quede
            // tapada por el golfista o por el hoyo.
            this.picCampo.Controls.Add(this.picPelota);
            this.picCampo.Controls.Add(this.picHoyo);
            this.picCampo.Controls.Add(this.picGolfista);

            // 
            // panelEntradas
            // 
            this.panelEntradas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panelEntradas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEntradas.Controls.Add(this.grpEntradas);
            this.panelEntradas.Controls.Add(this.btnLanzar);
            this.panelEntradas.Controls.Add(this.btnReiniciar);
            this.panelEntradas.Controls.Add(this.lblResultado);
            this.panelEntradas.Location = new System.Drawing.Point(956, 8);
            this.panelEntradas.Name = "panelEntradas";
            this.panelEntradas.Size = new System.Drawing.Size(288, 546);
            this.panelEntradas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEntradas.TabIndex = 6;

            // 
            // grpEntradas
            // 
            this.grpEntradas.Controls.Add(this.lblAngulo);
            this.grpEntradas.Controls.Add(this.numAngulo);
            this.grpEntradas.Controls.Add(this.lblFuerza);
            this.grpEntradas.Controls.Add(this.numFuerza);
            this.grpEntradas.Controls.Add(this.lblDirMin);
            this.grpEntradas.Controls.Add(this.numDirMin);
            this.grpEntradas.Controls.Add(this.lblDirMax);
            this.grpEntradas.Controls.Add(this.numDirMax);
            this.grpEntradas.Controls.Add(this.lblFuerzaVientoMin);
            this.grpEntradas.Controls.Add(this.numFuerzaVientoMin);
            this.grpEntradas.Controls.Add(this.lblFuerzaVientoMax);
            this.grpEntradas.Controls.Add(this.numFuerzaVientoMax);
            this.grpEntradas.Controls.Add(this.lblDistancia);
            this.grpEntradas.Controls.Add(this.numDistancia);
            this.grpEntradas.Controls.Add(this.lblTolerancia);
            this.grpEntradas.Controls.Add(this.numTolerancia);
            this.grpEntradas.Controls.Add(this.lblSemilla);
            this.grpEntradas.Controls.Add(this.numSemilla);
            this.grpEntradas.Controls.Add(this.lblOctave);
            this.grpEntradas.Controls.Add(this.txtOctave);
            this.grpEntradas.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpEntradas.ForeColor = System.Drawing.Color.Gold;
            this.grpEntradas.Location = new System.Drawing.Point(7, 7);
            this.grpEntradas.Name = "grpEntradas";
            this.grpEntradas.Size = new System.Drawing.Size(274, 360);
            this.grpEntradas.TabIndex = 0;
            this.grpEntradas.TabStop = false;
            this.grpEntradas.Text = "Parámetros de entrada";

            // 
            // lblAngulo
            // 
            this.lblAngulo.AutoSize = true;
            this.lblAngulo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblAngulo.ForeColor = System.Drawing.Color.White;
            this.lblAngulo.Location = new System.Drawing.Point(10, 22);
            this.lblAngulo.Name = "lblAngulo";
            this.lblAngulo.Size = new System.Drawing.Size(112, 13);
            this.lblAngulo.TabIndex = 0;
            this.lblAngulo.Text = "Ángulo de salida (°)";

            // 
            // numAngulo
            // 
            this.numAngulo.DecimalPlaces = 1;
            this.numAngulo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.numAngulo.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            this.numAngulo.Location = new System.Drawing.Point(146, 20);
            this.numAngulo.Maximum = new decimal(new int[] { 85, 0, 0, 0 });
            this.numAngulo.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            this.numAngulo.Name = "numAngulo";
            this.numAngulo.Size = new System.Drawing.Size(94, 22);
            this.numAngulo.TabIndex = 1;
            this.numAngulo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numAngulo.Value = new decimal(new int[] { 40, 0, 0, 0 });

            // 
            // lblFuerza
            // 
            this.lblFuerza.AutoSize = true;
            this.lblFuerza.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblFuerza.ForeColor = System.Drawing.Color.White;
            this.lblFuerza.Location = new System.Drawing.Point(10, 48);
            this.lblFuerza.Name = "lblFuerza";
            this.lblFuerza.Size = new System.Drawing.Size(116, 13);
            this.lblFuerza.TabIndex = 2;
            this.lblFuerza.Text = "Fuerza de salida (m/s)";

            // 
            // numFuerza
            // 
            this.numFuerza.DecimalPlaces = 1;
            this.numFuerza.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.numFuerza.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            this.numFuerza.Location = new System.Drawing.Point(146, 46);
            this.numFuerza.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            this.numFuerza.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            this.numFuerza.Name = "numFuerza";
            this.numFuerza.Size = new System.Drawing.Size(94, 22);
            this.numFuerza.TabIndex = 3;
            this.numFuerza.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFuerza.Value = new decimal(new int[] { 40, 0, 0, 0 });

            // 
            // lblDirMin
            // 
            this.lblDirMin.AutoSize = true;
            this.lblDirMin.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblDirMin.ForeColor = System.Drawing.Color.White;
            this.lblDirMin.Location = new System.Drawing.Point(10, 74);
            this.lblDirMin.Name = "lblDirMin";
            this.lblDirMin.Size = new System.Drawing.Size(128, 13);
            this.lblDirMin.TabIndex = 4;
            this.lblDirMin.Text = "Dirección viento mín. (°)";

            // 
            // numDirMin
            // 
            this.numDirMin.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.numDirMin.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            this.numDirMin.Location = new System.Drawing.Point(146, 72);
            this.numDirMin.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            this.numDirMin.Name = "numDirMin";
            this.numDirMin.Size = new System.Drawing.Size(94, 22);
            this.numDirMin.TabIndex = 5;
            this.numDirMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // lblDirMax
            // 
            this.lblDirMax.AutoSize = true;
            this.lblDirMax.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblDirMax.ForeColor = System.Drawing.Color.White;
            this.lblDirMax.Location = new System.Drawing.Point(10, 100);
            this.lblDirMax.Name = "lblDirMax";
            this.lblDirMax.Size = new System.Drawing.Size(131, 13);
            this.lblDirMax.TabIndex = 6;
            this.lblDirMax.Text = "Dirección viento máx. (°)";

            // 
            // numDirMax
            // 
            this.numDirMax.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.numDirMax.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            this.numDirMax.Location = new System.Drawing.Point(146, 98);
            this.numDirMax.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            this.numDirMax.Name = "numDirMax";
            this.numDirMax.Size = new System.Drawing.Size(94, 22);
            this.numDirMax.TabIndex = 7;
            this.numDirMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numDirMax.Value = new decimal(new int[] { 180, 0, 0, 0 });

            // 
            // lblFuerzaVientoMin
            // 
            this.lblFuerzaVientoMin.AutoSize = true;
            this.lblFuerzaVientoMin.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblFuerzaVientoMin.ForeColor = System.Drawing.Color.White;
            this.lblFuerzaVientoMin.Location = new System.Drawing.Point(10, 126);
            this.lblFuerzaVientoMin.Name = "lblFuerzaVientoMin";
            this.lblFuerzaVientoMin.Size = new System.Drawing.Size(128, 13);
            this.lblFuerzaVientoMin.TabIndex = 8;
            this.lblFuerzaVientoMin.Text = "Fuerza viento mín. (m/s²)";

            // 
            // numFuerzaVientoMin
            // 
            this.numFuerzaVientoMin.DecimalPlaces = 1;
            this.numFuerzaVientoMin.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.numFuerzaVientoMin.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            this.numFuerzaVientoMin.Location = new System.Drawing.Point(146, 124);
            this.numFuerzaVientoMin.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.numFuerzaVientoMin.Name = "numFuerzaVientoMin";
            this.numFuerzaVientoMin.Size = new System.Drawing.Size(94, 22);
            this.numFuerzaVientoMin.TabIndex = 9;
            this.numFuerzaVientoMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // lblFuerzaVientoMax
            // 
            this.lblFuerzaVientoMax.AutoSize = true;
            this.lblFuerzaVientoMax.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblFuerzaVientoMax.ForeColor = System.Drawing.Color.White;
            this.lblFuerzaVientoMax.Location = new System.Drawing.Point(10, 152);
            this.lblFuerzaVientoMax.Name = "lblFuerzaVientoMax";
            this.lblFuerzaVientoMax.Size = new System.Drawing.Size(131, 13);
            this.lblFuerzaVientoMax.TabIndex = 10;
            this.lblFuerzaVientoMax.Text = "Fuerza viento máx. (m/s²)";

            // 
            // numFuerzaVientoMax
            // 
            this.numFuerzaVientoMax.DecimalPlaces = 1;
            this.numFuerzaVientoMax.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.numFuerzaVientoMax.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            this.numFuerzaVientoMax.Location = new System.Drawing.Point(146, 150);
            this.numFuerzaVientoMax.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.numFuerzaVientoMax.Name = "numFuerzaVientoMax";
            this.numFuerzaVientoMax.Size = new System.Drawing.Size(94, 22);
            this.numFuerzaVientoMax.TabIndex = 11;
            this.numFuerzaVientoMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFuerzaVientoMax.Value = new decimal(new int[] { 20, 0, 0, 65536 });

            // 
            // lblDistancia
            // 
            this.lblDistancia.AutoSize = true;
            this.lblDistancia.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblDistancia.ForeColor = System.Drawing.Color.White;
            this.lblDistancia.Location = new System.Drawing.Point(10, 178);
            this.lblDistancia.Name = "lblDistancia";
            this.lblDistancia.Size = new System.Drawing.Size(122, 13);
            this.lblDistancia.TabIndex = 12;
            this.lblDistancia.Text = "Distancia del hoyo (m)";

            // 
            // numDistancia
            // 
            this.numDistancia.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.numDistancia.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            this.numDistancia.Location = new System.Drawing.Point(146, 176);
            this.numDistancia.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            this.numDistancia.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            this.numDistancia.Name = "numDistancia";
            this.numDistancia.Size = new System.Drawing.Size(94, 22);
            this.numDistancia.TabIndex = 13;
            this.numDistancia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numDistancia.Value = new decimal(new int[] { 160, 0, 0, 0 });

            // 
            // lblTolerancia
            // 
            this.lblTolerancia.AutoSize = true;
            this.lblTolerancia.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblTolerancia.ForeColor = System.Drawing.Color.White;
            this.lblTolerancia.Location = new System.Drawing.Point(10, 204);
            this.lblTolerancia.Name = "lblTolerancia";
            this.lblTolerancia.Size = new System.Drawing.Size(88, 13);
            this.lblTolerancia.TabIndex = 14;
            this.lblTolerancia.Text = "Tolerancia (m)";

            // 
            // numTolerancia
            // 
            this.numTolerancia.DecimalPlaces = 1;
            this.numTolerancia.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.numTolerancia.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            this.numTolerancia.Location = new System.Drawing.Point(146, 202);
            this.numTolerancia.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            this.numTolerancia.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            this.numTolerancia.Name = "numTolerancia";
            this.numTolerancia.Size = new System.Drawing.Size(94, 22);
            this.numTolerancia.TabIndex = 15;
            this.numTolerancia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTolerancia.Value = new decimal(new int[] { 50, 0, 0, 65536 });

            // 
            // lblSemilla
            // 
            this.lblSemilla.AutoSize = true;
            this.lblSemilla.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblSemilla.ForeColor = System.Drawing.Color.White;
            this.lblSemilla.Location = new System.Drawing.Point(10, 230);
            this.lblSemilla.Name = "lblSemilla";
            this.lblSemilla.Size = new System.Drawing.Size(105, 13);
            this.lblSemilla.TabIndex = 16;
            this.lblSemilla.Text = "Semilla aleatoria";

            // 
            // numSemilla
            // 
            this.numSemilla.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.numSemilla.Location = new System.Drawing.Point(146, 228);
            this.numSemilla.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            this.numSemilla.Name = "numSemilla";
            this.numSemilla.Size = new System.Drawing.Size(94, 22);
            this.numSemilla.TabIndex = 17;
            this.numSemilla.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSemilla.Value = new decimal(new int[] { 42, 0, 0, 0 });

            // 
            // lblOctave
            // 
            this.lblOctave.AutoSize = true;
            this.lblOctave.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblOctave.ForeColor = System.Drawing.Color.White;
            this.lblOctave.Location = new System.Drawing.Point(10, 258);
            this.lblOctave.Name = "lblOctave";
            this.lblOctave.Size = new System.Drawing.Size(210, 13);
            this.lblOctave.TabIndex = 18;
            this.lblOctave.Text = "Ruta de Octave (vacío = buscar en PATH)";

            // 
            // txtOctave
            // 
            this.txtOctave.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.txtOctave.Location = new System.Drawing.Point(10, 278);
            this.txtOctave.Name = "txtOctave";
            this.txtOctave.Size = new System.Drawing.Size(230, 22);
            this.txtOctave.TabIndex = 19;

            // 
            // btnLanzar
            // 
            this.btnLanzar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btnLanzar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLanzar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLanzar.ForeColor = System.Drawing.Color.White;
            this.btnLanzar.Location = new System.Drawing.Point(8, 379);
            this.btnLanzar.Name = "btnLanzar";
            this.btnLanzar.Size = new System.Drawing.Size(130, 40);
            this.btnLanzar.TabIndex = 1;
            this.btnLanzar.Text = "Lanzar";
            this.btnLanzar.UseVisualStyleBackColor = false;
            this.btnLanzar.Click += new System.EventHandler(this.btnLanzar_Click);

            // 
            // btnReiniciar
            // 
            this.btnReiniciar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnReiniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReiniciar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReiniciar.ForeColor = System.Drawing.Color.White;
            this.btnReiniciar.Location = new System.Drawing.Point(148, 379);
            this.btnReiniciar.Name = "btnReiniciar";
            this.btnReiniciar.Size = new System.Drawing.Size(130, 40);
            this.btnReiniciar.TabIndex = 2;
            this.btnReiniciar.Text = "Reiniciar";
            this.btnReiniciar.UseVisualStyleBackColor = false;
            this.btnReiniciar.Click += new System.EventHandler(this.btnReiniciar_Click);

            // 
            // lblResultado
            // 
            this.lblResultado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblResultado.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblResultado.ForeColor = System.Drawing.Color.White;
            this.lblResultado.Location = new System.Drawing.Point(8, 431);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(270, 106);
            this.lblResultado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultado.TabIndex = 3;
            this.lblResultado.Text = "Objetivo: sin evaluar";
            this.lblResultado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // grpRecorrido
            // 
            this.grpRecorrido.Controls.Add(this.dgvRecorrido);
            this.grpRecorrido.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpRecorrido.ForeColor = System.Drawing.Color.LimeGreen;
            this.grpRecorrido.Location = new System.Drawing.Point(8, 8);
            this.grpRecorrido.Name = "grpRecorrido";
            this.grpRecorrido.Size = new System.Drawing.Size(610, 546);
            this.grpRecorrido.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpRecorrido.TabIndex = 7;
            this.grpRecorrido.TabStop = false;
            this.grpRecorrido.Text = "Tabla 1 - Recorrido (un fotograma cada 0.5 s)";

            // 
            // dgvRecorrido
            // 
            this.dgvRecorrido.AllowUserToAddRows = false;
            this.dgvRecorrido.AllowUserToDeleteRows = false;
            this.dgvRecorrido.AllowUserToResizeRows = false;
            this.dgvRecorrido.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvRecorrido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvRecorrido.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecorrido.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colT,
            this.colX,
            this.colY,
            this.colVx,
            this.colVy,
            this.colVMag});
            this.dgvRecorrido.EnableHeadersVisualStyles = false;
            this.dgvRecorrido.Location = new System.Drawing.Point(10, 24);
            this.dgvRecorrido.Name = "dgvRecorrido";
            this.dgvRecorrido.ReadOnly = true;
            this.dgvRecorrido.RowHeadersVisible = false;
            this.dgvRecorrido.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecorrido.Size = new System.Drawing.Size(590, 512);
            this.dgvRecorrido.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRecorrido.TabIndex = 0;

            // 
            // colT
            // 
            this.colT.HeaderText = "t (s)";
            this.colT.Name = "colT";
            this.colT.ReadOnly = true;
            this.colT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // colX
            // 
            this.colX.HeaderText = "x (m)";
            this.colX.Name = "colX";
            this.colX.ReadOnly = true;
            this.colX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // colY
            // 
            this.colY.HeaderText = "y (m)";
            this.colY.Name = "colY";
            this.colY.ReadOnly = true;
            this.colY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // colVx
            // 
            this.colVx.HeaderText = "vx (m/s)";
            this.colVx.Name = "colVx";
            this.colVx.ReadOnly = true;
            this.colVx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // colVy
            // 
            this.colVy.HeaderText = "vy (m/s)";
            this.colVy.Name = "colVy";
            this.colVy.ReadOnly = true;
            this.colVy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // colVMag
            // 
            this.colVMag.HeaderText = "|v| (m/s)";
            this.colVMag.Name = "colVMag";
            this.colVMag.ReadOnly = true;
            this.colVMag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // grpViento
            // 
            this.grpViento.Controls.Add(this.dgvViento);
            this.grpViento.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpViento.ForeColor = System.Drawing.Color.Cyan;
            this.grpViento.Location = new System.Drawing.Point(634, 8);
            this.grpViento.Name = "grpViento";
            this.grpViento.Size = new System.Drawing.Size(610, 546);
            this.grpViento.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpViento.TabIndex = 8;
            this.grpViento.TabStop = false;
            this.grpViento.Text = "Tabla 2 - Agentes de generación aleatoria (viento por fotograma)";

            // 
            // dgvViento
            // 
            this.dgvViento.AllowUserToAddRows = false;
            this.dgvViento.AllowUserToDeleteRows = false;
            this.dgvViento.AllowUserToResizeRows = false;
            this.dgvViento.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvViento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvViento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvViento.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTViento,
            this.colDireccion,
            this.colFuerzaViento,
            this.colAx});
            this.dgvViento.EnableHeadersVisualStyles = false;
            this.dgvViento.Location = new System.Drawing.Point(10, 24);
            this.dgvViento.Name = "dgvViento";
            this.dgvViento.ReadOnly = true;
            this.dgvViento.RowHeadersVisible = false;
            this.dgvViento.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvViento.Size = new System.Drawing.Size(590, 512);
            this.dgvViento.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvViento.TabIndex = 0;

            // 
            // colTViento
            // 
            this.colTViento.HeaderText = "t (s)";
            this.colTViento.Name = "colTViento";
            this.colTViento.ReadOnly = true;
            this.colTViento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // colDireccion
            // 
            this.colDireccion.HeaderText = "Dirección (°)";
            this.colDireccion.Name = "colDireccion";
            this.colDireccion.ReadOnly = true;
            this.colDireccion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // colFuerzaViento
            // 
            this.colFuerzaViento.HeaderText = "Fuerza (m/s²)";
            this.colFuerzaViento.Name = "colFuerzaViento";
            this.colFuerzaViento.ReadOnly = true;
            this.colFuerzaViento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // colAx
            // 
            this.colAx.HeaderText = "ax (m/s²)";
            this.colAx.Name = "colAx";
            this.colAx.ReadOnly = true;
            this.colAx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            // 
            // ProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(1280, 670);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.Name = "ProcessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Movimiento Parabolico - Simulador de Golf";
            this.Load += new System.EventHandler(this.ProcessForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProcessForm_FormClosing);

            // Fin de la inicialización
            this.grpViento.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvViento)).EndInit();
            this.grpRecorrido.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecorrido)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSemilla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerancia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDistancia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFuerzaVientoMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFuerzaVientoMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDirMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDirMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFuerza)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngulo)).EndInit();
            this.grpEntradas.ResumeLayout(false);
            this.grpEntradas.PerformLayout();
            this.panelEntradas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPelota)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHoyo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGolfista)).EndInit();
            this.picCampo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCampo)).EndInit();
            this.tabTablas.ResumeLayout(false);
            this.tabPartida.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timerAnimacion;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPartida;
        private System.Windows.Forms.TabPage tabTablas;
        private System.Windows.Forms.PictureBox picCampo;
        private System.Windows.Forms.PictureBox picGolfista;
        private System.Windows.Forms.PictureBox picHoyo;
        private System.Windows.Forms.PictureBox picPelota;
        private System.Windows.Forms.Panel panelEntradas;
        private System.Windows.Forms.GroupBox grpEntradas;
        private System.Windows.Forms.Button btnLanzar;
        private System.Windows.Forms.Button btnReiniciar;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Label lblAngulo;
        private System.Windows.Forms.Label lblFuerza;
        private System.Windows.Forms.Label lblDirMin;
        private System.Windows.Forms.Label lblDirMax;
        private System.Windows.Forms.Label lblFuerzaVientoMin;
        private System.Windows.Forms.Label lblFuerzaVientoMax;
        private System.Windows.Forms.Label lblDistancia;
        private System.Windows.Forms.Label lblTolerancia;
        private System.Windows.Forms.Label lblSemilla;
        private System.Windows.Forms.Label lblOctave;
        private System.Windows.Forms.NumericUpDown numAngulo;
        private System.Windows.Forms.NumericUpDown numFuerza;
        private System.Windows.Forms.NumericUpDown numDirMin;
        private System.Windows.Forms.NumericUpDown numDirMax;
        private System.Windows.Forms.NumericUpDown numFuerzaVientoMin;
        private System.Windows.Forms.NumericUpDown numFuerzaVientoMax;
        private System.Windows.Forms.NumericUpDown numDistancia;
        private System.Windows.Forms.NumericUpDown numTolerancia;
        private System.Windows.Forms.NumericUpDown numSemilla;
        private System.Windows.Forms.TextBox txtOctave;
        private System.Windows.Forms.GroupBox grpRecorrido;
        private System.Windows.Forms.DataGridView dgvRecorrido;
        private System.Windows.Forms.DataGridViewTextBoxColumn colT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVx;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVMag;
        private System.Windows.Forms.GroupBox grpViento;
        private System.Windows.Forms.DataGridView dgvViento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTViento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDireccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFuerzaViento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAx;
    }
}
