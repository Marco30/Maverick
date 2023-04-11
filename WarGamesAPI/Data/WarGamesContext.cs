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
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 5,
            SocialSecurityNumber = "198507119595",
            FirstName = "Khaled",
            LastName = "Abo",
            FullName = "Khaled Abo",
            Email = "khaled@khaled.se",
            Password = "123456",
            MobilePhoneNumber = null,
            AgreeMarketing = true,
            SubscribeToEmailNotification = true,
            ProfileImage = null,
            Gender = "Man",
            AddressId = 1
        });
        
        modelBuilder.Entity<Address>().HasData(new Address
        {
            Id = 1,
            City = "Stockholm",
            Country = "Sweden",
            Street = "Röntgenvägen 5 lgh 1410",
            ZipCode = "14152",
            UserId = 5
        });

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
