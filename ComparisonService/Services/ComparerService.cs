using AnonymizationService;
using ComparisonService.db.Context;
using ComparisonService.Entities;
using Grpc.Core;

namespace ComparisonService.Services;

public class ComparerService : Comparer.ComparerBase
{
    private readonly ILogger<ComparerService> _logger;
    private readonly IConfiguration _config;
    private readonly ImageContext _imageContext;
    private readonly ComparisonContext _comparisonContext;
    private const double THRESHOLD = 0.05;

    public ComparerService(ILogger<ComparerService> logger, IConfiguration config, ImageContext imageContext, ComparisonContext comparisonContext)
    {
        _logger = logger;
        _config = config;
        _imageContext = imageContext;
        _comparisonContext = comparisonContext;
    }

    public override async Task<CompareRS?> Compare(CompareRQ request, ServerCallContext context)
    {
        if (request.Guid == null)
        {
            _logger.LogError("GUID param received in request was null, throwing error");
            throw new Exception("The received GUID was null. Aborting operation.");
        }
        
        _logger.LogInformation("Starting search on DB for received GUID");
        
        Image? dbImage = await _imageContext.Get(request.Guid);
        
        if (dbImage == null)
        {
            _logger.LogError("GUID not found in DB, throwing error");
            throw new Exception("The received GUID was not found in database. Aborting operation.");
        }

        _logger.LogInformation("Embeddings sucessfully retrieved from DB, starting to anonymize request image");
        _logger.LogDebug("Embeddings from DB: {}", dbImage.embeddings);
        
        var anonymizeRQ = new AnonymizeRQ();
        anonymizeRQ.Guid = request.Guid;
        anonymizeRQ.Image = request.Image;

        try
        {
            var anonymizeRS = await Utils.CallAnonymizerService(_logger, _config, anonymizeRQ);
            if (anonymizeRS == null)
            {
                throw new Exception("Anonymizer Service returned null");
            }

            var anonEmbeddings = anonymizeRS.Embeddings.ToArray();
            var dbEmbeddings = dbImage.embeddings;

            double distance = Accord.Math.Distance.Cosine(anonEmbeddings, dbEmbeddings);

            var compareRS = new CompareRS();

            // Distance = 1 - Similarity
            compareRS.IsEqual = (distance < THRESHOLD);

            //Saving comparison in DB
            _logger.LogInformation("Adding comparison to DB");
            await _comparisonContext.Add(dbEmbeddings, anonEmbeddings, compareRS.IsEqual);
            _logger.LogInformation("Comparison successfully added to DB");
            
            return compareRS;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error ocurred while processing image comparison: {}", ex);
            return null;
        }
    }
}