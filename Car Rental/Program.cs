using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using Car_Rental.Exceptions;
using Car_Rental.Dao;
using Car_Rental.Entity;
using Car_Rental.Exceptions;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Welcome to Car Rental System");

            // Initialize repository
            ICarLeaseRepository repository = new ICarLeaseRepositoryImpl();

            // Display menu options
            while (true)
            {
                Console.WriteLine("\n1. List Available Cars");
                Console.WriteLine("2. Add a New Car");
                Console.WriteLine("3. Remove a Car");
                Console.WriteLine("4. List Customers");
                Console.WriteLine("5. Add a Customer");
                Console.WriteLine("6. Create a Lease");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListAvailableCars(repository);
                        break;

                    case "2":
                        AddCar(repository);
                        break;

                    case "3":
                        RemoveCar(repository);
                        break;

                    case "4":
                        ListCustomers(repository);
                        break;

                    case "5":
                        AddCustomer(repository);
                        break;

                    case "6":
                        CreateLease(repository);
                        break;

                    case "7":
                        Console.WriteLine("Exiting... Thank you for using the system!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice! Please enter a number between 1 and 7.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ListAvailableCars(ICarLeaseRepository repository)
    {
        List<Vehicle> cars = repository.ListAvailableCars();
        Console.WriteLine("\nAvailable Cars:");
        foreach (var car in cars)
        {
            Console.WriteLine($"{car.VehicleID} - {car.Make} {car.Model}, Year: {car.Year}, Rate: {car.DailyRate:C}");
        }
    }

    static void AddCar(ICarLeaseRepository repository)
    {
        Console.Write("\nEnter Make: ");
        string make = Console.ReadLine();
        Console.Write("Enter Model: ");
        string model = Console.ReadLine();
        Console.Write("Enter Year: ");
        int year = int.Parse(Console.ReadLine());
        Console.Write("Enter Daily Rate: ");
        decimal rate = decimal.Parse(Console.ReadLine());
        Console.Write("Enter Passenger Capacity: ");
        int passengerCapacity = int.Parse(Console.ReadLine());
        Console.Write("Enter Engine Capacity: ");
        decimal engineCapacity = decimal.Parse(Console.ReadLine());

        Vehicle car = new Vehicle(0, make, model, year, rate, "available", passengerCapacity, engineCapacity);
        repository.AddCar(car);

        Console.WriteLine("Car added successfully.");
    }

    static void RemoveCar(ICarLeaseRepository repository)
    {
        Console.Write("\nEnter Car ID to remove: ");
        int carID = int.Parse(Console.ReadLine());

        try
        {
            repository.RemoveCar(carID);
            Console.WriteLine("Car removed successfully.");
        }
        catch (CarNotFoundException e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }

    static void ListCustomers(ICarLeaseRepository repository)
    {
        List<Customer> customers = repository.ListCustomers();
        Console.WriteLine("\nCustomers:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerID} - {customer.FirstName} {customer.LastName}, Email: {customer.Email}, Phone: {customer.PhoneNumber}");
        }
    }

    static void AddCustomer(ICarLeaseRepository repository)
    {
        Console.Write("\nEnter First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter Last Name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter Email: ");
        string email = Console.ReadLine();
        Console.Write("Enter Phone Number: ");
        string phone = Console.ReadLine();

        Customer customer = new Customer(0, firstName, lastName, email, phone);
        repository.AddCustomer(customer);

        Console.WriteLine("Customer added successfully.");
    }

    static void CreateLease(ICarLeaseRepository repository)
    {
        Console.Write("\nEnter Customer ID: ");
        int customerID = int.Parse(Console.ReadLine());
        Console.Write("Enter Vehicle ID: ");
        int carID = int.Parse(Console.ReadLine());
        Console.Write("Enter Lease Start Date (yyyy-MM-dd): ");
        DateOnly startDate = DateOnly.Parse(Console.ReadLine());

        Console.Write("Enter Lease End Date (yyyy-MM-dd): ");
        DateOnly endDate = DateOnly.Parse(Console.ReadLine());

        Console.Write("Enter Lease Type (Daily rent/Monthly rent): ");
        string type = Console.ReadLine();

        try
        {
            Lease lease = repository.CreateLease(customerID, carID, startDate, endDate,type);
            Console.WriteLine($"Lease created successfully. Lease ID: {lease.LeaseID}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}
