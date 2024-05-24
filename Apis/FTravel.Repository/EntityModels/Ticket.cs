using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Ticket : BaseEntity
{
    public int? TripId { get; set; }

    public int? TicketTypeId { get; set; }

    public string? SeatCode { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ServiceTicket> ServiceTickets { get; set; } = new List<ServiceTicket>();

    public virtual TicketType? TicketType { get; set; }

    public virtual Trip? Trip { get; set; }
}
