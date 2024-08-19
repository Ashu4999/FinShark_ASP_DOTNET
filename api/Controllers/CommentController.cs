using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extentions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commnetRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commnetRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commnetRepo = commnetRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments([FromQuery] CommentQueryObject commentQueryObject)
        {
            var comments = await _commnetRepo.GetAllAsync(commentQueryObject);
            var commnetDto = comments.Select(s => s.ToCommentDto());
            return Ok(commnetDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commnetRepo.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequestDto createCommentRequestDto)
        {   
            // use to validate payload using dto
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var isStockExists = await _stockRepo.IsStockExists(stockId);

            if (!isStockExists)
            {
                return BadRequest("Stock does not exists");
            }

            var userEmail = User.GetUserEmail();
            var foundUser = await _userManager.FindByEmailAsync(userEmail);

            var commentModel = createCommentRequestDto.ToCommentFromCreate(stockId);
            commentModel.AppUserId = foundUser.Id;
            await _commnetRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
        {   
            // use to validate payload using dto
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var foundComment = await _commnetRepo.UpdateAsync(id, updateCommentRequestDto);

            if (foundComment == null)
                return NotFound();

            return Ok(foundComment.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var deletedComment = await _commnetRepo.DeleteAsync(id);

            if (deletedComment == null)
                return NotFound();

            return NoContent();
        }
    }
}