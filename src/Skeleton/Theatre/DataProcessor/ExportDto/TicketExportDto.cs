using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Theatre.Validation;

namespace Theatre.DataProcessor.ExportDto
{
    public class TicketExportDto
    {
        [Range((double)ValidationConstraints.Ticket.MinPrice, (double)ValidationConstraints.Ticket.MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        //range 1-10
        [Range(ValidationConstraints.Ticket.MinRowNumber, ValidationConstraints.Ticket.MaxRowNumber)]
        public sbyte RowNumber { get; set; }
    }
}
