using api.Dtos.Comment;
using api.Extentions;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo ;
        private readonly IStockRepository _stockRepo ;
        private readonly UserManager<AppUser> _userManager ;
        public CommentController
        (ICommentRepository commentRepo ,
         IStockRepository stockRepo ,
         UserManager<AppUser> userManager
        )
        {
            _commentRepo = commentRepo ;
            _stockRepo = stockRepo ;
            _userManager = userManager ;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync() ;

            var commentsDto = comments.Select(c => c.ToCommentDto()) ;

            return Ok(commentsDto) ;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null){
                return NotFound() ;
            }
            return Ok(comment.ToCommentDto()); 
        }
        
        [HttpPost]
        [Route("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId , [FromBody]CreateCommetRequestDto commentDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!await _stockRepo.StockExists(stockId)){
                return BadRequest("stock with id {stockId} does not exists");
            }
   
            var appUser = await _userManager.FindByNameAsync(User.GetUsername());
            var commentModel = commentDto.ToCommentFromCreateDto(stockId) ;
            commentModel.AppUserId = appUser.Id ;
            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById) 
            , new {id = commentModel.Id} , commentModel.ToCommentDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id , [FromBody]UpdateCommentRequestDto update)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await _commentRepo.UpdateAsync(id , update.ToCommentFromUpdateDto());
            if(commentModel==null){
                return NotFound();
            }
            return Ok(commentModel.ToCommentDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult>Delete([FromRoute]int id)
        {
            var commentModel = await _commentRepo.DeleteAsync(id);
            if(commentModel==null){
                return NotFound();
            }
            return Ok(commentModel.ToCommentDto());
        }


    }
}