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
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        CinemaList cl;
        private int prevarea;
        private int prevdist; 
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenFileHandler(object obj, EventArgs e)
        {
            string path = null;
            try
            {
                path = MYIO.OpenCSV();
            }
            catch (FileLoadException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            List<string> rawdata = MYIO.ReadCSV(path);
            cl = new CinemaList(datagr, rawdata);
            cl.ConstructColumns();
            bar.Minimum = 1;
            bar.Maximum = cl.GetMaxSize;
            bar.Value = bar.Maximum;
            tb.Text = ((int)bar.Value).ToString();
            datagr.ItemsSource = cl.GetData;
            comboar.ItemsSource = cl.GetAreas;
            combodist.ItemsSource = cl.GetDistricts;
            //Bind to datagrid            
            UnlockAll();
            SetDefCombo();

        }

        private void SaveToNewFileHandler(object obj, EventArgs e)
        {
            cl.SortData();
            string path = MYIO.SaveCSV();
            if (path != null)
                MYIO.WriteToFile(cl.GetDefLine, cl.GetData, path);
        }
        private void AppendFileHandler(object obj, EventArgs e)
        {
            cl.SortData();
            string path = MYIO.SaveCSV();
            if (path != null)
                MYIO.AppendToFile(cl.GetDefLine, cl.GetData, path);
        }


        private void bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //N = Num
            cl.SortData();
            cl.ResizeData((int)bar.Value);
            cl.UpdateData();
            tb.Text = ((int)bar.Value).ToString();
        }

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
                MessageBox.Show("Please, enter a number that is lower than" + (int)bar.Maximum);
                return;
            }
            bar.Value = curn;

        }
        private void UnlockAll()
        {
            button.IsEnabled = true;
            bar.IsEnabled = true;
            comboar.IsEnabled = true;
            combodist.IsEnabled = true;
            sv.IsEnabled = true;
            ap.IsEnabled = true;
        }

        private void SelectionChangedAr(object sender, SelectionChangedEventArgs e)
        {
            if (comboar.SelectedValue.ToString() != null)
                cl.area_filter = comboar.SelectedValue.ToString();
            else
                MessageBox.Show("Dat shit is null :C");
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
        private void SelectionChangedDst(object sender, SelectionChangedEventArgs e)
        {
            if (combodist.SelectedValue.ToString() != null)
                cl.dist_filter = combodist.SelectedValue.ToString();
            else
                MessageBox.Show("Dat shit is null :C");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cl.SetToDef();
            SetDefCombo();
            bar.Maximum = cl.GetMaxSize;
            bar.Value = cl.GetMaxSize;
        }
        private void SetDefCombo()
        {
            comboar.SelectedIndex = 0;
            combodist.SelectedIndex = 0;
            prevarea = 0;
            prevdist = 0;
        }
    }
}
