using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.UserComments;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class UserCommentsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IUserCommentsServices _userCommentsServices;
        public UserCommentsController
            (
            FilminurkTARpe24Context context,
            IUserCommentsServices userCommentsServices
            )
        {
            _context = context;
            _userCommentsServices = userCommentsServices;
        }
        public IActionResult Index()
        {
            var result = _context.UserComments
                .Select(c => new UserCommentsIndexViewModel
                {
                    CommentID = c.CommentID,
                    CommentBody = c.CommentBody,
                    IsHarmful = (int)c.IsHarmful,
                    CommentCreatedAt = c.CommentCreatedAt,
                }
                );
            return View(result);
        }

        [HttpGet]
        public IActionResult NewComment()
        {
            //TO DO:  erista kas tegemist on admini või tavakasutajaga
            UserCommentsCreateViewModel newcomment = new();
            return View(newcomment);
        }

        [HttpPost, ActionName("NewComment")]
        // meetodile ei tohi panna allowanonymous
        public async Task<IActionResult> NewCommentPost(UserCommentsCreateViewModel newcommentVM)
        {
            //check dto
            //newcommentVM.CommenterUserID = "00000000-0000-0000-000000000001";
            //TO do: newcommenti manuaalne seadmine, asenda pärast kasutaja id-ga
            Console.WriteLine(newcommentVM.CommenterUserID);
            if (ModelState.IsValid)
            {

                var dto = new UserCommentDTO() { };

                dto.CommentID = newcommentVM.CommentID;
                   dto.CommentBody = newcommentVM.CommentBody;
                dto.CommenterUserID = newcommentVM.CommenterUserID;
                dto.CommentedScore = newcommentVM.CommentedScore;
                dto.CommentCreatedAt = newcommentVM.CommentCreatedAt;
                dto.CommentModified = newcommentVM.CommentModified;
                dto.IsHelpful = newcommentVM.IsHelpful;
                dto.IsHarmful = newcommentVM.IsHarmful;

                var result = await _userCommentsServices.NewComment(dto);
                if (result == null)
                {
                    return NotFound();
                }
                //TO DO: erista ära kas tegu on admini või kasutajaga, admin tagastub admin index aga kasutaja vastava filmi juurde
                return RedirectToAction(nameof(Index));
                //return RedirectToAction("Details", "Movies", id)
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAdmin(Guid id)
        {
            var requestedComment = await _userCommentsServices.DetailAsync(id);
            if (requestedComment == null)
            {
                return NotFound();
            }

            var commentVM = new UserCommentsIndexViewModel { };
            commentVM.CommentID = requestedComment.CommentID;
            commentVM.CommentBody = requestedComment.CommentBody;
            commentVM.CommenterUserID = requestedComment.CommenterUserID;
            commentVM.CommentedScore = requestedComment.CommentedScore;
            commentVM.CommentCreatedAt = requestedComment.CommentCreatedAt;
            commentVM.CommentModified = requestedComment.CommentModified;
            commentVM.CommentDeleteAt = requestedComment.CommentDeleteAt;
            
            return View(commentVM);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            var deleteEntry = await _userCommentsServices.DetailAsync(id);

            if (deleteEntry == null)
            {
                return NotFound();
            }

            var commentVM = new UserCommentsIndexViewModel();
            commentVM.CommentID = deleteEntry.CommentID;
            commentVM.CommentBody = deleteEntry.CommentBody;
            commentVM.CommenterUserID= deleteEntry.CommenterUserID;
            commentVM.CommentedScore= deleteEntry.CommentedScore;
            commentVM.CommentCreatedAt= deleteEntry.CommentCreatedAt;
            commentVM.CommentModified= deleteEntry.CommentModified;
            commentVM.CommentDeleteAt= deleteEntry.CommentDeleteAt;
            return View("DeleteAdmin", commentVM);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAdminPost(Guid id) 
        {
            var deleteThisComment = await _userCommentsServices.Delete(id);
            if (deleteThisComment == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
          
            
        }

    }
}
