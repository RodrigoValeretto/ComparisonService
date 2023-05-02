using AnonymizationService;
using ComparisonService.db.Context;
using ComparisonService.db.Repositories;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;

namespace ComparisonService.Services;

public class AnonymizerService : Anonymizer.AnonymizerBase
{
    private readonly ILogger<AnonymizerService> _logger;
    private readonly ImageContext _imageContext;

    public AnonymizerService(ILogger<AnonymizerService> logger, ImageContext imageContext)
    {
        _logger = logger;
        _imageContext = imageContext;
    }

    public override async Task<AnonymizeRS> Anonymize(AnonymizeRQ request, ServerCallContext context)
    {
        _logger.LogInformation("Creating connection with external anonymization service");
        var channel = GrpcChannel.ForAddress("http://localhost:5098");
        Anonymizer.AnonymizerClient client = new Anonymizer.AnonymizerClient(channel);
        
        _logger.LogInformation("Connection successfully acquired, sending request");
        _logger.LogDebug("AnonymizeRQ: {}", request);

        var response = await client.AnonymizeAsync(request);

        _logger.LogInformation("Response obtained successfully");
        _logger.LogDebug("AnonymizeRS: {}", request);
        
        _logger.LogInformation("Adding embedded response to DB");
        _imageContext.Add(request.GUID, response.AnonymizedImage);
        _logger.LogInformation("Embedded response added to DB successfully");

        return response;
    }
}