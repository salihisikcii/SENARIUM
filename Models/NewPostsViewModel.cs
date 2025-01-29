using System.ComponentModel.DataAnnotations;
using BlogApps.Entity;

namespace BlogApps.Models

{
    public class NewPostsViewModel
    {
        [Required(ErrorMessage = "Başlık alanı boş olamaz.")]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Pdf alanı boş olamaz.")]
        [Display(Name = "Senaryo")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Özet alanı boş olamaz.")]
        [Display(Name = "Özet")]
        public string? Description { get; set; }




        public bool IsActive { get; set; }

        public string? Url { get; set; }

        [Required(ErrorMessage = "Tür alanı boş olamaz.")]
        [Display(Name = "Etiketler")]
        public Tag tags { get; set; } = new Tag();

        // [Required(ErrorMessage = "Format alanı boş olamaz.")]
        [Display(Name = "Kategoriler")]
        public Categories cateories { get; set; } = new Categories();



        // [Required(ErrorMessage = "Senaryo Resmi alanı boş olamaz.")]
        [Display(Name = "Senaryo Resmi")]
        public string? image { get; set; }

        [Display(Name = "PostId")]
        public int PostId { get; set; }


        [Required]
        [Display(Name = "Pdf Yayınlansın mı")]
        public bool PdfisActive { get; set; }

        [Required(ErrorMessage = "Fiyat alanı boş olamaz.")]
        [Range(1, 10000, ErrorMessage = "Fiyat 1 ile 10.000 TL arasında olmalıdır.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Tür  alanı boş olamaz.")]
        public List<Tag> Tags { get; set; } = new();
        
        public List<int> SelectedTags { get; set; } = new List<int>();
        public int? CategoryId { get; set; } 
        [Required(ErrorMessage = "Format  alanı boş olamaz.")]

        public List<Categories> Categories { get; set; } = new();


    }
}
