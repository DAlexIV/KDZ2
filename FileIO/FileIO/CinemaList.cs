using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using Cinema;
namespace FileIO
{
    public static class MYIO
    {
        public static string OpenCSV()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "Text documents (.csv)|*.csv"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
                // Open document
                return dlg.FileName;
            else
                throw new FileLoadException("File wasn't opened!");

        }
        public static List<string> ReadCSV(string path)
        {
            StreamReader file = new StreamReader(path);
            List<string> rdedstring = new List<string>();
            while (file.Peek() != -1)
                rdedstring.Add(file.ReadLine());
            return rdedstring;
        }
        public static string SaveCSV()
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "Text documents (.csv)|*.csv"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                return dlg.FileName;
            }
            else
            {
                MessageBox.Show("File for saving wasn't choosen");
                return null;
            }
        }
        public static void WriteToFile(string header, List<Theatre> data, string path)
        {
            string[] lines = ParseDateToStringArray(header, data);
            File.WriteAllLines(path, lines, Encoding.UTF8);
        }
        public static void AppendToFile(string header, List<Theatre> data, string path)
        {
            string[] lines = ParseDateToStringArray(header, data);
            if (!File.Exists(path))
                MessageBox.Show("File not found, WTF?");
            else
            {
                StreamWriter sr = new StreamWriter(path, true, Encoding.UTF8);
                for (int i = 0; i < lines.Length; ++i)
                    sr.WriteLine(lines[i]);
            }

        }
        private static string[] ParseDateToStringArray(string header, List<Theatre> data)
        {
            string[] lines = new string[data.Count() + 1];
            lines[0] = header;
            for (int i = 0; i < data.Count(); ++i)
            {
                for (int k = 0; k < data[i].Values.Length; ++k)
                    if (Theatre.intloc.ContainsKey(k))
                        lines[i + 1] += "\"" + data[i].IntValues[Theatre.intloc[k]].ToString() + ";\"";
                    else
                        lines[i + 1] += "\"" + data[i].Values[k] + ";\"";
            }
            return lines;
        }

    }
}
