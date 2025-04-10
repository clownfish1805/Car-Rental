using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Car_Rental.Entity;
using Car_Rental.Exceptions;
using Car_Rental.Util;

namespace Car_Rental.Dao
{
    public class ICarLeaseRepositoryImpl : ICarLeaseRepository
    {
        private readonly SqlConnection connection;

        public ICarLeaseRepositoryImpl()
        {
            DBConnection db = new DBConnection();
            connection = db.GetConnection();
        }

        // ==================== Car Management ====================

        public void AddCar(Vehicle car)
        {
            string query = "INSERT INTO Vehicle (make, model, year, dailyRate, status, passengerCapacity, engineCapacity) VALUES (@make, @model, @year, @rate, @status, @passenger, @engine)";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@make", car.Make);
            cmd.Parameters.AddWithValue("@model", car.Model);
            cmd.Parameters.AddWithValue("@year", car.Year);
            cmd.Parameters.AddWithValue("@rate", car.DailyRate);
            cmd.Parameters.AddWithValue("@status", car.Status);
            cmd.Parameters.AddWithValue("@passenger", car.PassengerCapacity);
            cmd.Parameters.AddWithValue("@engine", car.EngineCapacity);
            cmd.ExecuteNonQuery();
        }

        public void RemoveCar(int carID)
        {
            string query = "DELETE FROM Vehicle WHERE vehicleID = @id";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", carID);
            int rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new CarNotFoundException($"Car with ID {carID} not found.");
        }

        public List<Vehicle> ListAvailableCars()
        {
            List<Vehicle> cars = new();
            string query = "SELECT * FROM Vehicle WHERE status = 'available'";
            using SqlCommand cmd = new SqlCommand(query, connection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cars.Add(ReadVehicle(reader));
            }
            return cars;
        }

        public List<Vehicle> ListRentedCars()
        {
            List<Vehicle> cars = new();
            string query = "SELECT * FROM Vehicle WHERE status = 'notAvailable'";
            using SqlCommand cmd = new SqlCommand(query, connection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cars.Add(ReadVehicle(reader));
            }
            return cars;
        }

        public Vehicle FindCarById(int carID)
        {
            string query = "SELECT * FROM Vehicle WHERE vehicleID = @id";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", carID);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                return ReadVehicle(reader);
            throw new CarNotFoundException($"Car with ID {carID} not found.");
        }

        private Vehicle ReadVehicle(SqlDataReader reader)
        {
            return new Vehicle
            {
                VehicleID = reader.GetInt32(0),
                Make = reader.GetString(1),
                Model = reader.GetString(2),
                Year = reader.GetInt32(3),
                DailyRate = reader.GetDecimal(4),
                Status = reader.GetString(5),
                PassengerCapacity = reader.GetInt32(6),
                EngineCapacity = reader.GetDecimal(7)
            };
        }

        // ==================== Customer Management ====================

        public void AddCustomer(Customer customer)
        {
            string query = "INSERT INTO Customer (firstName, lastName, email, phoneNumber) VALUES (@firstName, @lastName, @email, @phone)";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@firstName", customer.FirstName);
            cmd.Parameters.AddWithValue("@lastName", customer.LastName);
            cmd.Parameters.AddWithValue("@email", customer.Email);
            cmd.Parameters.AddWithValue("@phone", customer.PhoneNumber);
            cmd.ExecuteNonQuery();
        }

        public void RemoveCustomer(int customerID)
        {
            string query = "DELETE FROM Customer WHERE customerID = @id";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", customerID);
            int rows = cmd.ExecuteNonQuery();
            if (rows == 0)
                throw new CustomerNotFoundException($"Customer with ID {customerID} not found.");
        }

        public List<Customer> ListCustomers()
        {
            List<Customer> customers = new();
            string query = "SELECT * FROM Customer";
            using SqlCommand cmd = new SqlCommand(query, connection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(new Customer(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)
                ));
            }
            return customers;
        }

        public Customer FindCustomerById(int customerID)
        {
            string query = "SELECT * FROM Customer WHERE customerID = @id";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", customerID);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Customer(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)
                );
            }
            throw new CustomerNotFoundException($"Customer with ID {customerID} not found.");
        }

        // ==================== Lease Management ====================

        public Lease CreateLease(int customerID, int carID, DateOnly startDate, DateOnly endDate, string type)
        {
            string query = "INSERT INTO Lease (vehicleID, customerID, startDate, endDate, type) " +
                "OUTPUT INSERTED.leaseID " +
                "VALUES (@vehicleID, @customerID, @startDate, @endDate, @type)";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@vehicleID", carID);
            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@startDate", startDate.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@endDate", endDate.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@type", type.Trim());

            int leaseID = (int)cmd.ExecuteScalar();

            UpdateCarStatus(carID, "notAvailable");

            return new Lease(leaseID, carID, customerID, startDate, endDate, type);
        }

        public Lease ReturnCar(int leaseID)
        {
            string query = "SELECT * FROM Lease WHERE leaseID = @id";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", leaseID);
            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Lease lease = new Lease(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    DateOnly.FromDateTime(reader.GetDateTime(3)),
                    DateOnly.FromDateTime(reader.GetDateTime(4)),
                    reader.GetString(5)
                );
                reader.Close();

                UpdateCarStatus(lease.VehicleID, "available");

                return lease;
            }
            throw new LeaseNotFoundException($"Lease with ID {leaseID} not found.");
        }

        public void UpdateCarStatus(int vehicleID, string status)
        {
            string query = "UPDATE Vehicle SET status = @status WHERE vehicleID = @id";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@id", vehicleID);
            cmd.ExecuteNonQuery();
        }

        // ==================== Payment ====================

        public void RecordPayment(Lease lease, double amount)
        {
            string query = "INSERT INTO Payment (leaseID, paymentDate, amount) VALUES (@leaseID, GETDATE(), @amount)";
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@leaseID", lease.LeaseID);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.ExecuteNonQuery();
        }
    }
}
