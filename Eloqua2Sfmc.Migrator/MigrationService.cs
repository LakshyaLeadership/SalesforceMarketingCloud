using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class MigrationService
{
    static async Task Main(string[] args)
    {
        string cmsApiUrl = "https://your-cms-api.com"; // Replace with your CMS API endpoint
        string sfmcApiUrl = "https://your-sfmc-api.com"; // Replace with your SFMC API endpoint
        string sfmcSubscriberListId = "your-subscriber-list-id"; // Replace with your SFMC subscriber list ID
        string sfmcEmailTemplateId = "your-email-template-id"; // Replace with your SFMC email template ID

        // Step 1: Pull daily email content from CMS via API
        string emailContent = await PullEmailContentFromCMS(cmsApiUrl);

        // Step 2: Create email in SFMC via API
        string emailId = await CreateEmailInSFMC(sfmcApiUrl, sfmcEmailTemplateId, emailContent);

        // Step 3: Add subscribers to SFMC list via API
        await AddSubscribersToSFMCList(sfmcApiUrl, sfmcSubscriberListId);

        // Step 4: Schedule email send in SFMC via API
        await ScheduleEmailSendInSFMC(sfmcApiUrl, emailId);

        Console.WriteLine("Email creation and scheduling completed successfully.");
        Console.ReadLine();
    }

    static async Task<string> PullEmailContentFromCMS(string cmsApiUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            // Make API call to your CMS to retrieve email content
            HttpResponseMessage response = await client.GetAsync(cmsApiUrl);
            response.EnsureSuccessStatusCode();

            // Extract the email content from the API response
            string emailContent = await response.Content.ReadAsStringAsync();
            return emailContent;
        }
    }

    static async Task<string> CreateEmailInSFMC(string sfmcApiUrl, string emailTemplateId, string emailContent)
    {
        using (HttpClient client = new HttpClient())
        {
            // Set up SFMC API request headers and authentication
            client.DefaultRequestHeaders.Add("Authorization", "Bearer <your-access-token>"); // Replace with your SFMC access token
            client.DefaultRequestHeaders.Add("Content-Type", "application/json");

            // Define the email payload
            var emailPayload = new
            {
                name = "Daily Email",
                emailTemplate = new
                {
                    id = emailTemplateId
                },
                emailContent = new
                {
                    content = emailContent
                },
                // Add more properties as needed (e.g., images, personalization)
            };

            // Convert the email payload to JSON
            string emailPayloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(emailPayload);

            // Make API call to create email in SFMC
            HttpResponseMessage response = await client.PostAsync(sfmcApiUrl + "/email", new StringContent(emailPayloadJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // Extract the email ID from the API response
            string emailId = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync()).id;
            return emailId;
        }
    }

    static async Task AddSubscribersToSFMCList(string sfmcApiUrl, string subscriberListId)
    {
        using (HttpClient client = new HttpClient())
        {
            // Set up SFMC API request headers and authentication
            client.DefaultRequestHeaders.Add("Authorization", "Bearer <your-access-token>"); // Replace with your SFMC access token
            client.DefaultRequestHeaders.Add("Content-Type=", "application/json");

            // Define the subscriber payload
            var subscriberPayload = new
            {
                contactKey = "subscriber1@example.com",
                emailAddress = "subscriber1@example.com",
                // Add more subscriber properties as needed
            };

            // Convert the subscriber payload to JSON
            string subscriberPayloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(subscriberPayload);

            // Make API call to add subscriber to SFMC list
            HttpResponseMessage response = await client.PostAsync(sfmcApiUrl + $"/lists/{subscriberListId}/subscribers", new StringContent(subscriberPayloadJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }
    }

    static async Task<string> RetrieveAccessToken(string sfmcApiUrl, string clientId, string clientSecret)
    {
        using (HttpClient client = new HttpClient())
        {
            // Set up SFMC token endpoint URL
            string tokenEndpoint = $"{sfmcApiUrl}/v2/token";

            // Set up token request payload
            var tokenRequestPayload = new
            {
                grant_type = "client_credentials",
                client_id = clientId,
                client_secret = clientSecret
            };

            // Convert token request payload to JSON
            string tokenRequestPayloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(tokenRequestPayload);

            // Make token request to retrieve access token
            HttpResponseMessage response = await client.PostAsync(tokenEndpoint, new StringContent(tokenRequestPayloadJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // Extract access token from the API response
            string accessToken = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync()).access_token;
            return accessToken;
        }
    }


    static async Task ScheduleEmailSendInSFMC(string sfmcApiUrl, string emailId)
    {
        using (HttpClient client = new HttpClient())
        {
            // Set up SFMC API request headers and authentication
            client.DefaultRequestHeaders.Add("Authorization", "Bearer <your-access-token>"); // Replace with your SFMC access token
            client.DefaultRequestHeaders.Add("Content-Type", "application/json");

            // Define the send payload
            var sendPayload = new
            {
                email = new
                {
                    id = emailId
                },
                // Add more properties as needed (e.g., schedule, subscribers)
            };

            // Convert the send payload to JSON
            string sendPayloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(sendPayload);

            // Make API call to schedule email send in SFMC
            HttpResponseMessage response = await client.PostAsync(sfmcApiUrl + "/sends", new StringContent(sendPayloadJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }
    }
}
