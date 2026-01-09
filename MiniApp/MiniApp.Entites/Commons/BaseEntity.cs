using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Models.Commons
{
    public abstract class BaseEntity<Tkey> where Tkey : struct
    {
        public Tkey Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
