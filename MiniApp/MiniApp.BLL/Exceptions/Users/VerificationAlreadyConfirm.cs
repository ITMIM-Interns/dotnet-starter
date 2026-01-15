using MiniApp.BLL.Exceptions.Commons;

namespace MiniApp.BLL.Exceptions.Users
{
    public class VerificationAlreadyConfirmed : InvalidAccountException
    {
        public VerificationAlreadyConfirmed(string message) : base(message) { }
      
    }
}
