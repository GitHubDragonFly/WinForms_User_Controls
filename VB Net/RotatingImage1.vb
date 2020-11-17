'* Design by Godra
'*
'* 21-AUG-16  v1.0
'* 
'* This is a simple rotating image control.
'*
'* Its purpose is to demonstrate the VB Net code for the following:
'*  - Limiting the control's paint region (making the control round to fit the image)
'*  - Rotation of the image around its center (arbitrary angle of rotation, -360 to 360 degree range)
'*  - Horizontal/Vertical flipping
'*  - Hardcoding an image and using it as a built-in image
'*

Imports System.ComponentModel

Public Class RotatingImage1
    Inherits Control

    '* Declare internal timer to be used for endless rotation of the image
    Private WithEvents Tmr As Timer

    '* Declare built-in/hardcoded image (96x96 "Load Image" picture with transparent background)
    Private ReadOnly PictureStream As New IO.MemoryStream(New Byte() _
    {&H89, &H50, &H4E, &H47, &HD, &HA, &H1A, &HA, &H0, &H0, &H0, &HD, &H49, &H48, &H44, &H52,
    &H0, &H0, &H0, &H60, &H0, &H0, &H0, &H60, &H8, &H6, &H0, &H0, &H0, &HE2, &H98, &H77,
    &H38, &H0, &H0, &H0, &H9, &H70, &H48, &H59, &H73, &H0, &H0, &HE, &HC3, &H0, &H0, &HE,
    &HC3, &H1, &HC7, &H6F, &HA8, &H64, &H0, &H0, &H3, &H2B, &H49, &H44, &H41, &H54, &H78, &H9C,
    &HED, &H9B, &HD1, &H95, &HAB, &H30, &HC, &H5, &HA9, &H8B, &H82, &HA8, &H87, &H6A, &H68, &H86,
    &H62, &HFC, &H12, &H20, &H9, &H1, &H3B, &H2B, &H81, &H75, &H26, &H79, &H97, &H8F, &HF9, &HD9,
    &H13, &H9B, &HC1, &HB2, &H6C, &H6F, &HAC, &H34, &H29, &HA5, &HE6, &H82, &H3, &H17, &H50, &H7,
    &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17,
    &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50,
    &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7,
    &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17,
    &H50, &H7, &H17, &H50, &H7, &H17, &H50, &H7, &H17, &HF8, &H1D, &H86, &HD4, &HDD, &H86, &HAB,
    &H1B, &H52, &HFA, &H81, &H0, &H8C, &HA9, &H6F, &H9B, &HD4, &HCC, &HB6, &H5F, &H30, &H78, &H15,
    &H18, &HBA, &HDB, &H68, &HB5, &HA9, &H1F, &H7F, &H21, &H0, &H63, &H9F, &HDA, &H5B, &HD7, &HED,
    &H6C, &HCB, &HF, &H5E, &H5, &HC6, &HBE, &HBD, &H8D, &H56, &H97, &HE6, &H4, &HF8, &HF6, &H0,
    &H4, &HCD, &H16, &H8E, &HB8, &H8C, &HE, &H11, &H1E, &HBA, &HE6, &HDC, &H6C, &H99, &H2, &HD8,
    &HAC, &H70, &HF4, &HB5, &H6B, &HDB, &HF8, &H27, &HC3, &HB6, &H8F, &HB6, &HD, &HCB, &HE8, &HAF,
    &H9B, &H2D, &HFB, &HE0, &H2D, &HFD, &HB5, &H7D, &H1A, &HD, &H41, &HB8, &H2F, &H15, &HDB, &H81,
    &HF2, &H4C, &H88, &HEC, &H67, &H97, &H80, &HD4, &HDE, &H80, &H83, &H2, &H30, &H9F, &H16, &HE,
    &HCD, &H96, &HD2, &HD2, &H75, &H76, &H49, &H9B, &HDA, &H1B, &H2, &H50, &H78, &H4E, &HD4, &HFA,
    &H1F, &H13, &H80, &HC3, &HB3, &HE5, &H43, &HE6, &H2C, &H9B, &HBA, &HB5, &HCF, &H79, &H16, &H6F,
    &HF8, &H33, &H83, &HCA, &HCF, &H9F, &HFA, &H33, &H66, &H20, &H1E, &H80, &HC3, &HB3, &HE5, &HD3,
    &H20, &H1B, &H33, &HE0, &H39, &HF0, &H6F, &H83, &H68, &H5C, &H12, &H8B, &HCF, &H9F, &HDB, &H47,
    &H9D, &HE8, &H2A, &H77, &HE8, &H5B, &HAF, &HAD, &H83, &H3C, &H5, &HF5, &HAF, &H3E, &H8B, &HED,
    &H8D, &H4B, &H62, &H29, &H73, &H9D, &HD9, &H7, &H7, &HE0, &HC4, &HFA, &H5F, &H7C, &H51, &H5B,
    &H9F, &HC5, &H8D, &H76, &HEA, &HD7, &HB0, &H7F, &H94, &H2, &H10, &H7C, &HA4, &HAE, &HDB, &HE1,
    &HA9, &HD9, &H92, &HCB, &H9E, &H79, &HF0, &H2D, &H27, &HAA, &H79, &HE9, &H7B, &H1F, &HA8, &HF9,
    &H6F, &HD6, &H13, &HD0, &HFE, &H59, &HCF, &HF6, &H41, &HEB, &H7F, &HF5, &H0, &HBC, &H5E, &H38,
    &H87, &H65, &H16, &H2D, &H41, &H58, &HB5, &HB3, &H7, &H73, &HDF, &HF6, &H9E, &H35, &HAE, &HD,
    &H74, &H7B, &HFE, &HEF, &HFA, &HF0, &HAF, &H54, &H42, &H3A, &HBD, &HB8, &H2, &HF0, &H33, &HE0,
    &H2, &HEA, &HE0, &H2, &HEA, &HE0, &H2, &HEA, &HE0, &H2, &HEA, &HE0, &H2, &HEA, &HE0, &H2,
    &HEA, &HE0, &H2, &HEA, &HE0, &H2, &HEA, &HE0, &H2, &HEA, &HE0, &H2, &HEA, &HE0, &H2, &HEA,
    &HE0, &H2, &HEA, &H38, &H3E, &HBC, &HFF, &HBA, &H37, &HF2, &H7B, &H72, &H15, &HDC, &HD, &H22,
    &H2B, &H4, &H14, &H71, &H36, &H38, &H71, &HE7, &H7B, &H51, &H23, &H0, &HFE, &H3B, &HDF, &HC7,
    &H8D, &HD4, &HF0, &H76, &H5B, &HB6, &H64, &HD0, &H72, &H85, &HB9, &HBE, &HC1, &H32, &HF5, &H1B,
    &H51, &HFD, &HE6, &H75, &H78, &H5C, &H61, &H1E, &HBA, &HBD, &H3B, &H1A, &H0, &H77, &HCD, &HCF,
    &H6B, &HDF, &H78, &HBD, &HD8, &H4A, &H7C, &H95, &H49, &H9E, &HEA, &HB5, &H3A, &HD5, &H6F, &H9B,
    &H80, &H39, &HDE, &HED, &H71, &HF5, &HBA, &HFE, &H6C, &HEE, &H4E, &HBA, &H7A, &H0, &HDC, &HEB,
    &H7F, &HF6, &H92, &H3E, &H5F, &HA7, &H73, &H7A, &H6F, &H39, &H59, &HFD, &HE6, &H6B, &H7F, &HBC,
    &H7A, &HE3, &H54, &H0, &HDC, &H15, &H62, &HD9, &H97, &HCD, &HFF, &HD0, &HC1, &HDB, &H37, &H53,
    &HFD, &HF6, &H69, &HF, &HC, &HF, &H80, &HBD, &H44, &HE4, &H41, &HB6, &HA0, &H2A, &H1B, &H14,
    &H7B, &H41, &H6F, &H64, &HF5, &H9B, &HB5, &H7D, &HB9, &HF2, &HC3, &HBF, &HF, &HD8, &H3, &HE0,
    &HAE, &HF9, &HF1, &H2C, &H35, &HC6, &H9F, &HFF, &H44, &H55, &HBF, &H59, &H9F, &H1F, &H50, &H25,
    &H67, &HFE, &HA0, &H7F, &H93, &HC9, &HF, &HCA, &H34, &H83, &HB7, &H33, &HCD, &H55, &HFB, &H9,
    &H56, &HBF, &H5, &HFC, &HF2, &HC7, &HFC, &HC1, &H9A, &HEB, &HFF, &HF6, &H5, &HAC, &H1B, &H70,
    &H44, &HF5, &HDB, &HF3, &H38, &H6A, &H7A, &HB7, &HC7, &HA9, &H2E, &H5F, &HC2, &H7E, &H24, &H30,
    &HC6, &H99, &HEF, &H3F, &H6F, &H67, &H7, &HB5, &H90, &HC2, &HD9, &HAC, &HF8, &H38, &H0, &H6C,
    &HF5, &HDB, &HB1, &H3, &HC0, &HC9, &HC, &HF8, &H6F, &H9, &HAE, &H7E, &HBE, &H2, &HB0, &HE2,
    &H9E, &H95, &H96, &HFF, &H49, &HAE, &H0, &H84, &H90, &HF9, &H36, &H17, &H9C, &HF9, &H82, &H1,
    &HF8, &H4E, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1,
    &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75,
    &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70,
    &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1,
    &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75, &H70, &H1, &H75,
    &H70, &H1, &H75, &HFE, &H1, &H31, &HBB, &H32, &H3A, &HEC, &H5C, &H4C, &H60, &H0, &H0, &H0,
    &H0, &H49, &H45, &H4E, &H44, &HAE, &H42, &H60, &H82})

