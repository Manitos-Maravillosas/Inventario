namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models
{
    public class Department
    {
        public int IdDepartment { get; set; }
        public string Name { get; set; }

        // Navigation property for related cities
        public ICollection<City> Cities { get; set; }
    }

    public class City
    {
        public int IdCity { get; set; }
        public string Name { get; set; }

        // Foreign key
        public int IdDepartment { get; set; }

        // Navigation property for the related department
        public Department Department { get; set; }

        // Navigation property for related addresses
        public ICollection<Address> Addresses { get; set; }
    }
    public class Address
    {
        public int IdAddress { get; set; }
        public string Signs { get; set; }

        // Foreign key
        public int IdCity { get; set; }

        // Navigation property for the related city
        public City City { get; set; }
    }

}
