using Microsoft.AspNetCore.Identity;

namespace ERecruitmentSystem02.Common.Utility
{
    public class PasswordHandler : IPasswordHandler
    {
        private readonly IPasswordHasher<object> _passwordHasher;

        public PasswordHandler(IPasswordHasher<object> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
