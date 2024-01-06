using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IClientService
    {
        List<Client> GetAll();
        Client GetById(string id);
        OperationResult Add(Client newClient);
        OperationResult Update(Client newClient);
        OperationResult Delete(string id);
        List<string> GetDepartmentNames();
        List<string> GetCitiesByDepartmentName(string idDepartment);

    }
}
