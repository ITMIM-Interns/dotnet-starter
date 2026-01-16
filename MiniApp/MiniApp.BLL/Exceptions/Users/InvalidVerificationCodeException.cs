using MiniApp.BLL.Exceptions.Commons;

namespace MiniApp.BLL.Exceptions.Users
{
    public  class InvalidVerificationCodeException : InvalidAccountException
    {
        public InvalidVerificationCodeException(string message):base(message) { }
    }
}
