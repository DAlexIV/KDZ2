using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Contacts
    {
        string[] cont;
        public Contacts(List<string> stck)
        {   
            cont = stck.ToArray();
        }

    }
}
