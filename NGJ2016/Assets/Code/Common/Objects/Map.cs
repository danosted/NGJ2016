using Assets.Code.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Common.Objects
{
    public class Map
    {
        public string Name { get; set; }
        public List<CellBase> Nodes { get; set; }
    }
}
