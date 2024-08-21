using System;
using System.Collections.Generic;

namespace api.Models
{
    public partial class Friendship
    {
        public int FriendshipId { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User Friend { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
