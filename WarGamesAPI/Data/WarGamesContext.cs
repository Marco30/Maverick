using Microsoft.EntityFrameworkCore;
using WarGamesAPI.Model;

namespace WarGamesAPI.Data;

public class WarGamesContext : DbContext
{
    public DbSet<Address> Address => Set<Address>();
    public DbSet<Answer> Answer => Set<Answer>();
    public DbSet<Question> Question => Set<Question>();
    public DbSet<User> User => Set<User>();
    public DbSet<Conversation> Conversation => Set<Conversation>();


    public WarGamesContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithOne(u => u.Address)
            .HasForeignKey<User>(u => u.AddressId);

        modelBuilder.Entity<Conversation>()
            .HasOne(c => c.User)
            .WithMany(u => u.Conversations)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.User)
            .WithMany(u => u.Questions)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Conversation)
            .WithMany(c => c.Questions)
            .HasForeignKey(q => q.ConversationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Conversation)
            .WithMany(c => c.Answers)
            .HasForeignKey(a => a.ConversationId)
            .OnDelete(DeleteBehavior.NoAction);
    }

}