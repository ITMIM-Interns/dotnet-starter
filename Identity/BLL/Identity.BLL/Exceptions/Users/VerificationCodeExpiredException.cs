using Identity.BLL.Exceptions.Commons;

namespace Identity.BLL.Exceptions.Users
{
    public class VerificationCodeExpiredException : InvalidAccountException
    {
        public VerificationCodeExpiredException(string message) : base(message) { }
        
    }
}
