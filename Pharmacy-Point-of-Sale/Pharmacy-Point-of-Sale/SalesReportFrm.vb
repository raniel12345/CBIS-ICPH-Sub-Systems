﻿Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Threading

Public Class SalesReportFrm

    Private salesReportController As New SalesReportController
    Private LoadDailyRepThread As Thread
    Private LoadWeeklyRepThread As Thread
    Private LoadWeeklyRepSummaryThread As Thread
    Private LoadDailyRepSummaryThread As Thread
    Private GenerateExcelRepThread As Thread

    Private Sub SalesReportFrm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.CheckForIllegalCrossThreadCalls = False
    End Sub

    Public Sub DisplayStockOutTransactionData(ByVal totalSalesPrice As Decimal, ByVal id As Integer, ByVal transDate As String, ByVal patientType As String, ByVal price As Decimal, ByVal quantity As Integer, ByVal itemName As String, ByVal username As String)
        SalesTransactionDGV.ForeColor = Color.Black
        'Dim medItems As New DataTable("stockOutTransactions")
        Dim TotalPrice As Decimal

        Try
            TotalPrice = Decimal.Parse(price * quantity)
            SalesTransactionDGV.Rows.Add(id, transDate, itemName, price, quantity, TotalPrice, patientType, username)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error: 0x101 - displaying stock out transactions", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Me.InPatientTotalTxt.Text = totalSalesPrice
    End Sub

    Public Sub DisplayStockOutTransactionDataForSummary(ByVal totalSalesPrice As Decimal, ByVal id As Integer, ByVal transDate As String, ByVal patientType As String, ByVal price As Decimal, ByVal quantity As Integer, ByVal TotalPrice As Decimal, ByVal itemName As String, ByVal username As String)
        SalesTransactionDGV.ForeColor = Color.Black
        Dim medItems As New DataTable("stockOutTransactions")
        'Dim TotalPrice As Decimal

        Try
            'TotalPrice = Decimal.Parse(price * quantity)
            SalesTransactionDGV.Rows.Add(id, transDate, itemName, price, quantity, TotalPrice, patientType, username)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error: 0x101 - displaying stock out transactions", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Me.InPatientTotalTxt.Text = totalSalesPrice
    End Sub

    Private Sub GenerateDailyReports()
        Dim selectedDate As String = DailySalesDatePicker.Value.Date

        Dim inOrOutP As String = ""

        If Me.InPRbtn.Checked = True Then
            inOrOutP = "IN"
        End If

        If Me.outPRbtn.Checked = True Then
            inOrOutP = "OUT"
        End If

        If Me.allRbtn.Checked = True Then
            inOrOutP = ""
        End If

        Me.SalesTransactionDGV.Rows.Clear()
        Me.Cursor = Cursors.WaitCursor
        Me.salesReportController.GetDailyReports(AddressOf Me.DisplayStockOutTransactionData, selectedDate, inOrOutP)
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub LoadDailyReportsBtn_Click(sender As System.Object, e As System.EventArgs) Handles LoadDailyReportsBtn.Click
        Try
            Me.LoadDailyRepThread = New Thread(AddressOf Me.GenerateDailyReports)
            Me.LoadDailyRepThread.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Load Daily Report Button", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GenerateWeeMonYearReports()
        Try
            Dim startDate As String = weeklyStartDateTime.Value.Date
            Dim endDate As String = weeklyEndDateTime.Value.Date

            Dim inOrOutP As String = ""

            If Me.InPRbtn.Checked = True Then
                inOrOutP = "IN"
            End If

            If Me.outPRbtn.Checked = True Then
                inOrOutP = "OUT"
            End If

            If Me.allRbtn.Checked = True Then
                inOrOutP = ""
            End If

            Me.SalesTransactionDGV.Rows.Clear()
            Me.Cursor = Cursors.WaitCursor
            Me.salesReportController.GetWeeklyReports(AddressOf Me.DisplayStockOutTransactionData, startDate, endDate, inOrOutP)
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Load Weekly report button (2)", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub WeeklyReportBtn_Click(sender As System.Object, e As System.EventArgs) Handles WeeklyReportBtn.Click
        Try
            Me.LoadWeeklyRepThread = New Thread(AddressOf Me.GenerateWeeMonYearReports)
            Me.LoadWeeklyRepThread.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Weekly Report button", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GenerateWeeMonYearSummaryReports()
        'GetWeeklySummaryReports
        Try
            Dim startDate As String = weeklyStartDateTime.Value.Date
            Dim endDate As String = weeklyEndDateTime.Value.Date

            Dim inOrOutP As String = ""

            If Me.InPRbtn.Checked = True Then
                inOrOutP = "IN"
            End If

            If Me.outPRbtn.Checked = True Then
                inOrOutP = "OUT"
            End If

            If Me.allRbtn.Checked = True Then
                inOrOutP = ""
            End If

            'If weeklyStartDateTime.Value.Year <= weeklyEndDateTime.Value.Year Then
            Me.SalesTransactionDGV.Rows.Clear()
            Me.Cursor = Cursors.WaitCursor
            Me.salesReportController.GetWeeklySummaryReports(AddressOf Me.DisplayStockOutTransactionDataForSummary, startDate, endDate, inOrOutP)
            Me.Cursor = Cursors.Default
            'Else
            '    MessageBox.Show("Starting year must me grater than ending year", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Load Weekly report button (2)", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub WMYSummaryReportBtn_Click(sender As System.Object, e As System.EventArgs) Handles WMYSummaryReportBtn.Click
        Try
            Me.LoadWeeklyRepSummaryThread = New Thread(AddressOf Me.GenerateWeeMonYearSummaryReports)
            Me.LoadWeeklyRepSummaryThread.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Weekly/Monthly/Yearly Report button", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GenerateDailySummaryReports()
        Dim selectedDate As String = DailySalesDatePicker.Value.Date

        Dim inOrOutP As String = ""

        If Me.InPRbtn.Checked = True Then
            inOrOutP = "IN"
        End If

        If Me.outPRbtn.Checked = True Then
            inOrOutP = "OUT"
        End If

        If Me.allRbtn.Checked = True Then
            inOrOutP = ""
        End If

        Me.SalesTransactionDGV.Rows.Clear()
        Me.Cursor = Cursors.WaitCursor
        Me.salesReportController.GetDailySummaryReports(AddressOf Me.DisplayStockOutTransactionDataForSummary, selectedDate, inOrOutP)
        Me.Cursor = Cursors.Default
    End Sub


    Private Sub LoadDailySummaryReportsBtn_Click(sender As System.Object, e As System.EventArgs) Handles LoadDailySummaryReportsBtn.Click
        Try
            Me.LoadDailyRepSummaryThread = New Thread(AddressOf Me.GenerateDailySummaryReports)
            Me.LoadDailyRepSummaryThread.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Load Daily Summary Reports button", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GenerateExcelFileReports() As Boolean
        Try

            If SalesTransactionDGV.RowCount > 0 Then
                If Me.reportFileNameTxt.Text <> "" Then
                    Dim envUsername As String = Environment.UserName
                    Dim filename As String = Me.reportFileNameTxt.Text
                    Dim path As String = "C:\Users\" & envUsername & "\Documents\" & filename & ".xlsx"
                    MessageBox.Show("You can find the file in - " & path, "File path", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Me.Cursor = Cursors.WaitCursor


                    Dim APP As New Excel.Application
                    Dim worksheet As Excel.Worksheet
                    Dim workbook As Excel.Workbook
                    Dim misValue As Object = System.Reflection.Missing.Value
                    Dim i As Integer
                    Dim j As Integer

                    APP = New Excel.ApplicationClass
                    workbook = APP.Workbooks.Add(misValue)
                    worksheet = workbook.Sheets("sheet1")


                    For i = 0 To SalesTransactionDGV.RowCount - 1
                        For j = 0 To SalesTransactionDGV.ColumnCount - 1
                            For k As Integer = 1 To SalesTransactionDGV.Columns.Count
                                worksheet.Cells(1, k) = SalesTransactionDGV.Columns(k - 1).HeaderText
                                worksheet.Cells(i + 2, j + 1) = SalesTransactionDGV(j, i).Value.ToString()
                            Next
                        Next
                    Next

                    worksheet.SaveAs(path) '"C:\Users\Raniel\Desktop\test.xlsx"
                    workbook.Close()
                    APP.Quit()

                    Me.releaseObject(APP)
                    Me.releaseObject(workbook)
                    Me.releaseObject(worksheet)

                    Me.Cursor = Cursors.Default

                    MessageBox.Show("Done generating report", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return True
                    'If Me.GenerateExcelFileReports(path) Then
                    '    MessageBox.Show("Done generating report", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'End If
                Else
                    MessageBox.Show("Please enter the file name", "Missing File name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Generate excel file", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return False
    End Function

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub PrintBtn_Click(sender As System.Object, e As System.EventArgs) Handles PrintBtn.Click
        Try
            Me.GenerateExcelRepThread = New Thread(AddressOf Me.GenerateExcelFileReports)
            Me.GenerateExcelRepThread.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Generate Excel report button", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class