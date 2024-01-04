using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();
        Employee GetById(string id);
        List<string> GetBusinessNames();
        List<string> GetUserEmails();
        OperationResult Add(Employee newEmployee);
        OperationResult Update(Employee newEmployee);
        OperationResult Delete(string id);


    }
}
