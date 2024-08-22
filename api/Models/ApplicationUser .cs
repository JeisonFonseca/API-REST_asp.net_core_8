using Microsoft.AspNetCore.Identity;
using api.Models;
public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SecondLastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Bio { get; set; }
    public string ProfileImage { get; set; }
    //public UserRole Role { get; set; }  // Enum personalizado
}