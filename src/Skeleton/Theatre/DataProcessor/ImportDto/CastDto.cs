namespace Theatre.DataProcessor.ImportDto
{
    using System.Xml.Serialization;
    using System.ComponentModel.DataAnnotations;

    using static Theatre.Validation.ValidationConstraints.Cast;

    [XmlType("Cast")]
    public class CastDto
    {
        [Required]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(PhoneNumberRegex)]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsMainCharacter { get; set; }

        [Required]
        public int PlayId { get; set; }
    }
}
