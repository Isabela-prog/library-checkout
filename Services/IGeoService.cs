using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public interface IGeoService
    {
        (double Latitude, double Longitude)? GetCoordinates(string address);
    }
}
