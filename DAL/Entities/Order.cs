using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities;

public sealed record Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; }
    public string SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
