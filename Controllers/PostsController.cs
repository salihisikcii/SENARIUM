using BlogApps.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using BlogApps.Models;
using Microsoft.EntityFrameworkCore;
using BlogApps.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApps.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _Postrepostitory;
        private ICommentRepository _CommentRepository;
        private ITagRepository _TagRepository;
        private ICategoriesRepository _CategoriesRepository;
        private IUserRepository _UserRepository;
        private UserManager<User> _userManager;

        private ILikePostsRepository _LikePostsRepository;

        private IPostRequestRepository _PostRequestRepository;
        private readonly IAntiforgery _antiforgery;


        public PostsController(IPostRepository Postrepostitory, ICommentRepository CommentRepository, ITagRepository TagRepository, ICategoriesRepository CategoriesRepository, IUserRepository UserRepository, IAntiforgery antiforgery, ILikePostsRepository LikePostsRepository, UserManager<User> userManager, IPostRequestRepository postRequestRepository)
        {
            _Postrepostitory = Postrepostitory;
            _CommentRepository = CommentRepository;
            _TagRepository = TagRepository;
            _CategoriesRepository = CategoriesRepository;
            _UserRepository = UserRepository;
            _antiforgery = antiforgery;
            _LikePostsRepository = LikePostsRepository;
            _userManager = userManager;
            _PostRequestRepository = postRequestRepository;

        }

        public async Task<IActionResult> Index(string? categoryUrl, string? tagUrl, int page = 1, int pageSize = 6)
        {
            var postsQuery = _Postrepostitory.Posts
                         .Include(p => p.User)
                         .Where(p => p.IsActive && p.IsBest == false);

            if (!string.IsNullOrEmpty(categoryUrl))
            {
                postsQuery = postsQuery.Where(p => p.categories.Any(c => c.Url == categoryUrl));
            }

            if (!string.IsNullOrEmpty(tagUrl))
            {
                postsQuery = postsQuery.Where(p => p.tags.Any(t => t.Url == tagUrl));
            }

            // Toplam kayıt sayısını hesapla
            var totalPosts = await postsQuery.CountAsync();

            // İlgili sayfa için verileri getir
            var posts = await postsQuery
                      .OrderByDescending(p => p.PublishedOn)  // Burayı burada yapmak önemli
                      .Skip((page - 1) * pageSize)
                      .Take(pageSize)
                      .ToListAsync();

            Console.WriteLine("Total Posts: " + totalPosts);  // Burada sayfada olan veri sayısını kontrol et

            ViewBag.CurrentCategoryUrl = categoryUrl;
            ViewBag.CurrentPage = page; // Mevcut sayfa
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalPosts / pageSize); // Toplam sayfa sayısı
            ViewData["PageType"] = "Index";

            return View(new PostsViewModel
            {
                Posts = posts,
                CurrentCategory = categoryUrl,
                CurrentTag = tagUrl
            });
        }


        public IActionResult Search(string searchTerm)
        {
            var posts = _Postrepostitory.Posts
                .Include(p => p.User)
                .Where(p => p.Title != null && p.Title.Contains(searchTerm))
                .ToList();

            var foundpost = new PostsViewModel
            {
                Posts = posts,
                CurrentCategory = null,
                CurrentTag = null,
                IsSerach = true
            };
            return View("Index", foundpost);


        }
        public async Task<IActionResult> Popular(string? categoryUrl, string? tagUrl, int page = 1, int pageSize = 6)
        {
            // Tüm gönderi sorgusunu başlat
            var postsQuery = _Postrepostitory.Posts
                             .Include(p => p.User)
                         .Where(p => p.IsActive && p.IsBest == true);

            // Kategori filtresi
            if (!string.IsNullOrEmpty(categoryUrl))
            {
                postsQuery = postsQuery.Where(p => p.categories.Any(c => c.Url == categoryUrl));
            }

            // Etiket filtresi
            if (!string.IsNullOrEmpty(tagUrl))
            {
                postsQuery = postsQuery.Where(p => p.tags.Any(t => t.Url == tagUrl));
            }

            // Toplam gönderi sayısını hesapla
            var totalPosts = await postsQuery.CountAsync();

            // İlgili sayfa için verileri getir
            var posts = await postsQuery
                                .Skip((page - 1) * pageSize) // Doğru atlama hesabı
                                .Take(pageSize)
                                .ToListAsync();

            // Sayfa bilgilerini ViewBag'e aktar
            ViewBag.CurrentPage2 = page; // Mevcut sayfa
            ViewBag.TotalPages2 = (int)Math.Ceiling((double)totalPosts / pageSize); // Toplam sayfa sayısı

            ViewBag.CurrentCategoryUrl = categoryUrl;
            ViewBag.CurrentTag = tagUrl;
            ViewData["PageType"] = "Popular";

            // View'e PostsViewModel'i gönder
            return View(new PostsViewModel
            {
                Posts = posts,
                CurrentCategory = categoryUrl,
                CurrentTag = tagUrl
            });
        }


        [Authorize]
        public async Task<IActionResult> Details(string? url)
        {
            if (User == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _UserRepository.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                ViewBag.CurrentUser = user.Copyright;
            }
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            ViewBag.AntiForgeryToken = tokens.RequestToken;
            var item = await _Postrepostitory
            .Posts
            .Include(x => x.User)
            .Include(x => x.tags)
            .Include(x => x.LikePost)
            .Include(x => x.categories)
            .Include(x => x.comments)
            .ThenInclude(x => x.user)
            .FirstOrDefaultAsync(x => x.Url == url);

            if (item == null)
            {
                return NotFound();
            }
            if (item != null)
            {
                var likePost = await _LikePostsRepository.LikePosts
                    .FirstOrDefaultAsync(lp => lp.PostId == item.PostId && lp.UserId == userId);

                item.LikePost = likePost; // Tekil nesneyi bağla
            }
            var isPdfVisible = item!.PdfActive ||
                     _PostRequestRepository.PostRequests.Any(r => r.PostId == item.PostId && r.RequesterId == userId && r.IsAccepted);

            item.IsRequestRequired = isPdfVisible;

            var roles = await _userManager.GetRolesAsync(user!);
            ViewBag.UserRole = roles.FirstOrDefault();
            var hasRequested = _PostRequestRepository.PostRequests
                                   .Any(r => r.PostId == item.PostId && r.RequesterId == userId);
            var commentUserRoles = new Dictionary<string, List<string>>();
            foreach (var comment in item.comments)
            {
                var commentUserRolesList = await _userManager.GetRolesAsync(comment.user);
                commentUserRoles[comment.user.Id] = commentUserRolesList.ToList();
            }
            var modell = new PostDetailsModel
            {

                Post = item,
                HasRequested = hasRequested,
                ActiveUser = userId,
                CommentUserRoles = commentUserRoles

            };
            return View(modell);
        }
        [HttpPost]
        public async Task<IActionResult> RequestAccess(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _Postrepostitory
           .Posts.FirstOrDefaultAsync(x => x.PostId == postId);
            if (item == null)
                return NotFound();
            var request = new PostRequest
            {
                PostId = postId,
                RequesterId = userId,
                OwnerId = _Postrepostitory.Posts.Where(p => p.PostId == postId).Select(p => p.UserId).FirstOrDefault(),
                IsAccepted = false,
                RequestedOn = DateTime.Now
            };

            _PostRequestRepository.CreatePostRequest(request);
            await _PostRequestRepository.SaveAsync();

            return RedirectToAction("Details", new { url = item.Url });
        }
        [Authorize]
        public IActionResult Requests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = _PostRequestRepository.PostRequests
      .Include(r => r.Requester)
      .Include(r => r.Post)
      .Where(r => r.OwnerId == userId && !r.IsAccepted)

      .ToList();

            return View(requests);
        }
        [HttpPost]
        public IActionResult AcceptRequest(int requestId)
        {
            var request = _PostRequestRepository.PostRequests
                .FirstOrDefault(r => r.RequestId == requestId);
            if (request != null)
            {
                request.IsAccepted = true;
                _PostRequestRepository.SaveAsync();
            }

            // JSON dönüşü sağlıyoruz, böylece AJAX başarılı olduğunda işlem tamamlanacak
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var request = await _PostRequestRepository.PostRequests
                .FirstOrDefaultAsync(r => r.RequestId == requestId);

            if (request != null)
            {
                _PostRequestRepository.PostRequests.Remove(request);
                await _PostRequestRepository.SaveAsync();
            }

            // JSON dönüşü sağlıyoruz
            return Json(new { success = true });
        }





        [HttpPost]
        public async Task<IActionResult> LikePost(int postId)
        {
            // Kullanıcıyı al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Postu al
            var post = await _Postrepostitory.Posts.FirstOrDefaultAsync(p => p.PostId == postId);

            if (post == null)
            {
                return NotFound();
            }

            // Kullanıcının zaten beğenip beğenmediğini kontrol et
            var likePost = await _LikePostsRepository.LikePosts
                .FirstOrDefaultAsync(lp => lp.PostId == postId && lp.UserId == userId);

            if (likePost != null && likePost.kontrol == true)
            {
                likePost.kontrol = false;
                _LikePostsRepository.Update(likePost);
                post.Vote--;
            }
            else if (likePost != null && likePost.kontrol == false)
            {
                likePost.kontrol = true;
                _LikePostsRepository.Update(likePost);
                post.Vote++;
            }
            else
            {
                // Eğer kullanıcı daha önce beğenmemişse, beğeni ekle
                var newLike = new LikePosts
                {
                    PostId = postId,
                    UserId = userId,
                    kontrol = true
                };

                _LikePostsRepository.CreateLikePosts(newLike);

                post.Vote++;
            }
            await _LikePostsRepository.SaveAsync();
            // Postu güncelle
            _Postrepostitory.Update(post);
            await _Postrepostitory.SaveChangesAsync();

            // Beğeni sayısını ve kontrol durumunu JSON olarak döndür
            return Json(new { voteCount = post.Vote, isLiked = likePost?.kontrol ?? true });
        }


        [HttpPost]
        public async Task<IActionResult> AcceptCopyright(string accept)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _UserRepository.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                if (accept == "true")
                {
                    // Kullanıcının onayını veritabanında güncelle
                    user.Copyright = true;
                    await _UserRepository.SaveChangesAsync();

                    // İçeriği gösterilecek sayfaya yönlendirin
                    return RedirectToAction("Index", "Posts");
                }
                else
                {
                    // Anasayfaya yönlendir
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // Anasayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<JsonResult> AddComment(int PostId, string Text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _UserRepository.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var fullName = "";
            if (user == null)
            {
                fullName = "değer bulunamadı";
            }
            else
                fullName = user.FullName;


            var entity = new Comment
            {
                PostId = PostId,
                Text = Text,
                PublishedOn = DateTime.Now,
                UserId = userId ?? "",
            };
            _CommentRepository.CreateComment(entity);
            return Json(new
            {
                username,
                Text,
                entity.PublishedOn,
                fullName
            });
        }

        [Authorize]
        public IActionResult Create()
        {
            var model = new NewPostsViewModel
            {
                Tags = _TagRepository.Tags.ToList(),
                Categories = _CategoriesRepository.Categories.ToList(),
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(NewPostsViewModel model, IFormFile content, IFormFile image)
        {
            // Kullanıcı ID'si kontrolü
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("Kullanıcı oturum açmamış.");
            }

            if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Description) ||
                !model.SelectedTags.Any() || !model.SelectedTags.Any())
            {
                ModelState.AddModelError("", "Lütfen tüm zorunlu alanları doldurun.");
                model.Tags = _TagRepository.Tags.ToList();
                model.Categories = _CategoriesRepository.Categories.ToList();
                return View(model);
            }

            if (content == null || content.Length == 0 || image == null || image.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen hem PDF hem de resim dosyasını yükleyin.");
                model.Tags = _TagRepository.Tags.ToList();
                model.Categories = _CategoriesRepository.Categories.ToList();
                return View(model);
            }

            var post = new Post
            {
                Title = model.Title,
                Description = model.Description,
                Url = GenerateUrl(model.Title),
                UserId = userId ?? "0",
                PublishedOn = DateTime.Now,
                IsActive = false,
                Price = model.Price,
                Vote = 0,

                IsBest = false,
                PdfActive = model.PdfisActive,
            };

            // PDF dosyasını kaydetme ve hash oluşturma
            if (content != null && content.Length > 0)
            {
                var uniquePdfFileName = GenerateUniqueFileName(content.FileName);
                var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", uniquePdfFileName);

                using (var stream = new FileStream(pdfPath, FileMode.Create))
                {
                    await content.CopyToAsync(stream);
                }

                // SHA256 hash oluşturma
                string fileHash;
                using (var sha256 = SHA256.Create())
                {
                    using (var fileStream = new FileStream(pdfPath, FileMode.Open))
                    {
                        var hash = sha256.ComputeHash(fileStream);
                        fileHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }

                post.Content = uniquePdfFileName; // Dosya adı
                post.SHA256Hash = fileHash; // Hash değeri
            }

            // Resim dosyasını kaydetme
            if (image != null && image.Length > 0)
            {
                var uniqueImgFileName = GenerateUniqueFileName(image.FileName);
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", uniqueImgFileName);

                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                post.Image = uniqueImgFileName;
            }

            post.tags = _TagRepository.Tags.Where(t => model.SelectedTags.Contains(t.TagId)).ToList();
            post.categories = _CategoriesRepository.Categories.Where(c => model.CategoryId == c.CategoryId).ToList();

            _Postrepostitory.CreatePost(post);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var role = User.FindFirstValue(ClaimTypes.Role);
            var posts = _Postrepostitory.Posts;
            if (!(role == "Admin"))
            {
                posts = posts
            .Include(p => p.comments)
            .Where(x => x.UserId == userId);
            }
            return View(await posts.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _Postrepostitory.Posts
                .Include(p => p.tags)
                .Include(p => p.categories)
                .Include(p => p.comments)
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            var model = new NewPostsViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                PdfisActive = post.PdfActive,
                CategoryId = post.categories.FirstOrDefault()?.CategoryId,
                SelectedTags = post.tags!.Select(t => t.TagId).ToList(),
                Categories = await _CategoriesRepository.GetCategoriesAsync(),
                Tags = await _TagRepository.GetTagsAsync(),
                Price = post.Price,
                IsActive = post.IsActive
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(NewPostsViewModel model, IFormFile content, IFormFile image)
        {
            // Post'u veritabanından çek
            var post = await _Postrepostitory.Posts
                .Include(p => p.tags)
                .Include(p => p.categories) // Kategorileri de dahil ediyoruz
                .FirstOrDefaultAsync(p => p.PostId == model.PostId);

            if (post == null)
            {
                return NotFound();
            }

            // Post bilgilerini güncelle
            post.Title = model.Title;
            post.Description = model.Description;
            post.PdfActive = model.PdfisActive;
            post.Price = model.Price;
            post.IsActive = true;

            // Kategori güncellemesi
            var selectedCategory = await _CategoriesRepository.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == model.CategoryId);

            // Post'a ait mevcut kategoriyi temizle
            post.categories.Clear();

            // Yeni kategoriyi ekle
            if (selectedCategory != null)
            {
                post.categories.Add(selectedCategory);
            }

            // Tag güncellemesi
            if (post.tags != null)
            {
                post.tags.Clear(); // Mevcut tag'leri temizle
            }

            if (model.SelectedTags != null)
            {
                foreach (var tagId in model.SelectedTags)
                {
                    var tag = await _TagRepository.GetTagAsync(tagId);
                    if (tag != null)
                    {
                        if (post.tags == null)
                        {
                            post.tags = new List<Tag>();
                        }
                        post.tags.Add(tag);
                    }
                }
            }

            // PDF dosyasını yükleme
            if (content != null && content.Length > 0)
            {
                var uniquePdfFileName = GenerateUniqueFileName(content.FileName);
                var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", uniquePdfFileName);

                using (var stream = new FileStream(pdfPath, FileMode.Create))
                {
                    await content.CopyToAsync(stream);
                }

                // SHA256 hash oluşturma
                string fileHash;
                using (var sha256 = SHA256.Create())
                {
                    using (var fileStream = new FileStream(pdfPath, FileMode.Open))
                    {
                        var hash = sha256.ComputeHash(fileStream);
                        fileHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }

                post.Content = uniquePdfFileName; // Dosya adı
                post.SHA256Hash = fileHash; // Hash değeri
            }


            // Resim dosyasını yükleme
            if (image != null && image.Length > 0)
            {
                string imageFileName = GenerateUniqueFileName(image.FileName);
                string imagePath = Path.Combine("wwwroot/img", imageFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Yüklenen resim dosyasının adını veritabanına kaydet
                post.Image = imageFileName;
            }

            // Değişiklikleri veritabanına kaydet
            await _Postrepostitory.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult ZamanDamgasi()
        {
            var matching = new VerifyPdfViewModel
            {

                FullName = "",
                Title = "",
                PublishedOn = DateTime.Now,

            }; ;
            return View(matching);
        }
        [HttpPost]
        public async Task<IActionResult> ZamanDamgasi(IFormFile uploadedPdf)
        {
            if (uploadedPdf == null || uploadedPdf.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen bir PDF dosyası yükleyin.");
                return View(new VerifyPdfViewModel { IsVerified = false });
            }

            // PDF dosyasını geçici olarak kaydetme
            var tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await uploadedPdf.CopyToAsync(stream);
            }

            // SHA256 hash hesaplama
            string uploadedHash;
            using (var sha256 = SHA256.Create())
            {
                using (var stream = new FileStream(tempPath, FileMode.Open))
                {
                    var hash = sha256.ComputeHash(stream);
                    uploadedHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }

            // Veritabanında hash kontrolü
            var matchingPost = _Postrepostitory.Posts.Include(p => p.User).FirstOrDefault(p => p.SHA256Hash == uploadedHash);
            var isVerified = matchingPost != null;

            // Geçici dosyayı sil
            System.IO.File.Delete(tempPath);

            // Sonuç gösterme
            if (isVerified && matchingPost != null)

            {

                var matching = new VerifyPdfViewModel
                {
                    IsVerified = isVerified,
                    FullName = matchingPost.User!.FullName,
                    PublishedOn = matchingPost.PublishedOn,
                    Title = matchingPost.Title,

                };
                return View(matching);

            }
            else
            {
                var matching = new VerifyPdfViewModel
                {
                    IsVerified = isVerified,
                    FullName = "",
                    Title = "",
                    PublishedOn = DateTime.Now,

                }; ;
                return View(matching);

            }

        }
        public IActionResult Payment(decimal price, string title)
        {
            ViewBag.Price = price;
            ViewBag.Title = title;

            return View();
        }


        private string GenerateUniqueFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            return uniqueFileName;
        }
        private string GenerateUrl(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return string.Empty;
            }

            // Türkçe karakterlerin İngilizce karşılıkları
            var turkishCharacters = new Dictionary<char, string>
    {
        { 'ş', "s" },
        { 'Ş', "S" },
        { 'ğ', "g" },
        { 'Ğ', "G" },
        { 'ı', "i" },
        { 'İ', "I" },
        { 'ö', "o" },
        { 'Ö', "O" },
        { 'ç', "c" },
        { 'Ç', "C" },
        { 'ü', "u" },
        { 'Ü', "U" },
        { ' ', "-" },
    };

            // Başlık metnindeki karakterleri değiştir
            foreach (var character in turkishCharacters)
            {
                title = title.Replace(character.Key.ToString(), character.Value);
            }

            // URL'nin daha iyi görünmesi için küçük harflere çevir
            title = title.ToLower();

            // URL'deki özel karakterleri ve tire işaretlerini temizleme (isteğe bağlı)
            title = Regex.Replace(title, @"[^a-z0-9-]", "");

            // Tekrar eden tireleri kaldırma
            title = Regex.Replace(title, @"-+", "-");

            // Başındaki ve sonundaki tireleri kaldırma
            title = title.Trim('-');

            return title;
        }
    }
}
