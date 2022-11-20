using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TaskAuthenticationAuthorization.Models
{
    public enum Discount
    {
        None, Regular, Golden, Wholesale
    }
    public class Customer
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public Discount? Discount { get; set; }
        public ICollection<Order> Orders { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

    }

    
}
