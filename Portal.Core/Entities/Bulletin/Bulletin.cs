using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.Entities.Bulletin
{
    public class Bulletin : BaseEntity
    {
        public string Title { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Description { get; set; }
        public virtual int Issuer { get; set; }
        public virtual BulletinType Type { get; set; }
    }
}
