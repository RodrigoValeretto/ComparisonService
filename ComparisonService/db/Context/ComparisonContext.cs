using ComparisonService.Entities;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace ComparisonService.db.Context;

public class ComparisonContext : DbContext
{
    public ComparisonContext(DbContextOptions<ComparisonContext> options) : base(options) { }
    public DbSet<Comparison> comparisons { get; set; }

    public async Task<Comparison?> Get(long id)
    {
        return await comparisons.FindAsync(id);
    }

    public async Task Add(double[] embeddings1, double[] embeddings2, bool equals)
    {
        await comparisons.AddAsync(new Comparison()
        {
            equals = equals,
            embeddings1 = embeddings1,
            embeddings2 = embeddings2
        });
        await this.SaveChangesAsync();
    }
}