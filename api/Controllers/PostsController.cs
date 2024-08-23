using api.Models;
using api.Models.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con las publicaciones (posts).
    /// </summary>
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

        /// <summary>
        /// Obtiene la lista de todas las publicaciones.
        /// </summary>
        /// <returns>Una colección de objetos Post.</returns>
        /// <response code="200">Devuelve la lista de publicaciones.</response>
        // GET: api/<PostsController>
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _context.Posts.ToList();
        }


        /// <summary>
        /// Obtiene una publicación específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la publicación.</param>
        /// <returns>Un objeto Post si se encuentra; de lo contrario, un estado 404 Not Found.</returns>
        /// <response code="200">Devuelve la publicación solicitada.</response>
        /// <response code="404">Si la publicación no se encuentra.</response>
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

        /// <summary>
        /// Crea una nueva publicación.
        /// </summary>
        /// <param name="postDTO">Los datos de la nueva publicación.</param>
        /// <returns>Un estado 201 Created si la creación es exitosa.</returns>
        /// <response code="201">Creación exitosa.</response>
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

        /// <summary>
        /// Actualiza una publicación existente.
        /// </summary>
        /// <param name="id">El ID de la publicación a actualizar.</param>
        /// <param name="postDTO">Los datos actualizados de la publicación.</param>
        /// <returns>Un estado 200 OK si la actualización es exitosa.</returns>
        /// <response code="200">Actualización exitosa.</response>
        /// <response code="404">Si la publicación no se encuentra.</response>
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

        /// <summary>
        /// Elimina una publicación por su ID.
        /// </summary>
        /// <param name="id">El ID de la publicación a eliminar.</param>
        /// <returns>Un estado 200 OK si la eliminación es exitosa.</returns>
        /// <response code="200">Eliminación exitosa.</response>
        /// <response code="404">Si la publicación no se encuentra.</response>
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
