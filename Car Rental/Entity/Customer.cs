using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Entity
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Customer() { }
        public Customer(int id, string first, string last, string email, string phone)
        {
            CustomerID = id;
            FirstName = first;
            LastName = last;
            Email = email;
            PhoneNumber = phone;
        }
    }
}
