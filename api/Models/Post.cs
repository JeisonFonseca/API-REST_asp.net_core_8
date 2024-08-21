using System;
using System.Collections.Generic;

namespace api.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Reactions = new HashSet<Reaction>();
        }

        public int PostId { get; set; }
        public int UserId { get; set; }
        public string? Content { get; set; }
        public string? MediaUrl { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Reaction> Reactions { get; set; }
    }
}
