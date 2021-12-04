using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Theatre.Validation;

namespace Theatre.Data.Models
{
    public class Ticket
    {

        [Key]
        public int Id { get; set; }

       // [Range(1.00, 100.00)]
       [Range((double)ValidationConstraints.Ticket.MinPrice, (double)ValidationConstraints.Ticket.MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        //range 1-10
        [Range(ValidationConstraints.Ticket.MinRowNumber, ValidationConstraints.Ticket.MaxRowNumber)]
        public sbyte RowNumber { get; set; }

        [Required]
        public int PlayId { get; set; }

        [Required]
        public int TheatreId { get; set; }


        [ForeignKey(nameof(TheatreId))]
        [Required]
        public virtual Theatre Theatre { get; set; }

        [ForeignKey(nameof(PlayId))]
        [Required]
        public virtual Play Play { get; set; }
    }
}
