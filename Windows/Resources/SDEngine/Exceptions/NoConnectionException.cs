using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEngine.Exceptions {
    class NoConnectionException : Exception {
        public NoConnectionException() : base("No network connection available") { }
    }
}
