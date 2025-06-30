using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberClone.Application.DTOs.Ride;

public class RideRequestDto
{
    public string PickupLocation { get; set; } = string.Empty;
    public string DropoffLocation { get; set; } = string.Empty;
    public Guid PassengerId { get; set; }
}
