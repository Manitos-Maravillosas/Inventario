using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services
{

    public interface IDeliveryService
    {
        List<Delivery> GetAllDeliveries();
        OperationResult UpdateStatus(string id);

        List<CompanyTrans> GetAllCompanies();

        List<string> GetAllCompaniesString();


    }
    public class DeliveryService : IDeliveryService

    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public DeliveryService(IConfiguration configuration)

        {
            _configuration = configuration;
        }

        public List<CompanyTrans> GetAllCompanies()
        {
            List<CompanyTrans> companies = new List<CompanyTrans>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spGetCompanyTrans", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

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
                                CompanyTrans company = new CompanyTrans
                                {
                                    IdCompanyTrans = Convert.ToInt32(dataReader["idCompanyTrans"]),
                                    Name = dataReader["name"].ToString(),
                                    PhoneNumber = dataReader["phoneNumber"].ToString()
                                };

                                companies.Add(company);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                // Handle the exception as per your application's policy
                companies.Clear(); // This will return an empty list in case of an error.
            }

            return companies;
        }
        public List<string> GetAllCompaniesString()
        {
            List<string> companies = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spGetCompanyTrans", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

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
                                companies.Add(dataReader["name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                // Handle the exception as per your application's policy
                companies.Clear(); // This will return an empty list in case of an error.
            }

            return companies;
        }



        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<Delivery> GetAllDeliveries()
        {
            List<Delivery> deliveries = new List<Delivery>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spDeliveryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idDelivery", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Delivery delivery = new Delivery
                                {
                                    Id = Convert.ToInt32(dataReader["idDelivery"]),
                                    NameTypeDelivery = dataReader["TypeDeliveryName"].ToString(),
                                    Signs = dataReader["Signs"].ToString(),
                                    Notes = dataReader["notes"].ToString(),
                                    DateAprox = Convert.ToDateTime(dataReader["dateAprox"]).Date, // Modificado aquí
                                    Total = Convert.ToSingle(dataReader["total"]),
                                    InternalCost = Convert.ToSingle(dataReader["internalCost"]),
                                    IdBill = Convert.ToInt32(dataReader["idBill"]),
                                };
                                deliveries.Add(delivery);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                deliveries.Clear();
                throw new CustomDataException(ex.Message, ex);
            }

            return deliveries;
        }

        //------------------------------------------------------------------------------------
        //                              UpdateStatus                                             
        //------------------------------------------------------------------------------------
        public OperationResult UpdateStatus(string id)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spDeliveryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idDelivery", id),
                            new SqlParameter("@operation", '3')
                        };

                        command.Parameters.AddRange(parameters);

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
                    throw new CustomDataException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
            return result;
        }

    }
}
