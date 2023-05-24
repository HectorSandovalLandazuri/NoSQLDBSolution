// See https://aka.ms/new-console-template for more information
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

MongoDBDataAccess db = new MongoDBDataAccess("MongoContactsDB",GetConnectionString());
string tableName = "Contacts";
ContactModel user = new ContactModel
{
    FirstName="Claudia",
    LastName="Sandoval"
};
user.EmailAddresses.Add(new EmailAddressModel { EmailAddress= "hola@hola.com" });
user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "claudia@hector.com" });

user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "5555-555-555" });
user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "1234-333-222" });

//CreateContact(db, tableName,user);
//UpdateContactsFirstName(db, tableName, "Manuel", "3e8b1ebc-28e5-4b49-ba9a-c853098ff3fe");
//RemovePhoneNumberFromUser(db, tableName, "5555-555-555", "3e8b1ebc-28e5-4b49-ba9a-c853098ff3fe");
//RemoveUser(db, tableName, "3e8b1ebc-28e5-4b49-ba9a-c853098ff3fe");
GetAllContacts(db, tableName);
//GetContactById(db, tableName, "3e8b1ebc-28e5-4b49-ba9a-c853098ff3fe");



Console.WriteLine("Done Processing MongoDB");


static void RemoveUser(MongoDBDataAccess db,string tableName,string id)
{
    Guid guid = new Guid(id);
    db.DeleteRecord<ContactModel>(tableName, guid); 
}

static void RemovePhoneNumberFromUser(MongoDBDataAccess db, string tableName, string phoneNumber, string id)
{
    Guid guid = new Guid(id);
    var contact = db.LoadRecordById<ContactModel>(tableName, guid);
    contact.PhoneNumbers = contact.PhoneNumbers.Where(x => x.PhoneNumber != phoneNumber).ToList();
    db.UpsertRecord(tableName, contact.Id, contact);
}

static void UpdateContactsFirstName(MongoDBDataAccess db, string tableName,string firstName,string id)
{
    Guid guid = new Guid(id);
    var contact = db.LoadRecordById<ContactModel>(tableName, guid);
    contact.FirstName = firstName;
    db.UpsertRecord(tableName,contact.Id, contact);


}


static void GetContactById(MongoDBDataAccess db, string tableName,string id)
{
    Guid guid= new Guid(id);
    var contact=db.LoadRecordById<ContactModel>(tableName,guid);
    Console.WriteLine($"Uno solo: {contact.FirstName} {contact.LastName}");
}

static void GetAllContacts(MongoDBDataAccess db, string tableName)
{
    var contacts = db.LoadRecords<ContactModel>(tableName);
    foreach (var contact in contacts)
    {
        Console.WriteLine($"Contact {contact.Id}: {contact.FirstName} {contact.LastName}");
    }
}

static void CreateContact(MongoDBDataAccess db,string tableName , ContactModel contact)
{
    db.UpsertRecord(tableName, contact.Id, contact);
}


static string GetConnectionString(string connectionStringName = "Default")
{
    string output = "";
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");
    var config = builder.Build();
    output = config.GetConnectionString(connectionStringName);
    return output;
}
