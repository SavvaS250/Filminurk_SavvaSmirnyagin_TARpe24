namespace Filminurk.Models.UserComments
{
    public class UserCommentsCreateViewModel
    {
        public Guid CommentID { get; set; }
        public string CommenterUserID { get; set; } = "00000000-0000-0000-000000000001";
        public string CommentBody { get; set; }
        public int CommentedScore { get; set; }
        public int? IsHelpful { get; set; }//👍 ei saa laike
        public int? IsHarmful { get; set; }//👎 ega dislaike lisada

        /*Andmebaasi jaoks vajalikud andmed*/
        public DateTime? CommentCreatedAt { get; set; }
        public DateTime? CommentModified { get; set; }
        public DateTime? CommentDeleteAt { get; set; }
    }
}
