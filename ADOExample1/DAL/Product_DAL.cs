using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ADOExample1.Models;

namespace ADOExample1.DAL
{
    public class Product_DAL
    {
        string conString = ConfigurationManager.ConnectionStrings["adoConnectionstring"].ToString();

        //get all products

        public List<Product> GetAllProducts()
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection connection=new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllProducts1";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProucts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProucts);
                connection.Close();
                foreach(DataRow dr in dtProucts.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = dr["Remarks"].ToString()

                    });
                }

            }
            return productList;
        }

        ////get products by id

        //public List<Product> GetProductByID(int ProductID)
        //{
        //    List<Product> productList = new List<Product>();
        //    using (SqlConnection connection = new SqlConnection(conString))
        //    {
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "sp_GetProductByID1";

        //        command.Parameters.AddWithValue("@ProductID", ProductID);
        //        SqlDataAdapter sqlDA = new SqlDataAdapter(command);
        //        DataTable dtProucts = new DataTable();

        //        connection.Open();
        //        sqlDA.Fill(dtProucts);
        //        connection.Close();
        //        foreach (DataRow dr in dtProucts.Rows)
        //        {
        //            productList.Add(new Product
        //            {
        //                ProductID = Convert.ToInt32(dr["ProductID"]),
        //                ProductName = dr["ProductName"].ToString(),
        //                Price = Convert.ToDecimal(dr["Price"]),
        //                Qty = Convert.ToInt32(dr["Qty"]),
        //                Remarks = dr["Remarks"].ToString()

        //            });
        //        }

        //    }
        //    return productList;
        //}

        ////update product
        //public bool UpdateProduct(Product product)
        //{
        //    int i = 0;
        //    using (SqlConnection connection = new SqlConnection(conString))
        //    {
        //        SqlCommand command = new SqlCommand("sp_UpdatePRoducts", connection);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@ProductID", product.ProductID);
        //        command.Parameters.AddWithValue("@ProductName", product.ProductName);
        //        command.Parameters.AddWithValue("@Price", product.Price);
        //        command.Parameters.AddWithValue("@Qty", product.Qty);
        //        command.Parameters.AddWithValue("@Remarks", product.Remarks);



        //        connection.Open();
        //        i = command.ExecuteNonQuery();
        //        connection.Close();

        //    }
        //    if (i > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        public List<Product> GetProductByID(int productID)
        {
            List<Product> productList = new List<Product>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetProductByID1";

                command.Parameters.AddWithValue("@ProductID", productID);

                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = dr["Remarks"].ToString()
                    });
                }
            }

            return productList;
        }
        public bool UpdateProduct(Product product)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdatePRoducts", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
                connection.Close();
            }

            return rowsAffected > 0;
        }



        //insert product
        public bool InsertProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection=new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InsertPRoducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);



                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();

            }
            if (id > 0)
            { 
                return true;
            }
            else
            {
                return false;
            }
        }


        //delete product
        public string DeleteProduct(int productid)

        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteProduct1", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Productid", productid);
                command.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar,50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OUTPUTMESSAGE"].Value.ToString();
                connection.Close();
            }
            return result;

        }


    }
}