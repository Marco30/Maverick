using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WarGamesAPI.Data;
using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;


namespace WarGamesAPI.Repositories;

public class ChatHistoryRepository : IChatHistoryRepository
{
    readonly WarGamesContext _context;
    readonly IMapper _mapper;
    readonly ISearchService _searchService;
    readonly ILogger<ChatHistoryRepository> _logger;

    public ChatHistoryRepository(WarGamesContext context, IMapper mapper, ISearchService searchService, 
        ILogger<ChatHistoryRepository> logger)
    {
        _context = context;
        _mapper = mapper;
        _searchService = searchService;
        _logger = logger;
    }

    public async Task<Answer?> SaveQuestionAndAnswerAsync(QuestionDto userQuestion, AnswerDto answer, bool mockReply)
    {
        var userId = userQuestion.UserId;

        if (userQuestion.ConversationId == 0)
        {
            var conversationName = "";
            if (userQuestion.Text != null)
            {
                conversationName = string.Join(" ", userQuestion.Text.Trim().Split(Array.Empty<char>(), 
                    StringSplitOptions.RemoveEmptyEntries));
            }

            var conversation = await CreateConversationAsync(userQuestion.UserId, conversationName);
            userQuestion.ConversationId = conversation.Id;
        }

        var questionToSave = new Question
        {
            Text = userQuestion.Text, 
            UserId = userId, 
            ConversationId = userQuestion.ConversationId, 
            Date = DateTime.Now
        };

        await _context.Question.AddAsync(questionToSave);
        await _context.SaveChangesAsync();

        var answerToSave = new Answer
        {
            Text = answer.Text,
            Date = answer.Date,
            QuestionId = questionToSave.Id
        };

        await _context.Answer.AddAsync(answerToSave);
        await _context.SaveChangesAsync();

        var questionIndexObject = new QuestionAnswer
        {
            Id = questionToSave.Id,
            ConversationId = questionToSave.ConversationId,
            UserId = questionToSave.UserId,
            Text = questionToSave.Text,
            Source = "Question"
        };

        var answerIndexObject = new QuestionAnswer
        {
            Id = answerToSave.Id,
            ConversationId = questionToSave.ConversationId,
            UserId = questionToSave.UserId,
            Text = answerToSave.Text,
            Source = "Answer"
        };

        var objectsToAddToIndexUpdate = new List<QuestionAnswer> { questionIndexObject, answerIndexObject };

        try
        {
            await _searchService.IndexNewDocumentsAsync(objectsToAddToIndexUpdate);

        }
        catch (Exception e)
        {
            _logger.LogError(e, 
                "Failed to index new document. If you're using the manual search service in a development environment, " +
                "this operation is expected to fail and there's nothing to worry about. " +
                "If you're seeing this error in production, however, you need to investigate.");
        }


        return answerToSave;
    }

    public async Task UpdateConversationAsync(int conversationId)
    {
        var conversation = await _context.Conversation.SingleOrDefaultAsync(c => c.Id == conversationId);
        if (conversation is null)
        {
            throw new InvalidOperationException("Conversation not found");
        }

        conversation.Updated = DateTime.Now;
        await _context.SaveChangesAsync();

    }

