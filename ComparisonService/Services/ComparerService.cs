using ComparisonService.db.Repositories;
using Google.Protobuf;
using Grpc.Core;

namespace ComparisonService.Services;

public class ComparerService : Comparer.ComparerBase
{
    private readonly ILogger<ComparerService> _logger;
    private ImageRepository _imageRepository;

    public ComparerService(ILogger<ComparerService> logger, ImageRepository imageRepository)
    {
        _logger = logger;
        _imageRepository = imageRepository;
    }

    public override async Task<CompareRS> Compare(CompareRQ request, ServerCallContext context)
    {
        ByteString? dbImage = await _imageRepository.Get(request.GUID);
        ByteString rqImage = request.Image;

        if (dbImage == null)
        {
            throw new Exception("The received GUID was not found in database. Aborting operation.");
        }
        
        

        return new CompareRS();
    }
}