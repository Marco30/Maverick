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

    public async Task<QuestionDto?> SaveQuestionAsync(QuestionDto userQuestion)
    {
        var userId = (int)userQuestion.UserId!;

        
        if (userQuestion.ConversationId == 0)
        {

            var conversation = await CreateConversationAsync(userId);
            userQuestion.ConversationId = conversation!.Id;

        }

        var question = new Question
        {
            Text = userQuestion.Text, UserId = userId, 
            ConversationId = (int)userQuestion.ConversationId
        };

        try
        {
            await _context.Question.AddAsync(question);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Error saving question", e);
        }

        return _mapper.Map<QuestionDto>(question);

    }

    public async Task<AnswerDto?> SaveAnswerAsync(AnswerDto answer)
    {
        var answerToSave = new Answer
        {
            Text = answer.Text,
            Date = answer.Date,
            QuestionId = answer.QuestionId,
            ConversationId = answer.ConversationId
        };

        await _context.Answer.AddAsync(answerToSave);
        await _context.SaveChangesAsync();

        answer.Id = answerToSave.Id;

        return answer;

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

    public async Task<ConversationDto?> GetConversationAsync(int conversationId)
    {
        return await _context.Conversation.Where(c => c.Id == conversationId).ProjectTo<ConversationDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<bool> ConversationExists(int conversationId)
    {
        return await _context.Conversation.AnyAsync(c => c.Id == conversationId);
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



















    private async Task<Conversation?> CreateConversationAsync(int userId)
    {
        var conversation = new Conversation { UserId = userId };
        
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




}