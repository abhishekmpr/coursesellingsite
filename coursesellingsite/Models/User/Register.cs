using System.ComponentModel.DataAnnotations;

namespace coursesellingsite.Models
{
    public class Register
    {

        public int Id { get; set; }
        public string FirstName { get; set; }

        public string Password { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public int Postcode { get; set; }
        public string Description { get; set; }

    }
}
