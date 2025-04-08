using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Entity
{
   public class Payment
    {
        public int PaymentID { get; set; }
        public int LeaseID { get; set; }
        public DateOnly PaymentDate { get; set; }
        public decimal Amount { get; set; }

        public Payment() { }
        public Payment(int paymentID, int leaseID, DateOnly paymentDate, decimal amount)
        {
            PaymentID = paymentID;
            LeaseID = leaseID;
            PaymentDate = paymentDate;
            Amount = amount;
        }
    }
}
