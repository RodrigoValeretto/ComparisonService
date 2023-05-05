using ComparisonService.Entities;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace ComparisonService.db.Context;

public class ImageContext : DbContext
{
    public ImageContext(DbContextOptions<ImageContext> options) : base(options) { }
    public DbSet<Image> images { get; set; }

    public async Task<Image?> Get(string guid)
    {
        return await images.FindAsync(Guid.Parse(guid));
    }

    public async Task Add(string guid, double[] embeddings)
    {
        await images.AddAsync(new Image
        {
            guid = Guid.Parse(guid),
            embeddings = embeddings
        });
        await this.SaveChangesAsync();
    }
}