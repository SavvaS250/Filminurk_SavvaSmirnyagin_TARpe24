using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.Dto
{
    public class UserCommentDTO
    {
        [Key]
        public Guid? CommentID { get; set; }
        public string? CommenterUserID { get; set; }
        public string CommentBody { get; set; }
        public int CommetredScore { get; set; }
        public int IsHelpful { get; set; }//👍
        public int IsHarmful { get; set; }//👎

        /*Andmebaasi jaoks vajalikud andmed*/
        public DateTime CommentCreatedAt { get; set; }
        public DateTime CommentModified { get; set; }
        public DateTime? CommentDeleteAt { get; set; }
    }
}
