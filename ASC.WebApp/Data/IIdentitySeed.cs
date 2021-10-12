using ASC.Models.Configuration;
using ASC.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASC.WebApp.Data
{
    public interface IIdentitySeed
    {
        //Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole>roleManager, IOptions<ApplicationSettings> options);
        Task Seed(UserManager<ApplicationUser> userManager, RoleManager<ElCamino.AspNetCore.Identity.AzureTable.Model.IdentityRole> roleManager, IOptions<ApplicationSettings> options);
    }
}
