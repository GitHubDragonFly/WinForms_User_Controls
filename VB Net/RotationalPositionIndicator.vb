'* 06-FEB-16  v1.1
'*
'* This is a rotational position indicator control which can be used as weather vane as well.
'*
'* It features full circle positional arrow (360 degrees) whose home/zero position can be selected as N, E, W or S.
'*
'* The "Value" property is defined as double-precision floating point input and reflects the angle of the arrow.
'* The angle is in degrees and can be positive or negative, measured clockwise/counter-clockwise from zero position.
'* Any received value over 360 or below -360 degrees can be shown as value within -360 to 360 degrees range, calculated as:
'*
'*                             multiplier * 360 + remainder
'*
'* Multiplier is positive for positive angles and negative for negative angles.
'*
'* Example 1: received value is 450 degrees which corresponds to 90 degrees (1 * 360 + 90 = 450)
'* Example 2: received value is -725 degrees which corresponds to -5 degrees (-2 * 360 + (-5) = -725)
'*
'* The corresponding -360 to 360 degrees range angle value will always show on the control itself.
'* Optional suffix text can be shown after the degree value (suffix = N, NE, E, SE, S, SW, W or NW).
'*
'* If needed, the "Value" property can be used to show the actual received angle value.

Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Public Class RotationalPositionIndicator
    Inherits Control

#Region "Constructor"

    Public Sub New()
        MyBase.New()

        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.ContainerControl Or ControlStyles.SupportsTransparentBackColor, True)
        MyBase.DoubleBuffered = True
        DoubleBuffered = True
        MyBase.BackColor = Color.Transparent
        MyBase.ForeColor = Color.Black
        Size = New Size(160, 160)
        MinimumSize = New Size(60, 60)
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing Then

            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region "Properties"

    Private m_arrowColor As Color = Color.LawnGreen
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("The arrow color."), DefaultValue(GetType(Color), "LawnGreen")>
    Public Property RPI_ArrowColor As Color
        Get
            Return m_arrowColor
        End Get
        Set(value As Color)
            If m_arrowColor <> value Then
                m_arrowColor = value
                Invalidate()
            End If
        End Set
    End Property

    Private m_circleColor As Color = Color.Blue
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("The background circle color."), DefaultValue(GetType(Color), "Blue")>
    Public Property RPI_CircleColor As Color
        Get
            Return m_circleColor
        End Get
        Set(value As Color)
            If m_circleColor <> value Then
                m_circleColor = value
                Invalidate()
            End If
        End Set
    End Property

    Private m_zeroLineColor As Color = Color.Red
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("The arrow zero/home line color."), DefaultValue(GetType(Color), "Red")>
    Public Property RPI_ZeroLineColor As Color
        Get
            Return m_zeroLineColor
        End Get
        Set(value As Color)
            If m_zeroLineColor <> value Then
                m_zeroLineColor = value
                Invalidate()
            End If
        End Set
    End Property

    Enum Zero
        E = 0
        N = 90
        W = 180
        S = 270
    End Enum

    Private m_zeroPosition As Zero = Zero.E
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Indicates the arrow zero/home position (line on the background circle). If the control is used as Weather Vane then this line will always reflect the North."), DefaultValue(Zero.E)>
    Public Property RPI_ZeroLinePosition As Zero
        Get
            Return m_zeroPosition
        End Get
        Set(value As Zero)
            If m_zeroPosition <> value Then
                m_zeroPosition = value
                Me.Value = m_Value
            End If
        End Set
    End Property

    Private m_zeroLineShow As Boolean = True
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Show the zero/home line."), DefaultValue(True)>
    Public Property RPI_ZeroLineShow As Boolean
        Get
            Return m_zeroLineShow
        End Get
        Set(value As Boolean)
            If m_zeroLineShow <> value Then
                m_zeroLineShow = value
                Invalidate()
            End If
        End Set
    End Property

    Private m_suffixShow As Boolean
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Indicates whether to dispaly suffix text after the degrees value (suffix = N, NE, E, SE, S, SW, W or NW)."), DefaultValue(False)>
    Public Property RPI_ShowSuffix As Boolean
        Get
            Return m_suffixShow
        End Get
        Set(value As Boolean)
            If m_suffixShow <> value Then
                m_suffixShow = value
                Me.Value = m_Value
                Invalidate()
            End If
        End Set
    End Property

    Private m_string As String = "0.0" & "°"
    Private m_suffix As String = ""
    Private m_Value As Double = 0.0F
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Indicates the actual received arrow angle value in degrees. It could be any double-precision floating point value."), DefaultValue(0.0F)>
    Public Property Value() As Double
        Get
            Return m_Value
        End Get
        Set(value As Double)
            m_Value = value
            value += m_zeroPosition

            If value < 0 Then
                If (Math.Abs(CDec(value) Mod CDec(360.0F)) >= 337.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) <= 360.0F) OrElse (Math.Abs(CDec(value) Mod CDec(360.0F)) >= 0.0F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 22.5F) Then m_suffix = " E"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 22.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 67.5F Then m_suffix = " SE"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 67.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 112.5F Then m_suffix = " S"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 112.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 157.5F Then m_suffix = " SW"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 157.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 202.5F Then m_suffix = " W"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 202.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 247.5F Then m_suffix = " NW"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 247.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 292.5F Then m_suffix = " N"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 292.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 337.5F Then m_suffix = " NE"
            Else
                If (Math.Abs(CDec(value) Mod CDec(360.0F)) >= 337.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) <= 360.0F) OrElse (Math.Abs(CDec(value) Mod CDec(360.0F)) >= 0.0F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 22.5F) Then m_suffix = " E"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 22.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 67.5F Then m_suffix = " NE"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 67.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 112.5F Then m_suffix = " N"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 112.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 157.5F Then m_suffix = " NW"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 157.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 202.5F Then m_suffix = " W"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 202.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 247.5F Then m_suffix = " SW"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 247.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 292.5F Then m_suffix = " S"
                If Math.Abs(CDec(value) Mod CDec(360.0F)) >= 292.5F AndAlso Math.Abs(CDec(value) Mod CDec(360.0F)) < 337.5F Then m_suffix = " SE"
            End If

            If m_suffixShow Then
                m_string = Format(CDec(m_Value) Mod CDec(360.0F), "0.0") & "°" & m_suffix
            Else
                m_string = Format(CDec(m_Value) Mod CDec(360.0F), "0.0") & "°"
            End If

            Invalidate()
        End Set
    End Property

    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            If String.Compare(MyBase.Text, value) <> 0 Then
                MyBase.Text = value
            End If
            Invalidate()
        End Set
    End Property

