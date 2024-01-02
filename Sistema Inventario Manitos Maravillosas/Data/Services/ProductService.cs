using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models.Inventory;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public OperationResult Add(Product newProduct)
        {
            throw new NotImplementedException();
        }

        public OperationResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProductCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idClient", DBNull.Value),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@lastName1", DBNull.Value),
                            new SqlParameter("@lastName2", DBNull.Value),
                            new SqlParameter("@email", DBNull.Value),
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@operation", '2') // Operation for 'Read' is 2
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (!dataReader.HasRows)
                            {
                                // Handle the case when no data is returned
                                // You might want to log this or handle it according to your application's logic
                                return null; // Return the empty list
                            }

                            while (dataReader.Read())
                            {
                                Product product = new Product();
                             
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                // Handle the exception as per your application's policy
                products.Clear(); // This will return an empty list in case of an error.
            }

            return products;
        }

        public Product GetById(string id)
        {
            throw new NotImplementedException();
        }

        public OperationResult Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