#Region "Constructor"

    Public Sub New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.ContainerControl Or ControlStyles.SupportsTransparentBackColor, True)
        MyBase.DoubleBuffered = True
        DoubleBuffered = True
        BackColor = Color.Gainsboro
        Size = New Size(136, 136)
        Tmr = New Timer With {.Enabled = False}
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing Then
                If PictureStream IsNot Nothing Then
                    PictureStream.Dispose()
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region "Properties"

    Private imgSize As Single = 136 ' Initial size of the control
    Private img As Image = Nothing
    <Browsable(True), Category("Properties"), Description("Image to show and rotate. If none is selected then the built-in image will be used."), DefaultValue("")>
    Public Property RI_Image As Image
        Get
            Return img
        End Get
        Set(value As Image)
            If img IsNot value Then
                img = value
                If img IsNot Nothing Then
                    BackColor = Color.Transparent
                    '* To account for proper image rotation, the size of the control, both width and height, will be equal to
                    '* the loaded image's diagonal length measured from top-left corner to bottom-right corner
                    imgSize = Math.Sqrt(img.Width ^ 2 + img.Height ^ 2)
                Else
                    BackColor = Color.Gainsboro
                    '* Built-in image size is 96x96 which gives approximate diagonal length of 136.
                    imgSize = 136
                End If
                Size = New Size(imgSize, imgSize)
                Invalidate()
            End If
        End Set
    End Property

    Private m_Angle As Single
    <Browsable(True), Category("Properties"), Description("Angle of rotation (valid values -360 to 360)."), DefaultValue(0.0F)>
    Public Property RI_RotationAngle As Single
        Get
            Return m_Angle
        End Get
        Set(value As Single)
            If value < -360 OrElse value > 360 Then
                MessageBox.Show("Invalid value!")
                Exit Property
            End If
            If m_Angle <> value Then
                m_Angle = value
                Invalidate()
            End If
        End Set
    End Property

    Public Enum Direction
        Clockwise = 0
        Counterclockwise = 1
    End Enum

    Private m_direction As Direction = Direction.Clockwise
    <Browsable(True), Category("Properties"), Description("Direction of perpetual rotation (clockwise or counterclockwise)."), DefaultValue(Direction.Clockwise)>
    Public Property RI_PerpetualRotationDirection As Direction
        Get
            Return m_direction
        End Get
        Set(value As Direction)
            If m_direction <> value Then
                m_direction = value
                Invalidate()
            End If
        End Set
    End Property

    Private m_flipV As Boolean
    <Browsable(True), Category("Properties"), Description("Flip control vertically."), DefaultValue(False)>
    Public Property RI_FlipV As Boolean
        Get
            Return m_flipV
        End Get
        Set(value As Boolean)
            If m_flipV <> value Then
                m_flipV = value
                Invalidate()
            End If
        End Set
    End Property

    Private m_flipH As Boolean
    <Browsable(True), Category("Properties"), Description("Flip control horizontally."), DefaultValue(False)>
    Public Property RI_FlipH As Boolean
        Get
            Return m_flipH
        End Get
        Set(value As Boolean)
            If m_flipH <> value Then
                m_flipH = value
                Invalidate()
            End If
        End Set
    End Property

    Private m_Value As Boolean
    <Browsable(True), Category("Properties"), Description("Enable endless rotation."), DefaultValue(False)>
    Public Property Value As Boolean
        Get
            Return m_Value
        End Get
        Set(value As Boolean)
            If m_Value <> value Then
                m_Value = value
                If m_Value Then
                    If Not Tmr.Enabled Then Tmr.Enabled = True
                Else
                    Tmr.Enabled = False
                End If
                Invalidate()
            End If
        End Set
    End Property

    <Browsable(True), Category("Properties"), Description("Timer interval for endless rotation (valid values 5-10000)."), DefaultValue(100)>
    Public Property RI_PerpetualTimerInterval As Integer
        Get
            Return Tmr.Interval
        End Get
        Set(value As Integer)
            If value < 5 OrElse value > 10000 Then
                MessageBox.Show("Invalid value!")
                Exit Property
            End If
            If Tmr.Interval <> value Then
                Tmr.Interval = value
                Invalidate()
            End If
        End Set
    End Property

