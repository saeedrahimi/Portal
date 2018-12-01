using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Portal.Dto.Dtos.Bulletin;

namespace Portal.Dto.Dtos.Company
{
    public class CompanyGroupEditDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IEnumerable<SelectListItem> TypesList { get; set; }
    }
}
