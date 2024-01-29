
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace BookingFlightsPocAPI.Models
{
    public class FlightOffers
    {
        public FlightOffer[] data { get; set; }
    }

    public class FlightOffer
    {
        public Price price { get; set; }
        public Itinerary[] itineraries { get; set; }
    }

    public class Price
    {
        public string currency { get; set; }
        public string total { get; set; }
    }

    public class Itinerary
    {
        public Segment[] segments { get; set; }
        public string duration { get; set; }
    }

    public class Segment
    {
        public string carrierCode { get; set; }
        public string number { get; set; }
        public Airport departure { get; set; }
        public Airport arrival { get; set; }
        public string duration { get; set; }
    }

    public class Airport
    {
        public string iataCode { get; set; }
        public string terminal { get; set; }
        public string at { get; set; }
    }

    public class Flight
    {
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public string FlightNumber { get; set; }
        public string AircraftCode { get; set; }
        public decimal Price { get; set; }

        public bool IsOutbound { get; set; }
    }

    public class CompleteFlight 
    {
        public Flight OutboundFlight { get; set; }
        public Flight InboundFlight { get; set; }
        public string TotalPrice { get; set; }
    }


    
    public class FlightSearchRequest
    {
    
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }

          public int MaxResults { get; set; }
    }

}