'****************************************************************************
'* MyRectangleShape Control v1.0
'****************************************************************************

Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms.Design

Public Class MyRectangleShape
    Inherits Control

#Region "Constructor"

    Public Sub New()
        MyBase.New()

        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.ContainerControl Or ControlStyles.SupportsTransparentBackColor, True)
        MyBase.DoubleBuffered = True
        DoubleBuffered = True
        BackColor = Color.Transparent
        ForeColor = Color.DarkBlue
        Size = New Size(200, 200)
        MinimumSize = New Size(65, 65)

        AddHandler Resize, AddressOf MyRectangleShape_Resize
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler Resize, AddressOf MyRectangleShape_Resize
        MyBase.Dispose(disposing)
    End Sub

#End Region

#Region "Properties"

    Public Enum RectangleAspectRatioOption
        Fixed
        Free
    End Enum

    Private m_RectangleAspectRatioOption As RectangleAspectRatioOption = RectangleAspectRatioOption.Fixed
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), Description("The rectangle aspect ratio."), DefaultValue(GetType(RectangleAspectRatioOption), "Fixed")>
    Public Property RectangleAspectRatio() As RectangleAspectRatioOption
        Get
            Return m_RectangleAspectRatioOption
        End Get
        Set(value As RectangleAspectRatioOption)
            m_RectangleAspectRatioOption = value
            MyRectangleShape_Resize(Me, System.EventArgs.Empty)
            Invalidate()
        End Set
    End Property

    Private WithEvents Properties_rlo As New RectangleLineOptionsProperties()
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), DescriptionAttribute("Expand to set the Rectangle line options.")>
    Public Property RectangleLineOptions() As RectangleLineOptionsProperties
        Get
            Return Properties_rlo
        End Get
        Set(value As RectangleLineOptionsProperties)
            Properties_rlo = value
            Invalidate()
        End Set
    End Property

    Private WithEvents Properties_rfo As New RectangleFillOptionsProperties()
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), DescriptionAttribute("Expand to set the rectangle fill options.")>
    Public Property RectangleFillOptions() As RectangleFillOptionsProperties
        Get
            Return Properties_rfo
        End Get
        Set(value As RectangleFillOptionsProperties)
            Properties_rfo = value
            Invalidate()
        End Set
    End Property

    Private m_RectangleLineColorON As Boolean
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the rectangle line color ON."), DefaultValue(False)>
    Public Property RectangleLineColorON As Boolean
        Get
            Return m_RectangleLineColorON
        End Get
        Set(value As Boolean)
            m_RectangleLineColorON = value
            Invalidate()
        End Set
    End Property

    Private m_RectangleFillColorON As Boolean
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the rectangle fill color ON. The RectangleFillColor has to be set to other than Transparent."), DefaultValue(False)>
    Public Property RectangleFillColorON As Boolean
        Get
            Return m_RectangleFillColorON
        End Get
        Set(value As Boolean)
            m_RectangleFillColorON = value
            Invalidate()
        End Set
    End Property

    <Browsable(False)>
    Public Shadows Property BorderStyle As BorderStyle = BorderStyle.None

    <Browsable(True)>
    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            If String.Compare(MyBase.Text, value) <> 0 Then
                MyBase.Text = value
                Invalidate()
            End If
        End Set
    End Property

#End Region

