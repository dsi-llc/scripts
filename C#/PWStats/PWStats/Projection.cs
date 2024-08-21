using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWStats
{
    #region Ellipsoid
    public class Ellipsoid
    {
        protected string m_id = "6326";
        protected string m_name = "WGS_1984";                 // default World Geodetic System 1984 (WGS 84)
        protected double m_a = 6378137;                     // Semi-major axis [m]
        protected double m_rf = 298.257223563;              // Inverse flattening, reciprocal flattening
        protected double m_f = 0.003352810664747481;        // Flattening
        protected double m_e2 = 0.006694379990141317;       // First eccentricity squared 
        protected double[] m_c = new double[5];
        public string Name { get { return m_name; } set { m_name = value; } }
        public double SemiMajorAxis { get { return m_a; } set { m_a = value; } }
        public double SemiMinorAxis { get { return m_a * (1 - 1 / this.m_rf); } set { m_f = 1 - value / m_a; m_e2 = (2 - m_f) * m_f; m_rf = 1 / m_f; Coefficients(); } }
        public double InverseFlattening { get { return m_rf; } set { m_rf = value; m_f = 1.0 / m_rf; m_e2 = (2 - m_f) * m_f; Coefficients(); } }
        public double FirstEccentricity { get { return System.Math.Sqrt(m_e2); } }                                             // e² = (a² - b²)/a²
        public double SecondEccentricity { get { return System.Math.Sqrt(m_e2 / (1 - m_e2)); } }                                 // e² = (a² - b²)/b²
        public double Flattening { get { return m_f; } set { m_f = value; m_e2 = (2 - m_f) * m_f; m_rf = 1 / m_f; Coefficients(); } }   // f = (a - b)/a
        public double e2 { get { return m_e2; } }

        public Ellipsoid() { Coefficients(); }

        protected void Coefficients()
        {
            double e2 = this.m_e2;
            m_c[0] = 1.0 - (((175.0 / 16384.0 * e2 + 5.0 / 256.0) * e2 + 3.0 / 64.0) * e2 + 1.0 / 4.0) * e2;    //e0fn
            m_c[1] = 3.0 / 8.0 * (((-455.0 / 4096.0 * e2 + 15.0 / 128.0) * e2 + 1.0 / 4.0) * e2 + 1.0) * e2;    //e1fn
            m_c[2] = 15.0 / 256.0 * ((-77.0 / 128.0 * e2 + 3.0 / 4.0) * e2 + 1.0) * e2 * e2;                    //e2fn
            m_c[3] = 35.0 / 3072.0 * (-41.0 / 32.0 * e2 + 1.0) * e2 * e2 * e2;                                  //e3fn
            m_c[4] = -315.0 / 131072.0 * e2 * e2 * e2 * e2;
        }

        // Meridian arc from the Equator to latitude ϕ
        public double S_phi(double B)
        {
            return this.m_a * (m_c[0] * B
                            - m_c[1] * System.Math.Sin(2 * B)
                            + m_c[2] * System.Math.Sin(4 * B)
                            - m_c[3] * System.Math.Sin(6 * B)
                            + m_c[4] * System.Math.Sin(8 * B));    //mlfn
        }
        public double S_phi_derivative(double B)
        {
            return this.m_a * (m_c[0]
                            - 2 * m_c[1] * System.Math.Cos(2 * B)
                            + 4 * m_c[2] * System.Math.Cos(4 * B)
                            - 6 * m_c[3] * System.Math.Cos(6 * B)
                            + 8 * m_c[4] * System.Math.Cos(8 * B));
        }

        /// <summary>
        /// Convert from  geodetic coordinates (lon,lat,height) to geocentric coordinates
        /// 2016-07-04, N.T.Lam
        /// </summary>
        /// <param name="lon">longitude</param>
        /// <param name="lat">latitude</param>
        /// <param name="h">height</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void BLH2XYZ(double lon, double lat, double h, out double x, out double y, out double z)   // delme - what does "BLH" stand for?
        {
            double e2 = m_e2;
            double a2 = m_a * m_a;

            double deg2rad = System.Math.PI / 180;
            double B = lat * deg2rad;
            double L = lon * deg2rad;
            double sinB = System.Math.Sin(B);
            double cosB = System.Math.Cos(B);
            double sinL = System.Math.Sin(L);
            double cosL = System.Math.Cos(L);
            double N = m_a / System.Math.Sqrt(1 - e2 * sinB * sinB);   // The radius of curvature normal to the meridian
            x = (N + h) * cosB * cosL;
            y = (N + h) * cosB * sinL;
            z = ((1 - e2) * N + h) * sinB;
        }
        public void XYZ2BLH(double x, double y, double z, out double lon, out double lat, out double h)   // delme - what does "BLH" stand for?  Code needs commenting.
        {
            double a2 = m_a * m_a;
            double e2 = m_e2;                       // e² = (a² - b²)/a²
            double ep2 = e2 / (1 - e2);             // e'² = (a² - b²)/b²
            double b2 = a2 * (1 - e2);
            double b = m_a * (1 - 1 / this.m_rf);
            double p = System.Math.Sqrt(x * x + y * y);
            double t = System.Math.Atan2(m_a * z, b * p);
            double sint = System.Math.Sin(t);
            double cost = System.Math.Cos(t);
            double B = System.Math.Atan2(z + ep2 * b * System.Math.Pow(sint, 3), p - e2 * m_a * System.Math.Pow(cost, 3));
            double sinB = System.Math.Sin(B);
            double cosB = System.Math.Cos(B);
            double N = a2 / System.Math.Sqrt(a2 * cosB * cosB + b2 * sinB * sinB);
            h = p / cosB - N;
            double L = System.Math.Atan2(y, x);
            while (L < -180) { L += 2 * System.Math.PI; }
            while (L > 180) { L -= 2 * System.Math.PI; }
            double rad2deg = 180 / System.Math.PI;
            lat = B * rad2deg;
            lon = L * rad2deg;
        }
        public string ToWKT()
        {
            return String.Format("SPHEROID[\"{0}\",{1},{2},AUTHORITY[\"EPSG\",{3}]]",
                this.m_name, this.m_a, this.m_rf, this.m_id);
        }
    }
    #endregion

    #region CoordSys
    public class CoordSys
    {
        protected string m_id = "4326";                // Ellipsoid ID
        protected string m_name = "WGS 84";                 // Ellipsoid Name
        protected Ellipsoid m_ellipsoid;
        protected double[] m_shift = { 0, 0, 0 };           // ΔX, ΔY, ΔZ
        protected double[] m_rotation = { 0, 0, 0 };        // ω, ψ, ε
        protected double m_scale = 0;                       // ΔS
        protected bool m_wgs84 = true;
        public string Name { get { return m_name; } set { m_name = value; } }
        public Ellipsoid Ellipsoid { get { return m_ellipsoid; } set { m_ellipsoid = value; } }
        public bool IsWGS84 { get { return m_wgs84; } }

        public CoordSys() { m_ellipsoid = new Ellipsoid(); }
        public CoordSys(Ellipsoid ellipsoid) { m_ellipsoid = ellipsoid; }
        public CoordSys(string name, double dx, double dy, double dz, double rx, double ry, double rz, double k)
        {
            m_name = name;
            m_shift[0] = dx;
            m_shift[1] = dy;
            m_shift[2] = dz;
            m_rotation[0] = rx;
            m_rotation[1] = ry;
            m_rotation[2] = rz;
            m_scale = k;
        }

        public void ToWGS84(double x, double y, double z, out double X, out double Y, out double Z)
        {
            double sec2rad = System.Math.PI / 180 / 3600;
            double Tx = m_shift[0];
            double Ty = m_shift[1];
            double Tz = m_shift[2];
            double Rx = m_rotation[0] * sec2rad;
            double Ry = m_rotation[1] * sec2rad;
            double Rz = m_rotation[2] * sec2rad;
            double k = 1 + m_scale * 1e-6;
            double[] M = { Tx, Ty, Tz, Rx, Ry, Rz, k };
            Helmert(M, x, y, z, out X, out Y, out Z);
        }
        public void FromWGS84(double X, double Y, double Z, out double x, out double y, out double z)
        {
            double sec2rad = System.Math.PI / 180 / 3600;
            double Tx = -m_shift[0];
            double Ty = -m_shift[1];
            double Tz = -m_shift[2];
            double Rx = -m_rotation[0] * sec2rad;
            double Ry = -m_rotation[1] * sec2rad;
            double Rz = -m_rotation[2] * sec2rad;
            double k = 1 - m_scale * 1e-6;
            double[] M = { Tx, Ty, Tz, Rx, Ry, Rz, k };
            Helmert(M, X, Y, Z, out x, out y, out z);
        }
        public void Helmert(double[] M, double x, double y, double z, out double X, out double Y, out double Z)
        {
            X = M[6] * (x + M[5] * y - M[4] * z) + M[0];
            Y = M[6] * (-M[5] * x + y + M[3] * z) + M[1];
            Z = M[6] * (M[4] * x - M[3] * y + z) + M[2];
        }

        public virtual string MiCoordSys(double meridian, double scale, double x0, double y0)
        {
            return String.Format("CoordSys Earth Projection 8, 104, \"m\", {0}, 0, {1}, {2}, {3} ", meridian, scale, x0, y0);
        }
        public string ToWKT()
        {
            return String.Format("GEOGCS[\"GCS_{0}\",DATUM[\"D_{0}\",{1}],PRIMEM[\"Greenwich\",{2}],UNIT[\"Degree\", {3}],AUTHORITY[\"EPSG\",{4}]]",
                this.m_name, this.m_ellipsoid.ToWKT(), 0.0, System.Math.PI / 180, m_id);
        }
        public override string ToString() { return this.m_name; }
    }
    #endregion

    #region Projection
    public class Projection
    {
        protected string m_id = "4326";
        protected string m_name = "UTM";
        protected CoordSys m_coordsys = null;
        protected double m_scale = 0.9996;
        protected double m_x0 = 500e3;
        protected double m_y0 = 0;
        protected double m_central = 105;
        protected int m_zone;

        public Projection() { m_coordsys = new CoordSys(); }              // Default is WGS84
        public Projection(CoordSys datum) { m_coordsys = datum; }
        public Projection(Projection prj)
        {
            m_id = prj.m_id;
            m_name = prj.m_name;
            m_scale = prj.ScaleFactor;
            m_x0 = prj.FalseEasting;
            m_y0 = prj.FalseNorthing;
            m_central = prj.Meridian;
            m_zone = prj.Zone;
            m_coordsys = prj.CoordSys;
        }

        public Ellipsoid Ellipsoid
        {
            get { return m_coordsys != null ? m_coordsys.Ellipsoid : null; }
            set { if (m_coordsys != null) { m_coordsys.Ellipsoid = value; } }
        }
        public CoordSys CoordSys { get { return m_coordsys; } set { m_coordsys = value; } }
        public bool IsWGS84 { get { return m_coordsys.IsWGS84;/* m_datum is Datum;*/ } }
        public int Zone
        {
            get { return m_zone; }
            set
            {
                m_zone = value;
                m_central = CentralMeridian(m_zone);
                m_y0 = m_zone > 0 ? 0 : 10e6;
                m_name = String.Format("UTM Zone {0}", m_zone);
            }
        }
        public double Meridian { get { return m_central; } set { m_central = value; m_zone = UtmZone(m_central); } }
        public double ScaleFactor { get { return m_scale; } set { m_scale = value; } }
        public double FalseEasting { get { return m_x0; } set { m_x0 = value; } }
        public double FalseNorthing { get { return m_y0; } set { m_y0 = value; } }

        public static int UtmZone(double longitude, double latitude = 0)
        {
            int zone = (int)System.Math.Floor(longitude / 6) + (longitude < 180 ? +31 : -29);

            // Special Cases for Norway & Svalbard
            if ((latitude > 55) && (zone == 31) && (latitude < 64) && (longitude > 2)) { zone = 32; }
            if ((latitude > 71) && (zone == 32) && (longitude < 9)) { zone = 31; }
            if ((latitude > 71) && (zone == 32) && (longitude > 8)) { zone = 33; }
            if ((latitude > 71) && (zone == 34) && (longitude < 21)) { zone = 33; }
            if ((latitude > 71) && (zone == 34) && (longitude > 20)) { zone = 35; }
            if ((latitude > 71) && (zone == 36) && (longitude < 33)) { zone = 35; }
            if ((latitude > 71) && (zone == 36) && (longitude > 32)) { zone = 37; }
            if (latitude < 0) zone = -zone;
            return zone;
        }
        public static double CentralMeridian(int zone)
        {
            #region NDL 2019-06-03 
            double ICM = 0;
            zone = System.Math.Abs(zone);
            if (zone < 31)         //  FIND THE  ZONE FOR LONGITUDE LESS  THAN 0 DEGREES (West)
            { ICM = -(183 - (6 * zone)); }
            else                  //  FIND THE ZONE FOR LONGITUDE GREATER THAN 0 DEGREES (East)
            { ICM = (3 + (6 * (zone - 31))); }
            return ICM;
            #endregion
            //return 6 * zone - 183;

            //return 6 * (Math.Abs(zone) - 1) + 3 - 180; 
            //double z = Math.Abs(zone);
            //return 6 * z + (z >= 31 ? -183 : +177);
        }
        public static double CentralMeridian(double lon)
        {
            double meridian = 0;
            int zone = UtmZone(lon);
            if (lon > 1000 || lon < -1000) lon = 0;
            if (lon > 180) lon -= 360;
            if (lon < 0)
            {
                //Middle Longitude
                meridian = -(183 - (6 * zone));
            }
            else
            {
                meridian = (3 + (6 * (zone - 31)));
            }
            return meridian;
            //return CentralMeridian(zone);
        }

        // Convert geographic coordinates (lat/lon) to UTM coordinates
        public void Geo_2_UTM(double lon, double lat, out double x, out double y)
        {
            double a = this.m_coordsys.Ellipsoid.SemiMajorAxis;
            double e2 = this.m_coordsys.Ellipsoid.e2;

            double deg2rad = System.Math.PI / 180;
            double B = lat * deg2rad;
            double L = lon * deg2rad;
            double lambda = L - m_central * deg2rad;
            double sinB = System.Math.Sin(B);
            double cosB = System.Math.Cos(B);
            double S = this.m_coordsys.Ellipsoid.S_phi(B);
            double N = a / System.Math.Sqrt(1 - e2 * sinB * sinB);    // Radius of curvature
            double ep = System.Math.Sqrt(e2 / (1 - e2));
            double n = ep * cosB;
            double t = sinB / cosB;
            double t2 = t * t;
            double n2 = n * n;
            double Lc = lambda * cosB;
            double Lc2 = Lc * Lc;
            x = N * (((5 - 18 * t2 + t2 * t2 + 14 * n2 - 58 * t2 * n2) / 120 * Lc2
                + (1 - t2 + n2) / 6) * Lc2 + 1) * Lc;
            y = S + (N * lambda / 2 * sinB) * (((61 - 58 * t2 + t2 * t2 + 270 * n2 - 330 * t2 * n2) / 360 * Lc2
                + (5 - t2 + 9 * n2 + 4 * n2 * n2) / 12) * Lc2 + 1) * Lc;
            x = m_scale * x + m_x0;
            y = m_scale * y + m_y0;
        }

        // Convert UTM coordinates to lat/lon
        public void UTM_2_Geo(double x, double y, out double lon, out double lat)
        {
            double a = this.m_coordsys.Ellipsoid.SemiMajorAxis;
            double e2 = this.m_coordsys.Ellipsoid.e2;

            x = (x - m_x0) / m_scale;
            y = (y - m_y0) / m_scale;

            double rad2deg = 180 / System.Math.PI;
            double ep2 = e2 / (1 - e2);

            double B = y / a;
            double dB, S, Sp;
            for (int i = 0; i < 30; i++)
            {
                S = this.m_coordsys.Ellipsoid.S_phi(B);
                Sp = this.m_coordsys.Ellipsoid.S_phi_derivative(B);
                dB = -(S - y) / Sp;
                B += dB;
                if (System.Math.Abs(dB) < 1.0e-12) { break; }
            }
            double sinB = System.Math.Sin(B);
            double cosB = System.Math.Cos(B);
            double t = sinB / cosB;
            double t2 = t * t;
            double N = a / System.Math.Sqrt(1 - e2 * sinB * sinB);
            double R = a * (1 - e2) * System.Math.Pow(1 - e2 * sinB * sinB, -1.5);
            double n2 = ep2 * cosB * cosB;
            double B3 = 1 + 2 * t2 + n2;
            double B4 = 5 + 3 * t2 + n2 - 4 * n2 * n2 - 9 * t2 * n2;
            double B5 = 5 + 28 * t2 + 24 * t2 * t2 + 6 * n2 + 8 * t2 * n2;
            double B6 = 61 + 90 * t2 + 46 * n2 + 45 * t2 * t2 - 252 * t2 * n2 - 3 * n2 * n2
                        - 66 * t2 * n2 * n2 - 90 * t2 * t2 * n2 + 255 * t2 * t2 * n2 * n2;
            double xn = (x / N);
            double xn2 = xn * xn;
            B = B - 0.5 * t * N / R * (((B6 / 360 * xn2 - B4 / 12) * xn2 + 1) * xn2);
            double L = (((B5 / 120 * xn2 - B3 / 6) * xn2 + 1) * xn) / cosB;
            lat = B * rad2deg;
            lon = m_central + L * rad2deg;
            //MessageBox.Show(String.Format("a: {0}, e2: {1}, scale: {2}, x0: {3}, y0: {4}\n" + "Local: {5:F9}, {6:F9}\n",
            //    a, e2, m_scale, m_x0, m_y0, lon, lat));
        }

        public void BLH2WGS84(double lon, double lat, double h, out double X, out double Y, out double Z)
        {
            double Xg, Yg, Zg;
            m_coordsys.Ellipsoid.BLH2XYZ(lon, lat, h, out Xg, out Yg, out Zg);
            m_coordsys.ToWGS84(Xg, Yg, Zg, out X, out Y, out Z);
        }
        public void XYZ2WGS84(double x, double y, double z, out double X, out double Y, out double Z)   // delme - needs commenting.  XYZ can be WGS84 or something else.
                                                                                                        // delme - what coordinate system is XYZ supposed to be in?
        {
            double lon, lat, h = z, Xg, Yg, Zg;
            this.UTM_2_Geo(x, y, out lon, out lat);
            m_coordsys.Ellipsoid.BLH2XYZ(lon, lat, h, out Xg, out Yg, out Zg);
            m_coordsys.ToWGS84(Xg, Yg, Zg, out X, out Y, out Z);
            //MessageBox.Show(String.Format("Map: {0:F4}, {1:F4}, {2:F4}\n" + "Local: {3:F6}, {4:F6}\n" +
            //    "Geocentric: {5:F4}, {6:F4}, {7:F4}\n" + "WGS 84: {8:F4}, {9:F4}, {10:F4}\n", 
            //    x,y,z,lon,lat,Xg,Yg,Zg,X,Y,Z));
        }
        public void WGS842BLH(double X, double Y, double Z, out double lon, out double lat, out double h)
        {
            double Xg, Yg, Zg;
            m_coordsys.FromWGS84(X, Y, Z, out Xg, out Yg, out Zg);
            m_coordsys.Ellipsoid.XYZ2BLH(Xg, Yg, Zg, out lon, out lat, out h);
        }
        public void WGS842XYZ(double X, double Y, double Z, out double x, out double y, out double z)
        {
            double lon, lat, h, Xg, Yg, Zg;
            m_coordsys.FromWGS84(X, Y, Z, out Xg, out Yg, out Zg);
            m_coordsys.Ellipsoid.XYZ2BLH(Xg, Yg, Zg, out lon, out lat, out h);
            this.Geo_2_UTM(lon, lat, out x, out y);
            z = h;
            //MessageBox.Show(String.Format("WGS 84: {0:F4}, {1:F4}, {2:F4}\n" + "Geocentric: {3:F4}, {4:F4}, {5:F4}\n" +
            //     "Local: {6:F6}, {7:F6}\n" + "Map: {8:F4}, {9:F4}, {10:F4}\n",
            //    X, Y, Z, Xg, Yg, Zg, lon, lat, x, y, z));
        }

        public string MiCoordSys()
        {
            return m_coordsys.MiCoordSys(m_central, m_scale, m_x0, m_y0);
        }
        public override string ToString()
        {
            return this.m_name;
        }

        public string ToWKT()
        {
            m_name = String.Format("UTM Zone {0}", m_zone);
            /*Projection for UTM coordinates
            return String.Format("PROJCS[\"{0}\",{1},PROJECTION[\"Transverse_Mercator\"]," +
                "PARAMETER[\"Central_Meridian\",{2:F2}],PARAMETER[\"scale_factor\",{3:F4}]," +
                "PARAMETER[\"False_Easting\",{4:F2}],PARAMETER[\"False_Northing\",{5:F2}]," +
                "PARAMETER[\"Latitude_Of_Origin\",{6:F2}]," +
                //"PARAMETER[\"Standard_Parallel_1\",{8}],PARAMETER[\"Standard_Parallel_2\",{9}],"+
                "UNIT[\"Meter\",{7:F3}],AUTHORITY[\"EPSG\",{8}]]",
                this.m_name, m_coordsys.ToWKT(), m_central, m_scale, m_x0, m_y0, 0, 1.0, m_id);
            */
            //Projection for Lat/lon coordinates
            return m_coordsys.ToWKT();
        }

        public static void WriteProjectionFile(string fileName, int utmzone)
        {
            if (utmzone > -61 && utmzone < 61 && utmzone != 0)
            {
                Projection prj = new Projection();
                prj.Zone = utmzone;
                File.WriteAllText(Path.ChangeExtension(fileName, ".prj"), prj.ToWKT());
            }
        }
    }
    #endregion

}
