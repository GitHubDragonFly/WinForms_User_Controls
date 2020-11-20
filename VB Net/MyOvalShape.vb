'****************************************************************************
'* MyOvalShape Control v1.0
'****************************************************************************

Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms.Design

Public Class MyOvalShape
    Inherits Control

    Private OuterControlBounds, CircleBounds, FillBounds As Rectangle
    Private BackImage As Image
    Private WithEvents ArcPieRotateTimer As System.Windows.Forms.Timer

#Region "Constructor/Destructor"

    Public Sub New()
        MyBase.New()

        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.ContainerControl Or ControlStyles.SupportsTransparentBackColor, True)
        MyBase.DoubleBuffered = True
        DoubleBuffered = True
        BackColor = Color.Transparent
        ForeColor = Color.DarkBlue
        Size = New Size(200, 200)
        MinimumSize = New Size(65, 65)
        ArcPieRotateTimer = New System.Windows.Forms.Timer

        AddHandler Resize, AddressOf MyOvalShape_Resize
        AddHandler Properties_efo.PropertyChanged, AddressOf EllipseFillOptions_PropertyChanged
        AddHandler Properties_elo.PropertyChanged, AddressOf EllipseLineOptions_PropertyChanged
        AddHandler ArcPieRotateTimer.Tick, AddressOf ArcPieRotateTimer_Tick
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler Resize, AddressOf MyOvalShape_Resize
        RemoveHandler Properties_efo.PropertyChanged, AddressOf EllipseFillOptions_PropertyChanged
        RemoveHandler Properties_elo.PropertyChanged, AddressOf EllipseLineOptions_PropertyChanged
        RemoveHandler ArcPieRotateTimer.Tick, AddressOf ArcPieRotateTimer_Tick
        MyBase.Dispose(disposing)
    End Sub

#End Region

