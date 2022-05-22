using DatabaseArchitectureTenthLab.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;


BsonClassMap.RegisterClassMap<SuperVisor>(x =>
{
    x.AutoMap(); 
    x.MapMember(y => y.Name).SetElementName("name"); 
});


var superVisor = new SuperVisor()
{
    Id = Guid.NewGuid(),
    Age = 12,
    Department = "Program Systems and Technologies",
    Name = "Andrew Hoku",
    LeadingSubject = "Math",
    MiddleName = "Jojef"
};

Console.WriteLine(superVisor.ToBsonDocument());

var superVisorSecond = new SuperVisor()
{
    Id = Guid.NewGuid(),
    Age = 13,
    Department = "Some department",
    Name = "John Levit",
    LeadingSubject = "Chemistry",
    MiddleName = "Kayet"
};

Console.WriteLine(superVisorSecond.ToJson());


var superVisorThird = new SuperVisor()
{
    Id = Guid.NewGuid(),
    Age = 23,
    Department = "ThirdDepartment",
    Name = "John Malkovic",
    LeadingSubject = "English",
    MiddleName = "Fayout"
};

Console.WriteLine(superVisorThird.ToBsonDocument());

var conventionPack = new ConventionPack();
conventionPack.Add(new CamelCaseElementNameConvention());
ConventionRegistry.Register("camelCase", conventionPack, t => true);

var superVisorFourth = new SuperVisor()
{
    Id = Guid.NewGuid(),
    Age = 23,
    Department = "FourhtDepartment",
    Name = "Jazzy Jazzy",
    LeadingSubject = "Biology",
    MiddleName = "Faust"
};

Console.WriteLine(superVisorFourth.ToBsonDocument());