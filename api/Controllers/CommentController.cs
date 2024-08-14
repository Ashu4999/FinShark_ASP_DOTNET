using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController: ControllerBase
    {
        private readonly ICommnetRepository _commnetRepo;
        public CommentController(ICommnetRepository commnetRepo) {
            _commnetRepo = commnetRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments() {
            var comments = await _commnetRepo.GetAllAsync();
            var commnetDto = comments.Select(s => s.ToCommentDto());
            return Ok(commnetDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id) {
            var comment = await _commnetRepo.GetByIdAsync(id);

            if (comment == null) {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }
    }
}