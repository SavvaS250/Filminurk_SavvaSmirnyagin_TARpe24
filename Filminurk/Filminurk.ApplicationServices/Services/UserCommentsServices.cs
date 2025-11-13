using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;

namespace Filminurk.ApplicationServices.Services
{
    public class UserCommentsServices : IUserCommentsServices 
    {
        private readonly FilminurkTARpe24Context _context;

        public UserCommentsServices(FilminurkTARpe24Context context)
        {
            _context = context;
        }

        public async Task<UserComment> NewComment(UserCommentDTO newcommentDTO)
        {
            UserComment domain = new UserComment();

            domain.CommentID = Guid.NewGuid();
            domain.CommentBody = newcommentDTO.CommentBody;
            domain.CommenterUserID = newcommentDTO.CommenterUserID;
            domain.CommentedScore = newcommentDTO.CommentedScore;
            domain.CommentCreatedAt = DateTime.Now;
            domain.CommentModified = DateTime.Now;
            domain.IsHelpful = 0;
            domain.IsHarmful = 0;

            await _context.UserComments.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }
    }
}
