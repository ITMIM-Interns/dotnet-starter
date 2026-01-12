using MiniApp.BLL.Exceptions.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.BLL.Exceptions.Users
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string message) :base(message) { }
       
    }
}
