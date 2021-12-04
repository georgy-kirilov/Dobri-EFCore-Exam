using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Validation;

namespace Theatre.Data.Models
{
    public class Theatre
    {
        [Key]
        public int Id { get; set; }
        
        //[StringLength(30, MinimumLength =4)]
        [StringLength(ValidationConstraints.Theatre.NameMaxLength, MinimumLength = ValidationConstraints.Theatre.NameMinLength)]
        [Required]
        public string Name { get; set; }

        [Required]
        //between 1-10
        [Range(ValidationConstraints.Theatre.MinNumberOfHalls, ValidationConstraints.Theatre.MaxNumberOfHalls)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        //[StringLength(30, MinimumLength = 4)]
        [StringLength(ValidationConstraints.Theatre.DirectorMaxLength, MinimumLength = ValidationConstraints.Theatre.DirectorMinLength)]
        public string Director { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
