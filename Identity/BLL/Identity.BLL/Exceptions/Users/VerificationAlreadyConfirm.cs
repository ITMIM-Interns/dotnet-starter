using Identity.BLL.Exceptions.Commons;

namespace Identity.BLL.Exceptions.Users
{
    public class VerificationAlreadyConfirmed : InvalidAccountException
    {
        public VerificationAlreadyConfirmed(string message) : base(message) { }
      
    }
}
