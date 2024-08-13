using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CommentController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetComments()
        {
            var comments = _context.Comments.ToList();
            return Ok("Get all comments");
        }

        [HttpGet("{id}")]
        public IActionResult GetCommentById([FromRoute] int id)
        {
            var foundComment = _context.Comments.FirstOrDefault(s => s.Id == id);

            if(foundComment == null) {
                return NotFound();
            }

            return Ok(foundComment);
        }

        [HttpPost]
        public IActionResult CreateComment()
        {
            return Ok("In create comment");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateComment([FromRoute] int id)
        {
            return Ok($"Updated comment {id}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComment([FromRoute] int id)
        {
            return Ok($"Deleted comment {id}");
        }
    }
}