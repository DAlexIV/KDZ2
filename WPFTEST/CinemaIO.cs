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
        private List<Theatre> _origdata = new List<Theatre>();
        private List<Theatre> _data = new List<Theatre>();
        private List<string> _areas;
        private List<string> _districts;
        string[] spltdDefLine;
        public string area_filter;
        private string _defLine;
        private DataGrid _datagr;
        private int wassorted;
        private int N;
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
                return _data;
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
            _data = _origdata;
        }
        internal void ConstructColumns()
        {
            for (int i = 0; i < _data[0].Values.Length; i++)
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
            _datagr.ItemsSource = _data;
        }
        public void ResizeData(int n)
        {
            Theatre[] tmpar = new Theatre[n];
            if (n > _data.Count())
                _origdata.CopyTo(0, tmpar, 0, n);
            else
                _data.CopyTo(0, tmpar, 0, n);
            _data = tmpar.ToList();
            UpdateDict();
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
                            srtddata = _data.OrderBy(x => x.IntValues[Theatre.intloc[i]]).ToList();
                            wassorted = i;
                        }
                        else
                        {
                            srtddata = _data.OrderByDescending(x => x.IntValues[Theatre.intloc[i]]).ToList();
                            wassorted = -i;
                        }
                    }
                    else
                    {
                        if (_datagr.Columns[i].SortDirection == ListSortDirection.Ascending)
                        {
                            srtddata = _data.OrderBy(x => x.Values[i]).ToList();
                            wassorted = i;
                        }
                        else
                        {
                            srtddata = _data.OrderByDescending(x => x.Values[i]).ToList();
                            wassorted = -i;
                        }
                    }
                }
            _data = srtddata;
        }
        public void UpdateData()
        {
            if (area_filter != null)
                ReduceDataBy(area_filter, ar);
            if (wassorted != 0)
                _datagr.Columns[Math.Abs(wassorted)].SortDirection =
                    (wassorted > 0) ? ListSortDirection.Ascending : ListSortDirection.Descending;
            wassorted = 0;
            _datagr.ItemsSource = _data;
        }
        private void UpdateDict()
        {
            List<string> newares = new List<string>();
            List<string> newdistricts = new List<string>();
            for (int i = 0; i < _data.Count(); ++i)
            {
                if (!newares.Contains(_data[i].Values[5]))
                    newares.Add(_data[i].Values[5]);
                if (!newdistricts.Contains(_data[i].Values[6]))
                    newdistricts.Add(_data[i].Values[6]);
            }
            _areas = newares;
            _districts = newdistricts;
        }
        private void ReduceDataBy(string line, int col)
        {
            List<Theatre> newdata = new List<Theatre>();
            for (int i = 0; i < _data.Count(); ++i)
            {
                if (_data[i].Values[col] == line)
                    newdata.Add(_data[i]);
            }
            _data = newdata;
        }
    }
}
