namespace Theatre.DataProcessor.ImportDto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Theatre.Data.Models.Enums;

    using static Theatre.Validation.ValidationConstraints.Play;

    [XmlType("Play")]
    public class PlayDto
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required]
        [Range(MinRating, MaxRating)]
        public float Rating { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [StringLength(ScreenwriterMaxLength, MinimumLength = ScreenwriterMinLength)]
        public string Screenwriter { get; set; }
    }
}
