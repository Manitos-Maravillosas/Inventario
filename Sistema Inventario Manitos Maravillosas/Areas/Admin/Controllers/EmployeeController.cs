using EmailService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Identity.Data;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Helpers;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Text;
using System.Text.Encodings.Web;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class EmployeeController : Controller

    {
        private readonly IEmployeeService _employeeService;
        private readonly IBusinessService _businessService;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly ILogger<EmployeeController> _logger;
        private readonly EmailService.Models.IEmailSender _emailSender;
        private readonly IGenerateRandomPassword _generateRandomPassword;

        public EmployeeController(
            IEmployeeService employeeService,
            IBusinessService businessService,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            ILogger<EmployeeController> logger,
            EmailService.Models.IEmailSender emailSender)
        {
            _employeeService = employeeService;
            _businessService = businessService;
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            var employees = _employeeService.GetAll();

            return View(employees);
        }


        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            var businessNames = _businessService.GetBusinessNames();
            var roleNames = _employeeService.GetRoleNames();

            ViewBag.BusinessNames = new SelectList(businessNames);
            ViewBag.RoleNames = new SelectList(roleNames);

            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Employee employee)
        {

            if (ModelState.IsValid)
            {
                OperationResult result = _employeeService.Add(employee);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                await CreateUserAndRoleAsync(employee);
                ViewData["Success"] = "Empleado agregado correctamente!";

            }
            ViewBag.BusinessNames = new SelectList(_businessService.GetBusinessNames());
            ViewBag.RoleNames = new SelectList(_employeeService.GetRoleNames());
            return View();
        }

        public async Task<IActionResult> CreateUserAndRoleAsync(Employee employee)
        {
            string returnUrl = null;
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, employee.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, employee.Email, CancellationToken.None);

                var _generateRandomPassword = new GenerateRandomPassword();
                var password = _generateRandomPassword.GenerateRandomPasswordMethod(new PasswordOptions()
                {
                    RequireDigit = true,
                    RequiredLength = 9,
                    RequireLowercase = true,
                    RequireUppercase = true
                });
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var confirmation = await AssignUserToRoleAsync(user, employee.Role);

                    if (!confirmation)
                    {
                        ViewData["ErrorMessage"] = "No se pudo asignar el rol al usuario";
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    var toList = new List<(string email, string displayName)>
                    {
                        (employee.Email, employee.Email)
                    };

                    var message = new Message(toList, "Confirma tu cuenta", $"Por favor confirma tu cuenta haciendo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>click aquí</a>.<br>Tu contraseña temporal es: {password}");

                    await _emailSender.SendEmailAsync(message);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = employee.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        public async Task<bool> AssignUserToRoleAsync(AppUser user, string roleName)
        {

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return true;
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return false;
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(string id)
        {
            Employee employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.BusinessNames = new SelectList(_businessService.GetBusinessNames());
            return View(employee);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                OperationResult result = _employeeService.Update(employee);
                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.Message;
                }
                ViewData["Success"] = "Se ha modificado los datos del empleado!";

            }
            return View();
        }

        // GET: EmployeeController/Delete/5
        public async Task<ActionResult> DeleteAsync(string id)
        {
            string userId = _employeeService.GetUserId(id);
            OperationResult result = _employeeService.Delete(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Message;
            }

            var user = _userManager.FindByIdAsync(userId).Result;
            var resultUser = await _userManager.DeleteAsync(user);
            if (!resultUser.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);
            ViewData["Success"] = "Se ha eliminado al Empleado!";

            var employees = _employeeService.GetAll();
            return View("Index", employees);
        }


    }
}
