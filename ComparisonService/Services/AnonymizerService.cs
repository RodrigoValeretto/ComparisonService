using AnonymizationService;
using ComparisonService.db.Context;
using Grpc.Core;
using Grpc.Net.Client;

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
        var channel = GrpcChannel.ForAddress(address);
        Anonymizer.AnonymizerClient client = new Anonymizer.AnonymizerClient(channel);
        
        _logger.LogInformation("Connection successfully acquired, sending request");
        _logger.LogDebug("AnonymizeRQ: {}", request);

        AnonymizeRS response = new AnonymizeRS();
        
        try
        {
            response = await client.AnonymizeAsync(request);
            _logger.LogInformation("Response obtained successfully");
            _logger.LogDebug("AnonymizeRS: {}", request);
        
            _logger.LogInformation("Adding embedded response to DB");
            await _imageContext.Add(request.Guid, response.Embeddings.ToArray());
            _logger.LogInformation("Embedded response added to DB successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error communicating with anonymizer service: {}", ex);
        }
        
        return response;
    }
}