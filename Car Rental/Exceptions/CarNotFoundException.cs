using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Exceptions
{
    public class CarNotFoundException : Exception
    {
       
            public CarNotFoundException(string message) : base(message) { }
        
    }
}