#End Region

#Region "Protected Metods"

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        '* Create a new bitmap to hold the image and allow for its manipulation before it is displayed
        Dim backImage As Bitmap

        If img IsNot Nothing Then
            backImage = New Bitmap(img)
        Else
            backImage = New Bitmap(PictureStream)
        End If

        '* Rotate the image by using the arbitrary angle set by user.

        '* Move the origin to the center of the control:
        e.Graphics.TranslateTransform(ClientRectangle.Width / 2.0F, ClientRectangle.Height / 2.0F)
        '* Rotate the image:
        e.Graphics.RotateTransform(-m_Angle)
        '* Move the origin back to to the upper-left corner of the control:
        e.Graphics.TranslateTransform(-ClientRectangle.Width / 2.0F, -ClientRectangle.Height / 2.0F)

        '* Limit painting region so corners do not hide other close controls.
        Dim CircularPath As New System.Drawing.Drawing2D.GraphicsPath
        CircularPath.AddEllipse(ClientRectangle)
        Region = New Region(CircularPath)

        '* Pass the current image to the FlipHV sub to flip it (if H or V flip is enabled) and display it
        FlipHV(e.Graphics, backImage, m_flipV, m_flipH)

        backImage.Dispose()

        '* Reset the transformation
        e.Graphics.ResetTransform()
    End Sub

