// See https://aka.ms/new-console-template for more information
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        var c = GetCosmosInfo();
        CosmosDBDataAccess db = new CosmosDBDataAccess(c.endpointUrl, c.primaryKey, c.databaseName, c.containerName);
        //        ContactModel user = new ContactModel
        //        {
        //            FirstName = "Claudia",
        //            LastName = "Alvarado"
        //        };
        //        user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "hol@hola.com" });
        //        user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "claia@hector.com" });
        //        user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "3333-555-555" });
        //        user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "1234-555-222" });
        //        await CreateContact(db, user);
        //await UpdateContactsFirstName(db, "Fernando", "a7147476-aeb3-4cdf-8f94-6c12fa1c0ed2");
        //await RemovePhoneNumberFromUser(db, "3333-555-555", "a7147476-aeb3-4cdf-8f94-6c12fa1c0ed2");
        //await RemoveUser(db, "a7147476-aeb3-4cdf-8f94-6c12fa1c0ed2","Alvarado");
        await GetAllContacts(db);
        //await GetContactById(db, "a7147476-aeb3-4cdf-8f94-6c12fa1c0ed2");
        Console.WriteLine("Done Processing CosmosDB");
    }

    static async Task RemoveUser(CosmosDBDataAccess db,  string id,string lastName)
    {
        await db.DeleteRecordAsync<ContactModel>(id,lastName);
    }

    static async Task RemovePhoneNumberFromUser(CosmosDBDataAccess db, string phoneNumber, string id)
    {
        var contact = await db.LoadRecordByIdAsync<ContactModel>(id);
        contact.PhoneNumbers = contact.PhoneNumbers.Where(x => x.PhoneNumber != phoneNumber).ToList();
        await db.UpsertRecordAsync(contact);
    }

    static async Task UpdateContactsFirstName(CosmosDBDataAccess db, string firstName, string id)
    {
        var contact =await db.LoadRecordByIdAsync<ContactModel>(id);
        contact.FirstName = firstName;
        await db.UpsertRecordAsync(contact);


    }


    static async Task GetContactById(CosmosDBDataAccess db, string id)
    {
        var contact = await db.LoadRecordByIdAsync<ContactModel>(id);
        Console.WriteLine($"Uno solo: {contact.FirstName} {contact.LastName}");
    }

    static async Task GetAllContacts(CosmosDBDataAccess db)
    {
        var contacts = await db.LoadRecordsAsync<ContactModel>();
        foreach (var contact in contacts)
        {
            Console.WriteLine($"Contact {contact.Id}: {contact.FirstName} {contact.LastName}");
        }
    }

    static (string endpointUrl,string  primaryKey,string databaseName,string containerName) GetCosmosInfo()
    {
        (string endpointUrl, string primaryKey, string databaseName, string containerName) output;
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        var config = builder.Build();
        output.endpointUrl = config.GetValue<string>("CosmosDB:EndpointUrl");
        output.primaryKey = config.GetValue<string>("CosmosDB:PrimaryKey");
        output.databaseName = config.GetValue<string>("CosmosDB:DatabaseName");
        output.containerName = config.GetValue<string>("CosmosDB:ContainerName");
        return output;
    }

    static async Task CreateContact(CosmosDBDataAccess db, ContactModel contact)
    {
        await db.UpsertRecordAsync(contact);
    }
}

