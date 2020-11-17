' This control is an array of standard button controls.
' Each button action is defined within the ButtonClick sub inside the Private Methods region.
' Follow the ButtonClick sub's code pattern to add/remove as many button actions as needed.

Imports System.ComponentModel

Public Class ButtonControlArray
    Inherits Panel

#Region "Constructor"

    Public Sub New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.ContainerControl Or ControlStyles.SupportsTransparentBackColor, True)
        MyBase.DoubleBuffered = True
        DoubleBuffered = True
        BackColor = Color.DarkBlue
        ForeColor = Color.Black
        MyBase.AutoSize = True
        MyBase.AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
        BorderStyle = BorderStyle.FixedSingle
        For i As Integer = 1 To m_ButtonNumber
            AddNewButton(i)
        Next
    End Sub

#End Region

#Region "Properties"

    <Browsable(False)>
    Public Overrides Property AutoSize As Boolean
        Get
            Return MyBase.AutoSize
        End Get
        Set(value As Boolean)
            If MyBase.AutoSize <> value Then MyBase.AutoSize = value
        End Set
    End Property

    <Browsable(False)>
    Public Overrides Property AutoSizeMode As AutoSizeMode
        Get
            Return MyBase.AutoSizeMode
        End Get
        Set(value As AutoSizeMode)
            If MyBase.AutoSizeMode <> value Then MyBase.AutoSizeMode = value
        End Set
    End Property

    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("The foreground color of the text."), DefaultValue(GetType(Color), "Black")>
    Public Overrides Property ForeColor() As Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(value As Color)
            If MyBase.ForeColor <> value Then
                MyBase.ForeColor = value
                If Controls.Count > 0 Then
                    For i = 0 To Controls.Count - 1
                        CType(Controls.Item(i), Button).ForeColor = MyBase.ForeColor
                    Next
                End If
                Invalidate()
            End If
        End Set
    End Property

    Enum CtrlOrientation
        Horizontal = 0
        Vertical = 1
    End Enum

    Private m_ctrlOrientation As CtrlOrientation = CtrlOrientation.Horizontal
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Orientation of the buttons within the control."), DefaultValue(CtrlOrientation.Horizontal)>
    Public Property ButtonOrientation As CtrlOrientation
        Get
            Return m_ctrlOrientation
        End Get
        Set(value As CtrlOrientation)
            If m_ctrlOrientation <> value Then
                m_ctrlOrientation = value
                currentButtonNum = m_ButtonNumber
                AddRemoveAll()
                Invalidate()
            End If
        End Set
    End Property

    Private currentButtonNum As Integer = 6
    Private m_ButtonNumber As Integer = 6
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Number of buttons in the array (valid values 2 to 60)."), DefaultValue(6)>
    Public Property ButtonNumber As Integer
        Get
            Return m_ButtonNumber
        End Get
        Set(value As Integer)
            If value < 2 Then value = 2
            If value > 60 Then value = 60
            If m_ButtonNumber <> value Then
                currentButtonNum = m_ButtonNumber
                m_ButtonNumber = value
                AddRemoveAll()
                Invalidate()
            End If
        End Set
    End Property

    Private m_ButtonWidth As Integer = 38
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Single button width (valid values 24 to 300)."), DefaultValue(38)>
    Public Property ButtonWidth As Integer
        Get
            Return m_ButtonWidth
        End Get
        Set(value As Integer)
            If value < 24 Then value = 24
            If value > 300 Then value = 300
            If m_ButtonWidth <> value Then
                m_ButtonWidth = value
                currentButtonNum = m_ButtonNumber
                AddRemoveAll()
                Invalidate()
            End If
        End Set
    End Property

    Private m_ButtonHeight As Integer = 28
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Single button height (valid values 24 to 300)."), DefaultValue(28)>
    Public Property ButtonHeight As Integer
        Get
            Return m_ButtonHeight
        End Get
        Set(value As Integer)
            If value < 24 Then value = 24
            If value > 300 Then value = 300
            If m_ButtonHeight <> value Then
                m_ButtonHeight = value
                currentButtonNum = m_ButtonNumber
                AddRemoveAll()
                Invalidate()
            End If
        End Set
    End Property

    Private m_ButtonSpacing As Integer = 2
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Spacing between individual buttons (valid values 2 to 10)."), DefaultValue(2)>
    Public Property ButtonSpacing() As Integer
        Get
            Return m_ButtonSpacing
        End Get
        Set(value As Integer)
            If value < 2 Then value = 2
            If value > 10 Then value = 10
            If m_ButtonSpacing <> value Then
                m_ButtonSpacing = value
                currentButtonNum = m_ButtonNumber
                AddRemoveAll()
                Invalidate()
            End If
        End Set
    End Property

    Private m_ButtonText As String = "B"
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Text to show on all buttons, followed by the button number"), DefaultValue("B")>
    Public Property ButtonText() As String
        Get
            Return m_ButtonText
        End Get
        Set(value As String)
            If m_ButtonText <> value Then
                m_ButtonText = value
                currentButtonNum = m_ButtonNumber
                AddRemoveAll()
                Invalidate()
            End If
        End Set
    End Property

    Private m_ButtonBackColor As Color = Color.Lime
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Use this color as the BackColor for all buttons (UseVisualStyleBackColor property must be set to False)."), DefaultValue(GetType(Color), "Lime")>
    Public Property ButtonBackColor() As Color
        Get
            Return m_ButtonBackColor
        End Get
        Set(value As Color)
            If m_ButtonBackColor <> value Then
                m_ButtonBackColor = value
                currentButtonNum = m_ButtonNumber
                AddRemoveAll()
                Invalidate()
            End If
        End Set
    End Property

    Private m_ButtonUseVisualStyleBackColor As Boolean = True
    <Browsable(True), RefreshProperties(RefreshProperties.All), Description("Use Visual Style Back Color for the buttons."), DefaultValue(True)>
    Public Property ButtonUseVisualStyleBackColor() As Boolean
        Get
            Return m_ButtonUseVisualStyleBackColor
        End Get
        Set(value As Boolean)
            If m_ButtonUseVisualStyleBackColor <> value Then
                m_ButtonUseVisualStyleBackColor = value
                currentButtonNum = m_ButtonNumber
                AddRemoveAll()
                Invalidate()
            End If
        End Set
    End Property

