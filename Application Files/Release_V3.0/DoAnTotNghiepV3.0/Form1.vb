Imports System.IO

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim tutu As StreamWriter
        tutu = New StreamWriter("C:\Users\Lenovo\Desktop\LazyPipeGcode.txt")

        Dim dkA, dkB, R As Single                   ' đường kính, bán kính
        Dim y, ax, bx, beginA, beginY As Single     ' các biến của PT bậc 2
        Dim delta, a, b, c, d As Single             ' các biến của PT đường tròn
        Dim x1, xA, xB, xC As Single                ' tọa độ điểm thứ 2 cắt đoạn
        Dim x2, yA, yB, yC As Single                ' tọa độ điểm thứ 3 cắt đoạn
        Dim PI, alpha, Adb, rad, luongiac, luonggiac As Single
        Dim h, k, heso As Single
        'Dim Axis As Single              ' Trục A

        dkA = Val(TextBox1.Text)
        dkB = Val(TextBox2.Text)
        alpha = Val(TextBox3.Text)
        a = Val(TextBox4.Text)

        Adb = 180 * (dkB / dkA)                                 ' góc quay tương ứng đường kính ống B
        PI = 3.14159265
        rad = PI / 180                                          ' chuyển đổi radian
        luongiac = Math.Sin(alpha * rad)                        ' sin alpha
        R = Adb / 2                                             ' bán kính R
        b = 180 / 2                                             ' I(a,b)
        c = Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(R, 2)    ' c trong PTĐT
        beginA = (180 - Adb) / 2                                ' điểm bắt đầu cắt giao tuyến theo độ
        'beginY = (beginA * dkA) / 180                           ' điểm bắt đầu cắt giao tuyến theo Y

        'heso = (dkB / Adb)                                      ' tỷ số mm/độ => 1 độ ứng với số mm
        'h = y + heso
        'k = y - heso
        '--
        luonggiac = Math.Tan(alpha * rad) ' tan alpha
        xA = a                            ' tọa độ bắt đầu cắt đoạn
        xB = dkA / luonggiac              ' tọa độ thứ 2 cắt đoạn
        yB = dkA
        xC = xA                           ' tọa độ thứ 3 cắt đoạn
        yA = 0
        yC = dkA * 2
        '--
        tutu.WriteLine("G0 " + "X0" + " Y0" + " Z-5")
        tutu.WriteLine("G0 " + "F1000")

        If ComboBox1.SelectedItem = "Cắt giao tuyến" Then

            If ComboBox2.SelectedItem = "Ống A" Then

                For y = beginA To Adb + beginA Step +1

                    d = Math.Pow(y, 2) - (2 * b * y) + c
                    ax = 1
                    bx = -2 * a
                    'Axis = (y * Adb) / dkA
                    delta = Math.Pow(bx, 2) - (4 * ax * d)

                    If delta = 0 Then
                        x1 = x2 = ((-bx) / (2 * ax)) / luongiac
                        tutu.WriteLine("G1 " + "X" & x1.ToString() + " Y" & y.ToString() + " Z0")
                        'tutu.WriteLine("G1 " + "X" & x1.ToString() + " A" & Axis.ToString())

                    ElseIf delta > 0 Then
                        x1 = (((-bx) + Math.Sqrt(delta)) / (2 * ax)) / luongiac
                        x2 = (((-bx) - Math.Sqrt(delta)) / (2 * ax)) / luongiac
                        tutu.WriteLine("G1 " + "X" & x1.ToString() + " Y" & y.ToString() + " Z0")
                        'tutu.WriteLine("G1 " + "X" & x1.ToString() + " A" & Axis.ToString())

                    End If

                Next y

                For y = Adb + beginA To beginA Step -1

                    d = Math.Pow(y, 2) - (2 * b * y) + c
                    ax = 1
                    bx = -2 * a
                    'Axis = (y * Adb) / dkA
                    delta = Math.Pow(bx, 2) - (4 * ax * d)

                    If delta = 0 Then
                        x1 = x2 = ((-bx) / (2 * ax)) / luongiac
                        tutu.WriteLine("G1 " + "X" & x2.ToString() + " Y" & y.ToString() + " Z0")
                        'tutu.WriteLine("G1 " + "X" & x2.ToString() + " A" & Axis.ToString())

                    ElseIf delta > 0 Then
                        x1 = (((-bx) + Math.Sqrt(delta)) / (2 * ax)) / luongiac
                        x2 = (((-bx) - Math.Sqrt(delta)) / (2 * ax)) / luongiac
                        tutu.WriteLine("G1 " + "X" & x2.ToString() + " Y" & y.ToString() + " Z0")
                        'tutu.WriteLine("G1 " + "X" & x2.ToString() + " A" & Axis.ToString())

                    End If

                Next y



            ElseIf ComboBox2.SelectedItem = "Ống B" Then

                For y = 0 To dkA Step h

                Next y
                '--
                For y = dkA To dkA * 2 Step h

                Next y

            Else
                MessageBox.Show("Xin chọn ống")

            End If


            '---------------------------------------------------------------------------------------
        ElseIf ComboBox1.SelectedItem = "Cắt đoạn" Then

            For y = yA To yB

                'Axis = (y * Adb) / dkB

                x1 = (-y * (xB - xA) - (yB * xA) + (yA * xB)) / (yA - yB)
                tutu.WriteLine("G1 " + "X" & x1.ToString() + " Y" & y.ToString() + " Z0")
                'tutu.WriteLine("G1 " + "X" & x1.ToString() + " A" & Axis.ToString())

            Next y

            For y = yB To yC

                'Axis = (y * Adb) / dkB

                x1 = (-y * (xC - xB) - (yC * xB) + (yB * xC)) / (yB - yC)
                tutu.WriteLine("G1 " + "X" & x1.ToString() + " Y" & y.ToString() + " Z0")
                'tutu.WriteLine("G1 " + "X" & x1.ToString() + " A" & Axis.ToString())

            Next y

            '---------------------------------------------------------------------------------------
        Else
            MessageBox.Show("Xin chọn Chế độ cắt")

        End If

        tutu.WriteLine("G0 Z-5")
        tutu.WriteLine("G28")
        tutu.WriteLine("M2")
        tutu.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()

    End Sub
End Class
