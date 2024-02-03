using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRevitFinal.Domain
{
    public class LevelModel
    {
        public LevelModel(string name, Elevation elevation, BasePointType basePointType)
        {
            Name = name;
            Elevation = elevation;
            BasePointType = basePointType;
        }

        public double ElevationInMeters
        {
            get { return Math.Round(Elevation.SimpleValue / 3.28084, 2); } // Assuming Elevation.SimpleValue is in feet
        }
        public string Name { get; }
        public Elevation Elevation { get; }
        public BasePointType BasePointType { get;  }
    }
}
