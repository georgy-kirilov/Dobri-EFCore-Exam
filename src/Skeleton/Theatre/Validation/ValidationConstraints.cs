using System;

namespace Theatre.Validation
{
    public static class ValidationConstraints
    {
        public static class Theatre
        {
            public const int NameMinLength = 4;
            public const int NameMaxLength = 30;

            public const int MinNumberOfHalls = 1;
            public const int MaxNumberOfHalls = 10;

            public const int DirectorMinLength = 4;
            public const int DirectorMaxLength = 30;
        }

        public static class Play
        {
            public const int TitleMinLength = 4;
            public const int TitleMaxLength = 50;

            // Duration constraints

            public const float MinRating = 0;
            public const float MaxRating = 10;

            public const int DescriptionMaxLength = 700;
            
            public const int ScreenwriterMinLength = 4;
            public const int ScreenwriterMaxLength = 30;
        }

        public static class Cast
        {
            public const int FullNameMinLength = 4;
            public const int FullNameMaxLength = 30;

            public const string PhoneNumberRegex = @"^\+44-[0-9]{2}-[0-9]{3}-[0-9]{4}$";
        }

        public static class Ticket
        {
            public const decimal MinPrice = 1;
            public const decimal MaxPrice = 100;

            public const int MinRowNumber = 1;
            public const int MaxRowNumber = 10;
        }
    }
}
