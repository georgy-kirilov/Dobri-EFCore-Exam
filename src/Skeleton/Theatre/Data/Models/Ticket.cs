using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models
{
    public class Ticket
    {

        [Key]
        public int Id { get; set; }

       // [Range(1.00, 100.00)]
        public decimal Price { get; set; }

        [Required]
        //range 1-10
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

//•	Id – integer, Primary Key
//•	Price – decimal in the range [1.00….100.00] (required)
//•	RowNumber – sbyte in range[1….10](required)
//•	PlayId – integer, foreign key(required)
//•	TheatreId – integer, foreign key(required)
