using ComparisonService.Entities;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace ComparisonService.db.Context;

public class ImageContext : DbContext
{
    public ImageContext(DbContextOptions<ImageContext> options) : base(options) { }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>()
            .Property(p => p.Embeddings)
            .HasColumnType("jsonb");
    }

    public async Task<Image?> Get(string guid)
    {
        return await Images.FindAsync(guid);
    }

    public async Task Add(string guid, double[] embeddings)
    {
        await Images.AddAsync(new Image
        {
            Guid = Guid.Parse(guid),
            Embeddings = embeddings
        });
        await this.SaveChangesAsync();
    }
}