﻿using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones relacionadas con los usuarios.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly RedSocialContext _context;
        private readonly IConfiguration configuration;

        public UsersController(RedSocialContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }


        /// <summary>
        /// Obtiene un usuario específico por ID.
        /// </summary>
        /// <param name="id">El ID del usuario.</param>
        /// <returns>Un objeto User si se encuentra; de lo contrario, un estado 404 Not Found.</returns>
        /// <response code="200">Devuelve el usuario solicitado.</response>
        /// <response code="404">Si el usuario no se encuentra.</response>
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _context.Users.ToList();
        }

        // GET api/<UsersController>/5
        [Authorize]
        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="user">Los datos del usuario a registrar.</param>
        /// <returns>Un estado 200 OK si el registro es exitoso.</returns>
        /// <response code="200">Registro exitoso.</response>
        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<bool>> Post(UserDTO user)
        {
            User userEntity = user.ToEntity();

            userEntity.CreatedAt = DateTime.Now;
            userEntity.UpdatedAt = userEntity.CreatedAt;

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        /// <summary>
        /// Autentica a un usuario y genera un token JWT.
        /// </summary>
        /// <param name="loginDTO">Las credenciales del usuario.</param>
        /// <returns>Un token JWT si las credenciales son correctas; de lo contrario, un estado 204 No Content.</returns>
        /// <response code="200">Autenticación exitosa.</response>
        /// <response code="204">Si las credenciales son incorrectas.</response>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == loginDTO.Email && x.PasswordHash == loginDTO.Password);
            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("Email", user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var SignIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(5),
                    signingCredentials: SignIn
                    );
                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { token = tokenValue , User = user});
            }

            return NoContent();
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        /// <param name="id">El ID del usuario a actualizar.</param>
        /// <param name="updatedUserDto">Los nuevos datos del usuario.</param>
        /// <returns>Un estado 200 OK si la actualización es exitosa.</returns>
        /// <response code="200">Actualización exitosa.</response>
        /// <response code="404">Si el usuario no se encuentra.</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UserDTO updatedUserDto)
        {
            User? existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (existingUser is null)
            {
                return NotFound();
            }

            User updatedUser = updatedUserDto.ToEntity();

            existingUser.Bio = updatedUser.Bio;
            existingUser.DateOfBirth = updatedUser.DateOfBirth;

            existingUser.ProfileImage = updatedUser.ProfileImage;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.SecondLastName = updatedUser.SecondLastName;

            existingUser.Email = updatedUser.Email;
            existingUser.Username = updatedUser.Username;

            existingUser.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok();
        }



        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario a eliminar.</param>
        /// <returns>Un estado 200 OK si la eliminación es exitosa.</returns>
        /// <response code="200">Eliminación exitosa.</response>
        /// <response code="404">Si el usuario no se encuentra.</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user is null)
            {
                return NotFound();

            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [Authorize(Policy = "AdministradorPolicy")]  // Solo los administradores pueden asignar roles
        [HttpPost]
        [Route("AssignRole")]
        public IActionResult AssignRole([FromForm] int userId, [FromForm] string role)
        {
            // Agrega logs para depuración
            Console.WriteLine($"Buscando usuario con ID: {userId}");

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Convertir la cadena a enum UserRole
            if (Enum.TryParse<UserRole>(role, out var userRole))
            {
            //    user.Role = Role;
                _context.SaveChanges();
                return Ok(new { message = $"Rol {userRole} asignado correctamente al usuario {user.Username}" });
            }
            else
            {
                return BadRequest("Rol inválido.");
            }


        }


    }
}
