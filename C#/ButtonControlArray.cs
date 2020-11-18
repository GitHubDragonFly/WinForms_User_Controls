// This control is an array of standard button controls.
// Each button action is defined within the ButtonClick sub inside the Private Methods region.
// Follow the ButtonClick sub's code pattern to add/remove as many button actions as needed.

using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

public class ButtonControlArray : Panel
{
	#region "Constructor"

	public ButtonControlArray()
	{
		SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.ContainerControl | ControlStyles.SupportsTransparentBackColor, true);
		base.DoubleBuffered = true;
		DoubleBuffered = true;
		BackColor = Color.DarkBlue;
		ForeColor = Color.Black;
		base.AutoSize = true;
		base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		BorderStyle = BorderStyle.FixedSingle;

		for (int i = 1; i <= m_ButtonNumber; i++)
		{
			AddNewButton(i);
		}
	}

	#endregion

	#region "Properties"

	[Browsable(false)]
	public override bool AutoSize
	{
		get { return base.AutoSize; }
		set
		{
			if (base.AutoSize != value)
				base.AutoSize = value;
		}
	}

	[Browsable(false)]
	public override AutoSizeMode AutoSizeMode
	{
		get { return base.AutoSizeMode; }
		set
		{
			if (base.AutoSizeMode != value)
				base.AutoSizeMode = value;
		}
	}

	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("The foreground color of the text."), DefaultValue(typeof(Color), "Black")]
	public override Color ForeColor
	{
		get { return base.ForeColor; }
		set
		{
			if (base.ForeColor != value)
			{
				base.ForeColor = value;
				if (Controls.Count > 0)
				{
					for (int i = 0; i <= Controls.Count; i++)
					{
						((Button)Controls[i]).ForeColor = base.ForeColor;
					}
				}
				Invalidate();
			}
		}
	}

	public enum CtrlOrientation
	{
		Horizontal = 0,
		Vertical = 1
	}

	private CtrlOrientation m_ctrlOrientation = CtrlOrientation.Horizontal;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Orientation of the buttons within the control."), DefaultValue(CtrlOrientation.Horizontal)]
	public CtrlOrientation ButtonOrientation
	{
		get { return m_ctrlOrientation; }
		set
		{
			if (m_ctrlOrientation != value)
			{
				m_ctrlOrientation = value;
				currentButtonNum = m_ButtonNumber;
				AddRemoveAll();
				Invalidate();
			}
		}
	}

	private int currentButtonNum = 6;
	private int m_ButtonNumber = 6;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Number of buttons in the array (valid values 2 to 60)."), DefaultValue(6)]
	public int ButtonNumber
	{
		get { return m_ButtonNumber; }
		set
		{
			if (value < 2)
				value = 2;
			if (value > 60)
				value = 60;

			if (m_ButtonNumber != value)
			{
				currentButtonNum = m_ButtonNumber;
				m_ButtonNumber = value;
				AddRemoveAll();
				Invalidate();
			}
		}
	}

	private int m_ButtonWidth = 38;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Single button width (valid values 24 to 300)."), DefaultValue(38)]
	public int ButtonWidth
	{
		get { return m_ButtonWidth; }
		set
		{
			if (value < 24)
				value = 24;
			if (value > 300)
				value = 300;

			if (m_ButtonWidth != value)
			{
				m_ButtonWidth = value;
				currentButtonNum = m_ButtonNumber;
				AddRemoveAll();
				Invalidate();
			}
		}
	}

	private int m_ButtonHeight = 28;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Single button height (valid values 24 to 300)."), DefaultValue(28)]
	public int ButtonHeight
	{
		get { return m_ButtonHeight; }
		set
		{
			if (value < 24)
				value = 24;
			if (value > 300)
				value = 300;
			if (m_ButtonHeight != value)
			{
				m_ButtonHeight = value;
				currentButtonNum = m_ButtonNumber;
				AddRemoveAll();
				Invalidate();
			}
		}
	}

	private int m_ButtonSpacing = 2;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Spacing between individual buttons (valid values 2 to 10)."), DefaultValue(2)]
	public int ButtonSpacing
	{
		get { return m_ButtonSpacing; }
		set
		{
			if (value < 2)
				value = 2;
			if (value > 10)
				value = 10;

			if (m_ButtonSpacing != value)
			{
				m_ButtonSpacing = value;
				currentButtonNum = m_ButtonNumber;
				AddRemoveAll();
				Invalidate();
			}
		}
	}

	private string m_ButtonText = "B";
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Text to show on all buttons, followed by the button number"), DefaultValue("B")]
	public string ButtonText
	{
		get { return m_ButtonText; }
		set
		{
			if (m_ButtonText != value)
			{
				m_ButtonText = value;
				currentButtonNum = m_ButtonNumber;
				AddRemoveAll();
				Invalidate();
			}
		}
	}

	private Color m_ButtonBackColor = Color.Lime;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Use this color as the BackColor for all buttons (UseVisualStyleBackColor property must be set to False)."), DefaultValue(typeof(Color), "Lime")]
	public Color ButtonBackColor
	{
		get { return m_ButtonBackColor; }
		set
		{
			if (m_ButtonBackColor != value)
			{
				m_ButtonBackColor = value;
				currentButtonNum = m_ButtonNumber;
				AddRemoveAll();
				Invalidate();
			}
		}
	}

	private bool m_ButtonUseVisualStyleBackColor = true;
	[Browsable(true), RefreshProperties(RefreshProperties.All), Description("Use Visual Style Back Color for the buttons."), DefaultValue(true)]
	public bool ButtonUseVisualStyleBackColor
	{
		get { return m_ButtonUseVisualStyleBackColor; }
		set
		{
			if (m_ButtonUseVisualStyleBackColor != value)
			{
				m_ButtonUseVisualStyleBackColor = value;
				currentButtonNum = m_ButtonNumber;
				AddRemoveAll();
				Invalidate();
			}
		}
	}

	#endregion

	#region "Private Methods"

	private void AddNewButton(int i)
	{
		Button newArrayButton = new Button();
		Controls.Add(newArrayButton);
		newArrayButton.Width = m_ButtonWidth;
		newArrayButton.Height = m_ButtonHeight;

		if (m_ctrlOrientation == CtrlOrientation.Horizontal)
		{
			newArrayButton.Top = 4;
			newArrayButton.Left = (Controls.Count - 1) * (m_ButtonWidth + m_ButtonSpacing) + 4;
		}
		else
		{
			newArrayButton.Top = (Controls.Count - 1) * (m_ButtonHeight + m_ButtonSpacing) + 4;
			newArrayButton.Left = 4;
		}

		newArrayButton.BackColor = m_ButtonBackColor;
		newArrayButton.ForeColor = ForeColor;
		newArrayButton.Text = m_ButtonText + i.ToString();
		newArrayButton.UseVisualStyleBackColor = m_ButtonUseVisualStyleBackColor;
		newArrayButton.TextAlign = ContentAlignment.MiddleCenter;
		newArrayButton.Click += ButtonClick; //Other event handlers can be added in the same manner
	}

	//This sub will need to be modified to suit user's needs.
	//Different actions can be performed with a click of any of the buttons.
	//More or less buttons can be defined.
	private void ButtonClick(System.Object sender, System.EventArgs e)
	{
		if ((sender.GetType().GetProperty("Text").GetValue(sender, null)).ToString() == (m_ButtonText + "1"))
		{
			MessageBox.Show("Button pressed - " + m_ButtonText + "1"); //<-- Replace with your own action
		}
		else if ((sender.GetType().GetProperty("Text").GetValue(sender, null)).ToString() == (m_ButtonText + "2"))
		{
			MessageBox.Show("Button pressed - " + m_ButtonText + "2"); //<-- Replace with your own action
		}
		else if ((sender.GetType().GetProperty("Text").GetValue(sender, null)).ToString() == (m_ButtonText + "3"))
		{
			MessageBox.Show("Button pressed - " + m_ButtonText + "3"); //<-- Replace with your own action
		}
		else if ((sender.GetType().GetProperty("Text").GetValue(sender, null)).ToString() == (m_ButtonText + "4"))
		{
			MessageBox.Show("Button pressed - " + m_ButtonText + "4"); //<-- Replace with your own action
		}
		else if ((sender.GetType().GetProperty("Text").GetValue(sender, null)).ToString() == (m_ButtonText + "5"))
		{
			MessageBox.Show("Button pressed - " + m_ButtonText + "5"); //<-- Replace with your own action
		}
		else if ((sender.GetType().GetProperty("Text").GetValue(sender, null)).ToString() == (m_ButtonText + "6"))
		{
			MessageBox.Show("Button pressed - " + m_ButtonText + "6"); //<-- Replace with your own action
		}
	}

	private void Remove()
	{
		Controls.Remove((Button)Controls[Controls.Count - 1]);
	}

	private void AddRemoveAll()
	{
		for (int j = 1; j <= currentButtonNum; j++)
		{
			Remove();
		}

		for (int j = 1; j <= m_ButtonNumber; j++)
		{
			AddNewButton(j);
		}
	}

	#endregion

}
