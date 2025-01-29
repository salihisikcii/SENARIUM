using BlogApps.Entity;

namespace BlogApps.Models

{
   public class VerifyPdfViewModel
   {
      public bool? IsVerified { get; set; }

      public string? FullName { get; set; }
      public DateTime? PublishedOn { get; set; }

      public string? Title { get; set; }

   }
}
