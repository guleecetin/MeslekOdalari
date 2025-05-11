using MeslekOdalari.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace MeslekOdalari.DataAccess.Context
{
    public class MeslekOdalariContext:DbContext
    {
        public MeslekOdalariContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<News> Newss { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<Rooms> Roomss { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<SubHeader> SubHeaders { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Regulation> Regulations { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<AuditBoard> AuditBoards { get; set; }
        public DbSet<DisciplinaryBoard> DisciplinaryBoards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Banner>().ToCollection("Banners");
            modelBuilder.Entity<Contact>().ToCollection("Contacts");
            modelBuilder.Entity<Counter>().ToCollection("Counters");
            modelBuilder.Entity<Feature>().ToCollection("Features");
            modelBuilder.Entity<Message>().ToCollection("Messages");
            modelBuilder.Entity<News>().ToCollection("Newss");
            modelBuilder.Entity<Quest>().ToCollection("Quests");
            modelBuilder.Entity<Rooms>().ToCollection("Roomss");
            modelBuilder.Entity<Video>().ToCollection("Videos");
            modelBuilder.Entity<SubHeader>().ToCollection("SubHeaders");
            modelBuilder.Entity<Project>().ToCollection("Projects");
            modelBuilder.Entity<Regulation>().ToCollection("Regulations");
            modelBuilder.Entity<Board>().ToCollection("Boards");
            modelBuilder.Entity<AuditBoard>().ToCollection("AuditBoards ");
            modelBuilder.Entity<DisciplinaryBoard>().ToCollection("DisciplinaryBoards");
        }

    }
}
