using System;
using System.ComponentModel.DataAnnotations;

namespace SAP1EMU.GUI.Models
{
    public class InputModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Date of Birth"), DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}