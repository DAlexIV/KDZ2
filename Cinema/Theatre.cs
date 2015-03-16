using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Theatre
    {
        /// <summary>
        /// Dictionary that determines int values in data
        /// </summary>
        public static Dictionary<int, int> intloc = new Dictionary<int, int>() 
        {
            {0, 0},
            {18, 1},
            {19, 2}
        };

        /// <summary>
        /// List contains contact data columns 
        /// </summary>
        public static List<int> toCont = new List<int>() { 5, 6, 7, 10, 11, 12, 15, 19, 20 };

        readonly int N; // Number of values in line
        const int numOfIntVal = 3; // Number of int values
        Contacts cont; // Current contacts for saving

        private string[] _values; // Stores string values
        // Gets access to string values
        public string[] Values
        {
            get { return _values; }
            set { _values = value; }
        }

        private int[] _intValues; //Stores int values
        // Get access to int values
        public int[] IntValues
        {
            get { return _intValues; }
            set { _intValues = value; }
        }
        /// <summary>
        /// Constructs cinema class
        /// </summary>
        /// <param name="line"> Raw string to construct</param>
        /// <param name="N"> Number of columns </param>
        public Theatre(string line, int N)
        {
            this.N = N;
            Values = new string[N];
            IntValues = new int[numOfIntVal];

            string[] sep = { ";\"" };
            string[] cur = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            if (cur.Length != 23)
                throw new ArgumentException("Bad line! Ignored!");

            ParseLine(cur);

        }
        /// <summary>
        /// Parses line to int and string arrays
        /// </summary>
        /// <param name="cur"></param>
        private void ParseLine(string[] cur)
        {
            List<string> tomove = new List<string>();
            for (int i = 0; i < cur.Length; ++i)
            {
                if (toCont.Contains(i))
                    tomove.Add(cur[i]);
                if (intloc.ContainsKey(i))
                {
                    if (!int.TryParse(cur[i].Trim('"'), out IntValues[intloc[i]]))
                        // If it's int
                        if (cur[i].Trim('"') != "")
                            throw new FormatException("What is this -"
                                + cur[i].Trim('"') + "number of this is " + i);
                        else
                            // If it's string
                            IntValues[intloc[i]] = 0;
                }
                else
                    Values[i] = cur[i].Trim('"');
            }
            cont = new Contacts(tomove);
        }
    }
}
