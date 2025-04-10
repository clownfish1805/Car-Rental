using NUnit.Framework;
using Car_Rental.Dao;
using Car_Rental.Entity;
using System.Linq;

namespace CarRental.Tests
{
    [TestFixture]
    public class CarTests
    {
        private ICarLeaseRepositoryImpl repo;

        [SetUp]
        public void Setup()
        {
            repo = new ICarLeaseRepositoryImpl();
        }

        [Test]
        public void AddCar_SuccessfullyAdded()
        {
            // Arrange
            var vehicle = new Vehicle
            {
                Make = "Hyundai",
                Model = "i20",
                Year = 2022,
                DailyRate = 55,
                Status = "available",
                PassengerCapacity = 5,
                EngineCapacity = 1.2m
            };

            // Act
            repo.AddCar(vehicle);
            var cars = repo.ListAvailableCars();

            // Assert
            var addedCar = cars.LastOrDefault(c => c.Make == "Hyundai" && c.Model == "i20");
            Assert.IsNotNull(addedCar);
            Assert.AreEqual("Hyundai", addedCar.Make);
            Assert.AreEqual("i20", addedCar.Model);
        }
    }
}
