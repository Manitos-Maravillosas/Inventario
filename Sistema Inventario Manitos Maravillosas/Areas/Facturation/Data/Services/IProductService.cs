using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(string id);
        OperationResult Add(Product newProduct);
        OperationResult Update(Product product);
        OperationResult Delete(string id);

        Product GetStockById(string id, int quantity);

        OperationResult AddStock(string id, int quantity);

        OperationResult UpdateStock(string id, int quantity);

    }
}
