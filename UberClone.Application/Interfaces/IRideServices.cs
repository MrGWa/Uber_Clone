using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberClone.Application.DTOs.Ride;
using UberClone.Domain.Entities;

namespace UberClone.Application.Interfaces;

public interface IRideService
{
    Ride RequestRide(RideRequestDto dto);
    void AcceptRide(RideAcceptedDto dto);
    Ride CompleteRide(RideCompletedDto dto);
}
