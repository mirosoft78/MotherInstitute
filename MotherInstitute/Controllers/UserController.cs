using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System;
using System.Linq;

namespace MotherInstitute.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // INDEX
        // =====================================================

        public IActionResult Index()
        {
            var users = _context.Users.ToList();

            return View(users);
        }

        // =====================================================
        // CREATE USER
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.REGDDATE = DateTime.Now;

                // SAVE USER TABLE

                _context.Users.Add(user);

                // ============================================
                // IF MARKETING USER THEN SAVE AGENT TABLE ALSO
                // ============================================

                if (user.TYPE == "MARKETING")
                {
                    var existingAgent = _context.MarketingAgents
                        .FirstOrDefault(x => x.MobileNo == user.LOGINID);

                    if (existingAgent == null)
                    {
                        MarketingAgent agent = new MarketingAgent();

                        agent.Name = user.NAME;

                        agent.MobileNo = user.LOGINID;

                        agent.Address = "";

                        agent.JoiningDate = DateTime.Now;

                        agent.Status = "Active";

                        _context.MarketingAgents.Add(agent);
                    }
                }

                _context.SaveChanges();

                TempData["SuccessMessage"] =
                    "User saved successfully!";

                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] =
                "User not saved. Please check the details.";

            var users = _context.Users.ToList();

            return View("Index", users);
        }

        // =====================================================
        // EDIT PAGE
        // =====================================================

        public IActionResult Edit(string id)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.LOGINID == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // =====================================================
        // EDIT SAVE
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            var existingUser = _context.Users
                .FirstOrDefault(x => x.LOGINID == user.LOGINID);

            if (existingUser != null)
            {
                existingUser.NAME = user.NAME;

                existingUser.PSW = user.PSW;

                existingUser.TYPE = user.TYPE;

                existingUser.STATUS = user.STATUS;

                // ============================================
                // UPDATE MARKETING AGENT ALSO
                // ============================================

                if (user.TYPE == "MARKETING")
                {
                    var agent = _context.MarketingAgents
                        .FirstOrDefault(x => x.MobileNo == user.LOGINID);

                    if (agent != null)
                    {
                        agent.Name = user.NAME;
                    }
                }

                _context.SaveChanges();

                TempData["SuccessMessage"] =
                    "User updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] =
                    "User not found.";
            }

            return RedirectToAction("Index");
        }

        // =====================================================
        // INLINE UPDATE
        // =====================================================

        [HttpPost]
        public JsonResult UpdateInline([FromBody] UserUpdateModel model)
        {
            var existingUser = _context.Users
                .FirstOrDefault(x => x.LOGINID == model.OLDLOGINID);

            if (existingUser == null)
            {
                return Json(new
                {
                    success = false,
                    message = "User not found"
                });
            }

            try
            {
                if (existingUser.LOGINID != model.LOGINID)
                {
                    var duplicateUser = _context.Users
                        .FirstOrDefault(x => x.LOGINID == model.LOGINID);

                    if (duplicateUser != null)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "New Login ID already exists"
                        });
                    }

                    var newUser = new User
                    {
                        LOGINID = model.LOGINID,
                        NAME = model.NAME,
                        PSW = model.PSW,
                        TYPE = model.TYPE,
                        STATUS = model.STATUS,
                        REGDDATE = existingUser.REGDDATE
                    };

                    _context.Users.Add(newUser);

                    // ============================================
                    // UPDATE MARKETING AGENT MOBILE NO
                    // ============================================

                    if (model.TYPE == "MARKETING")
                    {
                        var agent = _context.MarketingAgents
                            .FirstOrDefault(x => x.MobileNo == model.OLDLOGINID);

                        if (agent != null)
                        {
                            agent.MobileNo = model.LOGINID;

                            agent.Name = model.NAME;
                        }
                    }

                    _context.Users.Remove(existingUser);
                }
                else
                {
                    existingUser.NAME = model.NAME;

                    existingUser.PSW = model.PSW;

                    existingUser.TYPE = model.TYPE;

                    existingUser.STATUS = model.STATUS;

                    // ============================================
                    // UPDATE MARKETING AGENT NAME
                    // ============================================

                    if (model.TYPE == "MARKETING")
                    {
                        var agent = _context.MarketingAgents
                            .FirstOrDefault(x => x.MobileNo == model.LOGINID);

                        if (agent != null)
                        {
                            agent.Name = model.NAME;
                        }
                    }
                }

                _context.SaveChanges();

                TempData["SuccessMessage"] =
                    "User updated successfully!";

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // DELETE
        // =====================================================

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            var user = _context.Users
                .FirstOrDefault(x => x.LOGINID == id);

            if (user != null)
            {
                // ============================================
                // DELETE MARKETING AGENT ALSO
                // ============================================

                if (user.TYPE == "MARKETING")
                {
                    var agent = _context.MarketingAgents
                        .FirstOrDefault(x => x.MobileNo == user.LOGINID);

                    if (agent != null)
                    {
                        _context.MarketingAgents.Remove(agent);
                    }
                }

                _context.Users.Remove(user);

                _context.SaveChanges();

                TempData["SuccessMessage"] =
                    "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] =
                    "User not found.";
            }

            return RedirectToAction("Index");
        }
    }
}