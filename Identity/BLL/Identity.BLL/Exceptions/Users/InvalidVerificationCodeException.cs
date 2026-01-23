using Identity.BLL.Exceptions.Commons;

namespace Identity.BLL.Exceptions.Users
{
    public  class InvalidVerificationCodeException : InvalidAccountException
    {
        public InvalidVerificationCodeException(string message):base(message) { }
    }
}
