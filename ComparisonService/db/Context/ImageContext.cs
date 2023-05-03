using ComparisonService.Entities;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace ComparisonService.db.Context;

public class ImageContext : DbContext
{
    public ImageContext(DbContextOptions<ImageContext> options) : base(options) { }
    public DbSet<Image> Images { get; set; }

    public async Task<Image?> Get(string guid)
    {
        return await Images.FindAsync(guid);
    }

    public async Task Add(string guid, ByteString image)
    {
        await Images.AddAsync(new Image
        {
            Guid = Guid.Parse(guid),
            ImageBytes = image.ToByteArray()
        });
        await this.SaveChangesAsync();
    }
}