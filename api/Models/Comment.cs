using System;
using System.Collections.Generic;

namespace api.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string? Content { get; set; }
        public string? MediaUrl { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
