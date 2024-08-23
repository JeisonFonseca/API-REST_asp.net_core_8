using Microsoft.OpenApi.Validations.Rules;

namespace api.Models.DTO
{
    [Serializable]
    public class PostDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string Content { get; set; } = string.Empty;
        public string MediaUrl { get; set; } = string.Empty;

        public Post ToEntity()
        {
            return new Post
            {
                PostId = PostId,
                Content = Content,
                MediaUrl = MediaUrl,
                UserId = UserId,
            };
        }

        public static PostDTO FromEntity(Post entity)
        {
            return new PostDTO
            {
                PostId = entity.PostId,
                MediaUrl = entity.MediaUrl,
                Content = entity.Content,
                UserId = entity.UserId,
            };
        }
    }
}
