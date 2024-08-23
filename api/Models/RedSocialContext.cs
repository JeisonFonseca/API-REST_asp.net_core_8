using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Npgsql;

namespace api.Models
{
    public partial class RedSocialContext : DbContext
    {
        static RedSocialContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Role>("userrole", new RoleNameTranslator()); // Basado en https://github.com/npgsql/efcore.pg/issues/2557#issuecomment-1318998191
        }
        public RedSocialContext()
        {
        }

        public RedSocialContext(DbContextOptions<RedSocialContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Friendship> Friendships { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Reaction> Reactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=postgresql;Port=5432;Database=RedSocial;Username=postgres;Password=temp;Pooling=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("friendshipstatus", new[] { "Pendiente", "Aceptado", "Bloqueado" })
                .HasPostgresEnum("reactiontype", new[] { "Me gusta", "Me encanta", "Me importa", "Jaja", "Guau", "Enfadado", "Triste" })
                .HasPostgresEnum("userrole", new[] { "Administrador", "Editor", "Lector" });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId).HasColumnName("commentId");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.MediaUrl)
                    .HasMaxLength(255)
                    .HasColumnName("mediaUrl");

                entity.Property(e => e.PostId).HasColumnName("postId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_Comments.postId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Comments.userId");
            });

            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.FriendId }, "UC_Friendships_user_friend")
                    .IsUnique();

                entity.Property(e => e.FriendshipId).HasColumnName("friendshipId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdAt");

                entity.Property(e => e.FriendId).HasColumnName("friendId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Friend)
                    .WithMany(p => p.FriendshipFriends)
                    .HasForeignKey(d => d.FriendId)
                    .HasConstraintName("FK_Friendships.friendId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FriendshipUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Friendships.userId");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.MessageId).HasColumnName("messageId");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.MediaUrl)
                    .HasMaxLength(255)
                    .HasColumnName("mediaUrl");

                entity.Property(e => e.ReadAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("readAt");

                entity.Property(e => e.ReceiverId).HasColumnName("receiverId");

                entity.Property(e => e.SenderId).HasColumnName("senderId");

                entity.Property(e => e.SentAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("sentAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .HasConstraintName("FK_Messages.receiverId");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSenders)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK_Messages.senderId");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.PostId).HasColumnName("postId");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.MediaUrl)
                    .HasMaxLength(255)
                    .HasColumnName("mediaUrl");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Posts.userId");
            });

            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.HasIndex(e => new { e.PostId, e.UserId }, "UC_Reactions_post_user")
                    .IsUnique();

                entity.Property(e => e.ReactionId).HasColumnName("reactionId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.PostId).HasColumnName("postId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_Reactions.postId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Reactions.userId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "Users_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "Users_username_key")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userId");


                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Bio).HasColumnName("bio");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .HasColumnName("passwordHash");

                entity.Property(e => e.ProfileImage)
                    .HasMaxLength(255)
                    .HasColumnName("profileImage");

                entity.Property(e => e.SecondLastName)
                    .HasMaxLength(50)
                    .HasColumnName("secondLastName");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updatedAt");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

                OnModelCreatingPartial(modelBuilder);
            }
        

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
