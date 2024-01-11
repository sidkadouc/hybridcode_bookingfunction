using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;  
using System.Collections.Generic;  
using System.Net.Http;  
using System.Threading.Tasks;  
using Newtonsoft.Json;
using msdemo.flightsearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace msdemo.flightsearch
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
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);

        string departureLocationCode = data.departure_location_code;
        string arrivalLocationCode = data.arrival_location_code;
        string returnDate = data.return_date;
        string departureDate = data.departure_date;

        await _api.ConnectOAuth();
        _logger.LogInformation(_api.bearerToken);
        var flights = await _api.SearchFlights(departureLocationCode, arrivalLocationCode, returnDate, departureDate);

        return new OkObjectResult(flights);
    }
}  
 
}
