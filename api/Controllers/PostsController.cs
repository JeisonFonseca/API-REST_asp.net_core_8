using api.Models;
using api.Models.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly RedSocialContext _context;
        private readonly IConfiguration configuration;

        public PostsController(RedSocialContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: api/<PostsController>
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _context.Posts.ToList();
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == id);
            if (post != null)
                return Ok(post);
            else
                return NotFound();
        }

        // POST api/<PostsController>
        [HttpPost]
        public IActionResult Post(PostDTO postDTO)
        {
            // Create the new post object
            var post = new Post();
            post.UserId = postDTO.UserId;
            if (postDTO.Content != null)
                post.Content = postDTO.Content;
            if (postDTO.MediaURL != null)
                post.MediaUrl = postDTO.MediaURL;
            // Insert it into the DB
            _context.Posts.Add(post);
            _context.SaveChanges();
            return Created();
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PostDTO postDTO)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == id);
            if (post != null)
            {
                if (postDTO.Content != null)
                    post.Content = postDTO.Content;
                if (postDTO.MediaURL != null)
                    post.MediaUrl = postDTO.MediaURL;
                _context.SaveChanges();
                return Ok(post);
            }
            else
                return NotFound();
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
                return Ok();
            }
            else
                return NotFound();
        }
    }
}
