using Assets.Code.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Common.Objects
{
    public class Inventory
    {
        public ItemBase SelectedItem { get; private set; }
        public List<ItemBase> Items { get; private set; }

        public Inventory()
        {
            Items = new List<ItemBase>();
        }
    }
}
