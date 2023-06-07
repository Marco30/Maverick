using AutoMapper;
using Newtonsoft.Json;
using Typesense;
using WarGamesAPI.DTO;
using WarGamesAPI.Interfaces;

namespace WarGamesAPI.Services;

public class SearchService : ISearchService
{
    readonly ILogger<SearchService> _logger;
    readonly IMapper _mapper;
    readonly ITypesenseClient _typeSense;
    

    public SearchService(ILogger<SearchService> logger, IMapper mapper, ITypesenseClient typeSense)
    {
        _logger = logger;
        _mapper = mapper;
        _typeSense = typeSense;

    }

    
    public async Task<SearchResultDto> SearchConversationHistoryAsync(int userId, string searchText)
    {
        
        await CreateQuestionAnswerCollection();

        var result = new SearchResultDto { SearchText = searchText };
        
        return result;
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



    

}