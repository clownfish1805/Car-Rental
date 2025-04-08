using System;
using System.Data.SqlClient;

namespace Car_Rental.Util
{
    public class DBConnection
    {
        private readonly string connectionString = "Server=DESKTOP-S6D65HG\\SQLEXPRESS01;Database=car_rental_db;Trusted_Connection=True;";

        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Database Connected Successfully!");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection Failed: {ex.Message}");
                return null;
            }
        }
    }
}