#End Region

#Region "Private Methods"

    Private Sub RotatingImage_DoubleClick(sender As Object, e As EventArgs) Handles Me.DoubleClick
        Value = Not Value
    End Sub

    Private Sub RotatingImage_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        Width = Height
    End Sub

    Private Sub Tmr_Tick(sender As Object, e As EventArgs) Handles Tmr.Tick
        If m_Angle = 359 OrElse m_Angle = -359 Then
            m_Angle = 0
        Else
            If m_direction = Direction.Clockwise Then
                m_Angle -= 1
            Else
                m_Angle += 1
            End If
        End If
        Invalidate()
    End Sub

    '* Reference: https://msdn.microsoft.com/en-us/library/3b575a03(v=vs.110).aspx?cs-save-lang=1&cs-lang=vb#code-snippet-1
    Private Sub FlipHV(g As Graphics, img As Bitmap, flipV As Boolean, flipH As Boolean)
        '* Declare offset variables for proper scaling/positioning of the image
        Dim ScaleFactor As Single = Width / imgSize
        Dim Xoffset As Integer = (Width - img.Width * ScaleFactor) / 2
        Dim Yoffset As Integer = (Height - img.Height * ScaleFactor) / 2

        '* Original image points:   Upper-Left (Xoffset, Yoffset)
        '*                          Upper-Right (Width - Xoffset, Yoffset)
        '*                          Lower-Left (Xoffset, Height - Yoffset))
        '*
        '* Use points() array to store destination points for the above mentioned points of the original image

        '* No flipping - Destination Points are the same as original
        Dim points() As Point = {New Point(Xoffset, Yoffset), New Point(Width - Xoffset, Yoffset), New Point(Xoffset, Height - Yoffset)}

        '* Flip image horizontally - Destination Points: (Width - Xoffset, Yoffset); (Xoffset, Yoffset); (Width - Xoffset, Height - Yoffset)
        If flipH Then points = {New Point(Width - Xoffset, Yoffset), New Point(Xoffset, Yoffset), New Point(Width - Xoffset, Height - Yoffset)}

        '* Flip image vertically
        If flipV Then
            If flipH Then '* Account for horizontal flip
                '* Destination Points: (Width - Xoffset, Height - Yoffset); (Xoffset, Height - Yoffset); (Width - Xoffset, Yoffset)
                points = {New Point(Width - Xoffset, Height - Yoffset), New Point(Xoffset, Height - Yoffset), New Point(Width - Xoffset, Yoffset)}
            Else
                '* Destination Points: (Xoffset, Height - Yoffset); (Width - Xoffset, Height - Yoffset); (Xoffset, Yoffset)
                points = {New Point(Xoffset, Height - Yoffset), New Point(Width - Xoffset, Height - Yoffset), New Point(Xoffset, Yoffset)}
            End If
        End If

        '* Draw image using the resulting points() array
        g.DrawImage(img, points)
    End Sub

#End Region

End Class
