using RideApp.Models;
using System;

namespace RideApp.Services
{
    public static class FareCalculator
    {
        public static double Calculate(Ride ride)
        {
            double baseFare = 2.0;
            double distanceKm = 5.0; // placeholder, replace with real calculation
            double timeMin = (ride.CompletedAt.Value - ride.CreatedAt).TotalMinutes;
            double surge = 1.0;

            return baseFare + (distanceKm * 0.8) + (timeMin * 0.5) * surge;
        }
    }
}
