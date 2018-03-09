using System.Configuration;
using CampManagement.Data;
using CampManagement.Domain.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CampManagement.Web.Startup))]
namespace CampManagement.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Administrators"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Administrators";
                roleManager.Create(role);

                // first we create Admin rool   
                var role2 = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role2.Name = "Users";
                roleManager.Create(role2);

                var user = new ApplicationUser();
                user.UserName = "Administrator";
                user.Email = "admin@prolo.com";

                //TODO Test
                string userPWD = ConfigurationManager.AppSettings["adminPWD"];

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Administrators");

                }
            }
        }
    }
}
