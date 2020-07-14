using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace Contenter.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        [Inject]
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) { }


        public static ApplicationDbContext Create() =>
            new ApplicationDbContext();
        
    }

    public class DbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };


            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);



            // создаем пользователей
            var admin = new ApplicationUser { Email = "trablohin@gmail.com", UserName = "trablohin@gmail.com" };
            string password = "_123!";
            var result = userManager.Create(admin, password);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }


            var person1 = new Person { Email = "123@gmail.com", Forename = "Andi", Sirname = "Gordon" };
            var person2 = new Person { Email = "234@gmail.com", Forename = "Robert", Sirname = "Brown" };
            var person3 = new Person { Email = "345@gmail.com", Forename = "Phil", Sirname = "Deakin" };

            var Membership = new Membership { MemebrshipNumber = 1, Type = MembershipType.Primary, AccountBalance = 500, Person = person1 };

            base.Seed(context);
        }
    }

    public static class ContextExtensions
    {
        
        public static string GetTableName<T>(this DbContext context) where T : class =>
            context is IObjectContextAdapter objContext 
            ? objContext.ObjectContext.GetTableName<T>() 
            : throw new InvalidCastException();
        

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex(@"FROM\s+(?<table>.+)\s+AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }
}