#End Region

#Region "Private Methods"

    Private Sub AddNewButton(i As Integer)
        Dim newArrayButton As New Button
        Controls.Add(newArrayButton)
        newArrayButton.Width = m_ButtonWidth
        newArrayButton.Height = m_ButtonHeight
        If m_ctrlOrientation = CtrlOrientation.Horizontal Then
            newArrayButton.Top = 4
            newArrayButton.Left = (Controls.Count - 1) * (m_ButtonWidth + m_ButtonSpacing) + 4
        Else
            newArrayButton.Top = (Controls.Count - 1) * (m_ButtonHeight + m_ButtonSpacing) + 4
            newArrayButton.Left = 4
        End If
        newArrayButton.BackColor = m_ButtonBackColor
        newArrayButton.ForeColor = ForeColor
        newArrayButton.Text = m_ButtonText & i.ToString
        newArrayButton.UseVisualStyleBackColor = m_ButtonUseVisualStyleBackColor
        newArrayButton.TextAlign = ContentAlignment.MiddleCenter
        AddHandler newArrayButton.Click, AddressOf ButtonClick 'Other event handlers can be added in the same manner
    End Sub

    'This sub will need to be modified to suit user's needs.
    'Different actions can be performed with a click of any of the buttons.
    'More or less buttons can be defined.
    Private Sub ButtonClick(sender As System.Object, e As System.EventArgs)
        If sender.GetType.GetProperty("Text").GetValue(sender, Nothing) = m_ButtonText & "1" Then
            MessageBox.Show("Button pressed - " & m_ButtonText & "1") '<-- Replace with your own action
        ElseIf sender.GetType.GetProperty("Text").GetValue(sender, Nothing) = m_ButtonText & "2" Then
            MessageBox.Show("Button pressed - " & m_ButtonText & "2") '<-- Replace with your own action
        ElseIf sender.GetType.GetProperty("Text").GetValue(sender, Nothing) = m_ButtonText & "3" Then
            MessageBox.Show("Button pressed - " & m_ButtonText & "3") '<-- Replace with your own action
        ElseIf sender.GetType.GetProperty("Text").GetValue(sender, Nothing) = m_ButtonText & "4" Then
            MessageBox.Show("Button pressed - " & m_ButtonText & "4") '<-- Replace with your own action
        ElseIf sender.GetType.GetProperty("Text").GetValue(sender, Nothing) = m_ButtonText & "5" Then
            MessageBox.Show("Button pressed - " & m_ButtonText & "5") '<-- Replace with your own action
        ElseIf sender.GetType.GetProperty("Text").GetValue(sender, Nothing) = m_ButtonText & "6" Then
            MessageBox.Show("Button pressed - " & m_ButtonText & "6") '<-- Replace with your own action
        End If
    End Sub

    Private Sub Remove()
        Controls.Remove(CType(Controls.Item(Controls.Count - 1), Button))
    End Sub

    Private Sub AddRemoveAll()
        For j As Integer = 1 To currentButtonNum
            Remove()
        Next
        For j As Integer = 1 To m_ButtonNumber
            AddNewButton(j)
        Next
    End Sub

#End Region

End Class