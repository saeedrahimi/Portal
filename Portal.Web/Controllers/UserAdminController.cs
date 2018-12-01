﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Portal.Core.Service.Company;
using Portal.Data.Identity.Contracts;
using Portal.Data.Identity.Models;
using Portal.Dto.Dtos.Company;
using Portal.Web.Models;

namespace Portal.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        private readonly IApplicationRoleManager _roleManager;
        private readonly IApplicationUserManager _userManager;
        private readonly ICompanyGroupService _companyGroupService;
        public UsersAdminController(IApplicationUserManager userManager,
                                    IApplicationRoleManager roleManager, ICompanyGroupService companyGroupService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _companyGroupService = companyGroupService;
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await _roleManager.GetAllCustomRolesAsync().ConfigureAwait(false), "Name", "Name");
            ViewBag.CompanyGroups = new SelectList((await _companyGroupService.GetAllGroupsAsync().ConfigureAwait(false)).Value, "Id", "Title");

            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, Guid selectedCompanyGroup, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
                var adminresult = await _userManager.CreateAsync(user, userViewModel.Password).ConfigureAwait(false);

                //Add User to the selected Roles
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await _userManager.AddToRolesAsync(user.Id, selectedRoles).ConfigureAwait(false);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await _roleManager.GetAllCustomRolesAsync().ConfigureAwait(false), "Name", "Name");
                            ViewBag.CompanyGroups = new SelectList((await _companyGroupService.GetAllGroupsAsync().ConfigureAwait(false)).Value, "Id", "Title");
                            return View();
                        }
                    }

                    _userManager.AddToCompanyGroup(user.Id, selectedCompanyGroup);
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(await _roleManager.GetAllCustomRolesAsync().ConfigureAwait(false), "Name", "Name");
                    ViewBag.CompanyGroups = new SelectList((await _companyGroupService.GetAllGroupsAsync().ConfigureAwait(false)).Value, "Id", "Title");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(await _roleManager.GetAllCustomRolesAsync().ConfigureAwait(false), "Name", "Name");
            ViewBag.CompanyGroups = new SelectList((await _companyGroupService.GetAllGroupsAsync().ConfigureAwait(false)).Value, "Id", "Title");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByIdAsync(id.Value).ConfigureAwait(false);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await _userManager.FindByIdAsync(id.Value).ConfigureAwait(false);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }

                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByIdAsync(id.Value).ConfigureAwait(false);

            ViewBag.RoleNames = await _userManager.GetRolesAsync(user.Id).ConfigureAwait(false);

            return View(user);
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByIdAsync(id.Value).ConfigureAwait(false);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user.Id).ConfigureAwait(false);
            var allCompanyGroups = await _companyGroupService.GetAllGroupsAsync();
            return View(new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                RolesList = (await _roleManager.GetAllCustomRolesAsync().ConfigureAwait(false)).Select(x => new SelectListItem
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                }),
                CompanyGroups = allCompanyGroups.Value.Select(x => new SelectListItem
                {
                    Selected = user.CompanyGroup?.Id == x.Id,
                    Text = x.Title,
                    Value = x.Id.ToString()
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id")] EditUserViewModel editUser, Guid selectedCompanyGroup, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(editUser.Id).ConfigureAwait(false);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;

                var userRoles = await _userManager.GetRolesAsync(user.Id).ConfigureAwait(false);

                selectedRole = selectedRole ?? new string[] { };

                var result = await _userManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray()).ConfigureAwait(false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }


                _userManager.AddToCompanyGroup(user.Id, selectedCompanyGroup);

                await _userManager.UpdateSecurityStampAsync(user.Id).ConfigureAwait(true);

                result = await _userManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray()).ConfigureAwait(false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                await _userManager.UpdateSecurityStampAsync(user.Id).ConfigureAwait(false);

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            return View(await _userManager.GetAllUsersAsync().ConfigureAwait(false));
        }


        public ActionResult AdminUsers()
        {
            return View(_roleManager.GetApplicationUsersInRole("Admin"));
        }
    }
}