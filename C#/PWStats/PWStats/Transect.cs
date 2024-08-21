using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWStats
{
    public class Point3D
    {
        public double X;
        public double Y;

        public Point3D(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class Pass
    {
        public Ship Ship;
        public int Dir;
        public double Speed;

        public Pass(Ship ship, int dir, double speed = 0)
        {
            this.Ship = ship;
            this.Dir = dir;
            this.Speed = speed;
        }
    }

    public class Transect<T> where T : Point3D
    {
        public string Name;
        public List<T> Points;
        public List<Pass> Passes;

        public Transect()
        {
            this.Points = new List<T>();
            this.Passes = new List<Pass>();
        }

        public int Count { get => this.Points != null ? this.Points.Count : 0; }

        public void Add(double x, double y)
        {
            var pt = new Point3D(x, y);
            this.Points.Add((T)pt);
        }

        public static List<Transect<T>> ReadLDB(string fileName)
        {
            if (!File.Exists(fileName)) { return null; }
            char[] delimiters = { ' ', '\t' };
            string line;
            string[] items;
            double x, y, z, m;
            int count, idx = 0;
            var result = new List<Transect<T>>();
            using (var fi = new StreamReader(fileName))
            {
                while (!fi.EndOfStream)
                {
                    line = fi.ReadLine();
                    if (line.StartsWith("*"))
                    {
                        if (line.StartsWith("*CoordSys="))
                        {
                            string coordSys = line.Substring(10);
                            //var cf = new CoordinateSystemFactory();
                            //this.CoordSys = cf.CreateFromWkt(coordSys);
                        }
                    }
                    else
                    {
                        idx++;
                        var transect = new Transect<T>();
                        transect.Name = line.Trim();
                        if (String.IsNullOrEmpty(transect.Name))
                        {
                            transect.Name = String.Format("Transect {0}", idx);
                        }
                        line = fi.ReadLine();
                        items = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        int.TryParse(items[0], out count);
                        for (int i = 0; i < count; i++)
                        {
                            line = fi.ReadLine();
                            items = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (items.Length > 1 && double.TryParse(items[0], out x) && double.TryParse(items[1], out y))
                            {
                                if (items.Length > 2) { double.TryParse(items[2], out z); } else { z = 0; }
                                if (items.Length > 3) { double.TryParse(items[3], out m); } else { m = 0; }
                                transect.Add(x, y);
                            }
                        }
                        if (count > 1)
                        {
                            result.Add(transect);
                        }
                    }
                }
            }
            return result;
        }

        public static List<Transect<T>> ReadP2D(string fileName)
        {
            if (!File.Exists(fileName)) { return null; }
            char[] delimiters = { ' ', '\t' };
            string line;
            string[] items;
            double x, y, z, m;
            int idx = 0;
            var result = new List<Transect<T>>();
            using (var fi = new StreamReader(fileName))
            {
                while (!fi.EndOfStream)
                {
                    idx++;
                    var transect = new Transect<T>();
                    transect.Name = fi.ReadLine().Trim();
                    if (String.IsNullOrEmpty(transect.Name) || transect.Name == "*")
                    {
                        transect.Name = String.Format("Transect {0}", idx);
                    }
                    while (!fi.EndOfStream)
                    {
                        line = fi.ReadLine();
                        if (line.StartsWith("*")) { break; }
                        else
                        {
                            items = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (items.Length > 1 && double.TryParse(items[0], out x) && double.TryParse(items[1], out y))
                            {
                                if (items.Length > 2) { double.TryParse(items[2], out z); } else { z = 0; }
                                if (items.Length > 3) { double.TryParse(items[3], out m); } else { m = 0; }
                                transect.Add(x, y);
                            }
                        }
                    }
                    if (transect.Count > 1)
                    {
                        result.Add(transect);
                    }
                }
            }
            return result;
        }

    }
}
