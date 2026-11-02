using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public record ProductResponse(
    int Id,
    string Name,
    decimal Price);
}