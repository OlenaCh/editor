using System;
using System.Collections.Generic;
using System.IO;

namespace Graphs {
    interface GraphIOInterface {
        void fromFile(string filename);
        void toFile(string filename);
    }
}