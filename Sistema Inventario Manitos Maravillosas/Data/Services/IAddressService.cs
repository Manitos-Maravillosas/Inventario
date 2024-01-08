using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IAddressService
    {        
        List<string> GetDepartmentNames();
        List<string> GetCitiesByDepartmentName(string idDepartment);
    }
}
