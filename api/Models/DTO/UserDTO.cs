namespace api.Models.DTO
{
    [Serializable]
    public class UserDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        public string Bio { get; set; } = string.Empty;
        public ICollection<PostDTO> Posts { get; set; } = new List<PostDTO>();

        public User ToEntity()
        {
            return new User
            {
                Email = Email,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                SecondLastName = SecondLastName,
                ProfileImage = ProfileImage,
                DateOfBirth = new DateOnly(
                    DateOfBirth.Year,
                    DateOfBirth.Month,
                    DateOfBirth.Day),
                Bio = Bio,
                Posts = Posts.Select(p => p.ToEntity()).ToList()
            };
        }
    }
}
