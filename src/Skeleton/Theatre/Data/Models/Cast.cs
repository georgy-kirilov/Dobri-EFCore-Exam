using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models
{
    public class Cast
    {
        [Key]
        public int Id { get; set; }


        //•	FullName – text with length [4, 30] (required)
        [Required]
        public string FullName { get; set; }

        [Required]
        public bool IsMainCharacter { get; set; }

        //text in the following format: "+44-{2 numbers}-{3 numbers}-{4 numbers}".
        //Valid phone numbers are: +44 - 53 - 468 - 3479, +44 - 91 - 842 - 6054, +44 - 59 - 742 - 3119
        [Required]
        public string PhoneNumber { get; set; }


        [Required]
        public int PlayId { get; set; }

        [ForeignKey(nameof(PlayId))]
        [Required]
        public virtual Play Play { get; set; }

    }
}
