using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApps.Entity;

namespace BlogApps.Models
{
    public class PostDetailsModel
    {

        public required Post Post { get; set; }
        public bool HasRequested { get; set; }


        public Dictionary<string, List<string>>? CommentUserRoles { get; set; }


        public string? ActiveUser { get; set; }

    }
}