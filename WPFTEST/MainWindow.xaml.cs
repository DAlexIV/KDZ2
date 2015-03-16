using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using FileIO;
using Cinema;
namespace WPFTEST
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CinemaList cl; // Currently opened cinemalist
        private int prevarea; // Previous selected area filter
        private int prevdist; // Previous selected district filter
        /// <summary>
        /// Initializes all complonents in window
        /// to default values
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Opens csv-file
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void OpenFileHandler(object obj, EventArgs e)
        {
            string path = null;

            // Gets path
            try
            {
                path = MYIO.OpenCSV();
            }
            catch (FileLoadException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Initializes CinemaList with data
            List<string> rawdata = MYIO.ReadCSV(path);
            try
            {
                cl = new CinemaList(datagr, rawdata);
            }
            catch (FileLoadException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            cl.ConstructColumns();
            bar.Minimum = 1;
            bar.Maximum = cl.GetMaxSize;
            bar.Value = bar.Maximum;
            tb.Text = ((int)bar.Value).ToString();

            // Fills data to form elements 
            datagr.ItemsSource = cl.GetData;
            comboar.ItemsSource = cl.GetAreas;
            combodist.ItemsSource = cl.GetDistricts;

            UnlockAll();
            SetDefCombo();

        }
        /// <summary>
        /// Saves to new or existing file
        /// (Rewrites it)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void SaveToNewFileHandler(object obj, EventArgs e)
        {
            cl.SortData();
            string path = MYIO.SaveCSV();
            if (path != null)
                MYIO.WriteToFile(cl.GetDefLine, cl.GetData, path);
        }
        /// <summary>
        /// Appends data to existing file
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void AppendFileHandler(object obj, EventArgs e)
        {
            cl.SortData();
            string path = MYIO.SaveCSV();
            if (path != null)
                MYIO.AppendToFile(cl.GetDefLine, cl.GetData, path);
        }

        /// <summary>
        /// Changes number of lines shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //N = Num
            cl.SortData();
            cl.ResizeData((int)bar.Value);
            cl.UpdateData();
            tb.Text = ((int)bar.Value).ToString();
        }
        /// <summary>
        /// Updates shown number of line with text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextEntered(object sender, RoutedEventArgs e)
        {
            int curn;
            if (!int.TryParse(tb.Text, out curn))
            {
                MessageBox.Show("Please, enter a number!");
                return;
            }
            if (curn <= 0)
            {
                MessageBox.Show("Please, enter a positive number!");
                return;
            }
            if (curn > (int)bar.Maximum)
            {
                MessageBox.Show("Please, enter a number that is lower than " + (int)bar.Maximum);
                return;
            }
            bar.Value = curn;

        }
        /// <summary>
        /// Unlock all elements
        /// when file is opened
        /// </summary>
        private void UnlockAll()
        {
            button.IsEnabled = true;
            bar.IsEnabled = true;
            comboar.IsEnabled = true;
            combodist.IsEnabled = true;
            sv.IsEnabled = true;
            ap.IsEnabled = true;
            showbut.IsEnabled = true;
        }
        /// <summary>
        /// Filters data for every changed area selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionChangedAr(object sender, SelectionChangedEventArgs e)
        {
            if (comboar.SelectedValue.ToString() != null)
                cl.area_filter = comboar.SelectedValue.ToString();
            else
                MessageBox.Show("I really don't know why it' happen :C");

            try
            {
                cl.UpdateData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                comboar.SelectedIndex = prevarea;
            }

            bar.Maximum = cl.GetDatasize;
            bar.Value = cl.GetDatasize;
            prevarea = comboar.SelectedIndex;

        }
        /// <summary>
        /// Filters data for every changed district selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionChangedDst(object sender, SelectionChangedEventArgs e)
        {
            if (combodist.SelectedValue.ToString() != null)
                cl.dist_filter = combodist.SelectedValue.ToString();
            else
                MessageBox.Show("I really don't know why it' happen :C");

            try
            {
                cl.UpdateData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                combodist.SelectedIndex = prevdist;
            }

            bar.Maximum = cl.GetDatasize;
            bar.Value = cl.GetDatasize;
            prevdist = comboar.SelectedIndex;
        }
        /// <summary>
        /// Shows all the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cl.SetToDef();
            SetDefCombo();
            bar.Maximum = cl.GetMaxSize;
            bar.Value = cl.GetMaxSize;
        }
        /// <summary>
        /// Sets all selected values to default
        /// </summary>
        private void SetDefCombo()
        {
            comboar.SelectedIndex = 0;
            combodist.SelectedIndex = 0;
            prevarea = 0;
            prevdist = 0;
        }
        /// <summary>
        /// Exits from application
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void Exit(object obj, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        /// <summary>
        /// Enter handler for textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TextBox_TextEntered(this, new RoutedEventArgs());
            }
        }
    }
}