#Region "Properties"

    Public Enum EllipseAspectRatioOption
        Fixed
        Free
    End Enum

    Public Enum EllipseFillTypeOption
        Arc
        HatchStyle
        LinearGradient
        PathGradient
        Pie
        Solid
    End Enum

    Public Enum EllipseFillLinearGradientShapeOption
        Normal
        Triangular
        SigmaBell
    End Enum

    Public Enum HStyle
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

    Public Enum HStyleBground
        Plain
        Glow
    End Enum

    Public Enum ArcPieRotateDir
        CW
        CCW
    End Enum

    Private WithEvents Properties_elo As New EllipseLineOptionsProperties()
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), Description("Expand to set the ellipse line options.")>
    Public Property EllipseLineOptions() As EllipseLineOptionsProperties
        Get
            Return Properties_elo
        End Get
        Set(value As EllipseLineOptionsProperties)
            Properties_elo = value
            Invalidate()
        End Set
    End Property

    Private WithEvents Properties_efo As New EllipseFillOptionsProperties()
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), Description("Expand to set the ellipse fill options.")>
    Public Property EllipseFillOptions() As EllipseFillOptionsProperties
        Get
            Return Properties_efo
        End Get
        Set(value As EllipseFillOptionsProperties)
            Properties_efo = value
            Invalidate()
        End Set
    End Property

    Private m_EllipseAspectRatioOption As EllipseAspectRatioOption = EllipseAspectRatioOption.Fixed
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.Repaint), Description("The ellipse aspect ratio."), DefaultValue(GetType(EllipseAspectRatioOption), "Fixed")>
    Public Property EllipseAspectRatio() As EllipseAspectRatioOption
        Get
            Return m_EllipseAspectRatioOption
        End Get
        Set(value As EllipseAspectRatioOption)
            m_EllipseAspectRatioOption = value
            MyOvalShape_Resize(Me, System.EventArgs.Empty)
            Invalidate()
        End Set
    End Property

    Private m_EllipseLineColorON As Boolean
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the ellipse line color ON."), DefaultValue(False)>
    Public Property EllipseLineColorON As Boolean
        Get
            Return m_EllipseLineColorON
        End Get
        Set(value As Boolean)
            m_EllipseLineColorON = value
            Invalidate()
        End Set
    End Property

    Private m_EllipseFillColorON As Boolean
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the ellipse fill color ON. The EllipseFillColor has to be set to other than Transparent."), DefaultValue(False)>
    Public Property EllipseFillColorON As Boolean
        Get
            Return m_EllipseFillColorON
        End Get
        Set(value As Boolean)
            m_EllipseFillColorON = value
            Invalidate()
        End Set
    End Property

    Private m_PieFillColorON As Boolean
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the pie fill color ON."), DefaultValue(False)>
    Public Property PieFillColorON As Boolean
        Get
            Return m_PieFillColorON
        End Get
        Set(value As Boolean)
            m_PieFillColorON = value
            Invalidate()
        End Set
    End Property

    Private m_ArcPieLineColorON As Boolean
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the arc or pie line color ON."), DefaultValue(False)>
    Public Property ArcPieLineColorON As Boolean
        Get
            Return m_ArcPieLineColorON
        End Get
        Set(value As Boolean)
            m_ArcPieLineColorON = value
            Invalidate()
        End Set
    End Property

    Private m_ArcPieSymmetryON As Boolean
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Turn the arc or pie symmetry ON."), DefaultValue(False)>
    Public Property ArcPieSymmetryON As Boolean
        Get
            Return m_ArcPieSymmetryON
        End Get
        Set(value As Boolean)
            m_ArcPieSymmetryON = value
            Invalidate()
        End Set
    End Property

    Private m_ArcPieRotate As Boolean
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("Rotate the arc or pie."), DefaultValue(False)>
    Public Property ArcPieRotate As Boolean
        Get
            Return m_ArcPieRotate
        End Get
        Set(value As Boolean)
            If m_ArcPieRotate <> value Then
                m_ArcPieRotate = value
                If m_ArcPieRotate Then
                    ArcPieRotateTimer.Enabled = True
                Else
                    ArcPieRotateTimer.Enabled = False
                End If
                Invalidate()
            End If
        End Set
    End Property

    Private m_ArcPieRotateInterval As Integer = 100
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The arc or pie rotation interval (valid values 20+ ms)."), DefaultValue(100)>
    Public Property ArcPieRotateInterval As Integer
        Get
            Return m_ArcPieRotateInterval
        End Get
        Set(value As Integer)
            If value < 20 Then
                MessageBox.Show("Keep this value above 20ms.")
                Exit Property
            End If
            m_ArcPieRotateInterval = value
            ArcPieRotateTimer.Interval = m_ArcPieRotateInterval
        End Set
    End Property

    Private m_ArcPieRotateDir As ArcPieRotateDir = ArcPieRotateDir.CCW
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The arc or pie rotation direction."), DefaultValue(GetType(ArcPieRotateDir), "CCW")>
    Public Property ArcPieRotateDirection() As ArcPieRotateDir
        Get
            Return m_ArcPieRotateDir
        End Get
        Set(value As ArcPieRotateDir)
            m_ArcPieRotateDir = value
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

        e.Graphics.SmoothingMode = SmoothingMode.HighQuality

        OuterControlBounds = New Rectangle(New Point(0, 0), New Size(Width - 1, Height - 1))
        CircleBounds = New Rectangle(Properties_elo.EllipseLineWidth / 2, Properties_elo.EllipseLineWidth / 2, Width - Properties_elo.EllipseLineWidth - 1, Height - Properties_elo.EllipseLineWidth - 1)
        FillBounds = New Rectangle(Properties_elo.EllipseLineWidth + 1, Properties_elo.EllipseLineWidth + 1, Width - 2.0F * Properties_elo.EllipseLineWidth - 3.0F, Height - 2.0F * Properties_elo.EllipseLineWidth - 3.0F)

        '* Limit painting region so corners do not hide other close controls
        Dim CircularPath As New GraphicsPath
        CircularPath.AddEllipse(OuterControlBounds)
        Region = New Region(CircularPath)

        BackImage = New Bitmap(Width, Height)
        Using g As Graphics = Graphics.FromImage(BackImage)
            g.SmoothingMode = SmoothingMode.HighQuality

            If Not (OuterControlBounds.Width < (3.0F * Properties_elo.EllipseLineWidth) OrElse OuterControlBounds.Height < (3.0F * Properties_elo.EllipseLineWidth)) Then

                Dim pgBrush As New PathGradientBrush(CircularPath)

                Dim color1 As Color
                If m_EllipseFillColorON Then
                    color1 = Color.FromArgb(Properties_efo.EllipseFillColor1Alpha, Properties_efo.EllipseFill_ON_Color)
                Else
                    color1 = Color.FromArgb(Properties_efo.EllipseFillColor1Alpha, Properties_efo.EllipseFillColor)
                End If
                Dim color2 As Color = Color.FromArgb(Properties_efo.EllipseFillColor2Alpha, color1)

                CircularPath.AddRectangle(CircleBounds)
                CircularPath.AddEllipse(CircleBounds)

                If Properties_elo.EllipseLineWidth > 0 Then
                    If m_EllipseLineColorON Then
                        g.DrawEllipse(New Pen(New SolidBrush(Properties_elo.EllipseLine_ON_Color), Properties_elo.EllipseLineWidth), CircleBounds)
                    Else
                        g.DrawEllipse(New Pen(New SolidBrush(Properties_elo.EllipseLineColor), Properties_elo.EllipseLineWidth), CircleBounds)
                    End If
                End If

                CircularPath.AddArc(FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle)

                pgBrush.CenterColor = color1
                pgBrush.SurroundColors = {color2}

                Dim lgBrush As New LinearGradientBrush(OuterControlBounds, color1, color2, Properties_efo.EllipseFillLinearGradientAngle, True)

                Dim hBrush As New HatchBrush(DirectCast(Properties_efo.EllipseFillHatchStyle, HatchStyle), color1, color2)

                If Not Properties_efo.EllipseFillColor = Color.Transparent Then
                    Select Case Properties_efo.EllipseFillType
                        Case EllipseFillTypeOption.Solid
                            If m_EllipseFillColorON Then
                                g.FillEllipse(New SolidBrush(Properties_efo.EllipseFill_ON_Color), FillBounds)
                            Else
                                g.FillEllipse(New SolidBrush(Properties_efo.EllipseFillColor), FillBounds)
                            End If
                        Case EllipseFillTypeOption.PathGradient
                            g.FillEllipse(pgBrush, FillBounds)
                        Case EllipseFillTypeOption.HatchStyle
                            If Properties_efo.EllipseFillHatchStyleBackground = HStyleBground.Glow Then
                                g.FillEllipse(pgBrush, FillBounds)
                                g.FillEllipse(hBrush, FillBounds)
                            Else
                                g.FillEllipse(hBrush, FillBounds)
                            End If
                        Case EllipseFillTypeOption.LinearGradient
                            Select Case Properties_efo.EllipseFillLinearGradientShape
                                Case EllipseFillLinearGradientShapeOption.Normal
                                    g.FillEllipse(lgBrush, FillBounds)
                                Case EllipseFillLinearGradientShapeOption.Triangular
                                    lgBrush.SetBlendTriangularShape(Properties_efo.EllipseFillLinearGradientShapeFocusPoint)
                                    g.FillEllipse(lgBrush, FillBounds)
                                Case EllipseFillLinearGradientShapeOption.SigmaBell
                                    lgBrush.SetSigmaBellShape(Properties_efo.EllipseFillLinearGradientShapeFocusPoint)
                                    g.FillEllipse(lgBrush, FillBounds)
                            End Select
                    End Select
                End If

                If Properties_efo.EllipseFillType = EllipseFillTypeOption.Arc Then
                    If Not Properties_efo.ArcPieLineWidth = 0 Then
                        FillBounds.Inflate(-Properties_efo.ArcPieLineWidth / 2, -Properties_efo.ArcPieLineWidth / 2)
                        Dim tempColor As Color
                        If m_ArcPieLineColorON Then
                            tempColor = Properties_efo.ArcPieLine_ON_Color
                        Else
                            tempColor = Properties_efo.ArcPieLineColor
                        End If
                        If m_ArcPieSymmetryON Then
                            g.DrawArc(New Pen(tempColor, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle + 180, -Properties_efo.ArcPieSweepAngle)
                            g.DrawArc(New Pen(tempColor, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle)
                        Else
                            g.DrawArc(New Pen(tempColor, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle)
                        End If
                    End If
                ElseIf Properties_efo.EllipseFillType = EllipseFillTypeOption.Pie Then
                    FillBounds.Inflate(-Properties_efo.ArcPieLineWidth / 2, -Properties_efo.ArcPieLineWidth / 2)

                    Dim temp1Color, temp2Color As Color

                    If m_ArcPieLineColorON Then
                        temp1Color = Properties_efo.ArcPieLine_ON_Color
                    Else
                        temp1Color = Properties_efo.ArcPieLineColor
                    End If

                    If m_PieFillColorON Then
                        temp2Color = Properties_efo.PieFill_ON_Color
                    Else
                        temp2Color = Properties_efo.PieFillColor
                    End If

                    If m_ArcPieSymmetryON Then
                        g.FillPie(New SolidBrush(temp2Color), FillBounds, -Properties_efo.ArcPieStartAngle + 180, -Properties_efo.ArcPieSweepAngle)
                        g.FillPie(New SolidBrush(temp2Color), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle)

                        If Properties_efo.ArcPieLineWidth <> 0 Then
                            g.DrawPie(New Pen(temp1Color, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle + 180, -Properties_efo.ArcPieSweepAngle)
                            g.DrawPie(New Pen(temp1Color, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle)
                        End If
                    Else
                        g.FillPie(New SolidBrush(temp2Color), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle)

                        If Properties_efo.ArcPieLineWidth <> 0 Then g.DrawPie(New Pen(temp1Color, Properties_efo.ArcPieLineWidth), FillBounds, -Properties_efo.ArcPieStartAngle, -Properties_efo.ArcPieSweepAngle)
                    End If
                End If
            End If
        End Using

        e.Graphics.DrawImage(BackImage, 0, 0)

        If Not String.IsNullOrEmpty(Text) Then
            Dim sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            e.Graphics.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(Width / 2, Height / 2), sf)
        End If

    End Sub

#End Region

#Region "Private Methods"

    Private Sub MyOvalShape_Resize(sender As Object, e As EventArgs)
        If m_EllipseAspectRatioOption = EllipseAspectRatioOption.Fixed Then
            If Height > Width Then
                Height = Width
            Else
                Width = Height
            End If
        End If
    End Sub

    Private Sub ArcPieRotateTimer_Tick(sender As System.Object, e As System.EventArgs)
        If m_ArcPieRotateDir = ArcPieRotateDir.CCW Then
            Properties_efo.ArcPieStartAngle += 1
            If Properties_efo.ArcPieStartAngle = 360 Then Properties_efo.ArcPieStartAngle = 0
        Else
            Properties_efo.ArcPieStartAngle -= 1
            If Properties_efo.ArcPieStartAngle = -360 Then Properties_efo.ArcPieStartAngle = 0
        End If
        Invalidate()
    End Sub

    Private Sub EllipseLineOptions_PropertyChanged(propertyName As String)
        Invalidate()
    End Sub

    Private Sub EllipseFillOptions_PropertyChanged(propertyName As String)
        Invalidate()
    End Sub

#End Region

    <Serializable(), TypeConverter(GetType(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(True)>
    Public Class EllipseLineOptionsProperties

        Event PropertyChanged(propertyName As String)

        Public Sub New()
        End Sub

        Private _lineColor As Color = Color.SteelBlue
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse line color."), DefaultValue(GetType(Color), "SteelBlue")>
        Public Property EllipseLineColor As Color
            Get
                Return _lineColor
            End Get
            Set(value As Color)
                _lineColor = value

                RaiseEvent PropertyChanged("EllipseLineColor")
            End Set
        End Property

        Private _lineONcolor As Color = Color.Red
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse line ON color."), DefaultValue(GetType(Color), "Red")>
        Public Property EllipseLine_ON_Color As Color
            Get
                Return _lineONcolor
            End Get
            Set(value As Color)
                _lineONcolor = value

                RaiseEvent PropertyChanged("EllipseLine_ON_Color")
            End Set
        End Property

        Private _lineWidth As Single = 2.0F
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse line width (values 0 to 20 in increments of 2 for better concentricity)."), Editor(GetType(NumericUpDownValueEditor), GetType(UITypeEditor)), DefaultValue(2.0F)>
        Public Property EllipseLineWidth As Single
            Get
                NumericUpDownValueEditor.nudControl.DecimalPlaces = 0
                NumericUpDownValueEditor.nudControl.Increment = 2
                NumericUpDownValueEditor.nudControl.Minimum = 0
                NumericUpDownValueEditor.nudControl.Maximum = 20
                NumericUpDownValueEditor.valueType = "Single"
                Return _lineWidth
            End Get
            Set(value As Single)
                If value < 0 OrElse value > 20 Then value = 2.0F

                _lineWidth = value

                RaiseEvent PropertyChanged("EllipseLineWidth")
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("Set ellipse line options")
        End Function

    End Class

    <Serializable(), TypeConverter(GetType(ExpandableObjectConverter)), RefreshProperties(RefreshProperties.Repaint), DesignTimeVisible(True)>
    Public Class EllipseFillOptionsProperties

        Event PropertyChanged(propertyName As String)

        Public Sub New()
        End Sub

        Private _EllipseFillType As EllipseFillTypeOption = EllipseFillTypeOption.PathGradient
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill type (PathGradient, LinearGradient, HatchStyle, Solid, Pie and Arc)."), DefaultValue(EllipseFillTypeOption.PathGradient)>
        Public Property EllipseFillType() As EllipseFillTypeOption
            Get
                Return _EllipseFillType
            End Get
            Set(value As EllipseFillTypeOption)
                _EllipseFillType = value

                If _EllipseFillType = EllipseFillTypeOption.HatchStyle Then
                    If EllipseFillColor = Color.Transparent Then
                        MessageBox.Show("The EllipseFillColor is currently set to Transparent. It will be changed to make the HatchStyle visible.")
                        EllipseFillColor = Color.Yellow
                    End If
                    If _hatchStyleBground = HStyleBground.Plain Then
                        EllipseFillColor1Alpha = 255
                        EllipseFillColor2Alpha = 0
                    Else
                        EllipseFillColor1Alpha = 255
                        EllipseFillColor2Alpha = 125
                    End If
                Else
                    EllipseFillColor1Alpha = 255
                    EllipseFillColor2Alpha = 125
                End If

                RaiseEvent PropertyChanged("EllipseFillType")
            End Set
        End Property

        Private _brushColor As Color = Color.Transparent
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill color."), DefaultValue(GetType(Color), "Transparent")>
        Public Property EllipseFillColor As Color
            Get
                Return _brushColor
            End Get
            Set(value As Color)
                _brushColor = value

                RaiseEvent PropertyChanged("EllipseFillColor")
            End Set
        End Property

        Private _fillONcolor As Color = Color.Red
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill ON color."), DefaultValue(GetType(Color), "Red")>
        Public Property EllipseFill_ON_Color As Color
            Get
                Return _fillONcolor
            End Get
            Set(value As Color)
                _fillONcolor = value

                RaiseEvent PropertyChanged("EllipseFill_ON_Color")
            End Set
        End Property

        Private _hatchStyle As HStyle = HStyle.BackwardDiagonal
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill hatch style."), DefaultValue(GetType(HStyle), "BackwardDiagonal")>
        Public Property EllipseFillHatchStyle As HStyle
            Get
                Return _hatchStyle
            End Get
            Set(value As HStyle)
                _hatchStyle = value

                RaiseEvent PropertyChanged("EllipseFillHatchStyle")
            End Set
        End Property

        Private _hatchStyleBground As HStyleBground = HStyleBground.Plain
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill hatch style background. When HatchStyle is enabled then selecting the Plain background will lock the Alpha values"), DefaultValue(GetType(HStyleBground), "Plain")>
        Public Property EllipseFillHatchStyleBackground As HStyleBground
            Get
                Return _hatchStyleBground
            End Get
            Set(value As HStyleBground)
                _hatchStyleBground = value

                If _EllipseFillType = EllipseFillTypeOption.HatchStyle Then
                    If _hatchStyleBground = HStyleBground.Plain Then
                        EllipseFillColor1Alpha = 255
                        EllipseFillColor2Alpha = 0
                    Else
                        EllipseFillColor1Alpha = 255
                        EllipseFillColor2Alpha = 125
                    End If
                End If

                RaiseEvent PropertyChanged("EllipseFillHatchStyleBackground")
            End Set
        End Property

        Private _angle As Single
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill linear gradient angle (valid values 0 to 180)."), Editor(GetType(NumericUpDownValueEditor), GetType(UITypeEditor)), DefaultValue(0.0F)>
        Public Property EllipseFillLinearGradientAngle As Single
            Get
                NumericUpDownValueEditor.nudControl.DecimalPlaces = 0
                NumericUpDownValueEditor.nudControl.Increment = 1
                NumericUpDownValueEditor.nudControl.Minimum = 0
                NumericUpDownValueEditor.nudControl.Maximum = 180
                NumericUpDownValueEditor.valueType = "Single"
                Return _angle
            End Get
            Set(value As Single)
                If value < 0 OrElse value > 180 Then value = 0.0F

                _angle = value

                RaiseEvent PropertyChanged("EllipseFillLinearGradientAngle")
            End Set
        End Property

        Private _FocusPoint As Single = 0.5F
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill linear gradient focus point for Triangular and SigmaBell shapes (valid values 0 to 1)."), Editor(GetType(NumericUpDownValueEditor), GetType(UITypeEditor)), DefaultValue(0.5F)>
        Public Property EllipseFillLinearGradientShapeFocusPoint As Single
            Get
                NumericUpDownValueEditor.nudControl.DecimalPlaces = 2
                NumericUpDownValueEditor.nudControl.Increment = 0.01
                NumericUpDownValueEditor.nudControl.Minimum = 0
                NumericUpDownValueEditor.nudControl.Maximum = 1
                NumericUpDownValueEditor.valueType = "Single"
                Return _FocusPoint
            End Get
            Set(value As Single)
                If EllipseFillLinearGradientShape = EllipseFillLinearGradientShapeOption.Normal OrElse value < 0 OrElse value > 1 Then value = 0.0F

                _FocusPoint = value

                RaiseEvent PropertyChanged("EllipseFillLinearGradientShapeFocusPoint")
            End Set
        End Property

        Private _alpha1 As Integer = 255
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill Color1 opacity value (valid values 0 to 255)."), Editor(GetType(TrackBarValueEditor), GetType(UITypeEditor)), DefaultValue(255)>
        Public Property EllipseFillColor1Alpha As Integer
            Get
                Return _alpha1
            End Get
            Set(value As Integer)
                If value < 0 OrElse value > 255 Then value = 255

                _alpha1 = value

                RaiseEvent PropertyChanged("EllipseFillColor1Alpha")
            End Set
        End Property

        Private _alpha2 As Integer = 125
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill Color2 opacity value (valid values 0 to 255)."), Editor(GetType(TrackBarValueEditor), GetType(UITypeEditor)), DefaultValue(125)>
        Public Property EllipseFillColor2Alpha As Integer
            Get
                Return _alpha2
            End Get
            Set(value As Integer)
                If value < 0 OrElse value > 255 Then value = 125

                _alpha2 = value

                RaiseEvent PropertyChanged("EllipseFillColor2Alpha")
            End Set
        End Property

        Private _EllipseFillLinearGradientShape As EllipseFillLinearGradientShapeOption = EllipseFillLinearGradientShapeOption.Normal
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Ellipse fill linear gradient shape (Normal, Triangular and SigmaBell)."), DefaultValue(GetType(EllipseFillLinearGradientShapeOption), "Normal")>
        Public Property EllipseFillLinearGradientShape() As EllipseFillLinearGradientShapeOption
            Get
                Return _EllipseFillLinearGradientShape
            End Get
            Set(value As EllipseFillLinearGradientShapeOption)
                _EllipseFillLinearGradientShape = value

                RaiseEvent PropertyChanged("EllipseFillLinearGradientShape")
            End Set
        End Property

        Private _StartAngle As Single
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie start angle (valid values -360 to 360)."), Editor(GetType(NumericUpDownValueEditor), GetType(UITypeEditor)), DefaultValue(0.0F)>
        Public Property ArcPieStartAngle As Single
            Get
                NumericUpDownValueEditor.nudControl.DecimalPlaces = 0
                NumericUpDownValueEditor.nudControl.Increment = 1
                NumericUpDownValueEditor.nudControl.Minimum = -360
                NumericUpDownValueEditor.nudControl.Maximum = 360
                NumericUpDownValueEditor.valueType = "Single"
                Return _StartAngle
            End Get
            Set(value As Single)
                If value < -360 OrElse value > 360 Then value = 0.0F

                _StartAngle = value

                RaiseEvent PropertyChanged("ArcPieStartAngle")
            End Set
        End Property

        Private _SweepAngle As Single = 45.0F
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie sweep angle (valid values -360 to 360). For positive values, sweep is done CCW from start angle."), Editor(GetType(NumericUpDownValueEditor), GetType(UITypeEditor)), DefaultValue(45.0F)>
        Public Property ArcPieSweepAngle As Single
            Get
                NumericUpDownValueEditor.nudControl.DecimalPlaces = 0
                NumericUpDownValueEditor.nudControl.Increment = 1
                NumericUpDownValueEditor.nudControl.Minimum = -360
                NumericUpDownValueEditor.nudControl.Maximum = 360
                NumericUpDownValueEditor.valueType = "Single"
                Return _SweepAngle
            End Get
            Set(value As Single)
                If value < -360 OrElse value > 360 Then value = 45.0F

                _SweepAngle = value

                RaiseEvent PropertyChanged("ArcPieSweepAngle")
            End Set
        End Property

        Private _fillPieColor As Color = Color.Yellow
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Pie color."), DefaultValue(GetType(Color), "Yellow")>
        Public Property PieFillColor As Color
            Get
                Return _fillPieColor
            End Get
            Set(value As Color)
                _fillPieColor = value

                RaiseEvent PropertyChanged("EllipseFillPieColor")
            End Set
        End Property

        Private _fillPieONColor As Color = Color.Red
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Pie ON color."), DefaultValue(GetType(Color), "Red")>
        Public Property PieFill_ON_Color As Color
            Get
                Return _fillPieONColor
            End Get
            Set(value As Color)
                _fillPieONColor = value

                RaiseEvent PropertyChanged("EllipseFillPieONColor")
            End Set
        End Property

        Private _fillLineColor As Color = Color.Yellow
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie line color."), DefaultValue(GetType(Color), "Yellow")>
        Public Property ArcPieLineColor As Color
            Get
                Return _fillLineColor
            End Get
            Set(value As Color)
                _fillLineColor = value

                RaiseEvent PropertyChanged("ArcPieLineColor")
            End Set
        End Property

        Private _fillLineONColor As Color = Color.Red
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie line ON color."), DefaultValue(GetType(Color), "Red")>
        Public Property ArcPieLine_ON_Color As Color
            Get
                Return _fillLineONColor
            End Get
            Set(value As Color)
                _fillLineONColor = value

                RaiseEvent PropertyChanged("ArcPieLine_ON_Color")
            End Set
        End Property

        Private _fillLineWidth As Single = 1.0F
        <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.All), Description("The Arc or Pie line width (valid values 0 to 10)."), Editor(GetType(NumericUpDownValueEditor), GetType(UITypeEditor)), DefaultValue(1.0F)>
        Public Property ArcPieLineWidth As Single
            Get
                NumericUpDownValueEditor.nudControl.DecimalPlaces = 0
                NumericUpDownValueEditor.nudControl.Increment = 1
                NumericUpDownValueEditor.nudControl.Minimum = 0
                NumericUpDownValueEditor.nudControl.Maximum = 10
                NumericUpDownValueEditor.valueType = "Single"
                Return _fillLineWidth
            End Get
            Set(value As Single)
                If value < 0 OrElse value > 10 Then value = 1.0F

                _fillLineWidth = value

                RaiseEvent PropertyChanged("ArcPieLineWidth")
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("Selected:  '" + EllipseFillType.ToString + "'")
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
        '
        'This class can be formatted in the same manner as the above NumericUpDownValueEditor class
        '(in which case it would be passing TrackBar values from within the properties instead of being hardcoded).
        'It was left in this format since it was used for 2 identical integer properties only.

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
