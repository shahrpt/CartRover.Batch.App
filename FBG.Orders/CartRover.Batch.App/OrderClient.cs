using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartRover.Batch.App
{
    class OrderClient
    {
        private int _OrderID;
        private int _EmpID;
        private int _ClientID;
        private DateTime _OrderDate;
        private DateTime _ShippedDate;
        private string _ClientLocation;
        private string _Taxes;
        private string _PaymentType;
        private DateTime _PaidDate;
        private string _Notes;
        private int _StatusID;
        private string _ExSource;
        private string _ExSourceID;


        public System.Int32 OrderID
        {
            get
            {
                return _OrderID;
            }
        }

        public int EmpID
        {
            get
            {
                return _EmpID;
            }
            set
            {
                _EmpID = value;
            }
        }

        public int ClientID
        {
            get
            {
                return _ClientID;
            }
            set
            {
                _ClientID = value;
            }
        }

        public DateTime OrderDate
        {
            get
            {
                return _OrderDate;
            }
            set
            {
                _OrderDate = value;
            }
        }

        public DateTime ShippedDate
        {
            get
            {
                return _ShippedDate;
            }
            set
            {
                _ShippedDate = value;
            }
        }

        public string ClientLocation
        {
            get
            {
                return _ClientLocation;
            }
            set
            {
                _ClientLocation = value;
            }
        }

        public string Taxes
        {
            get
            {
                return _Taxes;
            }
            set
            {
                _Taxes = value;
            }
        }

        public string PaymentType
        {
            get
            {
                return _PaymentType;
            }
            set
            {
                _PaymentType = value;
            }
        }

        public DateTime PaidDate
        {
            get
            {
                return _PaidDate;
            }
            set
            {
                _PaidDate = value;
            }
        }

        public string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                _Notes = value;
            }
        }

        public System.Int32 StatusID
        {
            get
            {
                return _StatusID;
            }
            set
            {
                _StatusID = value;
            }
        }

        public string ExSource
        {
            get
            {
                return _ExSource;
            }
            set
            {
                _ExSource = value;
            }
        }
        public string ExSourceID
        {
            get
            {
                return _ExSourceID;
            }
            set
            {
                _ExSourceID = value;
            }
        }

        public void Save()
        {
            try
            {
                using (OleDbConnection connection = Common.DBConnection())
                {
                    // Open connecton    
                    connection.Open();
                    // Execute command  
                    // Create a command and set its connection    
                    OleDbCommand cmd = new OleDbCommand();
                    ///cmd.CommandText = "INSERT INTO T_Orders1 " & "([OEmpID], [OClientID], [ODate], [OShippedDate], [OClientLocation], [OTaxes], [OPaymentType], [OPaidDate], [ONotes], [OStatus ID]) " & "VALUES(@EmpID, @ClientID, @OrderDate, @ShippedDate, @ClientLocation, @Taxes, @PaymentType, @PaidDate, @Notes, @StatusID)"
                    cmd.CommandText = "INSERT INTO T_Orders " + "([OEmpID], [OClientID], [ODate],[OShippedDate], [OClientLocation], [OTaxes], [OPaymentType], [OPaidDate],[ONotes],[OStatusID], [OExSource], [OExSourceID]) " + "VALUES(@EmpID, @ClientID, @OrderDate,@ShippedDate, @ClientLocation, @Taxes, @PaymentType, @PaidDateD,@Notes,@StatusID,@ExSource, @ExSourceID)";


                    // cmd.CommandText = "INSERT INTO T_Clients " & "([CName]) " & "VALUES('AAA')"

                    cmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = EmpID;
                    cmd.Parameters.Add("@ClientID", OleDbType.Integer).Value = ClientID;
                    cmd.Parameters.Add("@OrderDate", OleDbType.DBDate).Value = OrderDate;
                    cmd.Parameters.Add("@ShippedDate", OleDbType.DBDate).Value = ShippedDate;
                    cmd.Parameters.Add("@ClientLocation", OleDbType.Integer).Value = ClientLocation;
                    cmd.Parameters.Add("@Taxes", OleDbType.VarChar).Value = Taxes;
                    cmd.Parameters.Add("@PaymentType", OleDbType.VarChar).Value = PaymentType;
                    cmd.Parameters.Add("@PaidDate", OleDbType.DBDate).Value = PaidDate;
                    cmd.Parameters.Add("@Notes", OleDbType.LongVarChar).Value = Notes;
                    cmd.Parameters.Add("@StatusID", OleDbType.VarChar).Value = StatusID;
                    cmd.Parameters.Add("@ExSource", OleDbType.VarChar).Value = ExSource;
                    cmd.Parameters.Add("@ExSourceID", OleDbType.VarChar).Value = ExSourceID;
                    cmd.ExecuteNonQuery();

                    string QryID = "Select @@Identity";
                    cmd.CommandText = QryID;

                    var iResult = cmd.ExecuteScalar();



                    //if (iResult > 0)
                    //_OrderID = iResult;
                }

                //connection.Close();
            }
            catch (Exception ex)
            {
                //AddToLog("Error occured in fbOrder.save: " + ex.Message, true);
            }
        }

        public int CheckDuplicateOrder(string ExSource, string ExSourceID)
        {
            OleDbConnection conn = Common.DBConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader dr;
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT OID FROM T_Orders WHERE OExSource=@pExSource and OExSourceID=@pExSourceID";
                cmd.Parameters.Add("@pExSource", OleDbType.VarChar).Value = ExSource;
                cmd.Parameters.Add("@pExSourceID", OleDbType.VarChar).Value = ExSourceID;
                dr = cmd.ExecuteReader();

                int iOID = -1 ;
                while ((dr.Read()))
                    iOID = Convert.ToInt32(dr[0]);


                dr.Close();
                cmd.Dispose();
                conn.Close();
                
                return iOID;
            }


            catch (Exception ex)
            {
                //AddToLog("Error occured in CheckDuplicateOrder: " + ex.Message, true);
                return -1;
            }
        }
        public DateTime GetOrderProcessDT()
        {
            using (OleDbConnection connection = Common.DBConnection())
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataReader dr;

                try
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT Max(T_OrderProcessLog.OrderFetchDT) AS MaxOfOrderFetchDT FROM T_OrderProcessLog";
                    dr = cmd.ExecuteReader();

                    DateTime dtLOD = new DateTime();
                    while ((dr.Read()))
                        dtLOD = Convert.ToDateTime(dr[0]);
                    dr.Close();
                    cmd.Dispose();
                    connection.Close();

                    return dtLOD;
                }

                catch (Exception ex)
                {
                    //AddToLog("Error occured in GetOrderProcessDT: " + ex.Message, true);
                    return DateTime.MinValue;
                }
            }
        }

        public void UpdateOrderProcLog(DateTime dtOPL, string Status, int orderCout, DateTime ProcessStartDT, DateTime ProcessEndDT)
        {
            try
            {
                OleDbConnection conn = Common.DBConnection();

                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO T_OrderProcessLog ([OrderFetchDT],[OrderFetchStatus], [OrderCount], [ProcessStartDT], [ProcessEndDT] " + "VALUES (@OrderFetchDT, @OrderFetchStatus, @OrderCount, @ProcessStartDT, @ProcessEndDT)";
                    cmd.Parameters.Add("@OrderFetchDT", OleDbType.DBDate).Value = dtOPL;
                    cmd.Parameters.Add("@OrderFetchStatus", OleDbType.VarChar).Value = Status;
                    cmd.Parameters.Add("@OrderCount", OleDbType.Integer).Value = orderCout;
                    cmd.Parameters.Add("@ProcessStartDT", OleDbType.DBDate).Value = ProcessStartDT;
                    cmd.Parameters.Add("@ProcessEndDT", OleDbType.Integer).Value = ProcessEndDT;

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                //AddToLog("Error occured in fbOrder.UpdateOrderProcLog: " + ex.Message, true);
            }
        }
    }

}
