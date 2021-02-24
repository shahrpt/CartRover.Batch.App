Imports System.IO
Imports System.Net
Imports Newtonsoft.Json
'Imports RA.Platform
Imports Common2


Module Main

    Public msLogFileName As String
    Sub Main()
        Dim ProcStartDT As Date = Now
        msLogFileName = " C:\Log\FGB.Orders_" & Format(DateTime.Today, "ddMMyy") & Format(DateTime.Now, "hhmmss") & ".Log"
        AddToLog("Start FGB.Order", False)

        Dim oOrder As New fbOrder
        Dim dtLOD As Date = oOrder.GetOrderProcessDT
        Dim dtfromOPD As Date = dtLOD.AddDays(1) 'TODO: Define the date
        Dim dttoOPD As Date = dtLOD.AddDays(2) ' TODO: Define the date

        Dim sOPD As String = "from_date=" & CStr(dtfromOPD.Year) & "-" & CStr(dtfromOPD.Month) & "-" & CStr(dtfromOPD.Day) & "&to_date=" & CStr(dttoOPD.Year) & "-" & CStr(dttoOPD.Month) & "-" & CStr(dttoOPD.Day)
        Dim sReqParam As String = "https://api.cartrover.com/v1/merchant/orders/list/any?" & sOPD & "&api_user=2057bJafr2Oq&api_key=7Co880XS9QorI50"
        ' Create a request for the URL. 
        'Dim request As WebRequest = WebRequest.Create("https://api.cartrover.com/v1/merchant/orders/list/any?api_user=2057bJafr2Oq&api_key=7Co880XS9QorI50")
        Dim request As WebRequest = WebRequest.Create(sReqParam)
        'Dim request As WebRequest = WebRequest.Create("https://api.cartrover.com/v1/merchant/orders/list/any?from_Date=2020-10-09&to_date=2020-10-09&api_user=2057bJafr2Oq&api_key=7Co880XS9QorI50")
        'Dim request As WebRequest = WebRequest.Create("https://api.cartrover.com/v1/merchant/inventory?api_user=8781XK9C83ua&api_key=xSYfJraQ55t9DRU")

        ' If required by the server, set the credentials.
        request.Credentials = CredentialCache.DefaultCredentials

        Dim response As WebResponse = request.GetResponse()

        Console.WriteLine(CType(response, HttpWebResponse).StatusDescription)
        ' Get the stream containing content returned by the server.
        Dim dataStream As Stream = response.GetResponseStream()
        ' Open the stream using a StreamReader for easy access.
        Dim reader As New StreamReader(dataStream)
        Dim responseFromServer As String = reader.ReadToEnd()
        Console.WriteLine(responseFromServer)
        reader.Close()
        response.Close()

        AddToLog("Cart Rover Response Received", False)

        Dim oOrders As FBG_Order.Order = JsonConvert.DeserializeObject(Of FBG_Order.Order)(responseFromServer)

        AddToLog("Orders parsed", False)


        If oOrders.success_code Then

            AddToLog("Orders parsed - Count:" & oOrders.response.Count, False)

            For Each oRes As FBG_Order.Response In oOrders.response

                ' Dim oOrder As New fbOrder
                'Missing a code to idenfify cuctomer.  If new create one in system
                oOrder.ClientID = 4 'TODO: figure out if it is a new or existing customer
                oOrder.ClientLocation = 7
                oOrder.EmpID = 11
                oOrder.OrderDate = oRes.order_date 'TODO: Change it to correct val
                oOrder.ShippedDate = oRes.order_date 'TODO: Change it to correct val
                oOrder.Taxes = oRes.sales_tax
                oOrder.PaymentType = oRes.pay_type
                oOrder.PaidDate = oRes.order_date  'TODO: Change it to correct val
                oOrder.Notes = "CR Order"
                oOrder.StatusID = 1
                oOrder.ExSource = oRes.order_source
                Dim sExSourceID As String
                If Left(oRes.order_source, 7) = "Shopify" Then
                    sExSourceID = oRes.record_no
                    oOrder.ExSourceID = oRes.record_no
                    AddToLog("Shopify record-no:" & oRes.record_no, False)
                ElseIf Left(oRes.order_source, 11) = "CommerceHub" Then
                    sExSourceID = oRes.po_number
                    oOrder.ExSourceID = oRes.po_number
                    AddToLog("CommerceHub Po_number:" & oRes.po_number, False)
                Else
                    oRes.order_source = "HSN"
                    sExSourceID = oRes.po_number
                    oOrder.ExSourceID = oRes.po_number
                    AddToLog("HSN Po_number:" & oRes.po_number, False)
                End If

                If oOrder.CheckDuplicateOrder(oRes.order_source, sExSourceID) = False Then

                    oOrder.Save()
                    If oOrder.OrderID > 0 Then
                        AddToLog("Order " & oOrder.OrderID & " created ", False)
                        Dim iOrderID As Integer = oOrder.OrderID


                        For Each oItem As FBG_Order.Item In oRes.items

                            Dim oOrderDet As New fbOrderDetail
                            Dim iPID As Integer = oOrderDet.GetProdID(oItem.item)
                            If iPID > 0 Then
                                oOrderDet.OrderID = iOrderID
                                oOrderDet.ProductID = iPID
                                oOrderDet.Quantity = oItem.quantity
                                oOrderDet.UnitPrice = oItem.price
                                oOrderDet.Discount = oItem.discount

                                oOrderDet.Save()
                            Else
                                AddToLog("Error occured identifying item ID:" & oItem.item & " for order: " & oOrder.OrderID, False)
                            End If

                            AddToLog("Product " & iPID & " added to order:" & oOrder.OrderID, False)
                        Next
                    Else
                        'TODO: Add better commen exception deteail in log
                        AddToLog("Error occured creating order ", False)
                    End If

                Else
                    AddToLog("Order exist from Source " & oRes.order_source & "with ID:" & sExSourceID, False)
                End If

            Next
            Dim ProcEndDT As Date = Now
            oOrder.UpdateOrderProcLog(dtfromOPD, "Success", oOrders.response.Count, ProcStartDT, ProcEndDT)
            AddToLog("Done!", False)
        End If



    End Sub

    Private Sub checkit()

        'Dim oOrder As New fbOrder
        'If oOrder.CheckDuplicateOrder("Shopify", "2788017537186") = False Then

        'End If
    End Sub
    Public Sub AddToLog(ByVal Message As String, ByVal IsError As Boolean)
        MIO.WriteToFile(msLogFileName, Now.ToString & " - " & Message, True)
    End Sub

End Module