#Region "Protected Methods"

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        Dim rect1 As New Rectangle((Properties_rlo.RectangleLineWidth / 2.0F) + 2.0F, (Properties_rlo.RectangleLineWidth / 2.0F) + 2.0F, CSng(Width) - Properties_rlo.RectangleLineWidth - 5.0F, CSng(Height) - Properties_rlo.RectangleLineWidth - 5.0F)
        Dim rect2 As New Rectangle(Properties_rlo.RectangleLineWidth + 3.51F, Properties_rlo.RectangleLineWidth + 3.51F, CSng(Width) - 2.0F * (Properties_rlo.RectangleLineWidth) - 9.0F, CSng(Height) - 2.0F * (Properties_rlo.RectangleLineWidth) - 9.0F)

        If Not (rect2.Width < 1 OrElse rect2.Height < 1) Then
            Dim color1 As Color
            If m_RectangleFillColorON Then
                color1 = Color.FromArgb(Properties_rfo.RectangleFillColor1Alpha, Properties_rfo.RectangleFill_ON_Color)
            Else
                color1 = Color.FromArgb(Properties_rfo.RectangleFillColor1Alpha, Properties_rfo.RectangleFillColor)
            End If
            Dim color2 As Color = Color.FromArgb(Properties_rfo.RectangleFillColor2Alpha, color1)

            Dim gp As New GraphicsPath
            gp.AddRectangle(rect2)

            Dim pgBrush As New PathGradientBrush(gp) With {.CenterColor = color1, .SurroundColors = {color2}}

            Dim lgBrush As New LinearGradientBrush(rect2, color1, color2, Properties_rfo.RectangleFillLinearGradientAngle, True)

            Dim hBrush As New HatchBrush(DirectCast(Properties_rfo.RectangleFillHatchStyle, HatchStyle), color1, color2)

            If Not Properties_rfo.RectangleFillColor = Color.Transparent Then
                If Properties_rfo.RectangleFillType = RectangleFillOptionsProperties.RectangleFillTypeOption.Solid Then e.Graphics.FillRectangle(New SolidBrush(Properties_rfo.RectangleFillColor), rect2)
                If Properties_rfo.RectangleFillType = RectangleFillOptionsProperties.RectangleFillTypeOption.PathGradient Then e.Graphics.FillRectangle(pgBrush, rect2)
                If Properties_rfo.RectangleFillType = RectangleFillOptionsProperties.RectangleFillTypeOption.HatchStyle Then
                    If Properties_rfo.RectangleFillHatchStyleBackground = RectangleFillOptionsProperties.HStyleBground.Glow Then
                        e.Graphics.FillRectangle(pgBrush, rect2)
                        e.Graphics.FillRectangle(hBrush, rect2)
                    Else
                        e.Graphics.FillRectangle(New SolidBrush(BackColor), rect2)
                        e.Graphics.FillRectangle(hBrush, rect2)
                    End If
                End If
                If Properties_rfo.RectangleFillType = RectangleFillOptionsProperties.RectangleFillTypeOption.LinearGradient Then
                    If Properties_rfo.RectangleFillLinearGradientShape = RectangleFillOptionsProperties.RectangleFillLinearGradientShapeOption.Normal Then e.Graphics.FillRectangle(lgBrush, rect2)
                    If Properties_rfo.RectangleFillLinearGradientShape = RectangleFillOptionsProperties.RectangleFillLinearGradientShapeOption.Triangular Then
                        lgBrush.SetBlendTriangularShape(Properties_rfo.RectangleFillLinearGradientShapeFocusPoint)
                        e.Graphics.FillRectangle(lgBrush, rect2)
                    End If
                    If Properties_rfo.RectangleFillLinearGradientShape = RectangleFillOptionsProperties.RectangleFillLinearGradientShapeOption.SigmaBell Then
                        lgBrush.SetSigmaBellShape(Properties_rfo.RectangleFillLinearGradientShapeFocusPoint)
                        e.Graphics.FillRectangle(lgBrush, rect2)
                    End If
                End If
            Else
                e.Graphics.FillRectangle(New SolidBrush(Properties_rfo.RectangleFillColor), rect2)
            End If
        End If

        If Properties_rlo.RectangleLineWidth > 0 Then
            If m_RectangleLineColorON Then
                e.Graphics.DrawRectangle(New Pen(Properties_rlo.RectangleLine_ON_Color, Properties_rlo.RectangleLineWidth), rect1)
            Else
                e.Graphics.DrawRectangle(New Pen(Properties_rlo.RectangleLineColor, Properties_rlo.RectangleLineWidth), rect1)
            End If
        End If

        If Not String.IsNullOrEmpty(Text) Then
            Dim sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            e.Graphics.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(Width / 2, Height / 2), sf)
        End If

    End Sub

#End Region

#Region "Private Methods"

    Private Sub MyRectangleShape_Resize(sender As Object, e As EventArgs)
        If m_RectangleAspectRatioOption = RectangleAspectRatioOption.Fixed Then
            If Height > Width Then
                Height = Width
            Else
                Width = Height
            End If
        End If
    End Sub

    Private Sub RectangleLineOptions_PropertyChanged(propertyName As String) Handles Properties_rlo.PropertyChanged
        Invalidate()
    End Sub

    Private Sub RectangleFillOptions_PropertyChanged(propertyName As String) Handles Properties_rfo.PropertyChanged
        Invalidate()
    End Sub

