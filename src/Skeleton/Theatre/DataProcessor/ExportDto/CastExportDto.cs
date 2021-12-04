using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Validation;

namespace Theatre.DataProcessor.ExportDto
{
    public class CastExportDto
    {
        [Required]
        [StringLength(ValidationConstraints.Cast.FullNameMaxLength, MinimumLength = ValidationConstraints.Play.TitleMinLength)]
        public string FullName { get; set; }

        [Required]
        public bool IsMainCharacter { get; set; }

        [Required]

        public string MainCharacter { get; set; }
    }
}
