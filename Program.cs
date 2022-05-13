using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

IConfigurationBuilder builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json");
var config = builder.Build();

string? connectionString = config.GetConnectionString("DefaultConnection");
MongoClient client = new MongoClient(connectionString);
IMongoDatabase database = client.GetDatabase("deanery");

// GetDatabaseNames(client).GetAwaiter();
// Console.ReadLine();

// GetCollectionsNames(client).GetAwaiter();
// Console.ReadLine();

// Console.WriteLine(new Person("aga").ToJson());
// Console.ReadLine();

// BsonDocument bsonDocument = new BsonDocument
// {
//     {"Name","Aga"}
// };
//
// Person p = BsonSerializer.Deserialize<Person>(bsonDocument);
// Console.WriteLine(p.ToJson());

Person p = new Person("First");
BsonDocument bsonDocument = p.ToBsonDocument();
Console.WriteLine(bsonDocument);

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

static async Task GetCollectionsNames(MongoClient client)
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

class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Person(string name) => (Id, Name) = (Guid.NewGuid(), name);

    // public override string ToString()
    // {
    //     return $"Id-{Id};Name={Name}";
    // }
}