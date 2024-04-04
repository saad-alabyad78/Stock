using api.Dtos.Comment;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context ;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context; 
        }
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(a=>a.AppUser).ToListAsync() ;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(a=>a.AppUser).FirstOrDefaultAsync(i=>i.Id==id) ;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            return commentModel ;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var currComment = await _context.Comments.FirstOrDefaultAsync(c=>c.Id==id) ;
            if(currComment==null){
                return null ;
            }
            currComment.Title = commentModel.Title ;
            currComment.Content = commentModel.Content ;

            await _context.SaveChangesAsync() ;
            return currComment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c=>c.Id==id);
            if(commentModel==null){
                return null;
            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync() ;
            return commentModel ;
        }
    }
}