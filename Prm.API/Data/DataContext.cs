using Prm.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Prm.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, 
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext>  options) : base (options) {}

        
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole => 
            {
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.RolesUser)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.RolesUser)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            
            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Article>(article => 
            {
                article.HasOne(u => u.Author)
                    .WithMany(m => m.Articles)
                    .OnDelete(DeleteBehavior.Restrict);

            });


            
            builder.Entity<ArticleStudent>().HasKey(k=> new {k.ArticleId, k.StudentId});


            builder.Entity<ArticleStudent>()
                .HasOne(u => u.Article)
                .WithMany(m => m.Students)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ArticleStudent>()
                .HasOne(u => u.Student)
                .WithMany(m => m.ArticleStudents)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Photo>().HasQueryFilter(p => p.Approved);
        }
    }
}