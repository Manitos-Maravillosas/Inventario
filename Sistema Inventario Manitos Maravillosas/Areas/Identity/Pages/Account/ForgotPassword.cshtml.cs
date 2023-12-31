﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using EmailService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailService.Models.IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<AppUser> userManager, EmailService.Models.IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Se requiere el campo de correo")]
            [EmailAddress]
            [Display(Name = "Correo")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var toList = new List<(string email, string displayName)>
                {
                    (Input.Email, Input.Email)
                };

                var message = new Message(toList, "Reestablece tu contraseña", $"Por favor reestablece tu contraseña haciendo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>click aquí</a>.");

                await _emailSender.SendEmailAsync(message);

                /*
                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                */
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

    }
}
