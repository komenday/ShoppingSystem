using System.Collections.Generic;

namespace TaskAuthenticationAuthorization.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Customer> Customers { get; set; }

        public Role()
        {
            Customers = new List<Customer>();
        }
    }
}
