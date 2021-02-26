using CartRover.Batch.App.Model;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CartRover.Batch.App
{
    class CartRoverClient
    {
        Logger _log = LogManager.GetCurrentClassLogger();
        public CartRoverClient() { }
        public void GetCartRoverData(DateTime fromDate, DateTime toDate)
        {
            DateTime ProcStartDT = DateTime.Now;
            _log.Info("Start FGB.Order", false);
            OrderClient orderClient = new OrderClient();
            //DateTime dtLOD = oOrderClient.GetOrderProcessDT();
            /*DateTime dtfromOPD = dtLOD.AddDays(1); // TODO: Define the date
            DateTime dttoOPD = dtLOD.AddDays(2); // TODO: Define the date*/

            string sOPD = "from_date=" + System.Convert.ToString(fromDate.Year) + "-" + System.Convert.ToString(fromDate.Month) + "-" + System.Convert.ToString(toDate.Day) + "&to_date=" + System.Convert.ToString(toDate.Year) + "-" + System.Convert.ToString(toDate.Month) + "-" + System.Convert.ToString(toDate.Day);
            string sReqParam = "https://api.cartrover.com/v1/merchant/orders/list/any?" + sOPD + "&api_user=2057bJafr2Oq&api_key=7Co880XS9QorI50";
            WebRequest request = WebRequest.Create(sReqParam);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseFromServer = reader.ReadToEnd();
                    
                    _log.Info($"Cart Rover Response Received {responseFromServer}");

                    var oOrders = JsonConvert.DeserializeObject<Order>(responseFromServer);
                    _log.Info("Orders parsed", false);

                    if (oOrders.success_code)
                    {
                        _log.Info($"Orders parsed - Count: { oOrders.response.Count}");

                        foreach (Response oRes in oOrders.response)
                        {

                            // Dim oOrder As New fbOrder
                            // Missing a code to idenfify cuctomer.  If new create one in system
                            orderClient.ClientID = 4; // TODO: figure out if it is a new or existing customer
                            orderClient.ClientLocation = "7";
                            orderClient.EmpID = 11;
                            orderClient.OrderDate = Convert.ToDateTime(oRes.order_date); // TODO: Change it to correct val
                            orderClient.ShippedDate = Convert.ToDateTime(oRes.order_date); // TODO: Change it to correct val
                            orderClient.Taxes = oRes.sales_tax;
                            orderClient.PaymentType = oRes.pay_type;
                            orderClient.PaidDate = Convert.ToDateTime(oRes.order_date);  // TODO: Change it to correct val
                            orderClient.Notes = "CR Order";
                            orderClient.StatusID = 1;
                            orderClient.ExSource = oRes.order_source;
                            string sExSourceID;

                            if (oRes.order_source.Left(7) == "Shopify")
                            {
                                sExSourceID = oRes.record_no;
                                orderClient.ExSourceID = oRes.record_no;
                                _log.Info("Shopify record-no:" + oRes.record_no, false);
                            }
                            else if (oRes.order_source.Left(11) == "CommerceHub")
                            {
                                sExSourceID = oRes.po_number;
                                orderClient.ExSourceID = oRes.po_number;
                                _log.Info("CommerceHub Po_number:" + oRes.po_number, false);
                            }
                            else
                            {
                                oRes.order_source = "HSN";
                                sExSourceID = oRes.po_number;
                                orderClient.ExSourceID = oRes.po_number;
                                _log.Info("HSN Po_number:" + oRes.po_number, false);
                            }

                            if (orderClient.CheckDuplicateOrder(oRes.order_source, sExSourceID) >= 1)
                            {
                                orderClient.Save();
                                if (orderClient.OrderID > 0)
                                {
                                    _log.Info("Order " + orderClient.OrderID + " created ", false);
                                    int iOrderID = orderClient.OrderID;
                                    foreach (Item oItem in oRes.items)
                                    {
                                        OrderDetail oOrderDet = new OrderDetail();
                                        int iPID = oOrderDet.GetProdID(oItem.item);
                                        if (iPID > 0)
                                        {
                                            oOrderDet.OrderID = iOrderID;
                                            oOrderDet.ProductID = iPID;
                                            oOrderDet.Quantity = oItem.quantity;
                                            oOrderDet.UnitPrice = Convert.ToDecimal(oItem.price);
                                            oOrderDet.Discount = Convert.ToDecimal(oItem.discount);
                                            oOrderDet.SaveOrderDetail();
                                        }
                                        else
                                            _log.Error("Error occured identifying item ID:" + oItem.item + " for order: " + oOrderDet.OrderID);

                                        _log.Info("Product " + iPID + " added to order:" + oOrderDet.OrderID);
                                    }
                                }
                                else
                                _log.Error("Error occured creating order ", false);
                            }
                            else
                                _log.Warn("Order exist from Source " + oRes.order_source + "with ID:" + sExSourceID, false);
                        }
                        DateTime ProcEndDT = DateTime.Now;
                        orderClient.UpdateOrderProcLog(fromDate, "Success", oOrders.response.Count, ProcStartDT, ProcEndDT);
                        _log.Info("Done!", false);
                    }

                    reader.Close();

                }
                response.Close();
            }

        }
    }
}
