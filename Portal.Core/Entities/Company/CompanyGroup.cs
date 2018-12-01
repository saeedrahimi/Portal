using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.Entities.Bulletin;

namespace Portal.Core.Entities.Company
{
    public class CompanyGroup : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<BulletinType> BulletinTypes { get; set; }
    }
}
