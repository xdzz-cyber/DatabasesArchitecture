using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDbForDbArchitecture.Models;

IConfigurationBuilder builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json");
var config = builder.Build();

string? connectionString = config.GetConnectionString("DefaultConnection");
MongoClient client = new MongoClient(connectionString);
IMongoCollection<BsonDocument> supervisors = client.GetDatabase("TEST").GetCollection<BsonDocument>("supervisors");

GetDatabaseNames(client).GetAwaiter();
Console.ReadLine();

GetCollections(client).GetAwaiter();
Console.ReadLine();

var supervisorsList = supervisors.Find(new BsonDocument()).ToList();

SuperVisor sp = BsonSerializer.Deserialize<SuperVisor>(supervisorsList.First());
BsonDocument spBson = sp.ToBsonDocument();

Console.WriteLine(sp.ToJson());
Console.WriteLine(spBson);


static async Task GetDatabaseNames(MongoClient client)
{
    using (var cursor = await client.ListDatabasesAsync())
    {
        var databaseDocuments = await cursor.ToListAsync();
        foreach (var databaseDocument in databaseDocuments)
        {
            Console.WriteLine(databaseDocument["name"]);
        }
    }
}

static async Task GetCollections(MongoClient client)
{
    using (var cursor = await client.ListDatabasesAsync())
    {
        var dbs = await cursor.ToListAsync();
        foreach (var db in dbs)
        {
            Console.WriteLine("DB {0} has next collections:", db["name"]);
            IMongoDatabase database = client.GetDatabase(db["name"].ToString());
            using (var collCursor = await database.ListCollectionsAsync())
            {
                var colls = await collCursor.ToListAsync();
                foreach (var col in colls)
                {
                    Console.WriteLine(col["name"]);
                }
            }
            Console.WriteLine();
        }
    }
}
