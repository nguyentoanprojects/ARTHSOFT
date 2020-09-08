using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARTHSOFT.Models
{
    public class Contact
    {
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String Email { get; set; }
        public int PhoneNumber { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String ZIP { get; set; }

        public Contact()
        {
            LastName = String.Empty;
            FirstName = String.Empty;
            Email = String.Empty;
            PhoneNumber = 0;
            Address = String.Empty;
            City = String.Empty;
            ZIP = String.Empty;
        }
    }
}