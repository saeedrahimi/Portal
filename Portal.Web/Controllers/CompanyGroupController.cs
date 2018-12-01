using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Portal.Core.Service.Bulletin;
using Portal.Core.Service.Company;
using Portal.Dto.Dtos.Company;
using Portal.Dto.Result;
using Portal.Web.Models;

namespace Portal.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyGroupController : Controller
    {
        private readonly ICompanyGroupService _companyGroupService;
        private readonly IBulletinTypeService _bulletinTypeService;

        public CompanyGroupController(ICompanyGroupService companyGroupService, IBulletinTypeService bulletinTypeService)
        {
            _companyGroupService = companyGroupService;
            _bulletinTypeService = bulletinTypeService;
        }

        // GET: /CompanyGroup/
        public async Task<ActionResult> Index()
        {
            var result = await _companyGroupService.GetAllGroupsAsync();

            return result.OnBoth(r=>r.Succeeded ? View(r.Value): View());
        }

        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyGroupDto group)
        {
            if (ModelState.IsValid)
            {

                var result = _companyGroupService.AddGroup(group);

                result.OnSuccess(() => { })
                    .OnFailure(() => ModelState.AddModelError("", result.Error));
                if (!result.Succeeded)
                {
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = _companyGroupService.GetById(id.Value);
            if (!result.Succeeded)
            {
                return HttpNotFound();
            }
            var allTyps = await _bulletinTypeService.GetAllGroupsAsync();
            var groupModel = new CompanyGroupEditDto
            {
                Id = result.Value.Id,
                Title = result.Value.Title,
                Description = result.Value.Description,
                TypesList = allTyps.Value.Select(x => new SelectListItem
                {
                Selected = result.Value.BulletinTypes.Any(a=>a.Id == x.Id),
                Text = x.Title,
                Value = x.Id.ToString()
            })
            };
            return View(groupModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CompanyGroupEditDto group, params Guid[] selectedTypes)
        {
            if (ModelState.IsValid)
            {

                var result = _companyGroupService.EditGroup(group, selectedTypes);
                if (result.Succeeded)
                {
                }
                result.OnSuccess(() => { })
                    .OnFailure(() => ModelState.AddModelError("", result.Error));
                if (!result.Succeeded)
                {

                    var allTyps = await _bulletinTypeService.GetAllGroupsAsync();
                    group.TypesList = allTyps.Value.Select(x => new SelectListItem
                    {
                        Selected = selectedTypes.Any(a => a == x.Id),
                        Text = x.Title,
                        Value = x.Id.ToString()
                    });
                    return View();
                }
                return RedirectToAction("Index");
            }
            var allTyps2 = await _bulletinTypeService.GetAllGroupsAsync();
            group.TypesList = allTyps2.Value.Select(x => new SelectListItem
            {
                Selected = selectedTypes.Any(a => a == x.Id),
                Text = x.Title,
                Value = x.Id.ToString()
            });
            return View();
        }


        public async Task<ActionResult> Delete(Guid? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = _companyGroupService.GetById(id.Value);
            if (!result.Succeeded)
            {
                return HttpNotFound();
            }
            CompanyGroupDto groupModel = new CompanyGroupDto { Id = result.Value.Id, Title = result.Value.Title, Description = result.Value.Description };
            return View(groupModel);
        }

        //
        // POST: /CompanyGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid? id)
        {
            if (ModelState.IsValid)
            {
                if (!id.HasValue)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var result = _companyGroupService.GetById(id.Value);
                if (!result.Succeeded)
                {
                    return HttpNotFound();
                }
                var delResult = _companyGroupService.Delete(result.Value);
                if (!delResult.Succeeded)
                {
                    ModelState.AddModelError("", delResult.Error);
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}