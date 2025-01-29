using System.ComponentModel.DataAnnotations;

namespace BlogApps.Entity
{
    public class PostRequest
    {
        [Key]
        public int RequestId { get; set; }

        public int PostId { get; set; } // Talep edilen senaryo
        public Post? Post { get; set; }

        public string? RequesterId { get; set; } // Talepte bulunan kişi
        public User? Requester { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }

        public string? OwnerId { get; set; } // Senaryo sahibi
        public User? Owner { get; set; }

        public bool IsAccepted { get; set; } // Kabul edilmiş mi?
        public DateTime RequestedOn { get; set; }
    }
}
