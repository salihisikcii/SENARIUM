using BlogApps.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Data.Concrete.EfCore
{
    public static class SeedData
    {
        private const string adminUser = "admin";
        private const string adminPassword = "Admin_123";
        public static async void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();
                var user = await userManager.FindByNameAsync(adminUser);

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new UserRole("Üye") { NormalizedName = "UYE" } ,
                        new UserRole("Prodüktör") { NormalizedName = "PRODUKTOR" },
                        new UserRole("Senarist"){ NormalizedName = "SENARIST" },
                        new UserRole("Admin") { NormalizedName = "ADMIN" }
                    );
                    context.SaveChanges();
                }
                // Admin kullanıcısını oluştur
                if (user == null)
                {
                    user = new User
                    {
                        FullName = "Salih Işıkcı",
                        UserName = adminUser,
                        Email = "admin@salihisikci.com",
                        PhoneNumber = "5552954214"
                    };

                    // Kullanıcıyı oluştur
                    await userManager.CreateAsync(user, adminPassword);

                    // Kullanıcıya Admin rolünü ata
                    await userManager.AddToRoleAsync(user, "Admin");
                }

                // Rastgele kullanıcılar ekle
                var randomUsers = new List<(string FullName, string UserName, string Email)>
                {
                    ("Ali Demir", "alidemir", "ali@gmail.com"),
                    ("Merve Kılıç", "mervekilic", "merve@gmail.com"),
                    ("Emre Yılmaz", "emreyilmaz", "emre@gmail.com"),
                    ("Ayşe Çelik", "aysecelik", "ayse@gmail.com"),
                    ("Mustafa Özkan", "mustafaozkan", "mustafa@gmail.com"),
                    ("Elif Aksoy", "elifaksoy", "elif@gmail.com"),
                    ("Burak Şahin", "buraksahin", "burak@gmail.com")
                };

                foreach (var (fullName, userName, email) in randomUsers)
                {
                    var newUser = await userManager.FindByNameAsync(userName);
                    if (newUser == null)
                    {
                        newUser = new User
                        {
                            FullName = fullName,
                            UserName = userName,
                            Email = email
                        };

                        await userManager.CreateAsync(newUser, "deneme123");
                    }
                }



                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "Korku", Url = "korku", },
                        new Tag { Text = "Aksiyon", Url = "aksiyon", },
                        new Tag { Text = "Bilim Kurgu", Url = "bilim-kurgu", },
                        new Tag { Text = "Aşk", Url = "ask", },
                        new Tag { Text = "Anime", Url = "anime", },
                        new Tag { Text = "Aile", Url = "aile", },
                        new Tag { Text = "Gerilim", Url = "gerilim", },
                        new Tag { Text = "Polisiye", Url = "polisiye", },
                        new Tag { Text = "Dram", Url = "dram", },
                        new Tag { Text = "Macera", Url = "macera", },
                        new Tag { Text = "Savaş", Url = "savas", },
                        new Tag { Text = "Tarih", Url = "tarih", }
                        );
                    context.SaveChanges();
                }

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Categories { Text = "Mini Dizi", Url = "mini-dizi", },
                        new Categories { Text = "Tv Dizisi", Url = "tv-dizi", },
                        new Categories { Text = "Sinema Filmi", Url = "sinema-film", },
                        new Categories { Text = "Dijital Platform Dizisi", Url = "dijital-patform-dizi", },
                        new Categories { Text = "Dijital Platform Filmi", Url = "dijital-patform-film", }

                        );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                           new Post
                           {
                               Title = "Kaçış",
                               Content = "deneme1.pdf",
                               Description = "Yıldızlararası bir yolculukta, insanlar uzun zamandır kayıp bir gezegenin varlığını keşfederler. Bu gezegen, bilinen diğerlerinden çok farklı bir atmosfere ve doğaya sahiptir. Araştırmacılar, gezegenin gizemlerini çözmek için keşif yapmak üzere görevlendirilirler. Ancak keşif ekibi, gezegenin sırlarını çözmeye çalışırken beklenmedik tehlikelerle karşılaşır ve insanlığın varoluşuyla ilgili derin sorgulamalar yaparlar.",
                               Url = "kacis",
                               IsActive = true,
                               PublishedOn = DateTime.Now.AddDays(-10),
                               tags = context.Tags.Where(t => t.Text == "Korku" || t.Text == "Aksiyon" || t.Text == "Bilim Kurgu").ToList(),
                               categories = context.Categories.Where(t => t.Text == "Mini Dizi").ToList(),
                               Vote = 50,
                               Image = "bilimkurgu3.jpg",
                               UserId = "17408baa-0746-43fb-b108-1952ee4b8dad",
                               PdfActive = true,
                               IsBest = true,
                               Price = 5000,

                               comments = new List<Comment> {
                                   new Comment { Text = "Çok beğendim", PublishedOn = DateTime.Now.AddDays(-5), UserId ="2b62c99d-1b49-4a57-a6a5-d7764b35115c"},
                                   new Comment { Text = "Başarılı bir senaryo, dizisi çekileceği günü merakla bekliyorum", PublishedOn =DateTime.Now.AddDays(-2), UserId ="5fdde3a2-557f-461f-89f3-219ca5ef9735"},
                               }
                           },
                            new Post
                            {
                                Title = "2049",
                                Content = "deneme1.pdf",
                                Description = "Gelecekte, dünya yörüngesinde dev bir uzay istasyonu, insanlığın yeni yerleşim alanı olmuştur. İstasyonda yaşayan genç bir bilim insanı olan Maya ve onunla iletişim kurmak için görevlendirilen bir astronot olan Alex arasında beklenmedik bir bağ kurulur. İkisi de farklı zaman dilimlerinde doğmuş olsalar da, uzaydaki yalnızlıklarını ve duygularını paylaşırken birbirlerine yakınlaşırlar. Ancak, uzay istasyonundaki rutin bir bakım görevi sırasında, istasyonun kontrol sistemi çöker ve Maya ile Alex, uzayda kaybolur.",
                                Url = "2049",
                                IsActive = true,
                                PublishedOn = DateTime.Now.AddDays(-20),
                                tags = context.Tags.Where(t => t.Text == "Gerilim" || t.Text == "Savaş" || t.Text == "Bilim Kurgu").ToList(),
                                categories = context.Categories.Where(t => t.Text == "Sinema Filmi").ToList(),
                                Vote = 90,
                                Image = "mv4.jpg",
                                UserId = "5fdde3a2-557f-461f-89f3-219ca5ef9735",
                                PdfActive = true,
                                IsBest = true,
                                Price = 7000,


                                comments = new List<Comment> {
                                   new Comment { Text = "Enterasan bir senaryo başarılar", PublishedOn = DateTime.Now.AddDays(-5), UserId ="73863866-6ccc-4333-ae9d-6a934f35b376"},
                                   new Comment { Text = "Başarılı bir senaryo, filmi çekileceği günü merakla bekliyorum", PublishedOn =DateTime.Now.AddDays(-4), UserId ="ae5aa5db-d582-4e1a-9e9c-dca9b46f99f7"},
                               }
                            },
                            new Post
                            {
                                Title = "The Cellar",
                                Content = "deneme1.pdf",
                                Description = "İnsanlık, zamanın akışını kontrol eden bir cihazın keşfiyle büyük bir sıçrama yapar. Ancak, zamanın manipülasyonu, beklenmedik sonuçlara yol açar. Bir gün, cihazın beklenmedik bir arızası sonucu, zaman durur ve dünya sessizliğe gömülür. Ana karakter, bu sessizliği kırıp dünyayı kurtarmak için çılgınca bir mücadeleye girişir. Ancak, zamanın durmasıyla beraber ortaya çıkan gizemli bir güç, insanlığın geleceğini tehdit etmektedir. Ana karakter, zamanın sessizliğini ve dünyanın kaderini değiştirmek için zorlu bir yolculuğa çıkar.",
                                Url = "the-cellar",
                                IsActive = true,
                                PublishedOn = DateTime.Now.AddDays(-30),
                                tags = context.Tags.Where(t => t.Text == "Bilim Kurgu" || t.Text == "Polisiye" || t.Text == "Dram").ToList(),
                                categories = context.Categories.Where(t => t.Text == "Dijital Platform Dizisi").ToList(),
                                Vote = 150,
                                Image = "bilimkurgu9.jpg",
                                UserId = "ae5aa5db-d582-4e1a-9e9c-dca9b46f99f7",
                                PdfActive = true,
                                IsBest = true,
                                Price = 6000,


                                comments = new List<Comment> {
                                   new Comment { Text = "Çok beğendim", PublishedOn = DateTime.Now.AddDays(-5), UserId ="9df0499c-5574-4d55-905f-fa0d4160af19"},
                                   new Comment { Text = "Başarılı bir senaryo, dizisi çekileceği günü merakla bekliyorum", PublishedOn =DateTime.Now.AddDays(-2), UserId ="5fdde3a2-557f-461f-89f3-219ca5ef9735"},
                               }
                            },
                            new Post
                            {
                                Title = "Knife In The Dark",
                                Content = "deneme1.pdf",
                                Description = "Gelecekte, dünya yörüngesinde dev bir uzay istasyonu, insanlığın yeni yerleşim alanı olmuştur. İstasyonda yaşayan genç bir bilim insanı olan Maya ve onunla iletişim kurmak için görevlendirilen bir astronot olan Alex arasında beklenmedik bir bağ kurulur. İkisi de farklı zaman dilimlerinde doğmuş olsalar da, uzaydaki yalnızlıklarını ve duygularını paylaşırken birbirlerine yakınlaşırlar. Ancak, uzay istasyonundaki rutin bir bakım görevi sırasında, istasyonun kontrol sistemi çöker ve Maya ile Alex, uzayda kaybolur.",
                                Url = "knife-in-the-dark",
                                IsActive = true,
                                PublishedOn = DateTime.Now.AddDays(-14),
                                tags = context.Tags.Where(t => t.Text == "Tarih" || t.Text == "Aksiyon" || t.Text == "Aşk").ToList(),
                                categories = context.Categories.Where(t => t.Text == "Dijital Platform Filmi").ToList(),
                                Vote = 72,
                                Image = "bilimkurgu8.jpg",
                                UserId = "9df0499c-5574-4d55-905f-fa0d4160af19",
                                PdfActive = true,
                                Price = 5000,
                                IsBest = true,

                                comments = new List<Comment> {
                                   new Comment { Text = "Çok beğendim", PublishedOn = DateTime.Now.AddDays(-5), UserId ="17408baa-0746-43fb-b108-1952ee4b8dad"},
                                   new Comment { Text = "Başarılı bir senaryo, dizisi çekileceği günü merakla bekliyorum", PublishedOn =DateTime.Now.AddDays(-2), UserId ="ae5aa5db-d582-4e1a-9e9c-dca9b46f99f7"},
                               }
                            }


                        );
                    context.SaveChanges();

                }

            }
        }
    }
}
