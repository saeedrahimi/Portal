using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Portal.Core.Service.Bulletin;
using Portal.Core.Service.Company;
using Portal.Dto.Dtos.Bulletin;
using Portal.Dto.Dtos.Company;
using Portal.Dto.Result;
using Portal.Web.Models;

namespace Portal.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BulletinTypesController : Controller
    {
        private readonly IBulletinTypeService _bulletinTypeService;

        public BulletinTypesController(IBulletinTypeService bulletinTypeService)
        {
            _bulletinTypeService = bulletinTypeService;
        }

        // GET: /BulletinType/
        public async Task<ActionResult> Index()
        {
            var result = await _bulletinTypeService.GetAllGroupsAsync();

            return result.OnBoth(r=>r.Succeeded ? View(r.Value): View());
        }

        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BulletinTypeDto group)
        {
            if (ModelState.IsValid)
            {

                var result = _bulletinTypeService.AddType(group);

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
            var result = _bulletinTypeService.GetById(id.Value);
            if (!result.Succeeded)
            {
                return HttpNotFound();
            }
            var groupModel = new BulletinTypeDto() { Id = result.Value.Id, Title = result.Value.Title,Description = result.Value.Description};
            return View(groupModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BulletinTypeDto group)
        {
            if (ModelState.IsValid)
            {

                var result = _bulletinTypeService.EditType(group);

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


        public async Task<ActionResult> Delete(Guid? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = _bulletinTypeService.GetById(id.Value);
            if (!result.Succeeded)
            {
                return HttpNotFound();
            }
            var groupModel = new BulletinTypeDto { Id = result.Value.Id, Title = result.Value.Title, Description = result.Value.Description };
            return View(groupModel);
        }

        //
        // POST: /BulletinType/Delete/5
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
                var result = _bulletinTypeService.GetById(id.Value);
                if (!result.Succeeded)
                {
                    return HttpNotFound();
                }
                var delResult = _bulletinTypeService.Delete(result.Value);
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