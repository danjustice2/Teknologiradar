Sub ConvertExcelToJSON()
    Dim jsonOutput As String
    Dim lastRow As Long
    Dim techName As String
    Dim oea As Integer
    Dim tm As Integer
    Dim bu As Integer
    Dim velfaerd As Integer
    Dim ks As Integer
    Dim komstab As Integer
    Dim maturity As Integer
    Dim maxAdminValue As Integer
    Dim desktopPath As String
    Dim outputFileName As String
    Dim outputFilePath As String
    Dim isFirstTech As Boolean
    
    ' Set the output file name and path
    desktopPath = Environ("USERPROFILE") & "\Desktop\"
    outputFileName = "technologies.json"
    outputFilePath = desktopPath & outputFileName
    
    ' Clear any existing content in the output file
    Open outputFilePath For Output As #1
    Close #1
    
    ' Initialize the JSON output string
    jsonOutput = "{""technologies"": ["
    
    ' Find the last row of data in the "Overordnet" worksheet
    lastRow = Sheets("Overordnet").Cells(Rows.Count, "B").End(xlUp).row
    
    ' Loop through each row of data starting from row 9
    For i = 9 To lastRow
        ' Read the values of each column for the current row
        MsgBox techName
        techName = Sheets("Overordnet").Cells(i, "B").Value
        oea = Sheets("Overordnet").Cells(i, "E").Value
        tm = Sheets("Overordnet").Cells(i, "F").Value
        bu = Sheets("Overordnet").Cells(i, "G").Value
        velfaerd = Sheets("Overordnet").Cells(i, "H").Value
        ks = Sheets("Overordnet").Cells(i, "I").Value
        komstab = Sheets("Overordnet").Cells(i, "J").Value
        
        ' Skip the row if the techName is empty
        If techName = "" Then
            GoTo NextI
        End If
        
        ' Find the maximum value among the administrative categories
        maxAdminValue = WorksheetFunction.Max(oea, tm, bu, velfaerd, ks, komstab)
        
        ' Calculate the maturity value by subtracting 1 from the maximum administrative value
        maturity = maxAdminValue - 1
        
        ' Initialize the JSON object for the current technology
        If i <> 9 Then
            jsonOutput = jsonOutput & ","
        End If
        jsonOutput = jsonOutput & "{""techName"": """ & techName & """, ""x"": 0, ""y"": 0, ""description"": """", ""administrations"": {"
        
        ' Add the administrative categories to the JSON object
        jsonOutput = jsonOutput & """oea"": " & IIf(oea = maxAdminValue, "true", "false") & ","
        jsonOutput = jsonOutput & """tm"": " & IIf(tm = maxAdminValue, "true", "false") & ","
        jsonOutput = jsonOutput & """bu"": " & IIf(bu = maxAdminValue, "true", "false") & ","
        jsonOutput = jsonOutput & """velfaerd"": " & IIf(velfaerd = maxAdminValue, "true", "false") & ","
        jsonOutput = jsonOutput & """ks"": " & IIf(ks = maxAdminValue, "true", "false") & ","
        jsonOutput = jsonOutput & """komstab"": " & IIf(komstab = maxAdminValue, "true", "false") & "},"
        ' Add the link and maturity fields to the JSON object
        jsonOutput = jsonOutput & """link"": """", ""maturity"": " & maturity & "}"
NextI:
    Next i
    
    ' Complete the JSON output string
    jsonOutput = jsonOutput & "]}"
    
    ' Write the JSON output to the output file
    Open outputFilePath For Output As #1
    Print #1, jsonOutput
    Close #1
    
    MsgBox "Created JSON file at" + outputFilePath
End Sub

