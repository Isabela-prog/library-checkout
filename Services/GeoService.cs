using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Library.Services
{
    public class GeoService : IGeoService
    {
        private readonly HttpClient _httpClient;

        public GeoService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "LibraryApp/1.0");
        }

        public (double Latitude, double Longitude)? GetCoordinates(string address)
        {
            var encodedAddress = HttpUtility.UrlEncode(address);
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={encodedAddress}";

            var response = _httpClient.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode) return null;

            var json = response.Content.ReadAsStringAsync().Result;
            var results = JsonSerializer.Deserialize<List<NominatimResult>>(json);

            if (results == null || results.Count == 0) return null;

            var first = results[0];
            if (double.TryParse(first.Lat, out var lat) && double.TryParse(first.Lon, out var lon))
                return (lat, lon);

            return null;
        }

        private class NominatimResult
        {
            [JsonPropertyName("lat")]
            public string Lat { get; set; } = "";

            [JsonPropertyName("lon")]
            public string Lon { get; set; } = "";
        }
    }
}