using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    /// <summary>
    /// Strores contacts 
    /// </summary>
    class Contacts
    {
        // Array for getting values from class
        private string[] _cont;
        /// <summary>
        /// Access to _cont
        /// </summary>
        public string[] GetCont
        {
            get
            {
                return _cont;
            }
        }
        /// <summary>
        /// Sets data to _cont
        /// </summary>
        /// <param name="stck"> List of string data </param>
        public Contacts(List<string> stck)
        {   
            _cont = stck.ToArray();
        }

    }
}