#End Region

#Region "Protected Metods"

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim rect As RectangleF = New RectangleF(New PointF(1.0F, 1.0F), New SizeF(Width - 2, Height - 2))
        Dim rect2 As RectangleF = New RectangleF(New PointF(4.0F, 4.0F), New SizeF(Width - 8, Height - 8))
        Dim rect3 As RectangleF = New RectangleF(New PointF(Width / 2.0F - Height * 0.3F / 7.0F, Height * 3.1F / 7.0F), New SizeF(Height * 0.8F / 7.0F, Height * 0.8F / 7.0F))

        e.Graphics.SmoothingMode = SmoothingMode.HighQuality
        e.Graphics.FillEllipse(New SolidBrush(ControlPaint.Light(m_circleColor)), rect)
        e.Graphics.FillEllipse(New SolidBrush(m_circleColor), rect2)

        If m_zeroLineShow Then
            Select Case RPI_ZeroLinePosition
                Case Zero.N
                    e.Graphics.DrawLine(New Pen(m_zeroLineColor), New Point(Width / 2, 1.0F), New Point(Width / 2, Height / 2))
                Case Zero.E
                    e.Graphics.DrawLine(New Pen(m_zeroLineColor), New Point(Width - 2.0F, Height / 2), New Point(Width / 2, Height / 2))
                Case Zero.W
                    e.Graphics.DrawLine(New Pen(m_zeroLineColor), New Point(2.0F, Width / 2), New Point(Width / 2, Height / 2))
                Case Else 'Zero.S
                    e.Graphics.DrawLine(New Pen(m_zeroLineColor), New Point(Width / 2, Height - 2.0F), New Point(Width / 2, Height / 2))
            End Select
        End If

        e.Graphics.TranslateTransform(ClientRectangle.Width / 2.0F, ClientRectangle.Height / 2.0F)
        e.Graphics.RotateTransform(-(m_Value + CSng(m_zeroPosition)))
        e.Graphics.TranslateTransform(-ClientRectangle.Width / 2.0F, -ClientRectangle.Height / 2.0F)

        Dim points() As PointF = New PointF() {New PointF(Width / 2.0F, Height * 3.1F / 7.0F), New PointF(Width * 5.25F / 7.0F, Height * 3.1F / 7.0F),
                                               New PointF(Width * 5.25F / 7.0F, Height * 6.0F / 16.0F), New PointF(Width - 4.0F, Height * 3.5F / 7.0F),
                                               New PointF(Width * 5.25F / 7.0F, Height * 10.0F / 16.0F), New PointF(Width * 5.25F / 7.0F, Height * 3.9F / 7.0F),
                                               New PointF(Width / 2.0F, Height * 3.9F / 7.0F)}

        Dim gp As New GraphicsPath
        gp.AddPolygon(points)

        Dim lgBlend As New Blend(11) With {
            .Positions = New Single() {0.0F, 0.1F, 0.2F, 0.3F, 0.4F, 0.5F, 0.6F, 0.7F, 0.8F, 0.9F, 1.0F},
            .Factors = New Single() {0.0F, 0.1F, 0.2F, 0.3F, 0.4F, 0.5F, 0.4F, 0.3F, 0.2F, 0.1F, 0.0F}
        }

        Using lgBrush As New LinearGradientBrush(New Point(Width / 2, Height / 3), New Point(Width / 2, 2 * Height / 3), m_arrowColor, ControlPaint.Dark(m_arrowColor))
            lgBrush.Blend = lgBlend
            e.Graphics.FillEllipse(lgBrush, rect3)
            e.Graphics.FillPolygon(lgBrush, points)
        End Using

        e.Graphics.ResetTransform()

        Dim sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
        e.Graphics.DrawString(m_string, Font, New SolidBrush(ForeColor), New Point(Width / 2, Height * 2 / 3), sf)

        If Not String.IsNullOrEmpty(Text) Then
            e.Graphics.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(Width / 2, Height / 2), sf)
        Else
            e.Graphics.FillEllipse(New SolidBrush(ControlPaint.DarkDark(m_circleColor)), CSng(Width / 2 - 1.0F), CSng(Height / 2 - 1.0F), 2.0F, 2.0F)
        End If

    End Sub

#End Region

#Region "Private Methods"

    Private Sub RotationalPositionIndicator_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Width = Height
    End Sub

#End Region

End Class
