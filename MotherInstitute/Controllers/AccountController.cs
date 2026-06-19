using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

public class AccountController : Controller
{
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login", "Account"); // or Home 
    }
}
