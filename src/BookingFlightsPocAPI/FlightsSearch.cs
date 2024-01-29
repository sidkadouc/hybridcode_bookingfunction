using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using BookingFlightsPocAPI.Services;
using BookingFlightsPocAPI.Models;

namespace BookingFlightsPocAPI.flightsearch
{
    public class FlightsSearch
    {
        private readonly ILogger _logger;
        private readonly TravelAPI _api;

        public FlightsSearch(ILoggerFactory loggerFactory,TravelAPI api)
        {
            _logger = loggerFactory.CreateLogger<FlightsSearch>();
            _api = api;
        }

    [Function("FlightsSearch")]
     [OpenApiOperation(operationId: "FlightSearch")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("application/json", typeof(FlightSearchRequest),
            Description = "JSON request body containing Flight Search Request")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<CompleteFlight>),
            Description = "The OK response message containing a JSON result.")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        FlightSearchRequest flightsearchrequest = JsonConvert.DeserializeObject<FlightSearchRequest>(requestBody);
        await _api.ConnectOAuth();
        var flights = await _api.SearchFlights(flightsearchrequest);
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(flights);
        

        return response;
    }
}  
 
}
