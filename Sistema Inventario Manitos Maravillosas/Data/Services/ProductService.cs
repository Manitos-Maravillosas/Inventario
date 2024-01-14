using Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IProductService
    {
        OperationResult Add(Product Product);
        OperationResult Update(Product Product);
        OperationResult Delete(int idProduct);
        List<Product> GetAll();
        Product GetById(int idProduct);
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private OperationResult result = new OperationResult(true, "");

        public ProductService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public OperationResult Add(Product product)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProductCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idProduct", string.IsNullOrEmpty(product.IdProduct) ? DBNull.Value : product.IdProduct));
                        command.Parameters.Add(new SqlParameter("@productName", string.IsNullOrEmpty(product.ProductName) ? DBNull.Value : product.ProductName));
                        command.Parameters.Add(new SqlParameter("@stock", product.Stock == 0 ? DBNull.Value : product.Stock));
                        command.Parameters.Add(new SqlParameter("@cost", product.Cost == 0 ? DBNull.Value : product.Cost));
                        command.Parameters.Add(new SqlParameter("@price", product.Price == 0 ? DBNull.Value : product.Price));
                        command.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(product.Description) ? DBNull.Value : product.Description));
                        command.Parameters.Add(new SqlParameter("@status", product.Status == false ? DBNull.Value : product.Status));
                        command.Parameters.Add(new SqlParameter("@productCategory", string.IsNullOrEmpty(product.ProductCategory) ? DBNull.Value : product.ProductCategory));
                        command.Parameters.Add(new SqlParameter("@businessName", string.IsNullOrEmpty(product.BusinessName) ? DBNull.Value : product.BusinessName));
                        command.Parameters.Add(new SqlParameter("@operation", 1));

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 50000)
                {
                    result.Success = false;
                    result.Message = sqlEx.Message;
                    return result;
                }
                else
                {
                    throw new ApplicationException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }

            return result;
        }

        public OperationResult Update(Product Product)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProductCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idProduct", string.IsNullOrEmpty(Product.IdProduct) ? DBNull.Value : Product.IdProduct));
                        command.Parameters.Add(new SqlParameter("@productName", string.IsNullOrEmpty(Product.ProductName) ? DBNull.Value : Product.ProductName));
                        command.Parameters.Add(new SqlParameter("@stock", Product.Stock == 0 ? DBNull.Value : Product.Stock));
                        command.Parameters.Add(new SqlParameter("@cost", Product.Cost == 0 ? DBNull.Value : Product.Cost));
                        command.Parameters.Add(new SqlParameter("@price", Product.Price == 0 ? DBNull.Value : Product.Price));
                        command.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(Product.Description) ? DBNull.Value : Product.Description));
                        command.Parameters.Add(new SqlParameter("@status", Product.Status == false ? DBNull.Value : Product.Status));
                        command.Parameters.Add(new SqlParameter("@productCategory", string.IsNullOrEmpty(Product.ProductCategory) ? DBNull.Value : Product.ProductCategory));
                        command.Parameters.Add(new SqlParameter("@businessName", string.IsNullOrEmpty(Product.BusinessName) ? DBNull.Value : Product.BusinessName));

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 50000)
                {
                    result.Success = false;
                    result.Message = sqlEx.Message;
                    return result;
                }
                else
                {
                    throw new ApplicationException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }

            return result;
        }

        public OperationResult Delete(int idProduct)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProductCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idProduct", idProduct));
                        command.Parameters.Add(new SqlParameter("@productName", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@stock", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@cost", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@price", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@description", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@status", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@productCategory", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@businessName", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@operation", 4));

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 50000)
                {
                    result.Success = false;
                    result.Message = sqlEx.Message;
                    return result;
                }
                else
                {
                    throw new ApplicationException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }

            return result;
        }

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spProductCRUD", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@idProduct", DBNull.Value),
                        new SqlParameter("@productName", DBNull.Value),
                        new SqlParameter("@stock", DBNull.Value),
                        new SqlParameter("@cost", DBNull.Value),
                        new SqlParameter("@price", DBNull.Value),
                        new SqlParameter("@description", DBNull.Value),
                        new SqlParameter("@status", DBNull.Value),
                        new SqlParameter("@productCategory", DBNull.Value),
                        new SqlParameter("@businessName", DBNull.Value),
                        new SqlParameter("@operation", '2')
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {


                        while (dataReader.Read())
                        {
                            Product Product = new Product
                            {
                                IdProduct = Convert.ToString(dataReader["idProduct"]),
                                ProductName = Convert.ToString(dataReader["productName"]),
                                Stock = Convert.ToSingle(dataReader["stock"]),
                                Cost = Convert.ToSingle(dataReader["cost"]),
                                Price = Convert.ToSingle(dataReader["price"]),
                                Description = Convert.ToString(dataReader["description"]),
                                Status = Convert.ToBoolean(dataReader["status"]),
                                ProductCategory = Convert.ToString(dataReader["productCategory"]),
                                BusinessName = Convert.ToString(dataReader["businessName"])
                            };
                            products.Add(Product);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al leer datos del SqlDataReader.", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return products;
        }

        public Product GetById(int idProduct)
        {
            Product product = new Product();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spProductCRUD", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@idProduct", idProduct),
                        new SqlParameter("@productName", DBNull.Value),
                        new SqlParameter("@stock", DBNull.Value),
                        new SqlParameter("@cost", DBNull.Value),
                        new SqlParameter("@price", DBNull.Value),
                        new SqlParameter("@description", DBNull.Value),
                        new SqlParameter("@status", DBNull.Value),
                        new SqlParameter("@productCategory", DBNull.Value),
                        new SqlParameter("@businessName", DBNull.Value),
                        new SqlParameter("@operation", '2')
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {


                        while (dataReader.Read())
                        {
                            product = new Product
                            {
                                IdProduct = Convert.ToString(dataReader["idProduct"]),
                                ProductName = Convert.ToString(dataReader["productName"]),
                                Stock = Convert.ToSingle(dataReader["stock"]),
                                Cost = Convert.ToSingle(dataReader["cost"]),
                                Price = Convert.ToSingle(dataReader["price"]),
                                Description = Convert.ToString(dataReader["description"]),
                                Status = Convert.ToBoolean(dataReader["status"]),
                                ProductCategory = Convert.ToString(dataReader["productCategory"]),
                                BusinessName = Convert.ToString(dataReader["businessName"])
                            };
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return product;
        }
    }
}
