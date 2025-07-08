using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace Note_oriousWebApp.API.Helpers
{
    public class GenerateRandomStringHelper
    {
        // Define character sets for generating the random string
        private static readonly string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string Digits = "0123456789";
        private static readonly string Special = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        /* <summary>
            Generates a random string that includes at least one uppercase letter,
            one lowercase letter, one digit, and one special character.
            </summary>
            <param name="length">The total desired length of the string (minimum is 4)</param>
            <returns>A randomly generated secure string</returns>
        */

        public static string GenerateRandomString(int length)
        {
            // Enforce minimum length of 4 to fit all required character types
            if (length < 4)
            {
                throw new ArgumentException("Length must be at least 4 characters.");
            }

            // Initialize the random number generator
            Random random = new Random();

            // Use StringBuilder to efficiently construct the result string
            var result = new StringBuilder();

            // Ensure at least one uppercase letter is included
            result.Append(Uppercase[random.Next(Uppercase.Length)]);

            // Ensure at least one lowercase letter is included
            result.Append(Lowercase[random.Next(Lowercase.Length)]);

            // Ensure at least one digit is included
            result.Append(Digits[random.Next(Digits.Length)]);

            // Ensure at least one special character is included
            result.Append(Special[random.Next(Special.Length)]);

            // Combine all character sets to fill the remaining characters
            string allCharacters = Uppercase + Lowercase + Digits + Special;

            // Append random characters from the combined set to reach the desired length
            for (int i = 4; i < length; i++)
            {
                result.Append(allCharacters[random.Next(allCharacters.Length)]);
            }

            // Shuffle the final string to ensure the required characters aren't always at the beginning
            return new string(
                result
                    .ToString() // Convert StringBuilder to string
                    .ToCharArray() // Convert string to character array
                    .OrderBy(_ => random.Next()) // Randomly reorder the characters
                    .ToArray() // Convert back to array
            );
        }
    }
}
