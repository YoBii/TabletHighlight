using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExileCore2.PoEMemory.Components;
using ExileCore2.Shared;

namespace TabletHighlight
{
    internal enum ItemLocation {
        Inventory = 0,
        Stash = 1
    }
    internal class TabletType {
        internal static readonly string Irradiated = "Precursor Tablet";
        internal static readonly string Breach = "Breach Precursor Tablet";
        internal static readonly string Delirium = "Delirium Precursor Tablet";
        internal static readonly string Ritual = "Ritual Precursor Tablet";
        internal static readonly string Expedition = "Expedition Precursor Tablet";
        internal static readonly string Boss = "Overseer Precursor Tablet";

        internal static readonly List<String> All = new List<String> { Irradiated, Breach, Delirium, Ritual, Expedition, Boss };
    }

    internal struct TabletItem(Base baseComponent, Mods modsComponent, RectangleF rectangleF, ItemLocation location) {
        public Base baseComponent = baseComponent;
        public Mods mods = modsComponent;
        public RectangleF rect = rectangleF;
        public ItemLocation location = location;
    }
}
