using AnonymizationService;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;

namespace ComparisonService.Services;

public class AnonymizerService : Anonymizer.AnonymizerBase
{
    private readonly ILogger<AnonymizerService> _logger;

    public AnonymizerService(ILogger<AnonymizerService> logger)
    {
        _logger = logger;
    }

    public override Task<AnonymizeRS> Anonymize(AnonymizeRQ request, ServerCallContext context)
    {
        _logger.LogInformation("Creating connection with external anonymization service");
        var channel = GrpcChannel.ForAddress("http://localhost:5098");
        Anonymizer.AnonymizerClient client = new Anonymizer.AnonymizerClient(channel);
        
        _logger.LogInformation("Connection successfully acquired, sending request");
        _logger.LogDebug("AnonymizeRQ: {}", request);

        AnonymizeRS response = client.Anonymize(request);

        _logger.LogInformation("Response obtained successfully");
        _logger.LogDebug("AnonymizeRS: {}", request);

        return Task.FromResult(response);
    }
}