    public async Task<List<QuestionDto>> GetUserQuestionsAsync(int userId)
    {
        return await _context.Question.Where(q => q.UserId == userId)
            .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<QuestionDto?> GetQuestionAsync(int questionId)
    {
        return await _context.Question.Where(q => q.Id == questionId)
            .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public async Task<ConversationDto?> GetConversationDtoAsync(int conversationId)
    {
        var conversation = await _context.Conversation
            .Include(c => c.Questions)
            .ThenInclude(q => q.Answers)
            .Where(c => c.Id == conversationId).SingleOrDefaultAsync();

        if (conversation is null) return null;

        var messages = new List<QAItemDto>();

        foreach (var question in conversation.Questions)
        {
            var questionDto = _mapper.Map<QuestionDto>(question);
            var answerDtos = _mapper.Map<List<AnswerDto>>(question.Answers);

            var questionMessage = _mapper.Map<MessageDto>(questionDto);
            var answerMessages = _mapper.Map<List<MessageDto>>(answerDtos);

            foreach (var answer in answerMessages) answer.UserId = question.UserId;
            messages.Add(new QAItemDto { Question = questionMessage, Answers = answerMessages }); 
        }
        return new ConversationDto
        {
            Conversation = messages
        };   
    } 
    
    public async Task<string?> ChangeConversationNameAsync(int conversationId, string newName)
    {
        var conversation = await _context.Conversation.SingleOrDefaultAsync(c => c.Id == conversationId);
        if (conversation is null) return null;
        conversation.Name = await GenerateUniqueConversationNameAsync(conversation.UserId, newName);
        conversation.Updated = DateTime.Now;
        await _context.SaveChangesAsync();
        return conversation.Name;
    }

    public async Task<List<ConversationInfoDto>> GetConversationInfosAsync(int userId)
    {
        var conversations = await _context.Conversation
            .Include(c => c.Questions)
            .ThenInclude(q => q.Answers)
            .Where(c => c.UserId == userId).ToListAsync();

        return conversations.Select(conversation => _mapper.Map<ConversationInfoDto>(conversation)).ToList();
    }
    
    public async Task<List<AnswerDto>> GetAnswersAsync(int questionId)
    {
        return await _context.Answer.Where(a => a.QuestionId == questionId)
            .ProjectTo<AnswerDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<AnswerDto?> GetAnswerAsync(int answerId)
    {
        return await _context.Answer.Where(a => a.Id == answerId).ProjectTo<AnswerDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<List<QuestionDto>> GetQuestionsFromConversationAsync(int conversationId)
    {
        return await _context.Question.Where(q => q.ConversationId == conversationId)
            .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<List<AnswerDto>> GetAnswersFromConversationAsync(int conversationId)
    {
        var answers = await _context.Conversation
            .Where(c => c.Id == conversationId)
            .SelectMany(c => c.Questions)
            .SelectMany(q => q.Answers)
            .ProjectTo<AnswerDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return answers;
    } 
    
    public async Task DeleteQuestionAsync(int questionId)
    {
        var question = await _context.Question.FindAsync(questionId);
        if (question is null)
        {
            throw new Exception($"Question with id {questionId} not found");
        }

        var answersToQuestion = _context.Answer.Where(a => a.QuestionId == questionId);
        
        _context.Question.Remove(question);
        _context.Answer.RemoveRange(answersToQuestion);

        await _context.SaveChangesAsync();

    }

    public async Task DeleteAnswerAsync(int answerId)
    {
        var answer = await _context.Answer.FindAsync(answerId);
        if (answer is null)
        {
            throw new Exception($"Answer with id {answerId} not found");
        }

        _context.Answer.Remove(answer);

        await _context.SaveChangesAsync();

    }

    public async Task DeleteConversationAsync(int conversationId)
    {
        var conversation = await _context.Conversation
            .Include(c => c.Questions)
            .ThenInclude(q => q.Answers)
            .SingleOrDefaultAsync(c => c.Id == conversationId);

        if (conversation is null)
        {
            throw new Exception($"Conversation with id {conversationId} not found");
        }

        foreach(var question in conversation.Questions)
        {
            _context.Answer.RemoveRange(question.Answers);
        }

        _context.Question.RemoveRange(conversation.Questions);
        _context.Conversation.Remove(conversation);

        await _context.SaveChangesAsync();
    } 
    
    public async Task<Conversation?> CreateConversationAsync(int userId, string conversationName)
    {
        if (conversationName is null) throw new Exception("Conversation name is required");

        conversationName = await GenerateUniqueConversationNameAsync(userId, conversationName);

        var conversation = new Conversation
        {
            UserId = userId, 
            Name = conversationName,
            Date = DateTime.Now,
            Updated = DateTime.Now
        };

        try
        {
            await _context.Conversation.AddAsync(conversation);
            await _context.SaveChangesAsync();
            return conversation;

        }
        catch (Exception e)
        {
            throw new Exception("Error saving conversation", e);
        }

    }

    public async Task<List<Conversation>> GetUserConversationsAsync(int userId)
    {
        var conversations = await _context.Conversation
            .Include(c => c.Questions)
            .ThenInclude(q => q.Answers)
            .Where(c => c.UserId == userId).ToListAsync();

        return conversations;
    }

    public async Task<Conversation?> GetConversationAsync(int conversationId)
    {
        var conversation = await _context.Conversation
            .Include(c => c.Questions)
            .ThenInclude(q => q.Answers)
            .Where(c => c.Id == conversationId).SingleOrDefaultAsync();

        return conversation;
    }

    public async Task<List<QuestionAnswer>> GetQuestionAnswersAsync()
    {
        return await _context.QuestionAnswer.ToListAsync();
    }
    
    /*
 * This method is used for manual search of the database, for development purposes.
 */
    public async Task<SearchResultDto> SearchConversationHistoryAsync(int userId, string searchText)
    {
        var conversations = await GetUserConversationsAsync(userId);

        var result = new SearchResultDto
        {
            SearchText = searchText
        };

        foreach (var conversation in conversations)
        {
            foreach (var question in conversation.Questions)
            {

                if (question.Text!.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.ConversationIds.Add(conversation.Id);
                    result.QuestionIds.Add(question.Id);
                }

                foreach (var answer in question.Answers)
                {
                    if (answer.Text != null)
                    {
                        if (answer.Text.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        {
                            result.AnswerIds.Add(answer.Id);
                            if (!result.ConversationIds.Contains(conversation.Id))
                            {
                                result.ConversationIds.Add(question.ConversationId);
                            }
                        }
                    }
                }
            }
        }

        return result;
    }
    

    private async Task<string> GenerateUniqueConversationNameAsync(int userId, string name)
    {
        var userConversations =
            await _context.Conversation.Where(c => c.UserId == userId).ToListAsync();

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