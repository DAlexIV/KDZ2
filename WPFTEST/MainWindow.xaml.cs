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
        public MainWindow()
        {
            InitializeComponent();


        }
        private void OpenFileHandler(object obj, EventArgs e)
        {
            string path = MYIO.OpenCSV();
            List<string> rawdata = MYIO.ReadCSV(path);
            cl = new CinemaList(datagr, rawdata);
            cl.ConstructColumns();
            bar.Minimum = 1;
            bar.Maximum = cl.GetMaxSize;
            bar.Value = bar.Maximum;
            tb.Text = ((int)bar.Value).ToString();
            datagr.ItemsSource = cl.GetData;
            //Bind to datagrid            
            UnlockAll();
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
            combo.ItemsSource = cl.GetAreas;
        }

        private void TextBox_TextEntered(object sender, RoutedEventArgs e)
        {
            int curn;
            if (!int.TryParse(tb.Text, out curn))
            {
                MessageBox.Show("Please, enter a number!");
                return;
            }
            if (curn < 0)
            {
                MessageBox.Show("Please, enter a positive number!");
                return;
            }
            if (curn > (int)bar.Maximum)
            {
                MessageBox.Show("Please, enter a number that is lower than N - " + (int)bar.Maximum);
                return;
            }
            bar.Value = curn;

        }
        private void UnlockAll()
        {
            button.Visibility = Visibility.Visible;
            bar.Visibility = Visibility.Visible;
            tb.Visibility = Visibility.Visible;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cl.area_filter = combo.SelectedItem.ToString();
            cl.UpdateData();
        }
    }
}
