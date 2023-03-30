using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;
using WarGamesAPIAPI.JsonCRUD;

#pragma warning disable CS1998

namespace Courses.Api.Repositories;

public class QuestionRepository : IQuestionRepository
{
   
    public async Task<Question?> SaveQuestion(AskQuestionDto userQuestion)
    {
        var question = new Question
        {
            Id = new Random().Next(), Text = userQuestion.Text, UserId = userQuestion.UserId
        };

        Json.CheckAndAddDataToJson("Question", question);
        List<Question> allQuestions = Json.GetJsonData<Question>("Question");
        var confirmedQuestion = allQuestions.FirstOrDefault(m => m.Id == question.Id);
        return confirmedQuestion ?? null;
    }

    
    public async Task<Answer?> SaveAnswer(Answer answer)
    {
        answer.Id = new Random().Next();
        Json.CheckAndAddDataToJson("Answer", answer);
        List<Answer> allAnswers = Json.GetJsonData<Answer>("Answer");
        var confirmedAnswer = allAnswers.FirstOrDefault(m => m.Id == answer.Id);
        return confirmedAnswer ?? null;
    }

    public async Task<List<QuestionDto>> GetUserQuestions(int userId)
    {
        List<Question> allQuestions = Json.GetJsonData<Question>("Question");
        var result = (from question in allQuestions where question.UserId == userId 
            select new QuestionDto { Id = question.Id, UserId = question.UserId, Text = question.Text }).ToList();
        return result;
    }

    public async Task<QuestionDto?> GetQuestion(int questionId)
    {
        List<Question> allQuestions = Json.GetJsonData<Question>("Question");
        
        var question = allQuestions.FirstOrDefault(q => q.Id == questionId);
        return question != null ? new QuestionDto { Id = question.Id, UserId = question.UserId, Text = question.Text } : null;
    }


}