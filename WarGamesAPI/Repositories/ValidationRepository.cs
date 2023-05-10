using Microsoft.EntityFrameworkCore;
using WarGamesAPI.Data;
using WarGamesAPI.Interfaces;

namespace Courses.Api.Repositories;

public class ValidationRepository : IValidationRepository
{
    readonly WarGamesContext _context;

    public ValidationRepository(WarGamesContext context)
    {
        _context = context;
    }

    public async Task<bool> UserOwnsQuestion(int userId, int questionId)
    {
        var question = await _context.Question.FirstOrDefaultAsync(q => q.Id == questionId);
        if (question is null) return false;
        
        return question.UserId == userId;
    }

    public async Task<bool> UserOwnsAnswer(int userId, int answerId)
    {
        var answer = await _context.Answer.FirstOrDefaultAsync(a => a.Id == answerId);
        if (answer is null) return false;
        var question = await _context.Question.FirstOrDefaultAsync(q => q.Id == answer.QuestionId);
        if (question is null) return false;
        
        return question.UserId == userId;
    }

    public async Task<bool> UserOwnsConversation(int userId, int conversationId)
    {
        var conversation = await _context.Conversation.FirstOrDefaultAsync(c => c.Id == conversationId);
        if (conversation is null) return false;
        return conversation.UserId == userId;

    }

    public async Task<bool> UserOwnsLibraryConversation(int userId, int conversationId)
    {
        var conversation = await _context.LibraryConversation.FirstOrDefaultAsync(lc => lc.Id == conversationId);
        if (conversation is null) return false;
        return conversation.UserId == userId;

    }

    public async Task<bool> UserOwnsLibraryAnswer(int userId, int answerId)
    {
        var answer = await _context.LibraryAnswer.FirstOrDefaultAsync(la => la.Id == answerId);
        if (answer is null) return false;
        var question = await _context.LibraryQuestion.FirstOrDefaultAsync(lq => lq.Id == answer.LibraryQuestionId);
        if (question is null) return false;
        
        return question.UserId == userId;
    }

    public async Task<bool> UserOwnsLibraryQuestion(int userId, int questionId)
    {
        var question = await _context.LibraryQuestion.FirstOrDefaultAsync(lq => lq.Id == questionId);
        if (question is null) return false;

        return question.UserId == userId;
    }
    

}