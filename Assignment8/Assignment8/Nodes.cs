using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment8
{
    class Nodes
    {
        public Vector Coordinate { get; set; }
        public Vector Normal { get; set; }

        public Nodes() { }

        public Nodes(Vector coordinate, Vector normal)
        {
            Coordinate = coordinate;
            Normal = normal;
        }
    }
}
