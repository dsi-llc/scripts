using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PWStats
{
    public partial class frmMain : Form
    {
        double _timeGap = 1.0;              // Time gap between track point (hours)
        double _dockingSpeed = 0.01;        // A ship is considered docking if its speed belows this value (m/s)
        double _dockingDuration = 3;        // and the time is longer than this duration (hours)
        double _dockingDist = 200;          // Ship docking locations are grouping using this distance (m)
        List<Transect<Point3D>> _transects;
        List<Ship> _ships;
        SortedDictionary<string, Ship> _dict;
        List<DockingArea> _terminals;
        List<string> _shipDocking, _shipCrossing, _shipName;
        bool _givenTerminals;
        string _MMSI;
        int _pointCount = 0;
        double _minSpeed, _maxSpeed, _minX, _maxX, _minY, _maxY;
        float[,] _bounds = new float[13, 2];
        double[] _limits;
        double _interval;
        string _format = "#VALY{G}";
        bool _isLabel;

        public frmMain()
        {
            InitializeComponent();
            txtGap.Text = _timeGap.ToString();
            txtSpeed.Text = _dockingSpeed.ToString();
            txtTime.Text = _dockingDuration.ToString();
            txtDist.Text = _dockingDist.ToString();
            _shipDocking = new List<string>();
            _shipCrossing = new List<string>();
            _shipName = new List<string>();
            cbxDir.SelectedIndex = 0;
            cbxStat.SelectedIndex = 0;
            cbxOptions.SelectedIndex = 0;
            try { cbxShip.SelectedIndex = 0; } catch { }
            SetToolTip();
        }

        private void UpdateInfo()
        {
            int shipCount = _ships != null ? _ships.Count : 0;
            int transectCount = _transects != null ? _transects.Count : 0;
            int terminalCount = _terminals != null ? _terminals.Count : 0;
            lblInfo.Text = String.Format("Info: {0} ships, {1} points, {2} transects, {3} user specified terminals",
                shipCount, _pointCount, transectCount, terminalCount);
            btnProcess.Enabled = _pointCount > 0 && transectCount > 0;
        }

        private void rdbIndividuals_CheckedChanged(object sender, EventArgs e)
        {
            //nudBins.Enabled = rdbRanges.Checked;
        }

        private void btnShip_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Ship Info. Database files (*.csv)|*.csv|All files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                txtShipFile.Text = dlg.FileName;
                _ships = Ship.ReadShipInfo(dlg.FileName);
                int count = _ships != null ? _ships.Count : 0;
                if (count > 0)
                {
                    BuildShipDictionary();
                    btnTrack.Enabled = true;
                    UpdateInfo();
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _pointCount = 0;
            if (_ships == null || _ships.Count == 0) { return; }
            foreach (var ship in _ships)
            {
                ship.ClearTracks();
            }
            UpdateInfo();
            btnPlot.Enabled = false;
            btnExport.Enabled = false;
        }

        private void btnTrack_Click(object sender, EventArgs e)
        {
            //string exportPath = @"d:\DSI\Codes\Scripts\C#\PWStats\PWStats\bin\Tracks\";
            var dlg = new OpenFileDialog();
            dlg.Filter = "Ship Track files (*.csv)|*.csv|All files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                txtTrackFile.Text = dlg.FileName;
                _pointCount = ReadShipTracks(dlg.FileName);
                if (_pointCount > 0)
                {
                    foreach (var ship in _ships)
                    {
                        ship.SortTracks();
                        //ship.ExportTracks(Path.Combine(exportPath, ship.MMSI + ".ldb"));
                    }
                    btnTransect.Enabled = true;
                    UpdateInfo();
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void btnTransect_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Transect files (*.ldb;*.p2d)|*.ldb;*.p2d|All files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                txtTransect.Text = dlg.FileName;
                string fileExt = Path.GetExtension(dlg.FileName).ToLower();
                if (fileExt == ".ldb") { _transects = Transect<Point3D>.ReadLDB(dlg.FileName); }
                else if (fileExt == ".p2d") { _transects = Transect<Point3D>.ReadP2D(dlg.FileName); }
                if (_transects != null && _transects.Count > 0)
                {
                    UpdateInfo();
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            _givenTerminals = chkTerm.Checked && _terminals != null && _terminals.Count > 0;
            if (chkTerm.Checked && (_terminals == null || _terminals.Count == 0))
            {
                MessageBox.Show("You are using the user specified terminals but no valid terminal is provided.\n" +
                    "Automatic method will be used to find the docking terminals", "No Valid Terminal", MessageBoxButtons.OK);
            }
            this.Cursor = Cursors.WaitCursor;
            DateTime begin = dateTimePicker1.Value;
            DateTime end = dateTimePicker2.Value;
            double.TryParse(txtGap.Text, out _timeGap);
            double.TryParse(txtSpeed.Text, out _dockingSpeed);
            double.TryParse(txtTime.Text, out _dockingDuration);
            double.TryParse(txtDist.Text, out _dockingDist);

            Process(begin, end);

            this.Cursor = Cursors.Default;
            int count = 0;
            foreach (var xs in _transects) { count += xs.Passes.Count; }
            cbxStat_SelectedIndexChanged(sender, e);
            btnPlot.Enabled = true;
            btnExport.Enabled = true;
            string message = String.Format("Found {0} passes through {1} transect(s)!", count, _transects.Count);
            MessageBox.Show(message, "Process Result", MessageBoxButtons.OK);
        }


        private void cbxStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = cbxStat.SelectedIndex;
            if (idx == 13)
            {
                nudBins.Enabled = true;
                label9.Text = String.Format("({0:F2} - {1:F2} {2})", _minSpeed, _maxSpeed, "m/s");
                label9.Visible = true;
            }
            else if ((idx >= 4 && idx <= 9) || idx == 12)
            {
                string units = idx == 9 ? "HP" : "m";
                nudBins.Enabled = true;
                label9.Text = String.Format("({0} - {1} {2})", _bounds[idx, 0], _bounds[idx, 1], units);
                label9.Visible = true;
            }
            else
            {
                nudBins.Enabled = false;
                label9.Visible = false;
            }
            //groupBox1.Enabled = idx == 14;
            //int index = cbxShip.SelectedIndex;
            BindShip();

            int index = !String.IsNullOrEmpty(_MMSI) ? cbxShip.Items.IndexOf(_MMSI) : 0;
            if (index < 0) { index = 0; }
            try { cbxShip.SelectedIndex = index; } catch { }
        }

        private void BindShip()
        {
            if (cbxStat.SelectedIndex == 14)
            {
                cbxShip.DataSource = cbxOptions.SelectedIndex == 0 ? new BindingSource(_shipDocking, null) : new BindingSource(_shipName, null);
            }
            else
            {
                cbxShip.DataSource = cbxOptions.SelectedIndex == 0 ? new BindingSource(_shipCrossing, null) : new BindingSource(_shipName, null);
            }
        }

        private void cbxShip_SelectedIndexChanged(object sender, EventArgs e)
        {
            _MMSI = cbxShip.SelectedIndex > 0 ? (string)cbxShip.SelectedItem : "";
            GetShipInfo();
        }

        private void chkTerm_CheckedChanged(object sender, EventArgs e)
        {
            btnTerm.Enabled = chkTerm.Checked;
            txtDist.Enabled = !chkTerm.Checked;
        }

        private void btnTerm_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Terminal files (*.csv)|*.csv|All files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                txtTerm.Text = dlg.FileName;
                _terminals = DockingArea.LoadTerminal(dlg.FileName);
                UpdateInfo();
                this.Cursor = Cursors.Default;
            }
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            int idx = cbxStat.SelectedIndex;
            if (_transects == null || idx < 0) { return; }
            Series sr;
            int numBins;
            int[,] binData;
            _isLabel = chkLabel.Checked;
            int num, dir = cbxDir.SelectedIndex;

            //setup the chart
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add(new ChartArea());

            //CreateBins(_bounds[idx, 0], _bounds[idx, 1]);
            int count = _transects.Count;
            var ca = chart1.ChartAreas[0];
            var ax = ca.AxisX;
            ax.Interval = 1;
            var ay = ca.AxisY;
            ay.Title = "Number of Ship Passings";
            ax.TitleFont = new Font("Arial", 10, FontStyle.Bold);
            ay.TitleFont = new Font("Arial", 10, FontStyle.Bold);
            ax.MajorGrid.LineColor = Color.Silver;
            ay.MajorGrid.LineColor = Color.Silver;
            ca.BorderDashStyle = ChartDashStyle.Solid;
            ca.BorderColor = Color.Black;
            ax.Interval = 1;
            //ay.Interval = 1;
            //chart1.Legends.Clear();
            chart1.Series.Clear();
            switch (idx)
            {
                case 0:
                    #region Count
                    CreateShipCountChart(dir);
                    #endregion
                    break;
                case 1:
                    CreateChart("MMSI", (sh) => sh.MMSI.Trim());
                    break;
                case 2:
                    CreateChart("Vessel Name", (sh) => sh.Name.Trim());
                    break;
                case 3:
                    CreateChart("Vessel Type", (sh) => sh.Type.Trim());
                    break;
                case 4:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.Length);
                    CreateChart("Vessel Length (m)", numBins, binData);
                    break;
                case 5:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.Breadth);
                    CreateChart("Vessel Breadth (m)", numBins, binData);
                    break;
                case 6:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.HullDepth);
                    CreateChart("Hull Depth (m)", numBins, binData);
                    break;
                case 7:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.AISToStern);
                    CreateChart("Max. Draft (m)", numBins, binData);
                    break;
                case 8:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.AISToStern);
                    CreateChart("AIS To Stern (m)", numBins, binData);
                    break;
                case 9:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.HP);
                    CreateChart("Horse Power (HP)", numBins, binData);
                    break;
                case 10:
                    CreateChart("Number of Propellers",
                        (sh) => sh.NumProps > 0 ? sh.NumProps.ToString() : "No Data");
                    break;
                case 11:
                    CreateChart("Number of Propeller Blades",
                        (sh) => sh.PropBlades > 0 ? sh.PropBlades.ToString() : "No Data");
                    break;
                case 12:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.PropDiam);
                    CreateChart("Propeller Diameter (m)", numBins, binData);
                    break;
                case 13:
                    ProcessSpeedBins(out numBins, out binData);
                    CreateChart("Speed (m/s)", numBins, binData);
                    break;
                case 14:
                    if (_givenTerminals)
                    {
                        var series = new Series("Terminals");
                        series.ChartType = SeriesChartType.Line;
                        series.MarkerStyle = MarkerStyle.Triangle;
                        series.BorderDashStyle = ChartDashStyle.NotSet;
                        foreach (var dp in _terminals)
                        {
                            int i = series.Points.AddXY(dp.X, dp.Y);
                            series.Points[i].Label = dp.Name;
                        }
                        chart1.Series.Add(series);
                    }
                    foreach (var ship in _ships)
                    {
                        if (FoundShip(ship))
                        {
                            if (ship.Docks.Count > 0)
                            {
                                var series = new Series(ship.MMSI.Trim());
                                //series.ChartType = SeriesChartType.Point;
                                series.ChartType = SeriesChartType.Line;
                                series.MarkerStyle = MarkerStyle.Circle;
                                series.BorderDashStyle = ChartDashStyle.NotSet;
                                //if (_isLabel) { series.Label = _format; }
                                int pntCnt = 0;
                                foreach (var dp in ship.Docks)
                                {
                                    if (dp.Count > 0)
                                    {
                                        int i = series.Points.AddXY(dp.X, dp.Y);
                                        if (_isLabel) { series.Points[i].Label = String.Format("{0:F2} hrs", dp.Time.TotalHours); }
                                        pntCnt++;
                                    }
                                }
                                if (pntCnt > 0) { chart1.Series.Add(series); }
                            }
                        }
                    }
                    ax.Title = "Longitude (deg.)";
                    ay.Title = "Latitude (deg.)";
                    if (_minX < _maxX) { ax.Maximum = _maxX; ax.Minimum = _minX; }
                    if (_minY < _maxY) { ay.Maximum = _maxY; ay.Minimum = _minY; }
                    ax.Interval = Math.Abs(_maxX - _minX) / 2;
                    //ay.Interval = 0.01;
                    break;
            }
            btnSave.Enabled = true;
        }


        private void ExportTable(string fileName)
        {
            int idx = cbxStat.SelectedIndex;
            if (_transects == null || idx < 0) { return; }
            int numBins;
            int[,] binData;
            int num, dir = cbxDir.SelectedIndex;
            int count = _transects.Count;

            switch (idx)
            {
                case 0:
                    #region Count
                    using (var fo = new StreamWriter(fileName))
                    {
                        fo.WriteLine("Number of Ship Passings");
                        fo.WriteLine();
                        fo.WriteLine("Transects,Ship Count");
                        for (int i = 0; i < _transects.Count; i++)
                        {
                            var xs = _transects[i];
                            num = 0;
                            for (int j = 0; j < xs.Passes.Count; j++)
                            {
                                var ship = xs.Passes[j].Ship;
                                if (FoundShip(ship))
                                {
                                    if (dir == 0) { num++; }
                                    else if (dir == 1 && xs.Passes[j].Dir == 1) { num++; }
                                    else if (dir == 2 && xs.Passes[j].Dir == -1) { num++; }
                                }
                            }
                            fo.WriteLine("\"{0}\",{1}", xs.Name, num);
                        }
                    }
                    #endregion
                    break;
                case 1:
                    CreateTable("MMSI", (sh) => sh.MMSI.Trim(), fileName);
                    break;
                case 2:
                    CreateTable("Vessel Name", (sh) => sh.Name.Trim(), fileName);
                    break;
                case 3:
                    CreateTable("Vessel Type", (sh) => sh.Type.Trim(), fileName);
                    break;
                case 4:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.Length);
                    CreateTable("Vessel Length (m)", fileName, numBins, binData);
                    break;
                case 5:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.Breadth);
                    CreateTable("Vessel Breadth (m)", fileName, numBins, binData);
                    break;
                case 6:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.HullDepth);
                    CreateTable("Hull Depth (m)", fileName, numBins, binData);
                    break;
                case 7:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.AISToStern);
                    CreateTable("Max. Draft (m)", fileName, numBins, binData);
                    break;
                case 8:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.AISToStern);
                    CreateTable("AIS To Stern (m)", fileName, numBins, binData);
                    break;
                case 9:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.HP);
                    CreateTable("Horse Power (HP)", fileName, numBins, binData);
                    break;
                case 10:
                    CreateTable("Number of Propellers",
                        (sh) => sh.NumProps > 0 ? sh.NumProps.ToString() : "No Data", fileName);
                    break;
                case 11:
                    CreateTable("Number of Propeller Blades",
                        (sh) => sh.PropBlades > 0 ? sh.PropBlades.ToString() : "No Data", fileName);
                    break;
                case 12:
                    ProcessBins(out numBins, out binData, idx, (sh) => sh.PropDiam);
                    CreateTable("Propeller Diameter (m)", fileName, numBins, binData);
                    break;
                case 13:
                    ProcessSpeedBins(out numBins, out binData);
                    CreateTable("Speed (m/s)", fileName, numBins, binData);
                    break;
                case 14:
                    using (var fo = new StreamWriter(fileName))
                    {
                        fo.WriteLine("Ship Docking Locations");
                        fo.WriteLine();
                        fo.WriteLine("MMSI,Ship Name,Longitude,Latitude,Count,Duration (hours),HP,Terminal");
                        foreach (var ship in _ships)
                        {
                            if (FoundShip(ship))
                            {
                                if (ship.Docks.Count > 0)
                                {
                                    foreach (var dp in ship.Docks)
                                    {
                                        if (dp.Count > 0)
                                        {
                                            fo.WriteLine("\"{0}\",\"{1}\",{2},{3},{4},{5},{6},\"{7}\"",
                                                ship.MMSI, ship.Name, dp.X, dp.Y, dp.Count, Math.Round(dp.Time.TotalHours, 3), ship.HP, dp.Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private void GetShipInfo()
        {
            lblMMSI.Text = "";
            if (cbxShip.SelectedIndex == 0) return;
            foreach (var ship in _ships)
            {
                if (cbxOptions.SelectedIndex == 0 && ship.MMSI == _MMSI)
                {
                    lblMMSI.Text = string.Format("Name: {0}", ship.Name);
                    return;
                }
                else if (ship.Name == _MMSI)
                {
                    lblMMSI.Text = string.Format("MMSI: {0}", ship.MMSI);
                    return;
                }
            }
        }

        private bool FoundShip(Ship ship)
        {
            if (cbxOptions.SelectedIndex == 0)
            {
                return (String.IsNullOrEmpty(_MMSI) || ship.MMSI == _MMSI);
            }
            else
            {
                return (String.IsNullOrEmpty(_MMSI) || ship.Name == _MMSI);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "CSV file (*.csv)|*.csv|All files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                ExportTable(dlg.FileName);
                this.Cursor = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "PNG Images (*.png)|*.png|Windows Metafile Images (*.emf)|*.emf|All files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                string fileExt = Path.GetExtension(dlg.FileName).ToLower();
                if (fileExt == ".emf")
                {
                    chart1.SaveImage(dlg.FileName, ChartImageFormat.Emf);
                }
                else
                {
                    chart1.SaveImage(dlg.FileName, ChartImageFormat.Png);
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void BuildShipDictionary()
        {
            Array.Clear(_bounds, 0, _bounds.Length);
            for (int i = 0; i < 13; i++)
            {
                _bounds[i, 0] = float.MaxValue;
                _bounds[i, 1] = float.MinValue;
            }
            _dict = new SortedDictionary<string, Ship>();
            foreach (var ship in _ships)
            {
                if (!_dict.ContainsKey(ship.MMSI))
                {
                    _dict.Add(ship.MMSI, ship);
                }
                if (!float.IsNaN(ship.Length))
                {
                    if (_bounds[4, 0] > ship.Length) { _bounds[4, 0] = ship.Length; }
                    if (_bounds[4, 1] < ship.Length) { _bounds[4, 1] = ship.Length; }
                }
                if (!float.IsNaN(ship.Breadth))
                {
                    if (_bounds[5, 0] > ship.Breadth) { _bounds[5, 0] = ship.Breadth; }
                    if (_bounds[5, 1] < ship.Breadth) { _bounds[5, 1] = ship.Breadth; }
                }
                if (!float.IsNaN(ship.HullDepth))
                {
                    if (_bounds[6, 0] > ship.HullDepth) { _bounds[6, 0] = ship.HullDepth; }
                    if (_bounds[6, 1] < ship.HullDepth) { _bounds[6, 1] = ship.HullDepth; }
                }
                if (!float.IsNaN(ship.DraftMax))
                {
                    if (_bounds[7, 0] > ship.DraftMax) { _bounds[7, 0] = ship.DraftMax; }
                    if (_bounds[7, 1] < ship.DraftMax) { _bounds[7, 1] = ship.DraftMax; }
                }
                if (!float.IsNaN(ship.AISToStern))
                {
                    if (_bounds[8, 0] > ship.AISToStern) { _bounds[8, 0] = ship.AISToStern; }
                    if (_bounds[8, 1] < ship.AISToStern) { _bounds[8, 1] = ship.AISToStern; }
                }
                if (!float.IsNaN(ship.HP))
                {
                    if (_bounds[9, 0] > ship.HP) { _bounds[9, 0] = ship.HP; }
                    if (_bounds[9, 1] < ship.HP) { _bounds[9, 1] = ship.HP; }
                }
                if (ship.NumProps > 0)
                {
                    if (_bounds[10, 0] > ship.NumProps) { _bounds[10, 0] = ship.NumProps; }
                    if (_bounds[10, 1] < ship.NumProps) { _bounds[10, 1] = ship.NumProps; }
                }
                if (ship.PropBlades > 0)
                {
                    if (_bounds[11, 0] > ship.PropBlades) { _bounds[11, 0] = ship.PropBlades; }
                    if (_bounds[11, 1] < ship.PropBlades) { _bounds[11, 1] = ship.PropBlades; }
                }
                if (!float.IsNaN(ship.PropDiam))
                {
                    if (_bounds[12, 0] > ship.PropDiam) { _bounds[12, 0] = ship.PropDiam; }
                    if (_bounds[12, 1] < ship.PropDiam) { _bounds[12, 1] = ship.PropDiam; }
                }
            }
        }

        private SortedDictionary<string, int[]> GetShipKey(int count, Func<Ship, string> getKey)
        {
            var dict = new SortedDictionary<string, int[]>();
            foreach (var ship in _ships)
            {
                string key = getKey(ship);
                if (!dict.ContainsKey(key))
                {
                    int[] values = new int[count];
                    Array.Clear(values, 0, count);
                    dict.Add(key, values);
                }
            }
            return dict;
        }

        private void Process(DateTime begin, DateTime end)
        {
            double dT, dX, dY, dS, x1, y1, x2, y2, x3, y3;
            double speed = 0;
            bool compSpeed = false;
            var prj = new Projection();
            TimeSpan ts;
            _minSpeed = double.MaxValue;
            _maxSpeed = double.MinValue;
            _minX = double.MaxValue;
            _minY = double.MaxValue;
            _maxX = double.MinValue;
            _maxY = double.MinValue;
            var shipCrossing = new List<string>();
            var shipDocking = new List<string>();
            var shipName = new List<string>();

            foreach (var xs in _transects)
            {
                xs.Passes.Clear();
            }

            foreach (var ship in _ships)
            {
                //if (!String.IsNullOrEmpty(_MMSI) && ship.MMSI != _MMSI) { continue; }
                ship.Docks.Clear();
                if (_givenTerminals)
                {
                    foreach (var dp in _terminals) { ship.Docks.Add(new DockingArea(dp)); }
                }
                foreach (var trk in ship.Tracks)
                {
                    int count = trk.Count;
                    for (int i = 0; i < count - 1; i++)
                    {
                        var pt2 = trk.Points[i];
                        var pt3 = trk.Points[i + 1];
                        if (_minX > pt2.X) { _minX = pt2.X; }
                        if (_minY > pt2.Y) { _minY = pt2.Y; }
                        if (_maxX < pt2.X) { _maxX = pt2.X; }
                        if (_maxY < pt2.Y) { _maxY = pt2.Y; }
                        if (pt2.Time >= begin && pt3.Time <= end)
                        {
                            #region Compute Speed
                            compSpeed = false;
                            ts = pt3.Time.Subtract(pt2.Time);
                            dT = ts.TotalHours;
                            //speed = ComputeSpeed(prj, pt2, pt3);
                            prj.Meridian = pt2.X;
                            prj.Geo_2_UTM(pt2.X, pt2.Y, out x2, out y2);
                            prj.Geo_2_UTM(pt3.X, pt3.Y, out x3, out y3);
                            dX = x3 - x2;
                            dY = y3 - y2;
                            dS = Math.Sqrt(dX * dX + dY * dY);
                            speed = dS / (3600 * dT);
                            if (_minSpeed > speed) { _minSpeed = speed; }
                            if (_maxSpeed < speed) { _maxSpeed = speed; }
                            #endregion
                            #region Find Docking Areas
                            if (speed <= _dockingSpeed && dT >= _dockingDuration)
                            {
                                DockingArea loc = null;
                                foreach (var dp in ship.Docks)
                                {
                                    prj.Geo_2_UTM(dp.X, dp.Y, out x1, out y1);
                                    if (Math.Abs(x2 - x1) <= _dockingDist && Math.Abs(y2 - y1) <= _dockingDist)
                                    {
                                        loc = dp;
                                        loc.hp = ship.HP;
                                        loc.Count++;
                                        loc.Time += ts;
                                        break;
                                    }
                                }
                                if (loc == null)
                                {
                                    if (!_givenTerminals)
                                    {
                                        loc = new DockingArea() { X = pt2.X, Y = pt2.Y };
                                        ship.Docks.Add(loc);
                                        loc.Count++;
                                        loc.Time += ts;
                                    }
                                }
                                if (!shipDocking.Contains(ship.MMSI)) { shipDocking.Add(ship.MMSI); }
                                if (!shipName.Contains(ship.Name)) { shipName.Add(ship.Name); }
                            }
                            #endregion
                            #region Find Transects
                            if (dT <= _timeGap && _transects != null)
                            {
                                foreach (var xs in _transects)
                                {
                                    int num = xs.Count;
                                    for (int j = 0; j < num - 1; j++)
                                    {
                                        var pt0 = xs.Points[j];
                                        var pt1 = xs.Points[j + 1];
                                        int res = Geometry.Intersect(pt0, pt1, pt2, pt3);
                                        if (res != 0)
                                        {
                                            //if (!compSpeed)
                                            //{
                                            //    prj.Meridian = pt2.X;
                                            //    prj.Geo_2_UTM(pt2.X, pt2.Y, out x2, out y2);
                                            //    prj.Geo_2_UTM(pt3.X, pt3.Y, out x3, out y3);
                                            //    dX = x3 - x2;
                                            //    dY = y3 - y2;
                                            //    dS = Math.Sqrt(dX * dX + dY * dY);
                                            //    speed = dS / (3600 * dT);
                                            //    if (_minSpeed > speed) { _minSpeed = speed; }
                                            //    if (_maxSpeed < speed) { _maxSpeed = speed; }
                                            //    compSpeed = true;
                                            //}
                                            xs.Passes.Add(new Pass(ship, res, speed));
                                            if (!shipCrossing.Contains(ship.MMSI)) { shipCrossing.Add(ship.MMSI); }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }

            shipCrossing.Sort();
            _shipCrossing.Clear();
            _shipCrossing.Add("<All Ships>");
            _shipCrossing.AddRange(shipCrossing);


            shipDocking.Sort();
            _shipDocking.Clear();
            _shipDocking.Add("<All Ships>");
            _shipDocking.AddRange(shipDocking);

            shipName.Sort();
            _shipName.Clear();
            _shipName.Add("<All Ships>");
            _shipName.AddRange(shipName);
        }

        private double ComputeSpeed(Projection prj, TrackPoint pt1, TrackPoint pt2)
        {
            double speed = 0, dT, dX, dY, dS, x1, y1, x2, y2;
            prj.Meridian = pt1.X;
            prj.Geo_2_UTM(pt1.X, pt1.Y, out x1, out y1);
            prj.Geo_2_UTM(pt2.X, pt2.Y, out x2, out y2);
            dX = x2 - x1;
            dY = y2 - y1;
            dS = Math.Sqrt(dX * dX + dY * dY);
            dT = pt2.Time.Subtract(pt1.Time).TotalHours;
            speed = dS / (3600 * dT);
            return speed;
        }

        private int CreateBins(double minVal, double maxVal)
        {
            int numBins = (int)nudBins.Value;
            if (numBins < 1) { numBins = 1; }
            double range = maxVal - minVal;
            _interval = range / numBins;
            _limits = new double[numBins + 1];
            for (int i = 0; i <= numBins; i++)
            {
                _limits[i] = minVal + i * _interval;
            }
            return numBins;
        }

        private SortedDictionary<string, int[]> ProcessDict(Func<Ship, string> getKey)
        {
            int num, dir = cbxDir.SelectedIndex;
            int count = _transects.Count;
            var dict = GetShipKey(count, getKey);
            for (int i = 0; i < count; i++)
            {
                var xs = _transects[i];
                num = xs.Passes.Count;
                for (int j = 0; j < num; j++)
                {
                    bool ok = false;
                    if (dir == 0) { ok = true; }
                    else if (dir == 1 && xs.Passes[j].Dir == 1) { ok = true; }
                    else if (dir == 2 && xs.Passes[j].Dir == -1) { ok = true; }
                    if (ok)
                    {
                        var ship = xs.Passes[j].Ship;
                        if (FoundShip(ship))
                        {
                            string key = getKey(ship);
                            if (dict.ContainsKey(key))
                            {
                                dict[key][i] += 1;
                            }
                        }
                    }
                }
            }
            var keys = new List<string>();
            foreach (var kvp in dict)
            {
                int total = 0;
                for (int j = 0; j < count; j++) { total += kvp.Value[j]; }
                if (total == 0) { keys.Add(kvp.Key); }
            }
            foreach (var key in keys) { dict.Remove(key); }
            return dict;
        }

        private void CreateChart(string caption, Func<Ship, string> getKey)
        {
            var dict = ProcessDict(getKey);
            int count = _transects.Count;
            for (int i = 0; i < count; i++)
            {
                var xs = _transects[i];
                var sr = new Series(xs.Name);
                sr.ChartType = SeriesChartType.Column;
                foreach (var kvp in dict)
                {
                    sr.Points.AddXY(kvp.Key, kvp.Value[i]);
                }
                chart1.Series.Add(sr);
                if (_isLabel) { sr.Label = _format; }
            }
            chart1.ChartAreas[0].AxisX.Title = caption;
        }

        private void CreateTable(string caption, Func<Ship, string> getKey, string fileName)
        {
            var dict = ProcessDict(getKey);
            int count = _transects.Count;
            using (var fo = new StreamWriter(fileName))
            {
                fo.WriteLine("Number of Ship Passings");
                fo.WriteLine();

                fo.Write("\"{0}\"", caption);
                for (int i = 0; i < count; i++)
                {
                    var xs = _transects[i];
                    fo.Write(",\"{0}\"", xs.Name);
                }
                fo.WriteLine();

                foreach (var kvp in dict)
                {
                    fo.Write("\"{0}\"", kvp.Key);
                    for (int i = 0; i < count; i++)
                    {
                        fo.Write(",{0}", kvp.Value[i]);
                    }
                    fo.WriteLine();
                }
            }
        }

        private void ProcessBins(out int numBins, out int[,] binData, int idx, Func<Ship, float> getValue)
        {
            numBins = CreateBins(_bounds[idx, 0], _bounds[idx, 1]);
            int num, dir = cbxDir.SelectedIndex;
            int count = _transects.Count;
            binData = new int[count + 1, numBins + 1];
            Array.Clear(binData, 0, binData.Length);
            for (int i = 0; i < count; i++)
            {
                var xs = _transects[i];
                num = xs.Passes.Count;
                for (int j = 0; j < num; j++)
                {
                    bool ok = false;
                    if (dir == 0) { ok = true; }
                    else if (dir == 1 && xs.Passes[j].Dir == 1) { ok = true; }
                    else if (dir == 2 && xs.Passes[j].Dir == -1) { ok = true; }
                    if (ok)
                    {
                        var ship = xs.Passes[j].Ship;
                        if (FoundShip(ship))
                        {
                            float value = getValue(ship);
                            int index = !float.IsNaN(value) ? (int)((value - _bounds[idx, 0]) / _interval) : -1;
                            if (index >= 0 && index < numBins) { binData[i, index]++; } else { binData[i, numBins]++; }
                        }
                    }
                }
            }
            for (int j = 0; j < numBins; j++)
            {
                int total = 0;
                for (int i = 0; i < count; i++)
                {
                    total += binData[i, j];
                }
                if (total > 0)
                {
                    binData[count, j] = 1;
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void cbxOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindShip();
        }

        private void CreateChart(string caption, int numBins, int[,] binData)
        {
            int count = _transects.Count;
            for (int i = 0; i < count; i++)
            {
                var xs = _transects[i];
                var sr = new Series(xs.Name);
                sr.ChartType = SeriesChartType.Column;
                sr.Points.AddXY("No Data", binData[i, numBins]);
                for (int j = 0; j < numBins; j++)
                {
                    if (binData[count, j] > 0)
                    {
                        sr.Points.AddXY(String.Format("{0:F2} - {1:F2}", _limits[j], _limits[j + 1]), binData[i, j]);
                    }
                }
                chart1.Series.Add(sr);
                if (_isLabel) { sr.Label = _format; }
            }
            chart1.ChartAreas[0].AxisX.Title = caption;
        }

        private void CreateTable(string caption, string fileName, int numBins, int[,] binData)
        {
            int count = _transects.Count;
            using (var fo = new StreamWriter(fileName))
            {
                fo.WriteLine("Number of Ship Passings");
                fo.WriteLine();

                fo.Write("\"{0}\"", caption);
                for (int i = 0; i < count; i++)
                {
                    var xs = _transects[i];
                    fo.Write(",\"{0}\"", xs.Name);
                }
                fo.WriteLine();

                fo.Write("\"Missing Data\"");
                for (int i = 0; i < count; i++)
                {
                    fo.Write(",{0}", binData[i, numBins]);
                }
                fo.WriteLine();

                for (int j = 0; j < numBins; j++)
                {
                    if (binData[count, j] > 0)
                    {
                        fo.Write("\"{0:F2} - {1:F2}\"", _limits[j], _limits[j + 1]);
                        for (int i = 0; i < count; i++)
                        {
                            fo.Write(",{0}", binData[i, j]);
                        }
                        fo.WriteLine();
                    }
                }
            }
        }

        private void ProcessSpeedBins(out int numBins, out int[,] binData)
        {
            numBins = CreateBins(_minSpeed, _maxSpeed);
            int num, dir = cbxDir.SelectedIndex;
            int count = _transects.Count;
            binData = new int[count + 1, numBins + 1];
            Array.Clear(binData, 0, binData.Length);
            for (int i = 0; i < count; i++)
            {
                var xs = _transects[i];
                num = xs.Passes.Count;
                for (int j = 0; j < num; j++)
                {
                    bool ok = false;
                    if (dir == 0) { ok = true; }
                    else if (dir == 1 && xs.Passes[j].Dir == 1) { ok = true; }
                    else if (dir == 2 && xs.Passes[j].Dir == -1) { ok = true; }
                    if (ok)
                    {
                        var ship = xs.Passes[j].Ship;
                        if (FoundShip(ship))
                        {
                            double speed = xs.Passes[j].Speed;
                            int index = (int)((speed - _minSpeed) / _interval);
                            if (index >= 0 && index < numBins) { binData[i, index]++; } else { binData[i, numBins]++; }
                        }
                    }
                }
            }
            for (int j = 0; j < numBins; j++)
            {
                int total = 0;
                for (int i = 0; i < count; i++)
                {
                    total += binData[i, j];
                }
                if (total > 0)
                {
                    binData[count, j] = 1;
                }
            }
        }

        private void CreateShipCountChart(int dir)
        {
            var sr = new Series("Ship Count");
            sr.ChartType = SeriesChartType.Column;
            sr.Points.Clear();
            int count = _transects.Count;
            for (int i = 0; i < count; i++)
            {
                var xs = _transects[i];
                int num = 0;
                for (int j = 0; j < xs.Passes.Count; j++)
                {
                    var ship = xs.Passes[j].Ship;
                    if (FoundShip(ship))
                    {
                        if (dir == 0) { num++; }
                        else if (dir == 1 && xs.Passes[j].Dir == 1) { num++; }
                        else if (dir == 2 && xs.Passes[j].Dir == -1) { num++; }
                    }
                }
                sr.Points.AddXY(xs.Name, num);
            }
            chart1.Series.Add(sr);
            chart1.ChartAreas[0].AxisX.Title = "Transects";
            if (_isLabel) { sr.Label = _format; }
        }

        private int ReadShipTracks(string fileName)
        {
            int num, count = 0;
            if (!File.Exists(fileName)) { return count; }
            char[] delimiters = { ',' };
            string line, timeFormat = "yyyy/MM/dd HH:mm:ss";
            string[] items;
            double lat, lon;
            DateTime time, begin = DateTime.MaxValue, end = DateTime.MinValue;
            float value;
            string mmsi;
            using (var fi = new StreamReader(fileName))
            {
                line = fi.ReadLine();
                while (!fi.EndOfStream)
                {
                    line = fi.ReadLine();
                    items = line.Split(delimiters);
                    num = items.Length;
                    if (num > 3)
                    {
                        mmsi = items[0].Trim();
                        if (_dict.ContainsKey(mmsi))
                        {
                            if (DateTime.TryParseExact(items[1], timeFormat,
                                  System.Globalization.CultureInfo.InvariantCulture,
                                  System.Globalization.DateTimeStyles.None, out time) &&
                              double.TryParse(items[2], out lat) &&
                              double.TryParse(items[3], out lon))
                            {
                                var pt = new TrackPoint(time, lon, lat);
                                if (num > 4 && float.TryParse(items[4], out value)) { pt.SOG = value; }
                                if (num > 5 && float.TryParse(items[5], out value)) { pt.COG = value; }
                                if (num > 6 && float.TryParse(items[6], out value)) { pt.Heading = value; }
                                var ship = _dict[mmsi];
                                var track = ship.Tracks[0];
                                track.Add(pt);
                                if (begin > time) { begin = time; }
                                if (end < time) { end = time; }
                                count++;
                            }
                        }
                    }
                }
            }
            dateTimePicker1.Value = begin;
            dateTimePicker2.Value = end.AddDays(1);
            return count;
        }

        private void SetToolTip()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(txtShipFile, "File name contains ship information");
            toolTip.SetToolTip(btnShip, "Click to open the file contains ship information");
            toolTip.SetToolTip(txtTrackFile, "File name contains ship track data points");
            toolTip.SetToolTip(btnTrack, "Click to open the file contains ship track points");
            toolTip.SetToolTip(txtTransect, "File name contains cross-sections. The convention for the cross-section is going from left to right when facing downstream.");
            toolTip.SetToolTip(btnTransect, "Click to open the file contains cross-sections");
            toolTip.SetToolTip(btnClear, "Click to clear existing ship tracks");
            toolTip.SetToolTip(btnClose, "Click to quit this tool");
            toolTip.SetToolTip(btnProcess, "Click to process ship tracks for statistics");
            toolTip.SetToolTip(btnPlot, "Click to plot the selected statistics");
            toolTip.SetToolTip(btnExport, "Click to export the data to a *.CSV file");
            toolTip.SetToolTip(btnSave, "Click to save the plot to a *.PNG or *.EMF image");
            toolTip.SetToolTip(dateTimePicker1, "Begin date for the statistics");
            toolTip.SetToolTip(dateTimePicker2, "End date for the statistics");
            toolTip.SetToolTip(chkLabel, "Check this to show the values on the plot");
            toolTip.SetToolTip(cbxDir, "Select an option for ship traveling direction");
            toolTip.SetToolTip(cbxStat, "Select a statistical option to plot");
            toolTip.SetToolTip(cbxShip, "Select a specific ship or <All Ships> to plot");
            toolTip.SetToolTip(nudBins, "Specify the number of bins for statistics");
            toolTip.SetToolTip(txtGap, "Specify the maximum interval (in hours) as ship track gaps");
            toolTip.SetToolTip(txtSpeed, "A ship is considered docking if its speed slower or equal to this value in combination with time duration");
            toolTip.SetToolTip(txtTime, "A ship is considered docking if its speed slower or equal to a specific value for the time longer equal to this duration");
            toolTip.SetToolTip(txtDist, "The distance to group docking locations");
        }

    }
}
