using NUnit.Framework;
using Car_Rental.Dao;
using Car_Rental.Exceptions;

namespace CarRental.Tests
{
    [TestFixture]
    public class ExceptionTests
    {
        private ICarLeaseRepositoryImpl repo;

        [SetUp]
        public void Setup()
        {
            repo = new ICarLeaseRepositoryImpl();
        }

        [Test]
        public void FindCustomerById_ShouldThrowException_WhenIdNotFound()
        {
            Assert.Throws<CustomerNotFoundException>(() => repo.FindCustomerById(-999));
        }

        [Test]
        public void FindCarById_ShouldThrowException_WhenIdNotFound()
        {
            Assert.Throws<CarNotFoundException>(() => repo.FindCarById(-999));
        }

        [Test]
        public void ReturnCar_ShouldThrowException_WhenLeaseIdNotFound()
        {
            Assert.Throws<LeaseNotFoundException>(() => repo.ReturnCar(-999));
        }
    }
}
