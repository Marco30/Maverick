using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Typesense;
using WarGamesAPI.Data;
using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Model;
using WarGamesAPI.Services.TypeSenseModel;

namespace WarGamesAPI.Services;

public class SearchService : ISearchService
{
    readonly ILogger<SearchService> _logger;
    readonly IMapper _mapper;
    readonly ITypesenseClient _typeSense;
    readonly WarGamesContext _context;

    public SearchService(ILogger<SearchService> logger, IMapper mapper, ITypesenseClient typeSense, 
        WarGamesContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _typeSense = typeSense;
        _context = context;
    }

    
    public async Task<ConversationDto> SearchConversationAsync(int userId, string searchText, int conversationId)
    {
        var query = new SearchParameters(searchText, "text")
        {
            HighlightFullFields = "text",
            FilterBy = $"conversationid:{conversationId}",
            SortBy = "timestamp:desc",
            IncludeFields = "id,text,userid,conversationid,source"
        };

        var qaIndexObjects = new List<QuestionAnswerIndex>();  
        
        try
        {
            SearchResult<QuestionAnswerIndex> searchResult = await _typeSense.Search<QuestionAnswerIndex>("questionanswer", query);

            var searchHits = searchResult.Hits;

            foreach (var hit in searchHits)
            {
                
                var searchHitDocument = hit.Document;
                searchHitDocument.Text = hit.Highlights[0].Snippet;
                qaIndexObjects.Add(searchHitDocument);

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await ConvertToHighlightedConversationAsync(conversationId, qaIndexObjects);
    }
    
    public async Task<SearchResultDto> SearchConversationHistoryAsync(int userId, string searchText)
    {
        
        //await CreateQuestionAnswerCollection();
        //await IndexAllDocuments();


        var query = new SearchParameters(searchText, "text");

        
        List<QuestionAnswerIndex> matchedDocuments = new List<QuestionAnswerIndex>();

        try
        {
            SearchResult<QuestionAnswerIndex> searchResult = await _typeSense.Search<QuestionAnswerIndex>("questionanswer", query);

            int totalHits = searchResult.Found;
            var searchHits = searchResult.Hits;

            foreach (var hit in searchHits)
            {
                // Access the individual fields of each search hit
                var a = hit.Document;
                var highlights = hit.Highlights;
                var c = hit.TextMatch;
                var d = hit.VectorDistance;
                var e = hit.GetHashCode();

                matchedDocuments.Add(hit.Document);
            }

            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var result = new SearchResultDto
        {
            SearchText = searchText,
        }; 

        foreach (var document in matchedDocuments)
        {
            result.ConversationIds.Add(document.ConversationId);
            switch (document.Source)
            {
                case "Question":
                    result.QuestionIds.Add(Convert.ToInt32(document.Id));
                    break;
                case "Answer":
                    result.AnswerIds.Add(Convert.ToInt32(document.Id));
                    break;
            }
        }

       

        return result;
    }

    public async Task<ConversationDto> SearchConversationManuallyAsync(int userId, string searchText, int conversationId)
    {
        List<QuestionAnswer> questionAnswerList = await _context.QuestionAnswer.Where(qa => qa.ConversationId == conversationId).ToListAsync();

        var regex = new Regex(searchText, RegexOptions.IgnoreCase);

        var highlightedQAList = questionAnswerList;

        foreach (var item in highlightedQAList)
        {
            if (item.Text!.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            {
                item.Text = regex.Replace(item.Text, m => $"<mark>{m.Value}</mark>");
            }
        }

        var highlightedIds = new List<int>();

        foreach (var item in highlightedQAList)
        {
            highlightedIds.Add(item.Id);
        }


        var conversation = await _context.Conversation
            .Include(c => c.Questions)
            .ThenInclude(q => q.Answers)
            .Where(c => c.Id == conversationId).SingleOrDefaultAsync();

        
        var messages = new List<QAItemDto>();

        foreach (var question in conversation!.Questions)
        {
            var questionDto = _mapper.Map<QuestionDto>(question);
            var answerDtos = _mapper.Map<List<AnswerDto>>(question.Answers);

            var questionMessage = _mapper.Map<MessageDto>(questionDto);

            if (highlightedIds.Contains(questionMessage.Id))
            {
                var qaObject = highlightedQAList.FirstOrDefault(qa => qa.Id == questionMessage.Id);
                if (qaObject != null)  questionMessage.Text = qaObject.Text;
            }

            var answerMessages = _mapper.Map<List<MessageDto>>(answerDtos);

            foreach (var answer in answerMessages)
            {
                answer.UserId = question.UserId;
                
                if (highlightedIds.Contains(answer.Id))
                {
                    var qaObject = highlightedQAList.FirstOrDefault(qa => qa.Id == answer.Id);
                    if (qaObject != null)  answer.Text = qaObject.Text;
                }
            }



            messages.Add(new QAItemDto { Question = questionMessage, Answers = answerMessages }); 
        }
        return new ConversationDto
        {
            Conversation = messages
        };   



    }

    public async Task IndexAllDocuments()
    {
        List<QuestionAnswer> questionAnswerList = await _context.QuestionAnswer.ToListAsync();
        var qaIndexList = _mapper.Map<List<QuestionAnswerIndex>>(questionAnswerList);

        foreach (var document in qaIndexList)
        {
            try
            {
                var createDocumentResult = await _typeSense.CreateDocument<QuestionAnswerIndex>("questionanswer", document);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }

    public async Task IndexNewDocumentsAsync(List<QuestionAnswer> documents)
    {
        var indexDocuments = _mapper.Map<List<QuestionAnswerIndex>>(documents);

            try
            {
                foreach (var document in indexDocuments)
                {
                    var createDocumentResult = await _typeSense.CreateDocument("questionanswer", document);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        
    }

    public void ReindexMissingDocuments()
    {

        throw new NotImplementedException();
        // Fetch all documents from the database

        // Fetch all documents from Typesense

        // Find the IDs of the missing documents

        // For each missing ID, reindex the document
    }

    public async Task CreateQuestionAnswerCollection()
    {
        var schema = new Schema("QuestionAnswer",
            new List<Field>
            {
                new("id", FieldType.Int32, false),
                new("text", FieldType.String, false),
                new("userid", FieldType.String, false, true),
                new("conversationid", FieldType.String, false, true, false),
                new("source", FieldType.String, false, true, false),
                new("timestamp", FieldType.Int32, false)
            },
            "timestamp");


        try
        {
            CollectionResponse createCollectionResult = await _typeSense.CreateCollection(schema);
            string jsonString = JsonConvert.SerializeObject(createCollectionResult, Formatting.Indented);  

            _logger.LogInformation(jsonString); 
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while creating the collection");
            throw;
        }
    }

    



    private async Task<ConversationDto> ConvertToHighlightedConversationAsync(int conversationId, List<QuestionAnswerIndex> qaIndexObjects)
    {
        var highlightedIds = qaIndexObjects.Select(index => Convert.ToInt32(index.Id)).ToList();

        var conversation = await _context.Conversation
            .Include(c => c.Questions)
            .ThenInclude(q => q.Answers)
            .Where(c => c.Id == conversationId).SingleOrDefaultAsync();

        if (conversation is null)
        {
            throw new Exception($"Conversation with id {conversationId} not found");
        }

        var messages = new List<QAItemDto>();

        foreach (var question in conversation.Questions)
        {
            var questionDto = _mapper.Map<QuestionDto>(question);
            var answerDtos = _mapper.Map<List<AnswerDto>>(question.Answers);

            var questionMessage = _mapper.Map<MessageDto>(questionDto);

            if (highlightedIds.Contains(questionMessage.Id))
            {
                var qaIndex = qaIndexObjects.FirstOrDefault(qa => Convert.ToInt32(qa.Id) == questionMessage.Id);
                if (qaIndex != null)  questionMessage.Text = qaIndex.Text;
            }

            var answerMessages = _mapper.Map<List<MessageDto>>(answerDtos);

            foreach (var answer in answerMessages)
            {
                answer.UserId = question.UserId;
                
                if (highlightedIds.Contains(answer.Id))
                {
                    var qaIndex = qaIndexObjects.FirstOrDefault(qa => Convert.ToInt32(qa.Id) == answer.Id);
                    if (qaIndex != null)  answer.Text = qaIndex.Text;
                }
            }



            messages.Add(new QAItemDto { Question = questionMessage, Answers = answerMessages }); 
        }
        return new ConversationDto
        {
            Conversation = messages
        };   
    }


    

}