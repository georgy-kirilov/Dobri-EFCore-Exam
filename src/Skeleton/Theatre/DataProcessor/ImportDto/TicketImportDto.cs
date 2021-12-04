namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using static Theatre.Validation.ValidationConstraints.Ticket;

    public class TicketImportDto
    {
        [Required]
        [Range((double)MinPrice, (double)MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [Range(MinRowNumber, MaxRowNumber)]
        public sbyte RowNumber { get; set; }

        [Required]
        public int PlayId { get; set; }
    }
}
