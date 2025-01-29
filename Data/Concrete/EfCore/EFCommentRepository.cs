using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApps.Data.Abstract;
using BlogApps.Entity;

namespace BlogApps.Data.Concrete.EfCore
{
    public class EFCommentRepository : ICommentRepository
    {
         private BlogContext _context;

        public EFCommentRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Comment> Comments =>  _context.Comments;

        public void CreateComment(Comment comment)
        {
             _context.Comments.Add(comment);
            _context.SaveChanges();        }
    }
}

   