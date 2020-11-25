//* 06-FEB-16  v1.1
//*
//* This is a rotational position indicator control which can be used as weather vane as well.
//*
//* It features full circle positional arrow (360 degrees) whose home/zero position can be selected as N, E, W or S.
//*
//* The "Value" property is defined as double-precision floating point input and reflects the angle of the arrow.
//* The angle is in degrees and can be positive or negative, measured clockwise/counter-clockwise from zero position.
//* Any received value over 360 or below -360 degrees can be shown as value within -360 to 360 degrees range, calculated as:
//*
//*                             multiplier * 360 + remainder
//*
//* Multiplier is positive for positive angles and negative for negative angles.
//*
//* Example 1: received value is 450 degrees which corresponds to 90 degrees (1 * 360 + 90 = 450)
//* Example 2: received value is -725 degrees which corresponds to -5 degrees (-2 * 360 + (-5) = -725)
//*
//* The corresponding -360 to 360 degrees range angle value will always show on the control itself.
//* Optional suffix text can be shown after the degree value (suffix = N, NE, E, SE, S, SW, W or NW).
//*
//* If needed, the "Value" property can be used to show the actual received angle value.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

public class RotationalPositionIndicator : Control
{
	#region "Constructor"

	public RotationalPositionIndicator() : base()
	{
		Resize += RotationalPositionIndicator_Resize;

		SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.ContainerControl | ControlStyles.SupportsTransparentBackColor, true);
		base.DoubleBuffered = true;
		DoubleBuffered = true;
		base.BackColor = Color.Transparent;
		base.ForeColor = Color.Black;
		Size = new Size(160, 160);
		MinimumSize = new Size(60, 60);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing)
			{
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	#endregion

	#region "Properties"

	private Color m_arrowColor = Color.LawnGreen;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("The arrow color."), DefaultValue(typeof(Color), "LawnGreen")]
	public Color RPI_ArrowColor
	{
		get { return m_arrowColor; }
		set
		{
			if (m_arrowColor != value)
			{
				m_arrowColor = value;
				Invalidate();
			}
		}
	}

	private Color m_circleColor = Color.Blue;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("The background circle color."), DefaultValue(typeof(Color), "Blue")]
	public Color RPI_CircleColor
	{
		get { return m_circleColor; }
		set
		{
			if (m_circleColor != value)
			{
				m_circleColor = value;
				Invalidate();
			}
		}
	}

	private Color m_zeroLineColor = Color.Red;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("The arrow zero/home line color."), DefaultValue(typeof(Color), "Red")]
	public Color RPI_ZeroLineColor
	{
		get { return m_zeroLineColor; }
		set
		{
			if (m_zeroLineColor != value)
			{
				m_zeroLineColor = value;
				Invalidate();
			}
		}
	}

	public enum Zero
	{
		E = 0,
		N = 90,
		W = 180,
		S = 270
	}

	private Zero m_zeroPosition = Zero.E;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Indicates the arrow zero/home position (line on the background circle). If the control is used as Weather Vane then this line will always reflect the North."), DefaultValue(Zero.E)]
	public Zero RPI_ZeroLinePosition
	{
		get { return m_zeroPosition; }
		set
		{
			if (m_zeroPosition != value)
			{
				m_zeroPosition = value;
				Value = m_Value;
			}
		}
	}

	private bool m_zeroLineShow = true;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Show the zero/home line."), DefaultValue(true)]
	public bool RPI_ZeroLineShow
	{
		get { return m_zeroLineShow; }
		set
		{
			if (m_zeroLineShow != value)
			{
				m_zeroLineShow = value;
				Invalidate();
			}
		}
	}

	private bool m_suffixShow;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Indicates whether to dispaly suffix text after the degrees value (suffix = N, NE, E, SE, S, SW, W or NW)."), DefaultValue(false)]
	public bool RPI_ShowSuffix
	{
		get { return m_suffixShow; }
		set
		{
			if (m_suffixShow != value)
			{
				m_suffixShow = value;
				Value = m_Value;
				Invalidate();
			}
		}
	}

	private string m_string = "0.0" + "°";
	private string m_suffix = "";
	private double m_Value = 0f;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Indicates the actual received arrow angle value in degrees. It could be any double-precision floating point value."), DefaultValue(0f)]
	public double Value
	{
		get { return m_Value; }
		set
		{
			m_Value = value;
			value += (double)m_zeroPosition;

			decimal modValue = Math.Abs((decimal)value % 360M);

			if ((modValue >= 337.5M && modValue <= 360M) || (modValue >= 0 && modValue < 22.5M))
				m_suffix = " E";
			else if (modValue >= 22.5M && modValue < 67.5M)
            {
				if (value < 0)
					m_suffix = " SE";
				else
					m_suffix = " NE";
			}
			else if (modValue >= 67.5M && modValue < 112.5M)
            {
				if (value < 0)
					m_suffix = " S";
				else
					m_suffix = " N";
			}
			else if (modValue >= 112.5M && modValue < 157.5M)
            {
				if (value < 0)
					m_suffix = " SW";
				else
					m_suffix = " NW";
			}
			else if (modValue >= 157.5M && modValue < 202.5M)
            {
				m_suffix = " W";
			}
			else if (modValue >= 202.5M && modValue < 247.5M)
            {
				if (value < 0)
					m_suffix = " NW";
				else
					m_suffix = " SW";
			}
			else if (modValue >= 247.5M && modValue < 292.5M)
            {
				if (value < 0)
					m_suffix = " N";
				else
					m_suffix = " S";
			}
            else
            {
				if (value < 0)
					m_suffix = " NE";
				else
					m_suffix = " SE";
			}

			if (m_suffixShow)
				m_string = (Convert.ToDecimal(m_Value) % Convert.ToDecimal(360f)).ToString("0.0") + "°" + m_suffix;
			else
				m_string = (Convert.ToDecimal(m_Value) % Convert.ToDecimal(360f)).ToString("0.0") + "°";

			Invalidate();
		}
	}

	public override string Text
	{
		get { return base.Text; }
		set
		{
			if (string.Compare(base.Text, value) != 0)
			{
				base.Text = value;
				Invalidate();
			}
		}
	}

	#endregion

	#region "Protected Metods"

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);

		RectangleF rect = new RectangleF(new PointF(1f, 1f), new SizeF(Width - 2, Height - 2));
		RectangleF rect2 = new RectangleF(new PointF(4f, 4f), new SizeF(Width - 8, Height - 8));
		RectangleF rect3 = new RectangleF(new PointF(Width / 2f - Height * 0.3f / 7f, Height * 3.1f / 7f), new SizeF(Height * 0.8f / 7f, Height * 0.8f / 7f));

		e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
		e.Graphics.FillEllipse(new SolidBrush(ControlPaint.Light(m_circleColor)), rect);
		e.Graphics.FillEllipse(new SolidBrush(m_circleColor), rect2);

		if (m_zeroLineShow)
		{
			switch (RPI_ZeroLinePosition)
			{
				case Zero.N:
					e.Graphics.DrawLine(new Pen(m_zeroLineColor), new Point(Width / 2, 1), new Point(Width / 2, Height / 2));
					break;
				case Zero.E:
					e.Graphics.DrawLine(new Pen(m_zeroLineColor), new Point((int)(Width - 2f), Height / 2), new Point(Width / 2, Height / 2));
					break;
				case Zero.W:
					e.Graphics.DrawLine(new Pen(m_zeroLineColor), new Point(2, Width / 2), new Point(Width / 2, Height / 2));
					break;
				default: //Zero.S
					e.Graphics.DrawLine(new Pen(m_zeroLineColor), new Point(Width / 2, (int)(Height - 2f)), new Point(Width / 2, Height / 2));
					break;
			}
		}

		e.Graphics.TranslateTransform(ClientRectangle.Width / 2f, ClientRectangle.Height / 2f);
		e.Graphics.RotateTransform(-(Convert.ToSingle(m_Value) + Convert.ToSingle(m_zeroPosition)));
		e.Graphics.TranslateTransform(-ClientRectangle.Width / 2f, -ClientRectangle.Height / 2f);

		PointF[] points = new PointF[] {
			new PointF(Width / 2f, Height * 3.1f / 7f),
			new PointF(Width * 5.25f / 7f, Height * 3.1f / 7f),
			new PointF(Width * 5.25f / 7f, Height * 6f / 16f),
			new PointF(Width - 4f, Height * 3.5f / 7f),
			new PointF(Width * 5.25f / 7f, Height * 10f / 16f),
			new PointF(Width * 5.25f / 7f, Height * 3.9f / 7f),
			new PointF(Width / 2f, Height * 3.9f / 7f)
		};

		GraphicsPath gp = new GraphicsPath();
		gp.AddPolygon(points);

		Blend lgBlend = new Blend(11)
		{
			Positions = new float[] { 0f, 0.1f,	0.2f, 0.3f,	0.4f, 0.5f,	0.6f, 0.7f,	0.8f, 0.9f,	1f },
			Factors = new float[] {	0f,	0.1f, 0.2f,	0.3f, 0.4f,	0.5f, 0.4f,	0.3f, 0.2f, 0.1f, 0f }
		};

		using (LinearGradientBrush lgBrush = new LinearGradientBrush(new Point(Width / 2, Height / 3), new Point(Width / 2, 2 * Height / 3), m_arrowColor, ControlPaint.Dark(m_arrowColor)))
		{
			lgBrush.Blend = lgBlend;
			e.Graphics.FillEllipse(lgBrush, rect3);
			e.Graphics.FillPolygon(lgBrush, points);
		}

		e.Graphics.ResetTransform();

		StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(m_string, Font, new SolidBrush(ForeColor), new Point(Width / 2, Height * 2 / 3), sf);

		if (!string.IsNullOrEmpty(Text))
			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new Point(Width / 2, Height / 2), sf);
		else
			e.Graphics.FillEllipse(new SolidBrush(ControlPaint.DarkDark(m_circleColor)), Convert.ToSingle(Width / 2 - 1f), Convert.ToSingle(Height / 2 - 1f), 2f, 2f);

	}

	#endregion

	#region "Private Methods"

	private void RotationalPositionIndicator_Resize(object sender, EventArgs e)
	{
		Width = Height;
	}

	#endregion
}
