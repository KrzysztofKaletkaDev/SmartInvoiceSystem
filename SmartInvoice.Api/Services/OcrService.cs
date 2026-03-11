using System.Text.Json;
using OllamaSharp;
using OllamaSharp.Models;

namespace SmartInvoice.Api.Services;

public class OcrService
{
    private readonly OllamaApiClient _client;

    public OcrService()
    {
        _client = new OllamaApiClient(new Uri("http://localhost:11434"));
        _client.SelectedModel = "llama3.2-vision";
    }

    public async Task<InvoiceAiResult?> AnalyzeInvoiceAsync(byte[] imageBytes)
    {
        var prompt = @"You are an expert accountant extracting data from a Polish invoice. 
                    Carefully analyze the image and find the final TOTAL amounts.
                    - 'NetAmount' is the total net amount (Razem Netto).
                    - 'GrossAmount' is the total gross amount (Razem Brutto). 
                    DO NOT use the 'Already paid' (Zapłacono) or 'Left to pay' (Pozostało do zapłaty) amounts for the Net/Gross fields!
                    Return ONLY a JSON object with fields: ""Number"", ""Contractor"", ""NetAmount"", ""GrossAmount"", ""Date"" (yyyy-mm-dd).";

        var base64Image = Convert.ToBase64String(imageBytes);
        string fullResponse = "";

        var request = new GenerateRequest
        {
            Prompt = prompt,
            Images = new[] { base64Image }
        };

        await foreach (var res in _client.GenerateAsync(request))
        {
            fullResponse += res.Response;
        }

        try
        {
            int startIndex = fullResponse.IndexOf('{');
            int endIndex = fullResponse.LastIndexOf('}');

            if (startIndex >= 0 && endIndex > startIndex)
            {
                var cleanJson = fullResponse.Substring(startIndex, endIndex - startIndex + 1);

                return JsonSerializer.Deserialize<InvoiceAiResult>(
                    cleanJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }

            Console.WriteLine($"Nie znaleziono struktury JSON w odpowiedzi. Surowy tekst: {fullResponse}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd parsowania AI: {ex.Message}. Surowy tekst: {fullResponse}");
            return null;
        }
    }
}

public record InvoiceAiResult(string Number, string Contractor, decimal NetAmount, decimal GrossAmount, string Date);