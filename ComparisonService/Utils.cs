using AnonymizationService;
using Grpc.Core;
using Grpc.Net.Client;

namespace ComparisonService;

public interface Utils
{
    public static async Task<AnonymizeRS?> CallAnonymizerService(ILogger _logger, IConfiguration _config, AnonymizeRQ request)
    {
        _logger.LogInformation("Creating connection with external anonymization service");
        var address =
            $"{_config.GetValue<string>("ConfigSettings:AnonymizerHost")}:{_config.GetValue<string>("ConfigSettings:AnonymizerPort")}";
        var channel = GrpcChannel.ForAddress(address);
        Anonymizer.AnonymizerClient client = new Anonymizer.AnonymizerClient(channel);
        
        _logger.LogInformation("Connection successfully acquired, sending request");
        _logger.LogDebug("AnonymizeRQ: {}", request);

        try
        {
            var response = await client.AnonymizeAsync(request);
            _logger.LogInformation("Response obtained successfully");
            _logger.LogDebug("AnonymizeRS: {}", response);
        
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error communicating with anonymizer service: {}", ex);
            return null;
        }
    }
}