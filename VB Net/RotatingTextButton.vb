' This is Rotating Text Button control.

Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Public Class RotatingTextButton
    Inherits Button

#Region "Properties"

    <Browsable(False)>
    Public Overrides Property Text As String = ""

    <Browsable(False)>
    Public Overrides Property TextAlign As ContentAlignment = ContentAlignment.MiddleCenter

    Private WithEvents Colors_ctc As New CustomTextColors()
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), DescriptionAttribute("Expand to set custom text colors for LinearGradientBrush and PathGradientBrush.")>
    Public Property ButtonTextCustomColors() As CustomTextColors
        Get
            Return Colors_ctc
        End Get
        Set(ByVal value As CustomTextColors)
            Colors_ctc = value
            Invalidate()
        End Set
    End Property

    Private m_text As String = "RotatingTextButton"
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Text to show on the button."), DefaultValue("RotatingTextButton")>
    Public Property ButtonText As String
        Get
            Return m_text
        End Get
        Set(ByVal value As String)
            If m_text <> value Then
                m_text = value
            End If
            Invalidate()
        End Set
    End Property

    Private m_btnBckGradient As Boolean
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Enable button back color gradient."), DefaultValue(False)>
    Public Property ButtonBackColorGradient As Boolean
        Get
            Return m_btnBckGradient
        End Get
        Set(ByVal value As Boolean)
            If m_btnBckGradient <> value Then
                m_btnBckGradient = value
            End If
            Invalidate()
        End Set
    End Property

    Enum Direction
        Normal
        VerticalLeft
        UpsideDown
        VerticalRight
    End Enum

    Public m_Angle As Integer
    Public m_Dir As Direction = Direction.Normal
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("The direction of the text."), DefaultValue(Direction.Normal)>
    Public Property ButtonTextDirection() As Direction
        Get
            Return m_Dir
        End Get
        Set(ByVal value As Direction)
            If m_Dir <> value Then
                m_Dir = value
                If m_Dir = Direction.Normal OrElse m_Dir = Direction.VerticalRight Then m_Angle = 0
                If m_Dir = Direction.UpsideDown OrElse m_Dir = Direction.VerticalLeft Then m_Angle = 180
            End If
            Invalidate()
        End Set
    End Property

    Enum Brush
        SolidBrush = 0
        LinearGradientBrush = 1
        PathGradientBrush = 2
    End Enum

    Private m_brush As Brush = Brush.PathGradientBrush
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("The color gradient of the text."), DefaultValue(Brush.PathGradientBrush)>
    Public Property ButtonTextBrush As Brush
        Get
            Return m_brush
        End Get
        Set(ByVal value As Brush)
            If m_brush <> value Then
                m_brush = value
            End If
            Invalidate()
        End Set
    End Property

    Private m_alpha As Integer = 100
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Opacity value for text gradient colors (valid values 0 to 255)."), DefaultValue(100)>
    Public Property ButtonTextAlpha() As Integer
        Get
            Return m_alpha
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            If value > 255 Then value = 255
            If m_alpha <> value Then
                m_alpha = value
            End If
            Invalidate()
        End Set
    End Property

    Private m_lgAngle As Integer = 45
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Button text LinearGradient angle (valid values 0 to 360)."), DefaultValue(45)>
    Public Property ButtonTextLGAngle() As Integer
        Get
            Return m_lgAngle
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            If value > 360 Then value = 0
            If m_lgAngle <> value Then
                m_lgAngle = value
            End If
            Invalidate()
        End Set
    End Property

    '***************************************************************
    '* Property - Highlight Color
    '***************************************************************
    Private _Highlightcolor As Drawing.Color = Drawing.Color.Green
    Public Property HighlightColor() As Drawing.Color
        Get
            Return _Highlightcolor
        End Get
        Set(ByVal value As Drawing.Color)
            _Highlightcolor = value
        End Set
    End Property

    '***************************************************************
    '* Property - Highlight
    '***************************************************************
    Private OriginalBackcolor As Drawing.Color = Nothing
    Private _Highlight As Boolean
    Public Property Highlight() As Boolean
        Get
            Return _Highlight
        End Get
        Set(ByVal value As Boolean)
            If OriginalBackcolor = Nothing Then
                OriginalBackcolor = MyBase.BackColor
            End If

            If value Then
                MyBase.BackColor = _Highlightcolor
            Else
                MyBase.BackColor = OriginalBackcolor
            End If

            _Highlight = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        MyBase.New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.ContainerControl Or ControlStyles.SupportsTransparentBackColor, True)
        MyBase.DoubleBuffered = True
        DoubleBuffered = True
        MyBase.BackColor = Color.Black
        MyBase.ForeColor = Color.GreenYellow
        MyBase.Size = New Size(120, 45)
    End Sub

