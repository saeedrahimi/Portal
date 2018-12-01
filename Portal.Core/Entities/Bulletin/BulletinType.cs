using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.Entities.Bulletin
{
    public class BulletinType : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
