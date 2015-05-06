using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEngine.Exceptions {
    public class TooSoonException : Exception {
        public int RequestedDifference {
            get; internal set;
        }

        public DateTime CurrentTime {
            get; internal set;
        }

        public DateTime PreviousTime {
            get; internal set;
        }

        internal TooSoonException(int reqTime, DateTime prevTime) : base("Operation requested within the disabled period") {
            RequestedDifference = reqTime;
            CurrentTime = DateTime.Now;
            PreviousTime = prevTime;
        }
    }
}
