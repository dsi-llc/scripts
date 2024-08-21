using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWStats
{
    public class TrackPoint : Point3D
    {
        public DateTime Time;
        public float SOG;
        public float COG;
        public float Heading;

        public TrackPoint(double x, double y) : base(x, y)
        {
            SOG = float.NaN;
            COG = float.NaN;
            Heading = float.NaN;
        }

        public TrackPoint(DateTime time, double x, double y) : this(x, y)
        {
            this.Time = time;
        }

    }

    public class ShipTrack
    {
        public List<TrackPoint> Points;
        public int Count { get => Points != null ? Points.Count : 0; }

        public ShipTrack()
        {
            Points = new List<TrackPoint>();
        }

        public void Add(TrackPoint pt)
        {
            this.Points.Add(pt);
        }

        public void Sort()
        {
            this.Points = this.Points.OrderBy(pt => pt.Time).ToList();
        }

    }

    public class DockingArea
    {
        public string Name;
        public double hp;       // horsepower
        public double X;        // Longitude
        public double Y;        // Latitude
        public double R;        // Radius (m)
        public int Count;
        public TimeSpan Time;

        public DockingArea()
        {
            this.Count = 0;
            this.Time = TimeSpan.Zero;
        }

        public DockingArea(DockingArea dp) : this()
        {
            this.Name = dp.Name;
            this.hp = dp.hp;
            this.X = dp.X;
            this.Y = dp.Y;
            this.R = dp.R;
        }

        public static List<DockingArea> LoadTerminal(string fileName)
        {
            if (!File.Exists(fileName)) { return null; }
            char[] delimiters = { ',' };
            string[] items;
            double x, y, r;
            int idx = 0;
            var result = new List<DockingArea>();
            using (var fi = new StreamReader(fileName))
            {
                string line = fi.ReadLine();
                while (!fi.EndOfStream)
                {
                    idx++;
                    line = fi.ReadLine();
                    items = line.Split(delimiters);
                    if (items.Length > 3 && double.TryParse(items[0], out y) &&
                        double.TryParse(items[1], out x) && double.TryParse(items[2], out r))
                    {
                        var site = new DockingArea() { X = x, Y = y, R = r };
                        site.Name = String.Join(" ", items.Skip(3).ToArray());
                        result.Add(site);
                    }
                }
            }
            return result;
        }
    }

    public class Ship
    {
        public string MMSI;                 // MMSI
        public string Name;                 // Vessel name
        public string Type;                 // Type
        public float Length = float.NaN;    // Length(m)
        public float Breadth = float.NaN;   // Breadth(m)
        public float HullDepth = float.NaN; // Hull Depth(m)
        public float DraftMax = float.NaN;  // Draft Max.(m)
        public float AISToStern = float.NaN;// AIS to Stern(m)
        public float HP = float.NaN;        // HP
        public int NumProps = -1;           // # Props
        public int PropBlades = -1;         // # Prop Blades
        public float PropDiam = float.NaN;  // Prop Diameter(m)
        public List<ShipTrack> Tracks;
        public List<DockingArea> Docks;

        public Ship()
        {
            Tracks = new List<ShipTrack>();
            Tracks.Add(new ShipTrack());
            Docks = new List<DockingArea>();
        }

        public void SortTracks()
        {
            foreach (var track in this.Tracks) { track.Sort(); }
        }

        public void ClearTracks()
        {
            Tracks.Clear();
            Tracks.Add(new ShipTrack());
        }

        public static List<Ship> ReadShipInfo(string fileName)
        {
            if (!File.Exists(fileName)) { return null; }
            char[] delimiters = { ',' };
            string line;
            string[] items;
            int count;
            var result = new List<Ship>();
            using (var fi = new StreamReader(fileName))
            {
                line = fi.ReadLine();
                while (!fi.EndOfStream)
                {
                    line = fi.ReadLine();
                    items = line.Split(delimiters);
                    count = items.Length;
                    if (count > 0 && !String.IsNullOrEmpty(items[0]))
                    {
                        var ship = new Ship();
                        ship.MMSI = items[0].Trim();
                        if (count > 1 && !String.IsNullOrEmpty(items[1])) { ship.Name = items[1].Trim(); }
                        if (count > 2 && !String.IsNullOrEmpty(items[2])) { ship.Type = items[2].Trim(); }
                        if (count > 3 && !String.IsNullOrEmpty(items[3])) { float.TryParse(items[3].Trim(), out ship.Length); }
                        if (count > 4 && !String.IsNullOrEmpty(items[4])) { float.TryParse(items[4].Trim(), out ship.Breadth); }
                        if (count > 5 && !String.IsNullOrEmpty(items[5])) { float.TryParse(items[5].Trim(), out ship.HullDepth); }
                        if (count > 6 && !String.IsNullOrEmpty(items[6])) { float.TryParse(items[6].Trim(), out ship.DraftMax); }
                        if (count > 7 && !String.IsNullOrEmpty(items[7])) { float.TryParse(items[7].Trim(), out ship.AISToStern); }
                        if (count > 8 && !String.IsNullOrEmpty(items[8])) { float.TryParse(items[8].Trim(), out ship.HP); }
                        if (count > 9 && !String.IsNullOrEmpty(items[9])) { int.TryParse(items[9].Trim(), out ship.NumProps); }
                        if (count > 10 && !String.IsNullOrEmpty(items[10])) { int.TryParse(items[10].Trim(), out ship.PropBlades); }
                        if (count > 11 && !String.IsNullOrEmpty(items[11])) { float.TryParse(items[11].Trim(), out ship.PropDiam); }
                        result.Add(ship);
                    }
                }
            }
            return result;
        }

        public void ExportTracks(string fileName)
        {
            if (this.Tracks == null || this.Tracks.Count == 0 || this.Tracks[0].Count == 0) { return; }
            int count = this.Tracks.Count;
            using (var fo = new StreamWriter(fileName))
            {
                fo.WriteLine("* MMSI: {0}", this.MMSI.Trim());
                fo.WriteLine("* Name: {0}", this.Name);
                fo.WriteLine("* Type: {0}", this.Type);
                fo.WriteLine("* Length: {0}", this.Length);
                fo.WriteLine("* Breadth: {0}", this.Breadth);
                fo.WriteLine("* HullDepth: {0}", this.HullDepth);
                fo.WriteLine("* DraftMax: {0}", this.DraftMax);
                fo.WriteLine("* AISToStern: {0}", this.AISToStern);
                fo.WriteLine("* HP: {0}", this.HP);
                fo.WriteLine("* NumProps: {0}", this.NumProps);
                fo.WriteLine("* PropBlades: {0}", this.PropBlades);
                fo.WriteLine("* PropDiam: {0}", this.PropDiam);
                fo.WriteLine("* NumTracks: {0}", count);
                for (int i = 0; i < count; i++)
                {
                    var trk = this.Tracks[i];
                    fo.WriteLine("Track_{0}", i + 1);
                    fo.WriteLine("{0} {1}", trk.Count, 4);
                    foreach (var pt in trk.Points)
                    {
                        fo.WriteLine("{0:F6} {1:F6} {2:yyyyMMdd HHmmss}", pt.X, pt.Y, pt.Time);
                    }
                }
            }
        }
    }

}
