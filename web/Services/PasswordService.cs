using signup_example.Models;

namespace signup_example.Services
{
    public class PasswordService: IPasswordService
    {
        public Password SaltAndHashPassword(string password)
        {
            string salt = IPasswordService.CreateSalt(16); // Recommended salt length
            string saltedPassword = salt + password;

            return new Password
            {
                HashedPassword = IPasswordService.HashPassword(saltedPassword),
                Salt = salt
            };
        }

    }
}
