using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarGamesAPI.Data;
using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;


namespace Courses.Api.Repositories;

public class LibraryRepository : ILibraryRepository
{
    readonly WarGamesContext _context;
    readonly IMapper _mapper;

    public LibraryRepository(WarGamesContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ConversationInfoDto>> GetConversationInfosAsync(int userId)
    {
        var conversations = await _context.LibraryConversation
            .Include(lc => lc.LibraryQuestions)
            .Include(lc => lc.LibraryAnswers)
            .Where(lc => lc.UserId == userId).ToListAsync();

        return conversations.Select(conversation => _mapper.Map<ConversationInfoDto>(conversation)).ToList();

    }

    public async Task<ConversationDto?> GetConversationAsync(int conversationId)
    {
        var conversation = await _context.LibraryConversation
            .Include(lc => lc.LibraryQuestions)
            .Include(lc => lc.LibraryAnswers)
            .Where(lc => lc.Id == conversationId).SingleOrDefaultAsync();

        if (conversation is null) return null;

        var messages = new List<QAItemDto>();

        var questions = _mapper.Map<List<QuestionDto>>(conversation.LibraryQuestions);
        var answers = _mapper.Map<List<AnswerDto>>(conversation.LibraryAnswers);

        foreach (var question in questions)
        {
            List<AnswerDto> answersToAdd = answers.Where(a => a.QuestionId == question.Id).ToList();
            var questionMessage = _mapper.Map<MessageDto>(question);
            var answerMessages = _mapper.Map<List<MessageDto>>(answersToAdd);
            foreach (var answer in answerMessages) answer.UserId = question.UserId;
            messages.Add(new QAItemDto { Question = questionMessage, Answers = answerMessages }); 
            
        }
        return new ConversationDto
        {
            Conversation = messages
        };
        
    }

    public async Task<LibraryConversation?> CreateLibraryConversationAsync(int userId, string? conversationName)
    {
        if (conversationName is null) throw new Exception("Conversation name is required");

        conversationName = await GenerateUniqueConversationNameAsync(userId, conversationName);
        
        var libraryConversation = new LibraryConversation
        {
            UserId = userId,
            Name = conversationName,
            Date = DateTime.Now,
            Updated = DateTime.Now,
        };

        try
        {
            await _context.LibraryConversation.AddAsync(libraryConversation);
            await _context.SaveChangesAsync();
            return libraryConversation;

        }
        catch (Exception e)
        {
            throw new Exception("Error saving conversation", e);
        }

    }

    public async Task<LibraryConversation?> CreateLibraryConversationFromChatHistoryAsync(int userId, string? newName, 
        int originalConversationId)
    {
        
        var originalConversation = await _context.Conversation
            .Include(c => c.Questions)
            .ThenInclude(q => q.Answers)
            .SingleOrDefaultAsync(c => c.Id == originalConversationId);
        if (originalConversation is null || originalConversation.UserId != userId)
        {
            throw new Exception("Original conversation not found");
        }
    

        if (newName is null)
        {
            newName = originalConversation.Name;
        }
        else
        {
            newName = await GenerateUniqueConversationNameAsync(userId, newName);
        }
        
        var libraryConversation = new LibraryConversation
        {
            UserId = userId,
            Name = newName,
            Date = DateTime.Now,
            Updated = DateTime.Now,
            ChatHistoryConversationId = originalConversationId
        };

        try
        {
            await _context.LibraryConversation.AddAsync(libraryConversation);
            await _context.SaveChangesAsync();
            return libraryConversation;

        }
        catch (Exception e)
        {
            throw new Exception("Error saving conversation", e);
        }

    } 

    public async Task<LibraryConversation?> SaveQuestionAndAnswersToLibraryAsync(int questionId, int? libraryConversationId)
    {
        var question = await _context.Question.SingleOrDefaultAsync(q => q.Id == questionId);
        if (question is null)
        {
            throw new Exception($"Question with id {questionId} not found");
        }

        var userId = question.UserId;
        var historyAnswers = await _context.Answer.Where(a => a.QuestionId == questionId).ToListAsync();

        var libraryConversation = await _context.LibraryConversation.SingleOrDefaultAsync(c =>
            c.Id == libraryConversationId);


        if (libraryConversation is null)
        {
            libraryConversation = await CreateLibraryConversationAsync(question.UserId, question.Text);
        }
        
        libraryConversation!.Updated = DateTime.Now;

        
        var libraryQuestion = new LibraryQuestion
        {
            UserId = userId,
            Text = question.Text,
            Date = question.Date,
            ChatHistoryQuestionId = question.Id,
            LibraryConversationId = libraryConversation!.Id
        };
        _context.LibraryQuestion.Add(libraryQuestion);

        await _context.SaveChangesAsync();


        foreach (var answer in historyAnswers)
        {
            var libraryAnswer = new LibraryAnswer
            {
                Text = answer.Text,
                Date = answer.Date,
                LibraryQuestionId = libraryQuestion.Id,
                ChatHistoryAnswerId = answer.Id,
            };

            _context.LibraryAnswer.Add(libraryAnswer);
        }
        
        await _context.SaveChangesAsync();

        
        return libraryConversation;

        

    }

    public async Task<LibraryConversation?> SaveAnswerToLibraryAsync(int answerId, int? libraryConversationId)
    {
        var answer = await _context.Answer.SingleOrDefaultAsync(a => a.Id == answerId);
        if (answer is null)
        {
            throw new Exception($"Answer with id {answerId} not found");
        }

        var question = await _context.Question.SingleOrDefaultAsync(q => q.Id == answer.QuestionId);
        if (question is null)
        {
            throw new Exception($"Question with id {answer.QuestionId} not found");
        }

        var conversation = await _context.Conversation.SingleOrDefaultAsync(c => c.Id == question.ConversationId);
        if (conversation is null)
        {
            throw new Exception($"Conversation with id {question.ConversationId} not found");
        }

        var libraryConversation = new LibraryConversation();


        if (libraryConversationId is null)
        {
            libraryConversation = await CreateLibraryConversationFromChatHistoryAsync(conversation.UserId, question.Text, conversation.Id);
        }

        var libraryQuestion = libraryConversation!.LibraryQuestions.SingleOrDefault(lq => lq.ChatHistoryQuestionId == question.Id);

        // Check if question is already in library
        if (libraryQuestion is null)
        {
            libraryQuestion = new LibraryQuestion
            {
                UserId = question.UserId,
                Text = question.Text,
                Date = question.Date,
                ChatHistoryQuestionId = question.Id,
                LibraryConversationId = libraryConversation.Id
            };
            _context.LibraryQuestion.Add(libraryQuestion);
            await _context.SaveChangesAsync();
        }

        var libraryAnswer = new LibraryAnswer
        {
            Text = answer.Text,
            Date = answer.Date,
            LibraryQuestionId = libraryQuestion.Id,
            ChatHistoryAnswerId = answer.Id,
            LibraryConversationId = libraryConversation.Id
        };

        _context.LibraryAnswer.Add(libraryAnswer);
        await _context.SaveChangesAsync();

        return libraryConversation;
    }
    
    public async Task<LibraryConversation?> SaveQuestionToLibraryAsync(int questionId, int? libraryConversationId)
    {
        var question = await _context.Question.SingleOrDefaultAsync(q => q.Id == questionId);
        if (question is null)
        {
            throw new Exception($"Question with id {questionId} not found");
        }

        var userId = question.UserId;

        var libraryConversation = await _context.LibraryConversation.SingleOrDefaultAsync(c =>
            c.Id == libraryConversationId);


        if (libraryConversation is null)
        {
            libraryConversation = await CreateLibraryConversationFromChatHistoryAsync(question.UserId, question.Text, question.ConversationId);
        }
        
        libraryConversation!.Updated = DateTime.Now;

        
        var libraryQuestion = new LibraryQuestion
        {
            UserId = userId,
            Text = question.Text,
            Date = question.Date,
            ChatHistoryQuestionId = question.Id,
            LibraryConversationId = libraryConversation.Id
        };
        _context.LibraryQuestion.Add(libraryQuestion);

        await _context.SaveChangesAsync();

        return libraryConversation;

    }

    public async Task DeleteLibraryConversationAsync(int conversationId)
    {
        var conversation = await _context.LibraryConversation.FindAsync(conversationId);
        if (conversation is null)
        {
            throw new Exception($"Conversation with id {conversationId} not found");
        }

        var questions = await _context.LibraryQuestion.Where(q => q.LibraryConversationId == conversationId).ToListAsync();
        var answers = await _context.LibraryAnswer.Where(a => a.LibraryConversationId == conversationId).ToListAsync();

        _context.LibraryQuestion.RemoveRange(questions);
        _context.LibraryAnswer.RemoveRange(answers);
        _context.LibraryConversation.Remove(conversation);

        await _context.SaveChangesAsync();

    }

    public async Task DeleteLibraryAnswerAsync(int answerId)
    {
        var answer = await _context.LibraryAnswer.FindAsync(answerId);
        if (answer is null)
        {
            throw new Exception($"Answer with id {answerId} not found");
        }

        _context.LibraryAnswer.Remove(answer);

        await _context.SaveChangesAsync();

    }

    public async Task DeleteLibraryQuestionAsync(int questionId)
    {
        var question = await _context.LibraryQuestion.FindAsync(questionId);
        if (question is null)
        {
            throw new Exception($"Question with id {questionId} not found");
        }

        var answersToQuestion = _context.LibraryAnswer.Where(a => a.LibraryQuestionId == questionId);

        _context.LibraryQuestion.Remove(question);
        _context.LibraryAnswer.RemoveRange(answersToQuestion);

        await _context.SaveChangesAsync();

    }

    public async Task<string?> ChangeLibraryConversationNameAsync(int conversationId, string newName)
    {
        var conversation = await _context.LibraryConversation.SingleOrDefaultAsync(lc => lc.Id == conversationId);
        if (conversation is null) return null;
        conversation.Name = newName;
        conversation.Updated = DateTime.Now;
        await _context.SaveChangesAsync();
        return conversation.Name;
    }
    
    private async Task<string> GenerateUniqueConversationNameAsync(int userId, string name)
    {
        var userConversations =
            await _context.LibraryConversation.Where(c => c.UserId == userId).ToListAsync();

        if (userConversations.Count == 0)
        {
            return name;
        }

        var highestSuffix = userConversations.Select(c => GetSuffix(c.Name, name)).Max();

        if (userConversations.All(c => c.Name != name))
        {
            return name;
        }

        var suffixNumber = highestSuffix + 1;
        var result = $"{name}({suffixNumber})";

        while (userConversations.Any(c => c.Name == result))
        {
            suffixNumber++;
            result = $"{name}({suffixNumber})";
        }

        return result;
    }

    private int GetSuffix(string? oldName, string newName)
    {
        if (oldName is null || !oldName.StartsWith(newName)) return 0;

        var suffix = oldName[newName.Length..].Trim(); // extract suffix (n)

        if (!suffix.StartsWith("(") || !suffix.EndsWith(")")) return 0;

        suffix = suffix.Substring(1, suffix.Length - 2);    // remove parentheses
        int.TryParse(suffix, out var result);

        return result;
    }


}