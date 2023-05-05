using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WarGamesAPI.Data;
using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;


namespace Courses.Api.Repositories;

public class QuestionRepository : IQuestionRepository
{
    readonly WarGamesContext _context;
    readonly IMapper _mapper;

    public QuestionRepository(WarGamesContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Answer?> SaveQuestionAndAnswerAsync(QuestionDto userQuestion, AnswerDto answer)
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
            Text = userQuestion.Text, UserId = userId, ConversationId = userQuestion.ConversationId
        };

        await _context.Question.AddAsync(questionToSave);
        await _context.SaveChangesAsync();

        var answerToSave = new Answer
        {
            Text = answer.Text,
            Date = answer.Date,
            QuestionId = questionToSave.Id,
            ConversationId = questionToSave.ConversationId
        };

        await _context.Answer.AddAsync(answerToSave);
        await _context.SaveChangesAsync();

        

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

    public async Task<ConversationDto?> GetConversationAsync(int conversationId)
    {
        var conversation = await _context.Conversation
            .Include(c => c.Questions)
            .Include(c => c.Answers)
            .Where(c => c.Id == conversationId).SingleOrDefaultAsync();

        if (conversation is null) return null;

        var messages = new List<QAItemDto>();

        var questions = _mapper.Map<List<QuestionDto>>(conversation.Questions);
        var answers = _mapper.Map<List<AnswerDto>>(conversation.Answers);

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

    public async Task<string?> ChangeConversationNameAsync(NameConversationDto name)
    {
        var conversation = await _context.Conversation.SingleOrDefaultAsync(c => c.Id == name.ConversationId);
        if (conversation is null) return null;
        conversation.Name = name.NewName;
        conversation.Updated = DateTime.Now;
        await _context.SaveChangesAsync();
        return conversation.Name;
    }

    public async Task<List<ConversationInfoDto>> GetConversationInfosAsync(int userId)
    {
        var conversations = await _context.Conversation
            .Include(c => c.Questions)
            .Include(c => c.Answers)
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
        return await _context.Answer.Where(a => a.ConversationId == conversationId)
            .ProjectTo<AnswerDto>(_mapper.ConfigurationProvider).ToListAsync();
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
        var conversation = await _context.Conversation.FindAsync(conversationId);
        if (conversation is null)
        {
            throw new Exception($"Conversation with id {conversationId} not found");
        }

        var questions = await _context.Question.Where(q => q.ConversationId == conversationId).ToListAsync();
        var answers = await _context.Answer.Where(a => a.ConversationId == conversationId).ToListAsync();

        _context.Question.RemoveRange(questions);
        _context.Answer.RemoveRange(answers);
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