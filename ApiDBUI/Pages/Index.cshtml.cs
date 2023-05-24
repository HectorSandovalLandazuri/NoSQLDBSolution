using ApiDBUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace ApiDBUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory; 

        public IndexModel(ILogger<IndexModel> logger,IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory=httpClientFactory;
        }

        public async Task OnGet()
        {
            await CreateContact();
            await GetAllContacts();
        }

        private async Task CreateContact()
        {
            ContactModel user = new ContactModel
            {
                FirstName = "Claudia",
                LastName = "Sandoval"
            };
            user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "hola@hola.com" });
            user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "claudia@hector.com" });

            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "5555-555-555" });
            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "1234-333-222" });
            var _client=_httpClientFactory.CreateClient();
            var response = await _client.PostAsync("https://localhost:7102/api/contacts",new StringContent(JsonSerializer.Serialize(user),Encoding.UTF8,"application/json"));


        }

        private async Task GetAllContacts()
        {
            var _client=_httpClientFactory.CreateClient();
            var response = await _client.GetAsync("https://localhost:7102/api/contacts");

            List<ContactModel> contacts;

            if (response.IsSuccessStatusCode) 
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string responseText=await response.Content.ReadAsStringAsync();
                contacts=JsonSerializer.Deserialize<List<ContactModel>>(responseText,options);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

        }
            

    }
}