using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WarGamesAPI.Model;
using WarGamesAPI.Services.SearchServiceModel;

namespace WarGamesAPI.Data;

public class WarGamesContext : DbContext
{
    public DbSet<Address> Address => Set<Address>();
    public DbSet<User> User => Set<User>();
    
    public DbSet<Conversation> Conversation => Set<Conversation>();
    public DbSet<Question> Question => Set<Question>();
    public DbSet<Answer> Answer => Set<Answer>();
    
    public DbSet<LibraryConversation> LibraryConversation => Set<LibraryConversation>();
    public DbSet<LibraryQuestion> LibraryQuestion => Set<LibraryQuestion>();
    public DbSet<LibraryAnswer> LibraryAnswer => Set<LibraryAnswer>();

    public DbSet<Model.QuestionAnswer> QuestionAnswer => Set<Model.QuestionAnswer>();

    public WarGamesContext(DbContextOptions options) : base(options) {}


    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>("SharedIdSequence")
            .StartsAt(101)
            .IncrementsBy(1);

        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEXT VALUE FOR SharedIdSequence");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEXT VALUE FOR SharedIdSequence");
        });

        

        modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithOne(u => u.Address)
            .HasForeignKey<Address>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);


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

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Conversation)
            .WithMany(c => c.Questions)
            .HasForeignKey(q => q.ConversationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Model.QuestionAnswer>().ToView("QuestionAnswer");

        modelBuilder.Entity<LibraryConversation>()
            .HasOne(lc => lc.User)
            .WithMany(u => u.LibraryConversations)
            .HasForeignKey(lc => lc.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<LibraryConversation>()
            .HasOne(lc => lc.ChatHistoryConversation)
            .WithMany()
            .HasForeignKey(lc => lc.ChatHistoryConversationId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<LibraryQuestion>()
            .HasOne(sq => sq.LibraryConversation)
            .WithMany(lc => lc.LibraryQuestions)
            .HasForeignKey(lq => lq.LibraryConversationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<LibraryQuestion>()
            .HasOne(lq => lq.Question)
            .WithMany()
            .HasForeignKey(lq => lq.ChatHistoryQuestionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<LibraryAnswer>()
            .HasOne(la => la.Answer)
            .WithMany()
            .HasForeignKey(la => la.ChatHistoryAnswerId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<LibraryAnswer>()
            .HasOne(la => la.LibraryQuestion)
            .WithMany(lq => lq.LibraryAnswers)
            .HasForeignKey(la => la.LibraryQuestionId)
            .OnDelete(DeleteBehavior.NoAction);
        
        var userJson = File.ReadAllText(Path.Combine("Data", "SeedData", "User.json"));
        var users = JsonConvert.DeserializeObject<List<User>>(userJson);
        modelBuilder.Entity<User>().HasData(users!);
        
        var addressJson = File.ReadAllText(Path.Combine("Data","SeedData", "Address.json"));
        var addresses = JsonConvert.DeserializeObject<List<Address>>(addressJson);
        modelBuilder.Entity<Address>().HasData(addresses!);

        var conversationJson = File.ReadAllText(Path.Combine("Data","SeedData", "Conversation.json"));
        var conversations = JsonConvert.DeserializeObject<List<Conversation>>(conversationJson);
        modelBuilder.Entity<Conversation>().HasData(conversations!);

        var questionJson = File.ReadAllText(Path.Combine("Data","SeedData", "Question.json"));
        var questions = JsonConvert.DeserializeObject<List<Question>>(questionJson);
        modelBuilder.Entity<Question>().HasData(questions!);

        var answerJson = File.ReadAllText(Path.Combine("Data","SeedData", "Answer.json"));
        var answers = JsonConvert.DeserializeObject<List<Answer>>(answerJson);
        modelBuilder.Entity<Answer>().HasData(answers!);
    }

}
