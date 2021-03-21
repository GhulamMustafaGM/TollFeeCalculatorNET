using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculatorNET
{
    public class Car : Vehicle
    {
        public bool IsTollFreeVehicle()
        {
            return false;
        }
    }
}