#End Region

    <Serializable(), TypeConverter(GetType(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(True)>
    Public Class RectangleLineOptionsProperties

        Event PropertyChanged(propertyName As String)

        Public Sub New()
        End Sub

        Private _lineColor As Color = Color.SteelBlue
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Rectangle line color."), DefaultValue(GetType(Color), "SteelBlue")>
        Public Property RectangleLineColor As Color
            Get
                Return _lineColor
            End Get
            Set(value As Color)
                _lineColor = value
                RaiseEvent PropertyChanged("RectangleLineColor")
            End Set
        End Property

        Private _lineONcolor As Drawing.Color = Drawing.Color.Red
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Rectangle line ON color."), DefaultValue(GetType(Color), "Red")>
        Public Property RectangleLine_ON_Color As Drawing.Color
            Get
                Return _lineONcolor
            End Get
            Set(value As Drawing.Color)
                _lineONcolor = value
                RaiseEvent PropertyChanged("RectangleLine_ON_Color")
            End Set
        End Property

        Private _lineWidth As Single = 2.0F
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Rectangle line width (valid values 0 to 20)."), Editor(GetType(NumericUpDownValueEditor), GetType(UITypeEditor)), DefaultValue(2.0F)>
        Public Property RectangleLineWidth As Single
            Get
                NumericUpDownValueEditor.nudControl.DecimalPlaces = 0
                NumericUpDownValueEditor.nudControl.Increment = 1
                NumericUpDownValueEditor.nudControl.Minimum = 0
                NumericUpDownValueEditor.nudControl.Maximum = 20
                NumericUpDownValueEditor.valueType = "Single"
                Return _lineWidth
            End Get
            Set(value As Single)
                If Not IsNumeric(value) Then value = 2.0F
                If value < 0 Then value = 0.0F
                If value > 20 Then value = 20.0F
                _lineWidth = value
                RaiseEvent PropertyChanged("RectangleLineWidth")
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("Set Rectangle line options")
        End Function

    End Class

    <Serializable(), TypeConverter(GetType(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(True)>
    Public Class RectangleFillOptionsProperties

        Event PropertyChanged(propertyName As String)

        Public Sub New()
        End Sub

        Private _brushColor As Color = Color.Transparent
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill color."), DefaultValue(GetType(Color), "Transparent")>
        Public Property RectangleFillColor As Color
            Get
                Return _brushColor
            End Get
            Set(value As Color)
                _brushColor = value
                RaiseEvent PropertyChanged("RectangleFillColor")
            End Set
        End Property

        Private _fillONcolor As Drawing.Color = Drawing.Color.Red
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill ON color."), DefaultValue(GetType(Color), "Red")>
        Public Property RectangleFill_ON_Color As Drawing.Color
            Get
                Return _fillONcolor
            End Get
            Set(value As Drawing.Color)
                _fillONcolor = value
                RaiseEvent PropertyChanged("RectangleFill_ON_Color")
            End Set
        End Property

        Enum HStyle
            Horizontal = 0
            Vertical = 1
            ForwardDiagonal = 2
            BackwardDiagonal = 3
            Cross = 4
            DiagonalCross = 5
            Percent05 = 6
            Percent10 = 7
            Percent20 = 8
            Percent25 = 9
            Percent30 = 10
            Percent40 = 11
            Percent50 = 12
            Percent60 = 13
            Percent70 = 14
            Percent75 = 15
            Percent80 = 16
            Percent90 = 17
            LightDownwardDiagonal = 18
            LightUpwardDiagonal = 19
            DarkDownwardDiagonal = 20
            DarkUpwardDiagonal = 21
            WideDownwardDiagonal = 22
            WideUpwardDiagonal = 23
            LightVertical = 24
            LightHorizontal = 25
            NarrowVertical = 26
            NarrowHorizontal = 27
            DarkVertical = 28
            DarkHorizontal = 29
            DashedDownwardDiagonal = 30
            DashedUpwardDiagonal = 31
            DashedHorizontal = 32
            DashedVertical = 33
            SmallConfetti = 34
            LargeConfetti = 35
            ZigZag = 36
            Wave = 37
            DiagonalBrick = 38
            HorizontalBrick = 39
            Weave = 40
            Plaid = 41
            Divot = 42
            DottedGrid = 43
            DottedDiamond = 44
            Shingle = 45
            Trellis = 46
            Sphere = 47
            SmallGrid = 48
            SmallCheckerBoard = 49
            LargeCheckerBoard = 50
            OutlinedDiamond = 51
            SolidDiamond = 52
        End Enum

        Private _hatchStyle As HStyle = HStyle.BackwardDiagonal
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill hatch style."), DefaultValue(HStyle.BackwardDiagonal)>
        Public Property RectangleFillHatchStyle As HStyle
            Get
                Return _hatchStyle
            End Get
            Set(value As HStyle)
                _hatchStyle = value
                RaiseEvent PropertyChanged("RectangleFillHatchStyle")
            End Set
        End Property

        Enum HStyleBground
            Plain
            Glow
        End Enum

        Private _hatchStyleBground As HStyleBground = HStyleBground.Plain
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill hatch style background. When HatchStyle is enabled then selecting the Plain background will lock the Alpha values"), DefaultValue(HStyleBground.Plain)>
        Public Property RectangleFillHatchStyleBackground As HStyleBground
            Get
                Return _hatchStyleBground
            End Get
            Set(value As HStyleBground)
                _hatchStyleBground = value
                If _hatchStyleBground = HStyleBground.Plain Then
                    RectangleFillColor1Alpha = 255
                    RectangleFillColor2Alpha = 0
                End If
                RaiseEvent PropertyChanged("RectangleFillHatchStyleBackground")
            End Set
        End Property

        Private _angle As Single = 0.0F
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill linear gradient angle (valid values 0 to 180)."), Editor(GetType(NumericUpDownValueEditor), GetType(UITypeEditor)), DefaultValue(0.0F)>
        Public Property RectangleFillLinearGradientAngle As Single
            Get
                NumericUpDownValueEditor.nudControl.DecimalPlaces = 0
                NumericUpDownValueEditor.nudControl.Increment = 1
                NumericUpDownValueEditor.nudControl.Minimum = 0
                NumericUpDownValueEditor.nudControl.Maximum = 180
                NumericUpDownValueEditor.valueType = "Single"
                Return _angle
            End Get
            Set(value As Single)
                If Not IsNumeric(value) Then value = 45.0F
                If value < 0 Then value = 0.0F
                If value > 180 Then value = 180.0F
                _angle = value
                RaiseEvent PropertyChanged("RectangleFillLinearGradientAngle")
            End Set
        End Property

        Private _FocusPoint As Single = 0.0F
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill linear gradient focus point for Triangular and SigmaBell shapes (valid values 0 to 1)."), Editor(GetType(NumericUpDownValueEditor), GetType(UITypeEditor)), DefaultValue(0.0F)>
        Public Property RectangleFillLinearGradientShapeFocusPoint As Single
            Get
                NumericUpDownValueEditor.nudControl.DecimalPlaces = 2
                NumericUpDownValueEditor.nudControl.Increment = 0.01
                NumericUpDownValueEditor.nudControl.Minimum = 0
                NumericUpDownValueEditor.nudControl.Maximum = 1
                NumericUpDownValueEditor.valueType = "Single"
                Return _FocusPoint
            End Get
            Set(value As Single)
                If RectangleFillLinearGradientShape = RectangleFillLinearGradientShapeOption.Normal Then value = 0.0F
                If Not IsNumeric(value) Then value = 0.0F
                If value < 0 Then value = 0.0F
                If value > 1 Then value = 1.0F
                _FocusPoint = value
                RaiseEvent PropertyChanged("RectangleFillLinearGradientShapeFocusPoint")
            End Set
        End Property

        Private _alpha1 As Integer = 255
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill Color1 opacity value (valid values 0 to 255)."), Editor(GetType(TrackBarValueEditor), GetType(UITypeEditor)), DefaultValue(255)>
        Public Property RectangleFillColor1Alpha As Integer
            Get
                Return _alpha1
            End Get
            Set(value As Integer)
                If Not IsNumeric(value) Then value = 255
                If value < 0 Then value = 0
                If value > 255 Then value = 255
                If _RectangleFillType = RectangleFillTypeOption.HatchStyle Then
                    If RectangleFillHatchStyleBackground = HStyleBground.Plain Then
                        _alpha1 = 255
                    Else
                        _alpha1 = value
                    End If
                Else
                    _alpha1 = value
                End If
                RaiseEvent PropertyChanged("RectangleFillColor1Alpha")
            End Set
        End Property

        Private _alpha2 As Integer = 125
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill Color2 opacity value (valid values 0 to 255)."), Editor(GetType(TrackBarValueEditor), GetType(UITypeEditor)), DefaultValue(125)>
        Public Property RectangleFillColor2Alpha As Integer
            Get
                Return _alpha2
            End Get
            Set(value As Integer)
                If Not IsNumeric(value) Then value = 125
                If value < 0 Then value = 0
                If value > 255 Then value = 255
                If _RectangleFillType = RectangleFillTypeOption.HatchStyle Then
                    If RectangleFillHatchStyleBackground = HStyleBground.Plain Then
                        _alpha2 = 0
                    Else
                        _alpha2 = value
                    End If
                Else
                    _alpha2 = value
                End If
                RaiseEvent PropertyChanged("RectangleFillColor2Alpha")
            End Set
        End Property

        Enum RectangleFillTypeOption
            HatchStyle
            LinearGradient
            PathGradient
            Solid
        End Enum

        Private _RectangleFillType As RectangleFillTypeOption = RectangleFillTypeOption.PathGradient
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill color type (PathGradient, LinearGradient, HatchStyle and Solid)."), DefaultValue(RectangleFillTypeOption.PathGradient)>
        Public Property RectangleFillType As RectangleFillTypeOption
            Get
                Return _RectangleFillType
            End Get
            Set(value As RectangleFillTypeOption)
                _RectangleFillType = value
                If _RectangleFillType = RectangleFillTypeOption.HatchStyle Then
                    If RectangleFillColor = Color.Transparent Then
                        MessageBox.Show("The RectangleFillColor is currently set to Transparent. It will be changed to make the HatchStyle visible.")
                        RectangleFillColor = Color.Yellow
                    End If
                    If _hatchStyleBground = HStyleBground.Plain Then
                        RectangleFillColor1Alpha = 255
                        RectangleFillColor2Alpha = 0
                    Else
                        RectangleFillColor1Alpha = 255
                        RectangleFillColor2Alpha = 125
                    End If
                Else
                    RectangleFillColor1Alpha = 255
                    RectangleFillColor2Alpha = 125
                End If
                RaiseEvent PropertyChanged("RectangleFillType")
            End Set
        End Property

        Enum RectangleFillLinearGradientShapeOption
            Normal
            Triangular
            SigmaBell
        End Enum

        Private _RectangleFillLinearGradientShape As RectangleFillLinearGradientShapeOption = RectangleFillLinearGradientShapeOption.Normal
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The rectangle fill linear gradient shape (Normal, Triangular and SigmaBell)."), DefaultValue(RectangleFillLinearGradientShapeOption.Normal)>
        Public Property RectangleFillLinearGradientShape As RectangleFillLinearGradientShapeOption
            Get
                Return _RectangleFillLinearGradientShape
            End Get
            Set(value As RectangleFillLinearGradientShapeOption)
                _RectangleFillLinearGradientShape = value
                RaiseEvent PropertyChanged("RectangleFillLinearGradientShape")
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("Selected:  '" + RectangleFillType.ToString + "'")
        End Function

    End Class

    Public Class NumericUpDownValueEditor
        Inherits UITypeEditor

        'Class references:
        'http://stackoverflow.com/questions/14291291/how-to-add-numericupdown-control-to-custom-property-grid-in-c
        'https://msdn.microsoft.com/en-CA/library/ms171840.aspx?cs-save-lang=1&cs-lang=vb#code-snippet-1
        '30-JUL-2015 Converted to VB Net By Godra and modified.

        Public Shared nudControl As New NumericUpDown()
        Public Shared valueType As String

        Public Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.DropDown
        End Function

        Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Dim editorService As IWindowsFormsEditorService = Nothing
            If provider IsNot Nothing Then
                editorService = TryCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
            End If

            If editorService IsNot Nothing Then
                nudControl.Value = value
                editorService.DropDownControl(nudControl)
                If valueType = "Single" Then value = CSng(nudControl.Value)
                If valueType = "Integer" Then value = CInt(nudControl.Value)
            End If

            Return value
        End Function
    End Class

    Public Class TrackBarValueEditor
        Inherits UITypeEditor

        'Class references:
        'http://stackoverflow.com/questions/14291291/how-to-add-numericupdown-control-to-custom-property-grid-in-c
        'https://msdn.microsoft.com/en-CA/library/ms171840.aspx?cs-save-lang=1&cs-lang=vb#code-snippet-1
        '30-JUL-2015 Converted to VB Net By Godra and modified to use TrackBar instead of NumericUpDown control.

        Private WithEvents TrackBarControl As New TrackBar()
        Private ReadOnly toolTip1 As New ToolTip

        Public Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.DropDown
        End Function

        Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Dim editorService As IWindowsFormsEditorService = Nothing
            If provider IsNot Nothing Then
                editorService = TryCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
            End If

            If editorService IsNot Nothing Then
                TrackBarControl.Minimum = 0
                TrackBarControl.Maximum = 255
                TrackBarControl.LargeChange = 5
                TrackBarControl.Orientation = Orientation.Horizontal
                TrackBarControl.TickFrequency = 25
                TrackBarControl.TickStyle = TickStyle.TopLeft
                TrackBarControl.Value = value
                editorService.DropDownControl(TrackBarControl)
                value = CInt(TrackBarControl.Value)
                AddHandler TrackBarControl.Scroll, AddressOf TrackBarControl_Scroll
            End If

            Return value
        End Function

        Private Sub TrackBarControl_Scroll()
            toolTip1.SetToolTip(TrackBarControl, TrackBarControl.Value.ToString())
        End Sub

    End Class

End Class

