using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System;
using System.Linq;
using MotherInstitute.Services;

namespace MotherInstitute.Controllers
{
    public class MarketingAgentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorLogger _errorLogger;

        public MarketingAgentController(AppDbContext context, ErrorLogger errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;
        }

        // INDEX PAGE

        public IActionResult Index()
        {
            var data = _context.MarketingAgents
                               .OrderByDescending(x => x.ID)
                               .ToList();

            return View(data);
        }

        // SAVE + UPDATE

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(
            MarketingAgent model,
            string Password)
        {
            // INSERT

            if (model.ID == 0)
            {
                _context.MarketingAgents.Add(model);

                // SAVE IN USER TABLE

                User user = new User();

                user.LOGINID = model.MobileNo;

                user.NAME = model.Name;

                user.PSW = Password;

                user.TYPE = "MARKETING";

                user.STATUS = "ACTIVE";

                user.REGDDATE = DateTime.Now;

                _context.Users.Add(user);
            }
            else
            {
                // UPDATE

                var existing = _context.MarketingAgents
                                       .FirstOrDefault(x => x.ID == model.ID);

                if (existing != null)
                {
                    existing.Name = model.Name;

                    existing.MobileNo = model.MobileNo;

                    existing.Address = model.Address;

                    // UPDATE USER TABLE ALSO

                    var user = _context.Users
                        .FirstOrDefault(x => x.LOGINID == existing.MobileNo);

                    if (user != null)
                    {
                        user.LOGINID = model.MobileNo;

                        user.NAME = model.Name;
                    }
                }
            }

            // SAVE DATABASE

            try
            {
                // SAVE DATABASE

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Marketing Agent",
    "MarketingAgentController",
    "Save",
    model.ID == 0 ? "Save Agent Button" : "Update Agent Button",
    ex.StackTrace ?? "",
    User.Identity?.Name ?? "Guest",
    "",
    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
    Request.Headers["User-Agent"].ToString()
);

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index");
        }

        // EDIT

        public IActionResult Edit(int id)
        {
            var agent = _context.MarketingAgents
                                .FirstOrDefault(x => x.ID == id);

            if (agent == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.EditData = agent;

            var data = _context.MarketingAgents
                               .OrderByDescending(x => x.ID)
                               .ToList();

            return View("Index", data);
        }

        // DELETE

        public IActionResult Delete(int id)
        {
            var data = _context.MarketingAgents
                               .FirstOrDefault(x => x.ID == id);

            try
            {
                if (data != null)
                {
                    // DELETE USER

                    var user = _context.Users
                        .FirstOrDefault(x => x.LOGINID == data.MobileNo);

                    if (user != null)
                    {
                        _context.Users.Remove(user);
                    }

                    // DELETE AGENT

                    _context.MarketingAgents.Remove(data);

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Marketing Agent",
    "MarketingAgentController",
    "Delete",
    "Delete Button",
    ex.StackTrace ?? "",
    User.Identity?.Name ?? "Guest",
    "",
    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
    Request.Headers["User-Agent"].ToString()
);

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index");
        }
    }
}