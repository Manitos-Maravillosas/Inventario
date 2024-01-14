using Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IProductCategoryService
    {
        OperationResult Add(ProductCategory productCategory);
        OperationResult Update(ProductCategory productCategory);
        OperationResult Delete(int idProductCategory);
        List<ProductCategory> GetAll();
        ProductCategory GetById(int idProductCategory);

        public List<string> GetProductCategoryNames();
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private OperationResult result = new OperationResult(true, "");

        public ProductCategoryService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public OperationResult Add(ProductCategory productCategory)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProductCategoryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idProductCategory", productCategory.IdProductCategory != 0 ? DBNull.Value : productCategory.IdProductCategory));
                        command.Parameters.Add(new SqlParameter("@category", string.IsNullOrEmpty(productCategory.Category) ? DBNull.Value : productCategory.Category));
                        command.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(productCategory.Description) ? DBNull.Value : productCategory.Description));
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

        public OperationResult Update(ProductCategory productCategory)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProductCategoryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idProductCategory", productCategory.IdProductCategory == 0 ? DBNull.Value : productCategory.IdProductCategory));
                        command.Parameters.Add(new SqlParameter("@category", string.IsNullOrEmpty(productCategory.Category) ? DBNull.Value : productCategory.Category));
                        command.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(productCategory.Description) ? DBNull.Value : productCategory.Description));
                        command.Parameters.Add(new SqlParameter("@operation", 3));

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

        public OperationResult Delete(int idProductCategory)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProductCategoryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idProductCategory", idProductCategory));
                        command.Parameters.Add(new SqlParameter("@category", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@description", DBNull.Value));
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

        public List<ProductCategory> GetAll()
        {
            List<ProductCategory> productCategories = new List<ProductCategory>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spProductCategoryCRUD", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@idProductCategory", DBNull.Value),
                        new SqlParameter("@category", DBNull.Value),
                        new SqlParameter("@description", DBNull.Value),
                        new SqlParameter("@operation", '2')
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {


                        while (dataReader.Read())
                        {
                            ProductCategory productCategory = new ProductCategory
                            {
                                IdProductCategory = Convert.ToInt32(dataReader["idProductCategory"]),
                                Category = Convert.ToString(dataReader["category"]),
                                Description = Convert.ToString(dataReader["description"])
                            };
                            productCategories.Add(productCategory);
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

            return productCategories;
        }

        public ProductCategory GetById(int idProductCategory)
        {
            ProductCategory productCategory = new ProductCategory();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spProductCategoryCRUD", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@idProductCategory", idProductCategory),
                        new SqlParameter("@category", DBNull.Value),
                        new SqlParameter("@description", DBNull.Value),
                        new SqlParameter("@operation", '2')
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {


                        while (dataReader.Read())
                        {
                            productCategory = new ProductCategory
                            {
                                IdProductCategory = Convert.ToInt32(dataReader["idProductCategory"]),
                                Category = Convert.ToString(dataReader["category"]),
                                Description = Convert.ToString(dataReader["description"])
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

            return productCategory;
        }

        public List<string> GetProductCategoryNames()
        {
            List<string> categoryNames = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand getRolNamesCommand = new SqlCommand("spGetProductCategories", connection))
                {
                    getRolNamesCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader rolDataReader = getRolNamesCommand.ExecuteReader())
                    {
                        while (rolDataReader.Read())
                        {
                            string categoryName = rolDataReader["category"].ToString();
                            categoryNames.Add(categoryName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return categoryNames;
        }

    }
}
