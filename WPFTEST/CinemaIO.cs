using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
namespace WPFTEST
{
    class CinemaList
    {
        private const int ar = 5;
        private const int dst = 6;
        private const string def_area = "All Areas";
        private const string def_dist = "All Districts";
        private List<Theatre> _origdata = new List<Theatre>();
        private List<Theatre> _filtered_data = new List<Theatre>();
        private List<Theatre> _shown_data = new List<Theatre>();
        private List<string> _areas;
        private List<string> _districts;
        string[] spltdDefLine;

        private string _defLine;
        private DataGrid _datagr;
        private int _datasize;
        private int wassorted;
        private int N;
        public string area_filter;
        public string dist_filter;
        public int GetDatasize
        {
            get
            {
                return _datasize;
            }
        }
        public List<string> GetAreas
        {
            get
            {
                return _areas;
            }
        }
        public List<string> GetDistricts
        {
            get
            {
                return _districts;

            }
        }
        public DataGrid GetDataGrid
        {
            get
            {

                return _datagr;
            }
        }
        public int GetMaxSize
        {
            get
            {
                return _origdata.Count();
            }
        }
        public List<Theatre> GetData
        {
            get
            {
                return _shown_data;
            }
        }
        public string GetDefLine
        {
            get
            {
                return _defLine;
            }
        }
        public CinemaList(DataGrid datagr, List<string> rawdata)
        {
            this._datagr = datagr;
            List<string> myrd = rawdata.ToList();
            _defLine = rawdata[0];
            spltdDefLine = _defLine.Split(';');
            N = spltdDefLine.Length - 1;
            if (N + 1 != 24)
            {
                MessageBox.Show("Bad file! Try again.");
                return;
            }
            myrd.RemoveAt(0);
            for (int i = 0; i < myrd.Count(); ++i)
            {
                try
                {
                    _origdata.Add(new Theatre(myrd[i], N));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(i.ToString() + "/n" + ex.Message);
                }
            }
            _filtered_data = _origdata.ToList();
            _shown_data = _origdata.ToList();
            _datasize = _filtered_data.Count();
            SetDict();
        }
        internal void ConstructColumns()
        {
            for (int i = 0; i < _origdata[0].Values.Length; i++)
            {
                var col = new DataGridTextColumn();
                if (Theatre.intloc.ContainsKey(i))
                {
                    col.Header = spltdDefLine[i];
                    //Here i bind to the various indices.
                    var binding = new Binding("IntValues[" + Theatre.intloc[i] + "]");
                    col.Binding = binding;
                }
                else
                {
                    col.Header = spltdDefLine[i];
                    //Here i bind to the various indices.
                    var binding = new Binding("Values[" + i + "]");
                    col.Binding = binding;
                }
                _datagr.Columns.Add(col);
            }
            _datagr.ItemsSource = _origdata;
        }
        public void ResizeData(int n)
        {
            Theatre[] tmpar = new Theatre[n];
            if (n <= _filtered_data.Count())
                _filtered_data.CopyTo(0, tmpar, 0, n);
            else
                MessageBox.Show("Go fuck urself!");
            _shown_data = tmpar.ToList();
        }
        public void SortData()
        {
            List<Theatre> srtddata = new List<Theatre>();
            for (int i = 0; i < _datagr.Columns.Count(); ++i)
                if (_datagr.Columns[i].SortDirection != null)
                {
                    if (Theatre.intloc.ContainsKey(i))
                    {
                        if (_datagr.Columns[i].SortDirection == ListSortDirection.Ascending)
                        {
                            srtddata = _shown_data.OrderBy(x => x.IntValues[Theatre.intloc[i]]).ToList();
                            wassorted = i;
                        }
                        else
                        {
                            srtddata = _shown_data.OrderByDescending(x => x.IntValues[Theatre.intloc[i]]).ToList();
                            wassorted = -i;
                        }
                    }
                    else
                    {
                        if (_datagr.Columns[i].SortDirection == ListSortDirection.Ascending)
                        {
                            srtddata = _shown_data.OrderBy(x => x.Values[i]).ToList();
                            wassorted = i;
                        }
                        else
                        {
                            srtddata = _shown_data.OrderByDescending(x => x.Values[i]).ToList();
                            wassorted = -i;
                        }
                    }
                }
            if (srtddata.Any())
                _filtered_data = srtddata;
        }
        public void UpdateData()
        {
            //if (area_filter == def_area)
            // MessageBox.Show("You can't resize ")
                if (area_filter != null && area_filter != def_area)
                    ReduceDataBy(area_filter, ar);
                if (dist_filter != null && dist_filter != def_dist)
                    ReduceDataBy(dist_filter, dst);
            if (wassorted != 0)
                _datagr.Columns[Math.Abs(wassorted)].SortDirection =
                    (wassorted > 0) ? ListSortDirection.Ascending : ListSortDirection.Descending;
            wassorted = 0;
            _datagr.ItemsSource = _shown_data;
        }
        private void SetDict()
        {
            List<string> newares = new List<string>();
            List<string> newdistricts = new List<string>();
            newares.Add(def_area);
            newdistricts.Add(def_dist);
            for (int i = 0; i < _origdata.Count(); ++i)
            {
                if (!newares.Contains(_origdata[i].Values[5]))
                    newares.Add(_origdata[i].Values[5]);
                if (!newdistricts.Contains(_origdata[i].Values[6]))
                    newdistricts.Add(_origdata[i].Values[6]);
            }
            newares.Sort();
            newdistricts.Sort();
            _areas = newares;
            _districts = newdistricts;
            
        }
        private void ReduceDataBy(string line, int col)
        {

            List<Theatre> newdata = new List<Theatre>();
            for (int i = 0; i < _filtered_data.Count(); ++i)
            {
                if (_filtered_data[i].Values[col] == line)
                    newdata.Add(_filtered_data[i]);
            }
            if (newdata.Any())
            {
                _filtered_data = newdata;
                _datasize = _filtered_data.Count();
            }
            else
                throw new Exception("There is no such lines");
        }
        public void SetToDef()
        {
            _filtered_data = _origdata;
            _shown_data = _origdata;
            _datagr.ItemsSource = _shown_data;
            area_filter = def_area;
            dist_filter = def_dist;
        }

    }
}
