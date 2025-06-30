using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberClone.Application.DTOs.Ride;

public class RideAcceptedDto
{
    public int RideId { get; set; }
    public Guid DriverId { get; set; }
}
