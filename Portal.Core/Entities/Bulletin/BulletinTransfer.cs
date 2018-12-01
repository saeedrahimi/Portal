using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.Entities.Bulletin
{
    public class BulletinTransfer : BaseEntity
    {
        public virtual Bulletin Bulletin { get; set; }
        public virtual int FromUser { get; set; }
        public virtual int ToUser { get; set; }
        public DateTime SentOn { get; set; }
        public DateTime ReadOn { get; set; }
        public string Description { get; set; }
    }
}
