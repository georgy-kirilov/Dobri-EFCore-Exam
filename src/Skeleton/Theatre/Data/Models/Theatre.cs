using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.Data.Models
{
    public class Theatre
    {
        [Key]
        public int Id { get; set; }
        
        //[StringLength(30, MinimumLength =4)]
        [Required]
        public string Name { get; set; }

        [Required]
        //between 1-10
        public sbyte NumberOfHalls { get; set; }

        [Required]
        //[StringLength(30, MinimumLength = 4)]
        public string Director { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
