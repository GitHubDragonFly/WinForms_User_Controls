// This is Rotating Text Button control.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

public class RotatingTextButton : Button
{
	#region "Constructor"

	public RotatingTextButton() : base()
	{
		SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.ContainerControl | ControlStyles.SupportsTransparentBackColor, true);
		base.DoubleBuffered = true;
		DoubleBuffered = true;
		base.BackColor = Color.Black;
		base.ForeColor = Color.GreenYellow;
		base.Size = new Size(120, 45);

		Colors_ctc.PropertyChanged += CustomTextColors_PropertyChanged;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing)
				Colors_ctc.PropertyChanged -= CustomTextColors_PropertyChanged;
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	#endregion

	#region "Properties"

	[Browsable(false)]
	public override string Text { get; set; }

	[Browsable(false)]
	public override ContentAlignment TextAlign { get; set; }

	private CustomTextColors Colors_ctc = new CustomTextColors();

	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), DescriptionAttribute("Expand to set custom text colors for LinearGradientBrush and PathGradientBrush.")]
	public CustomTextColors ButtonTextCustomColors
	{
		get { return Colors_ctc; }
		set
		{
			Colors_ctc = value;
			Invalidate();
		}
	}

	private string m_text = "RotatingTextButton";
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Text to show on the button."), DefaultValue("RotatingTextButton")]
	public string ButtonText
	{
		get { return m_text; }
		set
		{
			if (m_text != value)
			{
				m_text = value;
				Invalidate();
			}
		}
	}

	private bool m_btnBckGradient;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Enable button back color gradient."), DefaultValue(false)]
	public bool ButtonBackColorGradient
	{
		get { return m_btnBckGradient; }
		set
		{
			if (m_btnBckGradient != value)
			{
				m_btnBckGradient = value;
				Invalidate();
			}
		}
	}

	public enum Direction
	{
		Normal,
		VerticalLeft,
		UpsideDown,
		VerticalRight
	}

	public int m_Angle;
	public Direction m_Dir = Direction.Normal;

	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("The direction of the text."), DefaultValue(Direction.Normal)]
	public Direction ButtonTextDirection
	{
		get { return m_Dir; }
		set
		{
			if (m_Dir != value)
			{
				m_Dir = value;

				if (m_Dir == Direction.Normal || m_Dir == Direction.VerticalRight)
					m_Angle = 0;
				else
					m_Angle = 180;

				Invalidate();
			}
		}
	}

	public enum Brush
	{
		SolidBrush = 0,
		LinearGradientBrush = 1,
		PathGradientBrush = 2
	}

	private Brush m_brush = Brush.PathGradientBrush;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("The color gradient of the text."), DefaultValue(Brush.PathGradientBrush)]
	public Brush ButtonTextBrush
	{
		get { return m_brush; }
		set
		{
			if (m_brush != value)
			{
				m_brush = value;
				Invalidate();
			}
		}
	}

	private int m_alpha = 100;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Opacity value for text gradient colors (valid values 0 to 255)."), DefaultValue(100)]
	public int ButtonTextAlpha
	{
		get { return m_alpha; }
		set
		{
			if (value < 0 || value > 255)
				value = 100;

			if (m_alpha != value)
			{
				m_alpha = value;
				Invalidate();
			}
		}
	}

	private int m_lgAngle = 45;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Button text LinearGradient angle (valid values 0 to 360)."), DefaultValue(45)]
	public int ButtonTextLGAngle
	{
		get { return m_lgAngle; }
		set
		{
			if (value < 0 || value > 360)
				value = 45;

			if (m_lgAngle != value)
			{
				m_lgAngle = value;
				Invalidate();
			}
		}
	}

	private Color _Highlightcolor = Color.Green;
	public Color HighlightColor
	{
		get { return _Highlightcolor; }
		set { _Highlightcolor = value; }
	}

	private Color OriginalBackcolor;
	private bool _Highlight;
	public bool Highlight
	{
		get { return _Highlight; }
		set
		{
			if (OriginalBackcolor == null)
				OriginalBackcolor = base.BackColor;

			if (value)
				base.BackColor = _Highlightcolor;
			else
				base.BackColor = OriginalBackcolor;

			_Highlight = value;
		}
	}

	#endregion

	#region "Private Methods"

	private void CustomTextColors_PropertyChanged(string propertyName)
	{
		Invalidate();
	}

	#endregion

	#region "Protected Methods"

	protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
	{
		base.OnPaint(e);

		GraphicsPath gp = new GraphicsPath();
		gp.AddRectangle(ClientRectangle);
		PathGradientBrush pgBrush = new PathGradientBrush(gp);

		if (m_btnBckGradient)
		{
			pgBrush.CenterColor = Color.FromArgb(180, 255, 255, 255);
			pgBrush.SurroundColors = new Color[] { Color.FromArgb(0, base.BackColor) };
			e.Graphics.FillRectangle(pgBrush, ClientRectangle);
		}


		if (m_text != null && (string.Compare(m_text, "") != 0))
		{
			StringFormat format = new StringFormat();

			if (m_Dir == Direction.VerticalLeft || m_Dir == Direction.VerticalRight)
				format.FormatFlags = StringFormatFlags.DirectionVertical;

			format.LineAlignment = StringAlignment.Center;
			format.Alignment = StringAlignment.Center;

			e.Graphics.TranslateTransform(ClientRectangle.Width / 2, ClientRectangle.Height / 2);
			e.Graphics.RotateTransform(-Convert.ToSingle(m_Angle));
			e.Graphics.TranslateTransform(-ClientRectangle.Width / 2, -ClientRectangle.Height / 2);

			Color color1, color2;

			if (Colors_ctc.CustomTextColorsEnable)
			{
				color1 = Colors_ctc.CustomTextColor1;
				color2 = Colors_ctc.CustomTextColor2;
			}
			else
			{
				color1 = ForeColor;
				color2 = ForeColor;
			}

			if (m_brush == Brush.SolidBrush)
			{
				e.Graphics.DrawString(ButtonText, Font, new SolidBrush(color1), ClientRectangle, format);
				return;
			}
			else if (m_brush == Brush.LinearGradientBrush)
			{
				LinearGradientBrush lgBrush = new LinearGradientBrush(ClientRectangle, color1, Color.FromArgb(m_alpha, ControlPaint.Light(color2)), m_lgAngle);
				e.Graphics.DrawString(ButtonText, Font, lgBrush, ClientRectangle, format);
			}
			else
			{
				pgBrush.CenterColor = ControlPaint.Light(color1);
				pgBrush.SurroundColors = new Color[] { Color.FromArgb(m_alpha, ControlPaint.Light(color2)) };
				e.Graphics.DrawString(ButtonText, Font, pgBrush, ClientRectangle, format);
			}
			e.Graphics.ResetTransform();
		}

	}

	#endregion

	[Serializable(), TypeConverter(typeof(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(true)]
	public class CustomTextColors
	{

		public event PropertyChangedEventHandler PropertyChanged;
		public delegate void PropertyChangedEventHandler(string propertyName);

		public CustomTextColors()
		{
		}

		private bool m_customColorsEnable = false;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Enable custom text colors for LinearGradientBrush and PathGradientBrush."), DefaultValue(false)]
		public bool CustomTextColorsEnable
		{
			get { return m_customColorsEnable; }
			set
			{
				m_customColorsEnable = value;

				PropertyChanged?.Invoke("CustomTextColorsEnable");
			}
		}

		private Color m_color1 = Color.Turquoise;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Button text custom color1."), DefaultValue(typeof(Color), "Turquoise")]
		public Color CustomTextColor1
		{
			get { return m_color1; }
			set
			{
				m_color1 = value;

                PropertyChanged?.Invoke("CustomTextColor1");
            }
		}

		private Color m_color2 = Color.SteelBlue;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Button text custom color2."), DefaultValue(typeof(Color), "SteelBlue")]
		public Color CustomTextColor2
		{
			get { return m_color2; }
			set
			{
				m_color2 = value;

                PropertyChanged?.Invoke("CustomTextColor2");
            }
		}

		public override string ToString()
		{
			if (m_customColorsEnable)
				return string.Format("Custom Colors Enabled");
			else
				return string.Format("Custom Colors Disabled");
		}

	}

}
