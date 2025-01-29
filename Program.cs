using BlogApps.Data.Concrete.EfCore;
using BlogApps.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using BlogApps.Entity;
using Microsoft.AspNetCore.Identity;
using BlogApps.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSignalR();

        builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
         new SmtpEmailSender(
        builder.Configuration["EmailSender:Host"],
        builder.Configuration.GetValue<int>("EmailSender:Port"),
        builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
        builder.Configuration["EmailSender:Username"],
        builder.Configuration["EmailSender:Password"])
);

        // Add services to the container.
        builder.Services.AddControllersWithViews();


        builder.Services.AddDbContext<BlogContext>(options =>
        {
            var config = builder.Configuration;
            var connectionsString = config.GetConnectionString("mysql_conneciton");
            var version = new MySqlServerVersion(new Version(8, 0));

            options.UseMySql(connectionsString, version);
        });
        builder.Services.AddIdentity<User, UserRole>().AddEntityFrameworkStores<BlogContext>().AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;

            options.User.RequireUniqueEmail = true;
            // options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";

            // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            // options.Lockout.MaxFailedAccessAttempts = 5;

            // options.SignIn.RequireConfirmedEmail = true;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
        });
        builder.Services.AddScoped<IPostRepository, EFPostRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddScoped<ITagRepository, EFTagRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddScoped<ICommentRepository, EFCommentRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddScoped<IUserRepository, EFUserRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddScoped<ICategoriesRepository, EFCategoriesRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddScoped<ILikePostsRepository, EFLikePostsRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddScoped<IPostRequestRepository, EFPostRequestRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek


        builder.Services.AddSignalR();

        var app = builder.Build();
        SeedData.TestVerileriniDoldur(app);

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllerRoute(
            name: "post_by_tags",
            pattern: "posts/tag/{tagurl?}",
                    defaults: new { controller = "Posts", action = "Index" }

            );
        app.MapControllerRoute(
       name: "post_by_category",
       pattern: "posts/category/{categoryurl?}",
               defaults: new { controller = "Posts", action = "Index" }

       );
        app.MapControllerRoute(
                  name: "post_by_categoriesandtags",
                  pattern: "posts/category/{categoryurl}/tag/{tagurl?}",
                   defaults: new { controller = "Posts", action = "Index" }
        );

         app.MapControllerRoute(
            name: "popular_by_tags",
            pattern: "posts/popular/tag/{tagurl?}",
                    defaults: new { controller = "Posts", action = "popular" }

            );
        app.MapControllerRoute(
       name: "popular_by_category",
       pattern: "posts/popular/category/{categoryurl?}",
               defaults: new { controller = "Posts", action = "popular" }

       );
        app.MapControllerRoute(
                  name: "popular_by_categoriesandtags",
                  pattern: "posts/popular/category/{categoryurl}/tag/{tagurl?}",
                   defaults: new { controller = "Posts", action = "popular" }
        );


        app.MapControllerRoute(
                  name: "post_details",
                  pattern: "posts/details/{url}",
                  defaults: new { controller = "Posts", action = "Details" }
                  );
        app.MapControllerRoute(
                    name: "user_profile",
                    pattern: "users/profile/{username}",
                    defaults: new { controller = "Users", action = "Profile" }
        );
        app.MapControllerRoute(
                   name: "user_profile_edit",
                   pattern: "users/profile/edit/{url}",
                   defaults: new { controller = "Users", action = "ProfileEdit" }
       );
        app.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
        app.Run();

    }
}