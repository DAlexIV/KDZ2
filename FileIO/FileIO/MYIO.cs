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
    /// <summary>
    /// Contains static methods for input and output
    /// </summary>
    public static class MYIO
    {
        /// <summary>
        /// Opens csv-file
        /// </summary>
        /// <returns>Path to file</returns>
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
        /// <summary>
        /// Reads data from csv-file
        /// </summary>
        /// <param name="path"> Path for reading </param>
        /// <returns> Collection of lines</returns>
        public static List<string> ReadCSV(string path)
        {
            StreamReader file = new StreamReader(path);
            List<string> rdedstring = new List<string>();
            while (file.Peek() != -1)
                rdedstring.Add(file.ReadLine());
            return rdedstring;
        }
        /// <summary>
        /// Saves csv-file
        /// </summary>
        /// <returns> Path for saving </returns>
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
        /// <summary>
        /// Writes data and header to file
        /// </summary>
        /// <param name="header"> First line </param>
        /// <param name="data"> All other lines </param>
        /// <param name="path"> Path to write </param>
        public static void WriteToFile(string header, List<Theatre> data, string path)
        {
            string[] lines = ParseDateToStringArray(header, data);
            File.WriteAllLines(path, lines, Encoding.UTF8);
        }
        /// <summary>
        /// Appends data and header to file
        /// </summary>
        /// <param name="header"> Header line </param>
        /// <param name="data"> Data lines </param>
        /// <param name="path"> Path to write </param>
        public static void AppendToFile(string header, List<Theatre> data, string path)
        {
            string[] lines = ParseDateToStringArray(header, data);
            if (!File.Exists(path))
                MessageBox.Show("File not found!");
            else
            {
                try
                {
                    StreamWriter sr = new StreamWriter(path, true, Encoding.UTF8);
                    for (int i = 0; i < lines.Length; ++i)
                        sr.WriteLine(lines[i]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }
        /// <summary>
        /// Parses data to string 
        /// </summary>
        /// <param name="header"> Header of data</param>
        /// <param name="data"> All other data</param>
        /// <returns>Array of lines ready for writing </returns>
        private static string[] ParseDateToStringArray(string header, List<Theatre> data)
        {
            string[] lines = new string[data.Count() + 1];
            lines[0] = header;
            for (int i = 0; i < data.Count(); ++i)
            {
                for (int k = 0; k < data[i].Values.Length; ++k)
                    if (Theatre.intloc.ContainsKey(k))
                        lines[i + 1] += ("\"" + data[i].IntValues[Theatre.intloc[k]].ToString() + ";\"");
                    else
                        lines[i + 1] += ("\"" + data[i].Values[k] + ";\"");
            }
            return lines;
        }

    }
}
