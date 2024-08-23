using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            FriendshipFriends = new HashSet<Friendship>();
            FriendshipUsers = new HashSet<Friendship>();
            MessageReceivers = new HashSet<Message>();
            MessageSenders = new HashSet<Message>();
            Posts = new HashSet<Post>();
            Reactions = new HashSet<Reaction>();
        }

        public int UserId { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondLastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ProfileImage { get; set; }
        public Role Role { get; set; }


        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Friendship> FriendshipFriends { get; set; }
        public virtual ICollection<Friendship> FriendshipUsers { get; set; }
        public virtual ICollection<Message> MessageReceivers { get; set; }
        public virtual ICollection<Message> MessageSenders { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Reaction> Reactions { get; set; }
    }

    // Enum para representar los roles
    public enum UserRole
    {
        Administrador,
        Editor,
        Lector
    }
}
