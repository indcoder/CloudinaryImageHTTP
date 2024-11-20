using System.Net.Http;
using System.Threading.Tasks;

public class CloudinaryImageUploadService : IImageUploadService
{
    private readonly HttpClient _httpClient;

    public CloudinaryImageUploadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string cloudname, string apikey, string signature, string timestamp)
    {
        var url = $"https://api.cloudinary.com/v1_1/{cloudname}/image/upload";

        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(imageStream), "file", fileName);
        content.Add(new StringContent(apikey), "api_key");
        content.Add(new StringContent(signature), "signature");
        content.Add(new StringContent(timestamp), "timestamp");

        var response = await _httpClient.PostAsync(url, content);
        Console.WriteLine($"Response: {response}");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        // Assuming the response contains a JSON object with a "secure_url" field
        var jsonResponse = System.Text.Json.JsonDocument.Parse(responseContent);
        return jsonResponse.RootElement.GetProperty("secure_url").GetString();
    }
}
