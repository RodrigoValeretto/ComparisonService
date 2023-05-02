using ComparisonService.db.Context;
using ComparisonService.db.Repositories;
using ComparisonService.Entities;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace ComparisonService.Services;

public class ComparerService : Comparer.ComparerBase
{
    private readonly ILogger<ComparerService> _logger;
    private readonly ImageContext _imageContext;

    public ComparerService(ILogger<ComparerService> logger, ImageContext imageContext)
    {
        _logger = logger;
        _imageContext = imageContext;
    }

    public override async Task<CompareRS> Compare(CompareRQ request, ServerCallContext context)
    {
        if (request.GUID == null)
        {
            throw new Exception("The received GUID was null. Aborting operation.");
        }
        Image? dbImage = await _imageContext.Get(request.GUID);
        ByteString rqImage = request.Image;

        if (dbImage == null)
        {
            throw new Exception("The received GUID was not found in database. Aborting operation.");
        }

        return new CompareRS();
    }
}