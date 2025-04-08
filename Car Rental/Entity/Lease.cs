using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Entity
{
    public class Lease
    {
        public int LeaseID { get; set; }
        public int VehicleID { get; set; }
        public int CustomerID { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Type { get; set; }

        public Lease() { }
        public Lease(int leaseID, int vehicleID, int customerID, DateOnly startDate, DateOnly endDate, string type)
        {
            LeaseID = leaseID;
            VehicleID = vehicleID;
            CustomerID = customerID;
            StartDate = startDate;
            EndDate = endDate;
            Type = type;
        }
    }
}
