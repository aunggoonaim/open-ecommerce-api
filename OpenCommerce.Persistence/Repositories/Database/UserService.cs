using MongoDB.Driver;
using Microsoft.Extensions.Options;
using OpenCommerce.Application.Repositories.Database;
using OpenCommerce.Domain.Entities;
using OpenCommerce.Domain.Setting;

namespace OpenCommerce.Persistence.Repositories.Database;

public class UserService : IUserService
{
    private readonly IMongoCollection<UserModel> _collection;

    public UserService(IOptions<MongoModelDB> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<UserModel>(settings.Value.UserCollectionName);
    }

    public async Task<List<UserModel>> GetAsync(CancellationToken token) =>
        await _collection.Find(_ => true).ToListAsync(token);

    public async Task<UserModel?> GetByIdAsync(string id, CancellationToken token) =>
        await _collection.Find(x => x.id == id).FirstOrDefaultAsync(token);

    public async Task<UserModel?> GetByUsernameAndPasswordAsync(string username, string password, CancellationToken token) =>
        await _collection.Find(x => x.username == username && x.password == password).FirstOrDefaultAsync(token);

    public async Task CreateAsync(UserModel newItem, CancellationToken token) =>
        await _collection.InsertOneAsync(newItem, cancellationToken: token);

    public async Task UpdateAsync(string id, UserModel updateItem, CancellationToken token) =>
        await _collection.ReplaceOneAsync(x => x.id == id, updateItem, cancellationToken: token);

    public async Task RemoveAsync(string id, CancellationToken token) =>
        await _collection.DeleteOneAsync(x => x.id == id, cancellationToken: token);
}
