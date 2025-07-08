namespace Note_oriousWebApp.API.Helpers
{
    public class PasswordHelper
    {

        // Method to hash a password using BCrypt
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
            }

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Method to verify a password against a hashed password
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentException("Password and Hashed Password cannot be null or empty", nameof(password));
            }

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
}
