//****************************************************************************
//* MyOvalShape Control v1.0
//****************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

public class MyOvalShape : Control
{
	private Rectangle OuterControlBounds;
	private Rectangle CircleBounds;
	private Rectangle FillBounds;
	private Image BackImage;

	private readonly Timer ArcPieRotateTimer;

	#region "Constructor/Destructor"

	public MyOvalShape() : base()
	{

		SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.ContainerControl | ControlStyles.SupportsTransparentBackColor, true);
		base.DoubleBuffered = true;
		DoubleBuffered = true;
		BackColor = Color.Transparent;
		ForeColor = Color.DarkBlue;
		Size = new Size(200, 200);
		MinimumSize = new Size(65, 65);
		ArcPieRotateTimer = new Timer();

		Resize += MyOvalShape_Resize;
		Properties_efo.PropertyChanged += EllipseFillOptions_PropertyChanged;
		Properties_elo.PropertyChanged += EllipseLineOptions_PropertyChanged;
		ArcPieRotateTimer.Tick += ArcPieRotateTimer_Tick;
	}

	protected override void Dispose(bool disposing)
	{
		Resize -= MyOvalShape_Resize;
		Properties_efo.PropertyChanged -= EllipseFillOptions_PropertyChanged;
		Properties_elo.PropertyChanged -= EllipseLineOptions_PropertyChanged;
		ArcPieRotateTimer.Tick -= ArcPieRotateTimer_Tick;

		base.Dispose(disposing);
	}

	#endregion

	#region "Properties"

	public enum EllipseAspectRatioOption
	{
		Fixed,
		Free
	}

	public enum EllipseFillTypeOption
	{
		Arc,
		HatchStyle,
		LinearGradient,
		PathGradient,
		Pie,
		Solid
	}

	public enum EllipseFillLinearGradientShapeOption
	{
		Normal,
		Triangular,
		SigmaBell
	}

	public enum HStyle
	{
		Horizontal = 0,
		Vertical = 1,
		ForwardDiagonal = 2,
		BackwardDiagonal = 3,
		Cross = 4,
		DiagonalCross = 5,
		Percent05 = 6,
		Percent10 = 7,
		Percent20 = 8,
		Percent25 = 9,
		Percent30 = 10,
		Percent40 = 11,
		Percent50 = 12,
		Percent60 = 13,
		Percent70 = 14,
		Percent75 = 15,
		Percent80 = 16,
		Percent90 = 17,
		LightDownwardDiagonal = 18,
		LightUpwardDiagonal = 19,
		DarkDownwardDiagonal = 20,
		DarkUpwardDiagonal = 21,
		WideDownwardDiagonal = 22,
		WideUpwardDiagonal = 23,
		LightVertical = 24,
		LightHorizontal = 25,
		NarrowVertical = 26,
		NarrowHorizontal = 27,
		DarkVertical = 28,
		DarkHorizontal = 29,
		DashedDownwardDiagonal = 30,
		DashedUpwardDiagonal = 31,
		DashedHorizontal = 32,
		DashedVertical = 33,
		SmallConfetti = 34,
		LargeConfetti = 35,
		ZigZag = 36,
		Wave = 37,
		DiagonalBrick = 38,
		HorizontalBrick = 39,
		Weave = 40,
		Plaid = 41,
		Divot = 42,
		DottedGrid = 43,
		DottedDiamond = 44,
		Shingle = 45,
		Trellis = 46,
		Sphere = 47,
		SmallGrid = 48,
		SmallCheckerBoard = 49,
		LargeCheckerBoard = 50,
		OutlinedDiamond = 51,
		SolidDiamond = 52
	}

	public enum HStyleBground
	{
		Plain,
		Glow
	}

	public enum ArcPieRotateDir
	{
		CW,
		CCW
	}

	private EllipseLineOptionsProperties Properties_elo = new EllipseLineOptionsProperties();
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), Description("Expand to set the ellipse line options.")]
	public EllipseLineOptionsProperties EllipseLineOptions
	{
		get { return Properties_elo; }
		set
		{
			if (Properties_elo != value)
            {
				Properties_elo = value;
				Invalidate();
			}
		}
	}

	private EllipseFillOptionsProperties Properties_efo = new EllipseFillOptionsProperties();
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), Description("Expand to set the ellipse fill options.")]
	public EllipseFillOptionsProperties EllipseFillOptions
	{
		get { return Properties_efo; }
		set
		{
			if (Properties_efo != value)
            {
				Properties_efo = value;
				Invalidate();
			}
		}
	}

	private EllipseAspectRatioOption m_EllipseAspectRatioOption = EllipseAspectRatioOption.Fixed;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), Description("The ellipse aspect ratio."), DefaultValue(typeof(EllipseAspectRatioOption), "Fixed")]
	public EllipseAspectRatioOption EllipseAspectRatio
	{
		get { return m_EllipseAspectRatioOption; }
		set
		{
			if (m_EllipseAspectRatioOption != value)
            {
				m_EllipseAspectRatioOption = value;
				MyOvalShape_Resize(this, EventArgs.Empty);
				Invalidate();
			}
		}
	}

	private bool m_EllipseLineColorON;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the ellipse line color ON."), DefaultValue(false)]
	public bool EllipseLineColorON
	{
		get { return m_EllipseLineColorON; }
		set
		{
			if (m_EllipseLineColorON != value)
			{
				m_EllipseLineColorON = value;
				Invalidate();
			}
		}
	}

	private bool m_EllipseFillColorON;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the ellipse fill color ON. The EllipseFillColor has to be set to other than Transparent."), DefaultValue(false)]
	public bool EllipseFillColorON
	{
		get { return m_EllipseFillColorON; }
		set
		{
			if (m_EllipseFillColorON != value)
            {
				m_EllipseFillColorON = value;
				Invalidate();
			}
		}
	}

	private bool m_PieFillColorON;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the pie fill color ON."), DefaultValue(false)]
	public bool PieFillColorON
	{
		get { return m_PieFillColorON; }
		set
		{
			if (m_PieFillColorON != value)
            {
				m_PieFillColorON = value;
				Invalidate();
			}
		}
	}

	private bool m_ArcPieLineColorON;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the arc or pie line color ON."), DefaultValue(false)]
	public bool ArcPieLineColorON
	{
		get { return m_ArcPieLineColorON; }
		set
		{
			if (m_ArcPieLineColorON != value)
            {
				m_ArcPieLineColorON = value;
				Invalidate();
			}
		}
	}

	private bool m_ArcPieSymmetryON;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the arc or pie symmetry ON."), DefaultValue(false)]
	public bool ArcPieSymmetryON
	{
		get { return m_ArcPieSymmetryON; }
		set
		{
			if (m_ArcPieSymmetryON != value)
            {
				m_ArcPieSymmetryON = value;
				Invalidate();
			}
		}
	}

	private bool m_ArcPieRotate;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Rotate the arc or pie."), DefaultValue(false)]
	public bool ArcPieRotate
	{
		get { return m_ArcPieRotate; }
		set
		{
			if (m_ArcPieRotate != value)
			{
				m_ArcPieRotate = value;

				if (m_ArcPieRotate)
					ArcPieRotateTimer.Enabled = true;
				else
					ArcPieRotateTimer.Enabled = false;

				Invalidate();
			}
		}
	}

	private int m_ArcPieRotateInterval = 100;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The arc or pie rotation interval (valid values 20+ ms)."), DefaultValue(100)]
	public int ArcPieRotateInterval
	{
		get { return m_ArcPieRotateInterval; }
		set
		{
			if (value < 20)
			{
				MessageBox.Show("Keep this value above 20ms.");
				return;
			}

			if (m_ArcPieRotateInterval != value)
            {
				m_ArcPieRotateInterval = value;
				ArcPieRotateTimer.Interval = m_ArcPieRotateInterval;
				Invalidate();
			}
		}
	}

	private ArcPieRotateDir m_ArcPieRotateDir = ArcPieRotateDir.CCW;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The arc or pie rotation direction."), DefaultValue(typeof(ArcPieRotateDir), "CCW")]
	public ArcPieRotateDir ArcPieRotateDirection
	{
		get { return m_ArcPieRotateDir; }
		set
		{
			if (m_ArcPieRotateDir != value)
            {
				m_ArcPieRotateDir = value;
				Invalidate();
			}
		}
	}

	[Browsable(false)]
	public BorderStyle BorderStyle { get; set; }

	[Browsable(true)]
	public override string Text
	{
		get { return base.Text; }
		set
		{
			if (base.Text != value)
			{
				base.Text = value;
				Invalidate();
			}
		}
	}

	#endregion

	#region "Protected Methods"

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);

		e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

		OuterControlBounds = new Rectangle(new Point(0, 0), new Size(Width - 1, Height - 1));
		CircleBounds = new Rectangle(Convert.ToInt16(Properties_elo.EllipseLineWidth / 2), Convert.ToInt16(Properties_elo.EllipseLineWidth / 2), Convert.ToInt16(Width - Properties_elo.EllipseLineWidth - 1), Convert.ToInt16(Height - Properties_elo.EllipseLineWidth - 1));
		FillBounds = new Rectangle(Convert.ToInt16(Properties_elo.EllipseLineWidth + 1), Convert.ToInt16(Properties_elo.EllipseLineWidth + 1), Convert.ToInt16(Width - 2f * Properties_elo.EllipseLineWidth - 3f), Convert.ToInt16(Height - 2f * Properties_elo.EllipseLineWidth - 3f));

		//* Limit painting region so corners do not hide other close controls
		GraphicsPath CircularPath = new GraphicsPath();
		CircularPath.AddEllipse(OuterControlBounds);
		Region = new Region(CircularPath);

		BackImage = new Bitmap(Width, Height);
		using (Graphics g = Graphics.FromImage(BackImage))
		{
			g.SmoothingMode = SmoothingMode.HighQuality;


			if (!(OuterControlBounds.Width < (3f * Properties_elo.EllipseLineWidth) || OuterControlBounds.Height < (3f * Properties_elo.EllipseLineWidth)))
			{
				PathGradientBrush pgBrush = new PathGradientBrush(CircularPath);

				Color color1, color2;

				if (m_EllipseFillColorON)
					color1 = Color.FromArgb(Properties_efo.EllipseFillColor1Alpha, Properties_efo.EllipseFill_ON_Color);
				else
					color1 = Color.FromArgb(Properties_efo.EllipseFillColor1Alpha, Properties_efo.EllipseFillColor);

				color2 = Color.FromArgb(Properties_efo.EllipseFillColor2Alpha, color1);

				CircularPath.AddRectangle(CircleBounds);
				CircularPath.AddEllipse(CircleBounds);

				if (Properties_elo.EllipseLineWidth > 0)
				{
					if (m_EllipseLineColorON)
						g.DrawEllipse(new Pen(new SolidBrush(Properties_elo.EllipseLine_ON_Color), Properties_elo.EllipseLineWidth), CircleBounds);
					else
						g.DrawEllipse(new Pen(new SolidBrush(Properties_elo.EllipseLineColor), Properties_elo.EllipseLineWidth), CircleBounds);
				}

				CircularPath.AddArc(FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle);

				pgBrush.CenterColor = color1;
				pgBrush.SurroundColors = new Color[] { color2 };

				LinearGradientBrush lgBrush = new LinearGradientBrush(OuterControlBounds, color1, color2, Properties_efo.EllipseFillLinearGradientAngle, true);

				HatchBrush hBrush = new HatchBrush((HatchStyle)Properties_efo.EllipseFillHatchStyle, color1, color2);

				if (!(Properties_efo.EllipseFillColor == Color.Transparent))
				{
                    switch (Properties_efo.EllipseFillType)
                    {
						case EllipseFillTypeOption.Solid:
							if (m_EllipseFillColorON)
								g.FillEllipse(new SolidBrush(Properties_efo.EllipseFill_ON_Color), FillBounds);
							else
								g.FillEllipse(new SolidBrush(Properties_efo.EllipseFillColor), FillBounds);

							break;
						case EllipseFillTypeOption.PathGradient:
							g.FillEllipse(pgBrush, FillBounds);

							break;
						case EllipseFillTypeOption.HatchStyle:
							if (Properties_efo.EllipseFillHatchStyleBackground == HStyleBground.Glow)
							{
								g.FillEllipse(pgBrush, FillBounds);
								g.FillEllipse(hBrush, FillBounds);
							}
							else
								g.FillEllipse(hBrush, FillBounds);

							break;
                        case EllipseFillTypeOption.LinearGradient:
                            switch (Properties_efo.EllipseFillLinearGradientShape)
                            {
                                case EllipseFillLinearGradientShapeOption.Normal:
									g.FillEllipse(lgBrush, FillBounds);

									break;
                                case EllipseFillLinearGradientShapeOption.Triangular:
									lgBrush.SetBlendTriangularShape(Properties_efo.EllipseFillLinearGradientShapeFocusPoint);
									g.FillEllipse(lgBrush, FillBounds);

									break;
                                case EllipseFillLinearGradientShapeOption.SigmaBell:
									lgBrush.SetSigmaBellShape(Properties_efo.EllipseFillLinearGradientShapeFocusPoint);
									g.FillEllipse(lgBrush, FillBounds);

									break;
                            }

							break;
                    }

				}

				if (Properties_efo.EllipseFillType == EllipseFillTypeOption.Arc)
				{
					if (!(Properties_efo.ArcPieLineWidth == 0))
					{
						FillBounds.Inflate(-Convert.ToInt16(Properties_efo.ArcPieLineWidth / 2), -Convert.ToInt16(Properties_efo.ArcPieLineWidth / 2));

						Color tempColor;

						if (m_ArcPieLineColorON)
							tempColor = Properties_efo.ArcPieLine_ON_Color;
						else
							tempColor = Properties_efo.ArcPieLineColor;

						if (m_ArcPieSymmetryON)
						{
							g.DrawArc(new Pen(tempColor, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle + 180, -Properties_efo.ArcPieSweepAngle);
							g.DrawArc(new Pen(tempColor, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle);
						}
						else
							g.DrawArc(new Pen(tempColor, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle);
					}
				} else if (Properties_efo.EllipseFillType == EllipseFillTypeOption.Pie) {
					FillBounds.Inflate(-Convert.ToInt16(Properties_efo.ArcPieLineWidth / 2), -Convert.ToInt16(Properties_efo.ArcPieLineWidth / 2));

					Color temp1Color, temp2Color;

					if (m_ArcPieLineColorON)
						temp1Color = Properties_efo.ArcPieLine_ON_Color;
					else
						temp1Color = Properties_efo.ArcPieLineColor;

					if (m_PieFillColorON)
						temp2Color = Properties_efo.PieFill_ON_Color;
					else
						temp2Color = Properties_efo.PieFillColor;

					if (m_ArcPieSymmetryON)
					{
						g.FillPie(new SolidBrush(temp2Color), FillBounds, -Properties_efo.ArcPieStartAngle + 180, -Properties_efo.ArcPieSweepAngle);
						g.FillPie(new SolidBrush(temp2Color), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle);

						if (!(Properties_efo.ArcPieLineWidth == 0))
						{
							g.DrawPie(new Pen(temp1Color, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle + 180, -Properties_efo.ArcPieSweepAngle);
							g.DrawPie(new Pen(temp1Color, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle);
						}
					}
					else
					{
						g.FillPie(new SolidBrush(temp2Color), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle);

						if (!(Properties_efo.ArcPieLineWidth == 0))
							g.DrawPie(new Pen(temp1Color, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle);
					}
				}
			}
		}

		e.Graphics.DrawImage(BackImage, 0, 0);

		if (!string.IsNullOrEmpty(Text))
		{
			StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new Point(Width / 2, Height / 2), sf);
		}

	}

	#endregion

	#region "Private Methods"

	private void MyOvalShape_Resize(object sender, EventArgs e)
	{
		if (m_EllipseAspectRatioOption == EllipseAspectRatioOption.Fixed)
		{
			if (Height > Width)
				Height = Width;
			else
				Width = Height;
		}
	}

	private void ArcPieRotateTimer_Tick(object sender, EventArgs e)
	{
		if (m_ArcPieRotateDir == ArcPieRotateDir.CCW)
		{
			Properties_efo.ArcPieStartAngle += 1;

			if (Properties_efo.ArcPieStartAngle == 360)
				Properties_efo.ArcPieStartAngle = 0;
		}
		else
		{
			Properties_efo.ArcPieStartAngle -= 1;

			if (Properties_efo.ArcPieStartAngle == -360)
				Properties_efo.ArcPieStartAngle = 0;
		}
		Invalidate();
	}

	private void EllipseLineOptions_PropertyChanged(string propertyName)
	{
		Invalidate();
	}

	private void EllipseFillOptions_PropertyChanged(string propertyName)
	{
		Invalidate();
	}

	#endregion

	[Serializable(), TypeConverter(typeof(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(true)]
	public class EllipseLineOptionsProperties
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public delegate void PropertyChangedEventHandler(string propertyName);

		public EllipseLineOptionsProperties() {	}

		private Color _lineColor = Color.SteelBlue;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse line color."), DefaultValue(typeof(Color), "SteelBlue")]
		public Color EllipseLineColor
		{
			get { return _lineColor; }
			set
			{
				_lineColor = value;

                PropertyChanged?.Invoke("EllipseLineColor");
            }
		}

		private Color _lineONcolor = Color.Red;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse line ON color."), DefaultValue(typeof(Color), "Red")]
		public Color EllipseLine_ON_Color
		{
			get { return _lineONcolor; }
			set
			{
				_lineONcolor = value;

                PropertyChanged?.Invoke("EllipseLine_ON_Color");
            }
		}

		private float _lineWidth = 2f;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse line width (values 0 to 20 in increments of 2 for better concentricity)."), Editor(typeof(NumericUpDownValueEditor), typeof(UITypeEditor)), DefaultValue(2f)]
		public float EllipseLineWidth
		{
			get
			{
				NumericUpDownValueEditor.nudControl.DecimalPlaces = 0;
				NumericUpDownValueEditor.nudControl.Increment = 2;
				NumericUpDownValueEditor.nudControl.Minimum = 0;
				NumericUpDownValueEditor.nudControl.Maximum = 20;
				NumericUpDownValueEditor.valueType = "Single";
				return _lineWidth;
			}
			set
			{
				if (value < 0 || value > 20)
					value = 2f;

				_lineWidth = value;

                PropertyChanged?.Invoke("EllipseLineWidth");
            }
		}

		public override string ToString()
		{
			return string.Format("Set ellipse line options");
		}

	}

	[Serializable(), TypeConverter(typeof(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(true)]
	public class EllipseFillOptionsProperties
	{

		public event PropertyChangedEventHandler PropertyChanged;
		public delegate void PropertyChangedEventHandler(string propertyName);

		public EllipseFillOptionsProperties() {	}

		private EllipseFillTypeOption _EllipseFillType = EllipseFillTypeOption.PathGradient;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill type (PathGradient, LinearGradient, HatchStyle, Solid, Pie and Arc)."), DefaultValue(EllipseFillTypeOption.PathGradient)]
		public EllipseFillTypeOption EllipseFillType
		{
			get { return _EllipseFillType; }
			set
			{
				_EllipseFillType = value;

				if (_EllipseFillType == EllipseFillTypeOption.HatchStyle)
				{
					if (EllipseFillColor == Color.Transparent)
					{
						MessageBox.Show("The EllipseFillColor is currently set to Transparent. It will be changed to make the HatchStyle visible.");
						EllipseFillColor = Color.Yellow;
					}

					if (_hatchStyleBground == HStyleBground.Plain)
					{
						EllipseFillColor1Alpha = 255;
						EllipseFillColor2Alpha = 0;
					}
					else
					{
						EllipseFillColor1Alpha = 255;
						EllipseFillColor2Alpha = 125;
					}
				}
				else
				{
					EllipseFillColor1Alpha = 255;
					EllipseFillColor2Alpha = 125;
				}

                PropertyChanged?.Invoke("EllipseFillType");
            }
		}

		private Color _brushColor = Color.Transparent;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill color."), DefaultValue(typeof(Color), "Transparent")]
		public Color EllipseFillColor
		{
			get { return _brushColor; }
			set
			{
				_brushColor = value;

                PropertyChanged?.Invoke("EllipseFillColor");
            }
		}

		private Color _fillONcolor = Color.Red;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill ON color."), DefaultValue(typeof(Color), "Red")]
		public Color EllipseFill_ON_Color
		{
			get { return _fillONcolor; }
			set
			{
				_fillONcolor = value;

                PropertyChanged?.Invoke("EllipseFill_ON_Color");
            }
		}

		private HStyle _hatchStyle = HStyle.BackwardDiagonal;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill hatch style."), DefaultValue(typeof(HStyle), "BackwardDiagonal")]
		public HStyle EllipseFillHatchStyle
		{
			get { return _hatchStyle; }
			set
			{
				_hatchStyle = value;

                PropertyChanged?.Invoke("EllipseFillHatchStyle");
            }
		}

		private HStyleBground _hatchStyleBground = HStyleBground.Plain;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill hatch style background. When HatchStyle is enabled then selecting the Plain background will lock the Alpha values"), DefaultValue(typeof(HStyleBground), "Plain")]
		public HStyleBground EllipseFillHatchStyleBackground
		{
			get { return _hatchStyleBground; }
			set
			{
				_hatchStyleBground = value;

				if (_EllipseFillType == EllipseFillTypeOption.HatchStyle)
				{
					if (_hatchStyleBground == HStyleBground.Plain)
					{
						EllipseFillColor1Alpha = 255;
						EllipseFillColor2Alpha = 0;
					}
					else
					{
						EllipseFillColor1Alpha = 255;
						EllipseFillColor2Alpha = 125;
					}
				}

                PropertyChanged?.Invoke("EllipseFillHatchStyleBackground");
            }
		}

		private float _angle;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill linear gradient angle (valid values 0 to 180)."), Editor(typeof(NumericUpDownValueEditor), typeof(UITypeEditor)), DefaultValue(0f)]
		public float EllipseFillLinearGradientAngle
		{
			get
			{
				NumericUpDownValueEditor.nudControl.DecimalPlaces = 0;
				NumericUpDownValueEditor.nudControl.Increment = 1;
				NumericUpDownValueEditor.nudControl.Minimum = 0;
				NumericUpDownValueEditor.nudControl.Maximum = 180;
				NumericUpDownValueEditor.valueType = "Single";
				return _angle;
			}
			set
			{
				if (value < 0 || value > 180)
					value = 0f;

				_angle = value;

                PropertyChanged?.Invoke("EllipseFillLinearGradientAngle");
            }
		}

		private float _FocusPoint = 0.5f;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill linear gradient focus point for Triangular and SigmaBell shapes (valid values 0 to 1)."), Editor(typeof(NumericUpDownValueEditor), typeof(UITypeEditor)), DefaultValue(0.5f)]
		public float EllipseFillLinearGradientShapeFocusPoint
		{
			get
			{
				NumericUpDownValueEditor.nudControl.DecimalPlaces = 2;
				NumericUpDownValueEditor.nudControl.Increment = 0.01M;
				NumericUpDownValueEditor.nudControl.Minimum = 0;
				NumericUpDownValueEditor.nudControl.Maximum = 1;
				NumericUpDownValueEditor.valueType = "Single";
				return _FocusPoint;
			}
			set
			{
				if (EllipseFillLinearGradientShape == EllipseFillLinearGradientShapeOption.Normal || value < 0 || value > 1)
					value = 0f;

				_FocusPoint = value;

                PropertyChanged?.Invoke("EllipseFillLinearGradientShapeFocusPoint");
            }
		}

		private int _alpha1 = 255;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill Color1 opacity value (valid values 0 to 255)."), Editor(typeof(TrackBarValueEditor), typeof(UITypeEditor)), DefaultValue(255)]
		public int EllipseFillColor1Alpha
		{
			get { return _alpha1; }
			set
			{
				if (value < 0 || value > 255)
					value = 255;

				_alpha1 = value;

                PropertyChanged?.Invoke("EllipseFillColor1Alpha");
            }
		}

		private int _alpha2 = 125;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill Color2 opacity value (valid values 0 to 255)."), Editor(typeof(TrackBarValueEditor), typeof(UITypeEditor)), DefaultValue(125)]
		public int EllipseFillColor2Alpha
		{
			get { return _alpha2; }
			set
			{
				if (value < 0 || value > 255)
					value = 125;

				_alpha2 = value;

                PropertyChanged?.Invoke("EllipseFillColor2Alpha");
            }
		}

		private EllipseFillLinearGradientShapeOption _EllipseFillLinearGradientShape = EllipseFillLinearGradientShapeOption.Normal;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill linear gradient shape (Normal, Triangular and SigmaBell)."), DefaultValue(typeof(EllipseFillLinearGradientShapeOption), "Normal")]
		public EllipseFillLinearGradientShapeOption EllipseFillLinearGradientShape
		{
			get { return _EllipseFillLinearGradientShape; }
			set
			{
				_EllipseFillLinearGradientShape = value;

                PropertyChanged?.Invoke("EllipseFillLinearGradientShape");
            }
		}

		private float _StartAngle;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie start angle (valid values -360 to 360)."), Editor(typeof(NumericUpDownValueEditor), typeof(UITypeEditor)), DefaultValue(0f)]
		public float ArcPieStartAngle
		{
			get
			{
				NumericUpDownValueEditor.nudControl.DecimalPlaces = 0;
				NumericUpDownValueEditor.nudControl.Increment = 1;
				NumericUpDownValueEditor.nudControl.Minimum = -360;
				NumericUpDownValueEditor.nudControl.Maximum = 360;
				NumericUpDownValueEditor.valueType = "Single";
				return _StartAngle;
			}
			set
			{
				if (value < -360 || value > 360)
					value = 0f;

				_StartAngle = value;

                PropertyChanged?.Invoke("ArcPieStartAngle");
            }
		}

		private float _SweepAngle = 45f;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie sweep angle (valid values -360 to 360). For positive values, sweep is done CCW from start angle."), Editor(typeof(NumericUpDownValueEditor), typeof(UITypeEditor)), DefaultValue(45f)]
		public float ArcPieSweepAngle
		{
			get
			{
				NumericUpDownValueEditor.nudControl.DecimalPlaces = 0;
				NumericUpDownValueEditor.nudControl.Increment = 1;
				NumericUpDownValueEditor.nudControl.Minimum = -360;
				NumericUpDownValueEditor.nudControl.Maximum = 360;
				NumericUpDownValueEditor.valueType = "Single";
				return _SweepAngle;
			}
			set
			{
				if (value < -360 || value > 360)
					value = 45f;

				_SweepAngle = value;

                PropertyChanged?.Invoke("ArcPieSweepAngle");
            }
		}

		private Color _fillPieColor = Color.Yellow;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Pie color."), DefaultValue(typeof(Color), "Yellow")]
		public Color PieFillColor
		{
			get { return _fillPieColor; }
			set
			{
				_fillPieColor = value;

                PropertyChanged?.Invoke("EllipseFillPieColor");
            }
		}

		private Color _fillPieONColor = Color.Red;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Pie ON color."), DefaultValue(typeof(Color), "Red")]
		public Color PieFill_ON_Color
		{
			get { return _fillPieONColor; }
			set
			{
				_fillPieONColor = value;

                PropertyChanged?.Invoke("EllipseFillPieONColor");
            }
		}

		private Color _fillLineColor = Color.Yellow;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie line color."), DefaultValue(typeof(Color), "Yellow")]
		public Color ArcPieLineColor
		{
			get { return _fillLineColor; }
			set
			{
				_fillLineColor = value;

                PropertyChanged?.Invoke("ArcPieLineColor");
            }
		}

		private Color _fillLineONColor = Color.Red;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie line ON color."), DefaultValue(typeof(Color), "Red")]
		public Color ArcPieLine_ON_Color
		{
			get { return _fillLineONColor; }
			set
			{
				_fillLineONColor = value;

                PropertyChanged?.Invoke("ArcPieLine_ON_Color");
            }
		}

		private float _fillLineWidth = 1f;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie line width (valid values 0 to 10)."), Editor(typeof(NumericUpDownValueEditor), typeof(UITypeEditor)), DefaultValue(1f)]
		public float ArcPieLineWidth
		{
			get
			{
				NumericUpDownValueEditor.nudControl.DecimalPlaces = 0;
				NumericUpDownValueEditor.nudControl.Increment = 1;
				NumericUpDownValueEditor.nudControl.Minimum = 0;
				NumericUpDownValueEditor.nudControl.Maximum = 10;
				NumericUpDownValueEditor.valueType = "Single";
				return _fillLineWidth;
			}
			set
			{
				if (value < 0 || value > 10)
					value = 1f;

				_fillLineWidth = value;

                PropertyChanged?.Invoke("ArcPieLineWidth");
            }
		}

		public override string ToString()
		{
			return string.Format("Selected:  '" + EllipseFillType.ToString() + "'");
		}

	}

	public class NumericUpDownValueEditor : UITypeEditor
	{
		//Class references:
		//http://stackoverflow.com/questions/14291291/how-to-add-numericupdown-control-to-custom-property-grid-in-c
		//https://msdn.microsoft.com/en-CA/library/ms171840.aspx?cs-save-lang=1&cs-lang=vb#code-snippet-1
		//30-JUL-2015 Converted to VB Net By Godra and modified.

		public static NumericUpDown nudControl = new NumericUpDown();

		public static string valueType;
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService editorService = null;
			if (provider != null)
				editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

			if (editorService != null)
			{
				nudControl.Value = Convert.ToDecimal(value);
				editorService.DropDownControl(nudControl);

				if (valueType == "Single")
					value = Convert.ToSingle(nudControl.Value);
				else if (valueType == "Integer")
					value = Convert.ToInt32(nudControl.Value);
			}

			return value;
		}
	}

	public class TrackBarValueEditor : UITypeEditor
	{

		//Class references:
		//http://stackoverflow.com/questions/14291291/how-to-add-numericupdown-control-to-custom-property-grid-in-c
		//https://msdn.microsoft.com/en-CA/library/ms171840.aspx?cs-save-lang=1&cs-lang=vb#code-snippet-1
		//30-JUL-2015 Converted to VB Net By Godra and modified to use TrackBar instead of NumericUpDown control.
		//
		//This class can be formatted in the same manner as the above NumericUpDownValueEditor class
		//(in which case it would be passing TrackBar values from within the properties instead of being hardcoded).
		//It was left in this format since it was used for 2 identical integer properties only.

		private readonly TrackBar TrackBarControl = new TrackBar();
		private readonly ToolTip toolTip1 = new ToolTip();

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService editorService = null;
			if (provider != null)
				editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

			if (editorService != null)
			{
				TrackBarControl.Minimum = 0;
				TrackBarControl.Maximum = 255;
				TrackBarControl.LargeChange = 5;
				TrackBarControl.Orientation = Orientation.Horizontal;
				TrackBarControl.TickFrequency = 25;
				TrackBarControl.TickStyle = TickStyle.TopLeft;
				TrackBarControl.Value = Convert.ToInt16(value);
				editorService.DropDownControl(TrackBarControl);
				value = Convert.ToInt32(TrackBarControl.Value);
				TrackBarControl.Scroll += TrackBarControl_Scroll;
			}

			return value;
		}

		private void TrackBarControl_Scroll(object sender, EventArgs e)
		{
			toolTip1.SetToolTip(TrackBarControl, TrackBarControl.Value.ToString());
		}
	}
}
