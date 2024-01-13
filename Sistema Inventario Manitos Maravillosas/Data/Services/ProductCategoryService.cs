using Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IProductCategoryService
    {
        OperationResult CreateProductCategory(ProductCategory productCategory);
        OperationResult UpdateProductCategory(ProductCategory productCategory);
        OperationResult DeleteProductCategory(int idProductCategory);
        List<ProductCategory> GetProductCategories();
        ProductCategory GetProductCategoryById(int idProductCategory);
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductCategoryService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public OperationResult CreateProductCategory(ProductCategory productCategory)
        {
            try
            {
                _context.ProductCategories.Add(productCategory);
                _context.SaveChanges();
                return new OperationResult(true, "Categoría de producto creada exitosamente");
            }
            catch (Exception e)
            {
                return new OperationResult(false, e.Message);
            }
        }

        public OperationResult UpdateProductCategory(ProductCategory productCategory)
        {
            try
            {
                _context.ProductCategories.Update(productCategory);
                _context.SaveChanges();
                return new OperationResult(true, "Categoría de producto actualizada exitosamente");
            }
            catch (Exception e)
            {
                return new OperationResult(false, e.Message);
            }
        }

        public OperationResult DeleteProductCategory(int idProductCategory)
        {
            try
            {
                var productCategory = _context.ProductCategories.Find(idProductCategory);
                _context.ProductCategories.Remove(productCategory);
                _context.SaveChanges();
                return new OperationResult(true, "Categoría de producto eliminada exitosamente");
            }
            catch (Exception e)
            {
                return new OperationResult(false, e.Message);
            }
        }

        public List<ProductCategory> GetProductCategories()
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

        public ProductCategory GetProductCategoryById(int idProductCategory)
        {
            return _context.ProductCategories.Find(idProductCategory);
        }
    }
}
