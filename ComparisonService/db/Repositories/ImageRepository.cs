using Google.Protobuf;
using Npgsql;

namespace ComparisonService.db.Repositories;

public class ImageRepository
{
    private readonly ILogger<ImageRepository> _logger;
    private NpgsqlConnection _connection;
    private IConfiguration _configuration;
    private const string TABLE_NAME = "Images";


    public ImageRepository(ILogger<ImageRepository> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _connection = new NpgsqlConnection(_configuration.GetConnectionString("postgres"));
        _connection.Open();
    }

    public async Task Add(string guid, ByteString image)
    {
        string commandText = $"insert into {TABLE_NAME} (guid, image) values (@guid, @image)";
        
        await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _connection))
        {
            cmd.Parameters.AddWithValue("guid", guid);
            cmd.Parameters.AddWithValue("image", image);

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while adding image to DB: {}", ex);
            }
        }
    }
    
    public async Task<ByteString?> Get(string guid)
    {
        string commandText = $"select * from {TABLE_NAME} where guid = @guid";
        
        await using (var cmd = new NpgsqlCommand(commandText, _connection))
        {
            cmd.Parameters.AddWithValue("guid", guid);

            try
            {
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        ByteString? image = ReadImage(reader);
                        return image;
                    }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while adding image to DB: {}", ex);
            }
        }

        return null;
    }

    private static ByteString? ReadImage(NpgsqlDataReader reader)
    {
        ByteString? image = reader["image"] as ByteString;

        return image;
    }
}