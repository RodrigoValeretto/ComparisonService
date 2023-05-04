using System.Net.Http.Headers;
using System.Text;
using AnonymizationService;
using ComparisonService.db.Context;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;

namespace ComparisonService.Services;

public class AnonymizerService : Anonymizer.AnonymizerBase
{
    private readonly ILogger<AnonymizerService> _logger;
    private readonly IConfiguration _config;
    private readonly ImageContext _imageContext;

    public AnonymizerService(ILogger<AnonymizerService> logger, IConfiguration config, ImageContext imageContext)
    {
        _logger = logger;
        _config = config;
        _imageContext = imageContext;
    }

    public override async Task<AnonymizeRS> Anonymize(AnonymizeRQ request, ServerCallContext context)
    {
        _logger.LogInformation("Creating connection with external anonymization service");
        var address =
            $"{_config.GetValue<string>("ConfigSettings:AnonymizerHost")}:{_config.GetValue<string>("ConfigSettings:AnonymizerPort")}";
        var client = new HttpClient();
        client.BaseAddress = new Uri(address);
        // Add an Accept header for JSON format.
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/bson"));

        _logger.LogInformation("Connection successfully acquired, sending request");
        var values = new Dictionary<string, string>
        {
            {"Guid", request.Guid},
            {"Image", request.Image.ToBase64()}
        };

        var content = new FormUrlEncodedContent(values);
        var json = JsonConvert.SerializeObject(values);
        _logger.LogDebug("AnonymizeRQ-JSON: {}", json);
        _logger.LogDebug("AnonymizeRQ-Content: {}", content);
        
        try
        {
            var httpContent = new StringContent(json, Encoding.ASCII, "application/json");
            var response = await client.PostAsync("/anonymize", httpContent);
            var rs = JsonConvert.DeserializeObject<AnonymizeRS>(await response.Content.ReadAsStringAsync());
            _logger.LogInformation("Response obtained successfully");
            _logger.LogDebug("Response: {}", response.ToString());
        
            _logger.LogInformation("Adding embedded response to DB");
            await _imageContext.Add(request.Guid, rs.AnonymizedImage);
            _logger.LogInformation("Embedded response added to DB successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error communicating with anonymizer service: {}", ex);
        }
        
        return null;
    }
}