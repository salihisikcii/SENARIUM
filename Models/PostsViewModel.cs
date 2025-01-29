using BlogApps.Entity;

namespace BlogApps.Models

{
   public class PostsViewModel
   {
      public List<Post> Posts { get; set; } = new();

      public string? CurrentCategory { get; set; }
      public string? CurrentTag { get; set; }
      public List<Categories> Categories { get; set; } = new();
      public List<Tag> Tags { get; set; } = new();

      public bool IsSerach { get; set; } = false;
   }
}
