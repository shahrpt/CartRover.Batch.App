using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartRover.Batch.App
{
    class Program
    {
        public static string msLogFileName;
        static void Main(string[] args)
        {
            DateTime ProcStartDT = DateTime.Now;
            msLogFileName = @" C:\Log\FGB.Orders_" + Strings.Format(DateTime.Today, "ddMMyy") + Strings.Format(DateTime.Now, "hhmmss") + ".Log";
            AddToLog("Start FGB.Order", false);

            fbOrder oOrder = new fbOrder();
            DateTime dtLOD = oOrder.GetOrderProcessDT;
            DateTime dtfromOPD = dtLOD.AddDays(1); // TODO: Define the date
            DateTime dttoOPD = dtLOD.AddDays(2); // TODO: Define the date

            string sOPD = "from_date=" + System.Convert.ToString(dtfromOPD.Year) + "-" + System.Convert.ToString(dtfromOPD.Month) + "-" + System.Convert.ToString(dtfromOPD.Day) + "&to_date=" + System.Convert.ToString(dttoOPD.Year) + "-" + System.Convert.ToString(dttoOPD.Month) + "-" + System.Convert.ToString(dttoOPD.Day);
            string sReqParam = "https://api.cartrover.com/v1/merchant/orders/list/any?" + sOPD + "&api_user=2057bJafr2Oq&api_key=7Co880XS9QorI50";
            // Create a request for the URL. 
            // Dim request As WebRequest = WebRequest.Create("https://api.cartrover.com/v1/merchant/orders/list/any?api_user=2057bJafr2Oq&api_key=7Co880XS9QorI50")
            WebRequest request = WebRequest.Create(sReqParam);
            // Dim request As WebRequest = WebRequest.Create("https://api.cartrover.com/v1/merchant/orders/list/any?from_Date=2020-10-09&to_date=2020-10-09&api_user=2057bJafr2Oq&api_key=7Co880XS9QorI50")
            // Dim request As WebRequest = WebRequest.Create("https://api.cartrover.com/v1/merchant/inventory?api_user=8781XK9C83ua&api_key=xSYfJraQ55t9DRU")

            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            WebResponse response = request.GetResponse();

            Console.WriteLine((HttpWebResponse)response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            response.Close();

            AddToLog("Cart Rover Response Received", false);

            FBG_Order.Order oOrders = JsonConvert.DeserializeObject<FBG_Order.Order>(responseFromServer);

            AddToLog("Orders parsed", false);


            if (oOrders.success_code)
            {
                AddToLog("Orders parsed - Count:" + oOrders.response.Count, false);

                foreach (FBG_Order.Response oRes in oOrders.response)
                {

                    // Dim oOrder As New fbOrder
                    // Missing a code to idenfify cuctomer.  If new create one in system
                    oOrder.ClientID = 4; // TODO: figure out if it is a new or existing customer
                    oOrder.ClientLocation = 7;
                    oOrder.EmpID = 11;
                    oOrder.OrderDate = oRes.order_date; // TODO: Change it to correct val
                    oOrder.ShippedDate = oRes.order_date; // TODO: Change it to correct val
                    oOrder.Taxes = oRes.sales_tax;
                    oOrder.PaymentType = oRes.pay_type;
                    oOrder.PaidDate = oRes.order_date;  // TODO: Change it to correct val
                    oOrder.Notes = "CR Order";
                    oOrder.StatusID = 1;
                    oOrder.ExSource = oRes.order_source;
                    string sExSourceID;
                    if (Left(oRes.order_source, 7) == "Shopify")
                    {
                        sExSourceID = oRes.record_no;
                        oOrder.ExSourceID = oRes.record_no;
                        AddToLog("Shopify record-no:" + oRes.record_no, false);
                    }
                    else if (Left(oRes.order_source, 11) == "CommerceHub")
                    {
                        sExSourceID = oRes.po_number;
                        oOrder.ExSourceID = oRes.po_number;
                        AddToLog("CommerceHub Po_number:" + oRes.po_number, false);
                    }
                    else
                    {
                        oRes.order_source = "HSN";
                        sExSourceID = oRes.po_number;
                        oOrder.ExSourceID = oRes.po_number;
                        AddToLog("HSN Po_number:" + oRes.po_number, false);
                    }

                    if (oOrder.CheckDuplicateOrder(oRes.order_source, sExSourceID) == false)
                    {
                        oOrder.Save();
                        if (oOrder.OrderID > 0)
                        {
                            AddToLog("Order " + oOrder.OrderID + " created ", false);
                            int iOrderID = oOrder.OrderID;


                            foreach (FBG_Order.Item oItem in oRes.items)
                            {
                                fbOrderDetail oOrderDet = new fbOrderDetail();
                                int iPID = oOrderDet.GetProdID(oItem.item);
                                if (iPID > 0)
                                {
                                    oOrderDet.OrderID = iOrderID;
                                    oOrderDet.ProductID = iPID;
                                    oOrderDet.Quantity = oItem.quantity;
                                    oOrderDet.UnitPrice = oItem.price;
                                    oOrderDet.Discount = oItem.discount;

                                    oOrderDet.Save();
                                }
                                else
                                    AddToLog("Error occured identifying item ID:" + oItem.item + " for order: " + oOrder.OrderID, false);

                                AddToLog("Product " + iPID + " added to order:" + oOrder.OrderID, false);
                            }
                        }
                        else
                            // TODO: Add better commen exception deteail in log
                            AddToLog("Error occured creating order ", false);
                    }
                    else
                        AddToLog("Order exist from Source " + oRes.order_source + "with ID:" + sExSourceID, false);
                }
                DateTime ProcEndDT = DateTime.Now;
                oOrder.UpdateOrderProcLog(dtfromOPD, "Success", oOrders.response.Count, ProcStartDT, ProcEndDT);
                AddToLog("Done!", false);
            }
        }
        
    }
}
