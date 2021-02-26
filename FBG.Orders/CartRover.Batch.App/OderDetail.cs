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

        public void SaveOrderDetail()
        {
            try
            {
                using (OleDbConnection connection = Common.DBConnection())
                {
                    using (OleDbCommand cmd = connection.CreateCommand())
                    {
                        {
                            cmd.CommandText = "INSERT INTO T_OrderDetails " + "([ODOID],[ODProductID],[ODQuantity],[ODUnitPrice],[ODDiscount]) " + "VALUES(@OrderID,@ProductID,@Quantity,@UnitPrice,@Discount)";
                        };
                        cmd.Parameters.Add("@OrderID", OleDbType.Integer).Value = OrderID;
                        cmd.Parameters.Add("@ProductID", OleDbType.Integer).Value = ProductID;
                        cmd.Parameters.Add("@Quantity", OleDbType.Integer).Value = Quantity;
                        cmd.Parameters.Add("@UnitPrice", OleDbType.Decimal).Value = UnitPrice;
                        cmd.Parameters.Add("@Discount", OleDbType.Decimal).Value = Discount;

                        cmd.ExecuteNonQuery();

                        string QryID = "Select @@Identity";
                        cmd.CommandText = QryID;
                        var iResult = cmd.ExecuteScalar();
                        if (iResult != null)
                            _OrderDetID = Convert.ToInt32(iResult);
                        
                    }
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error occured in fbOrderdetail.save: " + ex.Message);
            }
        }

        public int GetProdID(string sCode)
        {
            using (OleDbConnection conn = Common.DBConnection())
            {
                OleDbDataReader dr;
                try
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT PID, SKUCode, UPCCode FROM T_Products WHERE SKUCode=@pSKU OR UPCCode=@pUPC";
                    cmd.Parameters.Add("@CodeSKU", OleDbType.VarChar).Value = sCode;
                    cmd.Parameters.Add("@CodeUPC", OleDbType.VarChar).Value = sCode;
                    dr = cmd.ExecuteReader();
                    int iPID = -1;
                    while ((dr.Read()))
                    {
                        iPID = Convert.ToInt32(dr[0]);
                    }
                    dr.Close();
                    cmd.Dispose();
                    conn.Close();

                    return iPID;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occured in fbOrderdetail.GetProdID: " + ex.Message);

                }
            }
        }
    }

}
