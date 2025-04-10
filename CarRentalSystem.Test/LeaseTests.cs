using NUnit.Framework;
using Car_Rental.Dao;
using Car_Rental.Entity;
using System;
using System.Linq;

namespace CarRental.Tests
{
    [TestFixture]
    public class LeaseTests
    {
        private ICarLeaseRepositoryImpl repo;

        [SetUp]
        public void Setup()
        {
            repo = new ICarLeaseRepositoryImpl();
        }

        [Test]
        public void CreateLease_SuccessfullyCreated()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Elena",
                LastName = "Gilbert",
                Email = $"elena_{Guid.NewGuid()}@example.com",
                PhoneNumber = "1234567890"
            };
            repo.AddCustomer(customer);

            var car = new Vehicle
            {
                Make = "Honda",
                Model = "City",
                Year = 2021,
                DailyRate = 60,
                Status = "available",
                PassengerCapacity = 5,
                EngineCapacity = 1.5m
            };
            repo.AddCar(car);

            var customerID = repo.ListCustomers().Last().CustomerID;
            var carID = repo.ListAvailableCars().Last().VehicleID;

            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(3);

            // Act
            var lease = repo.CreateLease(customerID, carID, startDate, endDate, "Daily rent");

            // Assert
            Assert.NotNull(lease);
            Assert.AreEqual(customerID, lease.CustomerID);
            Assert.AreEqual(carID, lease.VehicleID);
        }

        [Test]
        public void ReturnCar_LeaseRetrievedSuccessfully()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Damon",
                LastName = "Salvatore",
                Email = $"damon_{Guid.NewGuid()}@example.com",
                PhoneNumber = "9876543210"
            };
            repo.AddCustomer(customer);

            var car = new Vehicle
            {
                Make = "Ford",
                Model = "EcoSport",
                Year = 2020,
                DailyRate = 50,
                Status = "available",
                PassengerCapacity = 5,
                EngineCapacity = 1.0m
            };
            repo.AddCar(car);

            var customerID = repo.ListCustomers().Last().CustomerID;
            var carID = repo.ListAvailableCars().Last().VehicleID;

            var lease = repo.CreateLease(
                customerID,
                carID,
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                "Daily rent"
            );

            // Act
            var returnedLease = repo.ReturnCar(lease.LeaseID);

            // Assert
            Assert.NotNull(returnedLease);
            Assert.AreEqual(lease.LeaseID, returnedLease.LeaseID);
        }
    }
}
