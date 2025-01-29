using System.ComponentModel.DataAnnotations;

namespace BlogApps.UserModels
{
    public class EditViewModel
    {
          public string? image { get; set; }

        public string? AboutMe { get; set; }
        public string? Linkedin { get; set; }
        public string? UserId { get; set; }
         [Required(ErrorMessage = "-Kullanıcı adı zorunludur.")]
        [RegularExpression(@"^[^\s]*$", ErrorMessage = "-Kullanıcı adı boşluk içeremez.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "-E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "-Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        public string FullName { get; set; } = string.Empty;

        [MinLength(8, ErrorMessage = "-Şifre en az 8 karakter olmalıdır.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "-Şifre en az 1 büyük harf ve 1 rakam içermelidir.")]
        public string Password { get; set; }  = string.Empty;

        [Compare("Password", ErrorMessage = "-Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }  = string.Empty;
        public IList<string>? SelectedRoles { get; set; }

    } 
}