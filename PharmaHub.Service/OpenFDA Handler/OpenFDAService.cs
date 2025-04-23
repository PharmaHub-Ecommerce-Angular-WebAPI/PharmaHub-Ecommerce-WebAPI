using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PharmaHub.Service.OpenFDA_Handler
{
    public class OpenFDAService
    {
        private readonly HttpClient _client;

        public OpenFDAService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<DrugSuggestion>> SearchDrugOpenFDA(string query)
        {
            string baseUrl = "https://api.fda.gov/drug/label.json?";
            string apiUrl = $"{baseUrl}search=openfda.brand_name:{query}*&limit=5";

            try
            {
                HttpResponseMessage response = await _client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonResponse);

                List<DrugSuggestion> drugList = new();
                HashSet<string> seenNames = new();

                if (jsonDoc.RootElement.TryGetProperty("results", out JsonElement results) &&
                    results.GetArrayLength() > 0)
                {
                    foreach (JsonElement item in results.EnumerateArray())
                    {
                        string brandName = "";
                        if (item.TryGetProperty("openfda", out JsonElement openFda) &&
                            openFda.TryGetProperty("brand_name", out JsonElement brandNamesJson))
                        {
                            if (brandNamesJson.ValueKind == JsonValueKind.Array && brandNamesJson.GetArrayLength() > 0)
                            {
                                brandName = brandNamesJson[0].GetString().ToLower();
                            }
                        }

                        int strengthValue = 0;
                        if (item.TryGetProperty("active_ingredient", out JsonElement activeIngredient))
                        {
                            foreach (JsonElement ingredient in activeIngredient.EnumerateArray())
                            {
                                string ingredientText = ingredient.GetString();
                                string numericValue = Regex.Match(ingredientText, @"\d+").Value;
                                if (int.TryParse(numericValue, out int parsedStrength))
                                {
                                    strengthValue = parsedStrength;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(brandName) && !seenNames.Contains(brandName))
                        {
                            drugList.Add(new DrugSuggestion
                            {
                                Name = brandName,
                                Strength = strengthValue
                            });
                            seenNames.Add(brandName);
                        }
                    }
                }

                return drugList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching OpenFDA data: {ex.Message}");
                return new List<DrugSuggestion>();
            }
        }
    }
}
