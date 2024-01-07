using Microsoft.AspNetCore.Identity;

namespace Sistema_Inventario_Manitos_Maravillosas.Helpers
{
    public interface IGenerateRandomPassword
    {

        public string GenerateRandomPasswordMethod(PasswordOptions opts);
    }
}