#End Region

#Region "Private Methods"

    Private Sub CustomTextColors_PropertyChanged(ByVal propertyName As String) Handles Colors_ctc.PropertyChanged
        Invalidate()
    End Sub

#End Region

#Region "Protected Methods"

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim gp As New GraphicsPath
        gp.AddRectangle(ClientRectangle)
        Dim pgBrush As New PathGradientBrush(gp)

        If m_btnBckGradient Then
            pgBrush.CenterColor = Color.FromArgb(180, 255, 255, 255)
            pgBrush.SurroundColors = New Color() {Color.FromArgb(0, MyBase.BackColor)}
            e.Graphics.FillRectangle(pgBrush, ClientRectangle)
        End If

        If (m_text IsNot Nothing AndAlso (String.Compare(m_text, "") <> 0)) Then

            Dim format As New StringFormat()

            If m_Dir = Direction.VerticalLeft OrElse m_Dir = Direction.VerticalRight Then
                format.FormatFlags = StringFormatFlags.DirectionVertical
            End If

            format.LineAlignment = StringAlignment.Center
            format.Alignment = StringAlignment.Center

            e.Graphics.TranslateTransform(ClientRectangle.Width / 2, ClientRectangle.Height / 2)
            e.Graphics.RotateTransform(-CSng(m_Angle))
            e.Graphics.TranslateTransform(-ClientRectangle.Width / 2, -ClientRectangle.Height / 2)

            Dim color1 As Color
            Dim color2 As Color

            If Colors_ctc.CustomTextColorsEnable Then
                color1 = Colors_ctc.CustomTextColor1
                color2 = Colors_ctc.CustomTextColor2
            Else
                color1 = ForeColor
                color2 = ForeColor
            End If

            If m_brush = 0 Then
                e.Graphics.DrawString(ButtonText, Font, New SolidBrush(color1), ClientRectangle, format)
                Exit Sub
            ElseIf m_brush = 1 Then
                Dim lgBrush As New LinearGradientBrush(ClientRectangle, color1, Color.FromArgb(m_alpha, ControlPaint.Light(color2)), m_lgAngle)
                e.Graphics.DrawString(ButtonText, Font, lgBrush, ClientRectangle, format)
            Else
                pgBrush.CenterColor = ControlPaint.Light(color1)
                pgBrush.SurroundColors = {Color.FromArgb(m_alpha, ControlPaint.Light(color2))}
                e.Graphics.DrawString(ButtonText, Font, pgBrush, ClientRectangle, format)
            End If
            e.Graphics.ResetTransform()
        End If

    End Sub

#End Region

    <Serializable(), TypeConverter(GetType(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(True)>
    Public Class CustomTextColors

        Event PropertyChanged(ByVal propertyName As String)

        Public Sub New()
        End Sub

        Private m_customColorsEnable As Boolean = False
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Enable custom text colors for LinearGradientBrush and PathGradientBrush."), DefaultValue(False)>
        Public Property CustomTextColorsEnable As Boolean
            Get
                Return m_customColorsEnable
            End Get
            Set(ByVal value As Boolean)
                If m_customColorsEnable <> value Then
                    m_customColorsEnable = value
                    RaiseEvent PropertyChanged("CustomTextColorsEnable")
                End If
            End Set
        End Property

        Private m_color1 As Color = Color.Turquoise
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Button text custom color1."), DefaultValue(GetType(Color), "Turquoise")>
        Public Property CustomTextColor1 As Color
            Get
                Return m_color1
            End Get
            Set(ByVal value As Color)
                m_color1 = value
                RaiseEvent PropertyChanged("CustomTextColor1")
            End Set
        End Property

        Private m_color2 As Color = Color.SteelBlue
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Button text custom color2."), DefaultValue(GetType(Color), "SteelBlue")>
        Public Property CustomTextColor2 As Color
            Get
                Return m_color2
            End Get
            Set(ByVal value As Color)
                m_color2 = value
                RaiseEvent PropertyChanged("CustomTextColor2")
            End Set
        End Property

        Public Overrides Function ToString() As String
            If m_customColorsEnable Then
                Return String.Format("Custom Colors Enabled")
            Else
                Return String.Format("Custom Colors Disabled")
            End If
        End Function

    End Class

End Class
