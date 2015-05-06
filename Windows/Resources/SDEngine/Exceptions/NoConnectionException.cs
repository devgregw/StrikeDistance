using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEngine.Exceptions {
    public class NoConnectionException : Exception {
        internal NoConnectionException() : base("No network connection available") { }
    }
}
