using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Data.Models;
using Theatre.Data.Models.Enums;
using Theatre.Validation;

namespace Theatre.DataProcessor.ExportDto
{
    public class PlaysExportDto
    {
        [Required]
        [StringLength(ValidationConstraints.Play.TitleMaxLength, MinimumLength = ValidationConstraints.Play.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        [Range(ValidationConstraints.Play.MinRating, ValidationConstraints.Play.MaxRating)]
        public float Rating { get; set; }

        [Required]
        public Genre Genre { get; set; }
        public ICollection<Cast> Casts { get; set; }
    }
}
