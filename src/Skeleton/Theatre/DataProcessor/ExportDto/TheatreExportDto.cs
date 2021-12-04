using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Data.Models;
using Theatre.Validation;

namespace Theatre.DataProcessor.ExportDto
{
    public class TheatreExportDto
    {
    
        [StringLength(ValidationConstraints.Theatre.NameMaxLength, MinimumLength = ValidationConstraints.Theatre.NameMinLength)]
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(ValidationConstraints.Theatre.MinNumberOfHalls, ValidationConstraints.Theatre.MaxNumberOfHalls)]
        public sbyte Halls { get; set; }

        [Required]
        public decimal TotalIncome{ get; set; }

        public ICollection<TicketExportDto> Tickets { get; set; }

    }
}
