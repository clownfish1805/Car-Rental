using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car_Rental.Entity;

namespace Car_Rental.Dao
{
    interface ICarLeaseRepository
    {
        // Car Management
        void AddCar(Vehicle car);
        void RemoveCar(int carID);
        List<Vehicle> ListAvailableCars();
        List<Vehicle> ListRentedCars();
        Vehicle FindCarById(int carID);

        // Customer Management
        void AddCustomer(Customer customer);
        void RemoveCustomer(int customerID);
        List<Customer> ListCustomers();
        Customer FindCustomerById(int customerID);

        // Lease Management
        Lease CreateLease(int customerID, int carID, DateOnly startDate, DateOnly endDate, string type);

        Lease ReturnCar(int leaseID);
        //List<Lease> ListActiveLeases();
        //List<Lease> ListLeaseHistory();

        // Payment
        void RecordPayment(Lease lease, double amount);
    }
}
