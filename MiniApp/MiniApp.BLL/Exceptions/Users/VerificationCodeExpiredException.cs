using MiniApp.BLL.Exceptions.Commons;

namespace MiniApp.BLL.Exceptions.Users
{
    public class VerificationCodeExpiredException : InvalidAccountException
    {
        public VerificationCodeExpiredException(string message) : base(message) { }
        
    }
}
