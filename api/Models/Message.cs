using System;
using System.Collections.Generic;

namespace api.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string? Content { get; set; }
        public string? MediaUrl { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? ReadAt { get; set; }

        public virtual User Receiver { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }
}
