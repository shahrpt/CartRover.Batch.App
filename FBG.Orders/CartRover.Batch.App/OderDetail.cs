using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartRover.Batch.App
{
    class OrderDetail
    {
        private int _OrderDetID;
        private int _OrderID;
        private int _ProductID;
        private int _Quantity;
        private decimal _UnitPrice;
        private decimal _Discount;




        public System.Int32 OrderDetID
        {
            get
            {
                return _OrderDetID;
            }
        }

        public System.Int32 OrderID
        {
            get
            {
                return _OrderID;
            }
            set
            {
                _OrderID = value;
            }
        }

        public System.Int32 ProductID
        {
            get
            {
                return _ProductID;
            }
            set
            {
                _ProductID = value;
            }
        }

        public System.Int32 Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                _Quantity = value;
            }
        }

        public System.Decimal UnitPrice
        {
            get
            {
                return _UnitPrice;
            }
            set
            {
                _UnitPrice = value;
            }
        }

        public System.Decimal Discount
        {
            get
            {
                return _Discount;
            }
            set
            {
                _Discount = value;
            }
        }

        public void Save()
        {
            try
            {
                string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Mahesh\Data\Dev.mdb";
                string strSQL = "SELECT * FROM Developer";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand cmd = new OleDbCommand(strSQL, connection);
                    cmd.CommandText = "INSERT INTO T_OrderDetails " + "([ODOID],[ODProductID],[ODQuantity],[ODUnitPrice],[ODDiscount]) " + "VALUES(@OrderID,@ProductID,@Quantity,@UnitPrice,@Discount)";
                    cmd.Parameters.Add("@OrderID", OleDbType.Integer).Value = OrderID;
                    cmd.Parameters.Add("@ProductID", OleDbType.Integer).Value = ProductID;
                    cmd.Parameters.Add("@Quantity", OleDbType.Integer).Value = Quantity;
                    cmd.Parameters.Add("@UnitPrice", OleDbType.Decimal).Value = UnitPrice;
                    cmd.Parameters.Add("@Discount", OleDbType.Decimal).Value = Discount;

                    cmd.ExecuteNonQuery();

                    string QryID = "Select @@Identity";
                    cmd.CommandText = QryID;

                    var iResult = cmd.ExecuteScalar();



                    //if (iResult > 0)
                    //_OrderDetID = iResult;
                    connection.Close();
                }

                //conn.Close();
            }
            catch (Exception ex)
            {
                AddToLog("Error occured in fbOrderdetail.save: " + ex.Message, true);
            }
        }

        public int GetProdID(string sCode)
        {
            OleDbConnection conn = Common.DBConnection();

            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader dr;



            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT PID, SKUCode, UPCCode FROM T_Products WHERE SKUCode=@pSKU OR UPCCode=@pUPC";
                cmd.Parameters.Add("@CodeSKU", OleDbType.VarChar).Value = sCode;
                cmd.Parameters.Add("@CodeUPC", OleDbType.VarChar).Value = sCode;


                dr = cmd.ExecuteReader();

                int iPID;
                while ((dr.Read()))
                    iPID = dr[0];


                dr.Close();
                cmd.Dispose();
                conn.Close();

                return iPID;
            }


            catch (Exception ex)
            {
                AddToLog("Error occured in fbOrderdetail.GetProdID: " + ex.Message, true);
                return 0;
            }
        }
    }

}
