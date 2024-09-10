using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeminarEF.Db.Models
{
    public class ChatDbContext: DbContext
    {
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }

        public ChatDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.LogTo(Console.WriteLine)
                .UseLazyLoadingProxies().UseNpgsql("Host = localhost; Username = example; Password = example; Database = chatdb;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Messages>(e =>
            {
            e.ToTable("messages");
            e.HasKey(x => x.Id).HasName("messages_pkey");
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Text).HasColumnName("text");
                e.Property(x=>x.ToUserId).HasColumnName("to_user_id");
                e.Property(x=>x.FromUserId).HasColumnName("from_user_id");

                // Связь один ко многим

                e.HasOne(x=>x.FromUser)
                .WithMany(x=>x.FromMessages)
                .HasForeignKey(x=>x.FromUserId)
                .HasConstraintName("messages_from_user_id_fkey");

                e.HasOne(x=>x.ToUser)
                .WithMany(x=>x.ToMessages)
                .HasForeignKey(x=>x.ToUserId);
            });

            modelBuilder.Entity<Users>(e => 
            {
                e.ToTable("Users");
                e.HasKey(x => x.Id).HasName("users_pkey");

                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Name).HasMaxLength(250).HasColumnName("name");
                
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
