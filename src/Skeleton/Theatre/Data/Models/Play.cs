using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Data.Models.Enums;
using Theatre.Validation;

namespace Theatre.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; set; }

       // [StringLength(50, MinimumLength =4)]
        [Required]
        [StringLength(ValidationConstraints.Play.TitleMaxLength, MinimumLength = ValidationConstraints.Play.TitleMinLength)]
        public string Title { get; set; }

        //{ hours: minutes: seconds}, with a minimum length of 1 hour. (required)
        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        // between 0.00….10.00
        [Range(ValidationConstraints.Play.MinRating, ValidationConstraints.Play.MaxRating)]
        public float Rating { get; set; }

        [Required]
        public Genre Genre { get; set; }

        //text with length up to 700 characters (required)
        [Required]
        [MaxLength(ValidationConstraints.Play.DescriptionMaxLength)]
        public string Description { get; set; }

        //text with length [4, 30] (required)
        [Required]
        [StringLength(ValidationConstraints.Play.ScreenwriterMaxLength, MinimumLength = ValidationConstraints.Play.ScreenwriterMinLength)]
        public string Screenwriter { get; set; }

        public ICollection<Cast> Casts { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}

