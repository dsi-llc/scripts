using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWStats
{

    public static class Geometry
    {
        public static double CCW(Point3D p1, Point3D p2, Point3D p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
        }

        /// <summary>
        /// Check if the segment (p2, p3) intersects the segment (p0,p1)
        /// The segment (p0,p1) is from left to right looking downstream.
        /// Return +1 if (p2, p3) oriented downstream.
        /// Return -1 if (p2, p3) oriented upstream.
        /// </summary>
        public static int Intersect(Point3D p0, Point3D p1, Point3D p2, Point3D p3)
        {
            double v1 = CCW(p0, p2, p1);
            double v2 = CCW(p0, p1, p3);
            double v3 = CCW(p2, p3, p0);
            double v4 = CCW(p2, p1, p3);
            if (v1 >= 0 && v2 >= 0 && v3 >= 0 && v4 >= 0) { return 1; }
            else if (v1 <= 0 && v2 <= 0 && v3 <= 0 && v4 <= 0) { return -1; }
            return 0;
        }
    }
}
