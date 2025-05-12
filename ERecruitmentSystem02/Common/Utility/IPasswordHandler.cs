namespace ERecruitmentSystem02.Common.Utility
{
    public interface IPasswordHandler
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
