using System.ComponentModel.DataAnnotations;

namespace EzyPeezy.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        public string Name { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "Contact No.")]
        public int cell_no { get; set; }

    }
}