using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto.AccountsDTOs;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Microsoft.AspNetCore.Identity;

namespace Filminurk.ApplicationServices.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailsServices _emailsServices;

        public AccountServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailsServices emailsServices
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailsServices = emailsServices;
        }

        public async Task<ApplicationUser> Register(ApplicationUserDTO userDTO)
        {
            var user = new ApplicationUser
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                ProfileType = userDTO.ProfileType,
                DisplayName = userDTO.DisplayName,

            };
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if (result.Succeeded) 
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            return user;
        }
    }
}
