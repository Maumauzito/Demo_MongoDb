using fiap_mongo.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace fiap_mongo.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<User> _user;

        public MongoService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("fiap");
            _user = database.GetCollection<User>("usuario");
        }

        public async Task<List<User>> GetAsync() =>
            await _user.Find(_ => true).ToListAsync();

        public async Task CreateAsync(User user) =>
            await _user.InsertOneAsync(user);

        public async Task<User?> GetByIdAsync(ObjectId id) =>
             await _user.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(ObjectId id, User pessoaAtualizada) =>
            await _user.ReplaceOneAsync(p => p.Id == id, pessoaAtualizada);

        public async Task DeleteAsync(ObjectId id) =>
            await _user.DeleteOneAsync(p => p.Id == id);
    }
}
