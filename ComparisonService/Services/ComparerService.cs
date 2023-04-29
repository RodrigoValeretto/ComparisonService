using Grpc.Core;

namespace ComparisonService.Services;

public class ComparerService : Comparer.ComparerBase
{
    private readonly ILogger<ComparerService> _logger;

    public ComparerService(ILogger<ComparerService> logger)
    {
        _logger = logger;
    }

    public override Task<CompareRS> Compare(CompareRQ request, ServerCallContext context)
    {
        return Task.FromResult(new CompareRS
        {
            IsEqual = false
        });
    }
}