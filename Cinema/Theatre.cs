using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Theatre
    {
        public static Dictionary<int, int> intloc = new Dictionary<int, int>() 
        {
            {0, 0},
            {18, 1},
            {19, 2}
        };
        public static List<int> toCont = new List<int>()
            { 5, 6, 7, 10, 11, 12, 15, 19, 20 };
        readonly int N;
        const int numOfIntVal = 3;
        Contacts cont;
        private string[] _values;
        public string[] Values
        {
            get { return _values; }
            set { _values = value; }
        }
        private int[] _intValues;
        public int[] IntValues {
            get { return _intValues; }
            set { _intValues = value; }
        }
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
                        if (cur[i].Trim('"') != "")
                            throw new FormatException("What is dat shit -" 
                                + cur[i].Trim('"') + "number of shit is " + i);
                        else
                            IntValues[intloc[i]] = 0;
                }
                else
                    Values[i] = cur[i].Trim('"');
            }
            cont = new Contacts(tomove);
        }
    }
}
