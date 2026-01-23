using Identity.BLL.Exceptions.Commons;

namespace Identity.BLL.Exceptions.Users
{
    public class VerificationNotFoundException : NotFoundException
    {
        public VerificationNotFoundException(string message) : base(message) { }
        
    }
}
