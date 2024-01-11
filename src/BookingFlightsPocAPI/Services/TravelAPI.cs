using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace msdemo.flightsearch.Services
{
    public class TravelAPI
    {
        private string apiKey;
        private string apiSecret;
        public string bearerToken;
        private HttpClient http;

        public TravelAPI(IConfiguration config, IHttpClientFactory httpFactory)
        {
            apiKey =  System.Environment.GetEnvironmentVariable("AmadeusAPI_APIKey", EnvironmentVariableTarget.Process);
            apiSecret = System.Environment.GetEnvironmentVariable("AmadeusAPI_APISecret", EnvironmentVariableTarget.Process);
            http = httpFactory.CreateClient("TravelAPI");
        }


        public async Task<List<CompleteFlight>> SearchFlights(string departureLocationCode, string arrivalLocationCode, string returnDate, string departureDate)
        {
            var endpoint = $"v2/shopping/flight-offers?originLocationCode={departureLocationCode}&destinationLocationCode={arrivalLocationCode}&departureDate={departureDate}&returnDate={returnDate}&adults=1&max=5&nonStop=true";
            ConfigBearerTokenHeader();
            var response = await http.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var flightOffers = JsonConvert.DeserializeObject<FlightOffers>(responseContent);

                List<CompleteFlight> flights = new List<CompleteFlight>();

                foreach (var offer in flightOffers.data)
                {
                    CompleteFlight flight = new CompleteFlight{
                        OutboundFlight = new Flight { 
                         AircraftCode = offer.itineraries[0].segments[0].carrierCode ,
                         ArrivalAirport = offer.itineraries[0].segments[0].arrival.iataCode, 
                         ArrivalDate =  offer.itineraries[0].segments[0].arrival.at, 
                         DepartureAirport =offer.itineraries[0].segments[0].departure.iataCode , 
                         DepartureDate = offer.itineraries[0].segments[0].departure.at, 
                         FlightNumber = offer.itineraries[0].segments[0].number, 
                         IsOutbound = true}, 
                        InboundFlight = new Flight { 
                             AircraftCode = offer.itineraries[1].segments[0].carrierCode ,
                         ArrivalAirport = offer.itineraries[1].segments[0].arrival.iataCode, 
                         ArrivalDate =  offer.itineraries[1].segments[0].arrival.at, 
                         DepartureAirport =offer.itineraries[1].segments[0].departure.iataCode , 
                         DepartureDate = offer.itineraries[1].segments[0].departure.at, 
                         FlightNumber = offer.itineraries[1].segments[0].number, 
                         IsOutbound = true
                        }, 
                        TotalPrice = $"{offer.price.total} {offer.price.currency}"
                    };
                    

                    flights.Add(flight);
                }
                return flights;
            }
            else
            {
                throw new Exception("Error calling Travel API error: " + response.ReasonPhrase + " status code: " + response.StatusCode);
            }
        }
    

    public async Task ConnectOAuth()
    {
        var message = new HttpRequestMessage(HttpMethod.Post, "/v1/security/oauth2/token");
        message.Content = new StringContent(
            $"grant_type=client_credentials&client_id={apiKey}&client_secret={apiSecret}",
            Encoding.UTF8, "application/x-www-form-urlencoded"
        );

        var results = await http.SendAsync(message);
        await using var stream = await results.Content.ReadAsStreamAsync();
        var oauthResults = await System.Text.Json.JsonSerializer.DeserializeAsync<OAuthResults>(stream);

        bearerToken = oauthResults.access_token;
    }

    private class OAuthResults
    {
        public string access_token { get; set; }
    }

    private void ConfigBearerTokenHeader()
    {
        http.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
    }

        internal Task SearchFlights(string departureLocation, string departureLocationCode, string arrivalLocation, string arrivalLocationCode, string arrivalDate, string departureDate)
        {
            throw new NotImplementedException();
        }
    }
}