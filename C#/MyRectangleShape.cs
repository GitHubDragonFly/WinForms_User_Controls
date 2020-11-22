//****************************************************************************
//* MyRectangleShape Control v1.0
//****************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

public class MyRectangleShape : Control
{
	#region "Constructor"

	public MyRectangleShape() : base()
	{
		SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.ContainerControl | ControlStyles.SupportsTransparentBackColor, true);
		base.DoubleBuffered = true;
		DoubleBuffered = true;
		BackColor = Color.Transparent;
		ForeColor = Color.DarkBlue;
		Size = new Size(200, 200);
		MinimumSize = new Size(65, 65);

		Resize += MyRectangleShape_Resize;
		Properties_rlo.PropertyChanged += RectangleLineOptions_PropertyChanged;
		Properties_rfo.PropertyChanged += RectangleFillOptions_PropertyChanged;
	}

	protected override void Dispose(bool disposing)
	{
		Resize -= MyRectangleShape_Resize;
		Properties_rlo.PropertyChanged -= RectangleLineOptions_PropertyChanged;
		Properties_rfo.PropertyChanged -= RectangleFillOptions_PropertyChanged;

		base.Dispose(disposing);
	}

	#endregion

	#region "Properties"

	public enum RectangleAspectRatioOption
	{
		Fixed,
		Free
	}

	private RectangleAspectRatioOption m_RectangleAspectRatioOption = RectangleAspectRatioOption.Fixed;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), Description("The rectangle aspect ratio."), DefaultValue(typeof(RectangleAspectRatioOption), "Fixed")]
	public RectangleAspectRatioOption RectangleAspectRatio
	{
		get { return m_RectangleAspectRatioOption; }
		set
		{
			if (m_RectangleAspectRatioOption != value)
            {
				m_RectangleAspectRatioOption = value;
				MyRectangleShape_Resize(this, EventArgs.Empty);
				Invalidate();
			}
		}
	}

	private RectangleLineOptionsProperties Properties_rlo = new RectangleLineOptionsProperties();
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), DescriptionAttribute("Expand to set the Rectangle line options.")]
	public RectangleLineOptionsProperties RectangleLineOptions
	{
		get { return Properties_rlo; }
		set
		{
			if (Properties_rlo != value)
            {
				Properties_rlo = value;
				Invalidate();
			}
		}
	}

	private RectangleFillOptionsProperties Properties_rfo = new RectangleFillOptionsProperties();
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), DescriptionAttribute("Expand to set the rectangle fill options.")]
	public RectangleFillOptionsProperties RectangleFillOptions
	{
		get { return Properties_rfo; }
		set
		{
			if (Properties_rfo != value)
            {
				Properties_rfo = value;
				Invalidate();
			}
		}
	}

	private bool m_RectangleLineColorON;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the rectangle line color ON."), DefaultValue(false)]
	public bool RectangleLineColorON
	{
		get { return m_RectangleLineColorON; }
		set
		{
			if (m_RectangleLineColorON != value)
            {
				m_RectangleLineColorON = value;
				Invalidate();
			}
		}
	}

	private bool m_RectangleFillColorON;
	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the rectangle fill color ON. The RectangleFillColor has to be set to other than Transparent."), DefaultValue(false)]
	public bool RectangleFillColorON
	{
		get { return m_RectangleFillColorON; }
		set
		{
			if (m_RectangleFillColorON != value)
            {
				m_RectangleFillColorON = value;
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

		Rectangle rect1 = new Rectangle(Convert.ToInt16(Properties_rlo.RectangleLineWidth / 2f + 2f), Convert.ToInt16(Properties_rlo.RectangleLineWidth / 2f + 2f), Convert.ToInt16(Convert.ToSingle(Width) - Properties_rlo.RectangleLineWidth - 5f), Convert.ToInt16(Convert.ToSingle(Height) - Properties_rlo.RectangleLineWidth - 5f));
		Rectangle rect2 = new Rectangle(Convert.ToInt16(Properties_rlo.RectangleLineWidth + 3.51f), Convert.ToInt16(Properties_rlo.RectangleLineWidth + 3.51f), Convert.ToInt16(Convert.ToSingle(Width) - 2f * Properties_rlo.RectangleLineWidth - 9f), Convert.ToInt16(Convert.ToSingle(Height) - 2f * Properties_rlo.RectangleLineWidth - 9f));

		if (!(rect2.Width < 1 || rect2.Height < 1))
		{
			Color color1, color2;

			if (m_RectangleFillColorON)
				color1 = Color.FromArgb(Properties_rfo.RectangleFillColor1Alpha, Properties_rfo.RectangleFill_ON_Color);
			else
				color1 = Color.FromArgb(Properties_rfo.RectangleFillColor1Alpha, Properties_rfo.RectangleFillColor);
			
			color2 = Color.FromArgb(Properties_rfo.RectangleFillColor2Alpha, color1);

			GraphicsPath gp = new GraphicsPath();
			gp.AddRectangle(rect2);

			PathGradientBrush pgBrush = new PathGradientBrush(gp) { CenterColor = color1, SurroundColors = new Color[] { color2 } };

			LinearGradientBrush lgBrush = new LinearGradientBrush(rect2, color1, color2, Properties_rfo.RectangleFillLinearGradientAngle, true);

			HatchBrush hBrush = new HatchBrush((HatchStyle)Properties_rfo.RectangleFillHatchStyle, color1, color2);

			if (!(Properties_rfo.RectangleFillColor == Color.Transparent))
			{
                switch (Properties_rfo.RectangleFillType)
                {
					case RectangleFillOptionsProperties.RectangleFillTypeOption.Solid:
						e.Graphics.FillRectangle(new SolidBrush(Properties_rfo.RectangleFillColor), rect2);

						break;
					case RectangleFillOptionsProperties.RectangleFillTypeOption.PathGradient:
						e.Graphics.FillRectangle(pgBrush, rect2);

						break;
					case RectangleFillOptionsProperties.RectangleFillTypeOption.HatchStyle:
						if (Properties_rfo.RectangleFillHatchStyleBackground == RectangleFillOptionsProperties.HStyleBground.Glow)
						{
							e.Graphics.FillRectangle(pgBrush, rect2);
							e.Graphics.FillRectangle(hBrush, rect2);
						}
						else
						{
							e.Graphics.FillRectangle(new SolidBrush(BackColor), rect2);
							e.Graphics.FillRectangle(hBrush, rect2);
						}

						break;
                    case RectangleFillOptionsProperties.RectangleFillTypeOption.LinearGradient:
                        switch (Properties_rfo.RectangleFillLinearGradientShape)
                        {
                            case RectangleFillOptionsProperties.RectangleFillLinearGradientShapeOption.Normal:
								e.Graphics.FillRectangle(lgBrush, rect2);

								break;
                            case RectangleFillOptionsProperties.RectangleFillLinearGradientShapeOption.Triangular:
								lgBrush.SetBlendTriangularShape(Properties_rfo.RectangleFillLinearGradientShapeFocusPoint);
								e.Graphics.FillRectangle(lgBrush, rect2);

								break;
                            case RectangleFillOptionsProperties.RectangleFillLinearGradientShapeOption.SigmaBell:
								lgBrush.SetSigmaBellShape(Properties_rfo.RectangleFillLinearGradientShapeFocusPoint);
								e.Graphics.FillRectangle(lgBrush, rect2);

								break;
                        }

						break;
                }
			}
			else
				e.Graphics.FillRectangle(new SolidBrush(Properties_rfo.RectangleFillColor), rect2);
		}

		if (Properties_rlo.RectangleLineWidth > 0)
		{
			if (m_RectangleLineColorON)
				e.Graphics.DrawRectangle(new Pen(Properties_rlo.RectangleLine_ON_Color, Properties_rlo.RectangleLineWidth), rect1);
			else
				e.Graphics.DrawRectangle(new Pen(Properties_rlo.RectangleLineColor, Properties_rlo.RectangleLineWidth), rect1);
		}

		if (!string.IsNullOrEmpty(Text))
		{
			StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new Point(Width / 2, Height / 2), sf);
		}
	}

	#endregion

	#region "Private Methods"

	private void MyRectangleShape_Resize(object sender, EventArgs e)
	{
		if (m_RectangleAspectRatioOption == RectangleAspectRatioOption.Fixed)
		{
			if (Height > Width)
				Height = Width;
			else
				Width = Height;
		}
	}

	private void RectangleLineOptions_PropertyChanged(string propertyName)
	{
		Invalidate();
	}

	private void RectangleFillOptions_PropertyChanged(string propertyName)
	{
		Invalidate();
	}

	#endregion

	[Serializable(), TypeConverter(typeof(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(true)]
	public class RectangleLineOptionsProperties
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public delegate void PropertyChangedEventHandler(string propertyName);

		public RectangleLineOptionsProperties() { }

		private Color _lineColor = Color.SteelBlue;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Rectangle line color."), DefaultValue(typeof(Color), "SteelBlue")]
		public Color RectangleLineColor
		{
			get { return _lineColor; }
			set
			{
				_lineColor = value;

                PropertyChanged?.Invoke("RectangleLineColor");
            }
		}

		private Color _lineONcolor = Color.Red;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Rectangle line ON color."), DefaultValue(typeof(Color), "Red")]
		public Color RectangleLine_ON_Color
		{
			get { return _lineONcolor; }
			set
			{
				_lineONcolor = value;

                PropertyChanged?.Invoke("RectangleLine_ON_Color");
            }
		}

		private float _lineWidth = 2f;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Rectangle line width (valid values 0 to 20)."), Editor(typeof(NumericUpDownValueEditor), typeof(UITypeEditor)), DefaultValue(2f)]
		public float RectangleLineWidth
		{
			get
			{
				NumericUpDownValueEditor.nudControl.DecimalPlaces = 0;
				NumericUpDownValueEditor.nudControl.Increment = 1;
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

                PropertyChanged?.Invoke("RectangleLineWidth");
            }
		}

		public override string ToString()
		{
			return string.Format("Set Rectangle line options");
		}

	}

	[Serializable(), TypeConverter(typeof(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(true)]
	public class RectangleFillOptionsProperties
	{

		public event PropertyChangedEventHandler PropertyChanged;
		public delegate void PropertyChangedEventHandler(string propertyName);

		public RectangleFillOptionsProperties() { }

		private Color _brushColor = Color.Transparent;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill color."), DefaultValue(typeof(Color), "Transparent")]
		public Color RectangleFillColor
		{
			get { return _brushColor; }
			set
			{
				_brushColor = value;

                PropertyChanged?.Invoke("RectangleFillColor");
            }
		}

		private Color _fillONcolor = Color.Red;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill ON color."), DefaultValue(typeof(Color), "Red")]
		public Color RectangleFill_ON_Color
		{
			get { return _fillONcolor; }
			set
			{
				_fillONcolor = value;

                PropertyChanged?.Invoke("RectangleFill_ON_Color");
            }
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

		private HStyle _hatchStyle = HStyle.BackwardDiagonal;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill hatch style."), DefaultValue(HStyle.BackwardDiagonal)]
		public HStyle RectangleFillHatchStyle
		{
			get { return _hatchStyle; }
			set
			{
				_hatchStyle = value;

                PropertyChanged?.Invoke("RectangleFillHatchStyle");
            }
		}

		public enum HStyleBground
		{
			Plain,
			Glow
		}

		private HStyleBground _hatchStyleBground = HStyleBground.Plain;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill hatch style background. When HatchStyle is enabled then selecting the Plain background will lock the Alpha values"), DefaultValue(HStyleBground.Plain)]
		public HStyleBground RectangleFillHatchStyleBackground
		{
			get { return _hatchStyleBground; }
			set
			{
				_hatchStyleBground = value;

				if (_hatchStyleBground == HStyleBground.Plain)
				{
					RectangleFillColor1Alpha = 255;
					RectangleFillColor2Alpha = 0;
				}

                PropertyChanged?.Invoke("RectangleFillHatchStyleBackground");
            }
		}

		private float _angle = 0f;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill linear gradient angle (valid values 0 to 180)."), Editor(typeof(NumericUpDownValueEditor), typeof(UITypeEditor)), DefaultValue(0f)]
		public float RectangleFillLinearGradientAngle
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

                PropertyChanged?.Invoke("RectangleFillLinearGradientAngle");
            }
		}

		private float _FocusPoint = 0f;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill linear gradient focus point for Triangular and SigmaBell shapes (valid values 0 to 1)."), Editor(typeof(NumericUpDownValueEditor), typeof(UITypeEditor)), DefaultValue(0f)]
		public float RectangleFillLinearGradientShapeFocusPoint
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
				if (RectangleFillLinearGradientShape == RectangleFillLinearGradientShapeOption.Normal || value < 0 || value > 1)
					value = 0f;

				_FocusPoint = value;

                PropertyChanged?.Invoke("RectangleFillLinearGradientShapeFocusPoint");
            }
		}

		private int _alpha1 = 255;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill Color1 opacity value (valid values 0 to 255)."), Editor(typeof(TrackBarValueEditor), typeof(UITypeEditor)), DefaultValue(255)]
		public int RectangleFillColor1Alpha
		{
			get { return _alpha1; }
			set
			{
				if (value < 0 || value > 255)
					value = 255;

				if (_RectangleFillType == RectangleFillTypeOption.HatchStyle)
				{
					if (RectangleFillHatchStyleBackground == HStyleBground.Plain)
						_alpha1 = 255;
					else
						_alpha1 = value;
				}
				else
					_alpha1 = value;

                PropertyChanged?.Invoke("RectangleFillColor1Alpha");
            }
		}

		private int _alpha2 = 125;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill Color2 opacity value (valid values 0 to 255)."), Editor(typeof(TrackBarValueEditor), typeof(UITypeEditor)), DefaultValue(125)]
		public int RectangleFillColor2Alpha
		{
			get { return _alpha2; }
			set
			{
				if (value < 0 || value > 255)
					value = 125;

				if (_RectangleFillType == RectangleFillTypeOption.HatchStyle)
				{
					if (RectangleFillHatchStyleBackground == HStyleBground.Plain)
						_alpha2 = 0;
					else
						_alpha2 = value;
				}
				else
					_alpha2 = value;

                PropertyChanged?.Invoke("RectangleFillColor2Alpha");
            }
		}

		public enum RectangleFillTypeOption
		{
			HatchStyle,
			LinearGradient,
			PathGradient,
			Solid
		}

		private RectangleFillTypeOption _RectangleFillType = RectangleFillTypeOption.PathGradient;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill color type (PathGradient, LinearGradient, HatchStyle and Solid)."), DefaultValue(RectangleFillTypeOption.PathGradient)]
		public RectangleFillTypeOption RectangleFillType
		{
			get { return _RectangleFillType; }
			set
			{
				_RectangleFillType = value;
				if (_RectangleFillType == RectangleFillTypeOption.HatchStyle)
				{
					if (RectangleFillColor == Color.Transparent)
					{
						MessageBox.Show("The RectangleFillColor is currently set to Transparent. It will be changed to make the HatchStyle visible.");
						RectangleFillColor = Color.Yellow;
					}
					if (_hatchStyleBground == HStyleBground.Plain)
					{
						RectangleFillColor1Alpha = 255;
						RectangleFillColor2Alpha = 0;
					}
					else
					{
						RectangleFillColor1Alpha = 255;
						RectangleFillColor2Alpha = 125;
					}
				}
				else
				{
					RectangleFillColor1Alpha = 255;
					RectangleFillColor2Alpha = 125;
				}

                PropertyChanged?.Invoke("RectangleFillType");
            }
		}

		public enum RectangleFillLinearGradientShapeOption
		{
			Normal,
			Triangular,
			SigmaBell
		}

		private RectangleFillLinearGradientShapeOption _RectangleFillLinearGradientShape = RectangleFillLinearGradientShapeOption.Normal;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill linear gradient shape (Normal, Triangular and SigmaBell)."), DefaultValue(RectangleFillLinearGradientShapeOption.Normal)]
		public RectangleFillLinearGradientShapeOption RectangleFillLinearGradientShape
		{
			get { return _RectangleFillLinearGradientShape; }
			set
			{
				_RectangleFillLinearGradientShape = value;

                PropertyChanged?.Invoke("RectangleFillLinearGradientShape");
            }
		}

		public override string ToString()
		{
			return string.Format("Selected:  '" + RectangleFillType.ToString() + "'");
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

