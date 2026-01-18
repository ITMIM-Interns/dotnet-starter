using Identity.BLL.Exceptions.Commons;

namespace Identity.BLL.Exceptions.Users
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string message) :base(message) { }
       
    }
}
