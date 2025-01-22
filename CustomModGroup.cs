using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletHighlight {
    internal class CustomModGroup {
        internal List<string> Mods;
        internal bool Matched;

    internal CustomModGroup(List<string> mods) {
            Mods = mods;
            Matched = false;
        }
    }
}
