using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Contacts
    {
        private string[] _cont;
        public string[] GetCont
        {
            get
            {
                return _cont;
            }
        }
        public Contacts(List<string> stck)
        {   
            _cont = stck.ToArray();
        }

    }
}
