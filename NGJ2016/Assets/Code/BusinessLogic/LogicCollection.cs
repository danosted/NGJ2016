using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.BusinessLogic
{
    public class LogicCollection
    {
        private static LogicCollection _logicCollection;
        public static LogicCollection Instance
        {
            get
            {
                if (_logicCollection == null)
                {
                    _logicCollection = new LogicCollection();
                }
                return _logicCollection;
            }
        }

        private CellLogic _cellLogic;
        public CellLogic CellLogic
        {
            get
            {
                if (_cellLogic == null)
                {
                    _cellLogic = new CellLogic();
                }
                return _cellLogic;
            }
        }

        private SelectionLogic _selectionLogic;
        public SelectionLogic SelectionLogic
        {
            get
            {
                if (_selectionLogic == null)
                {
                    _selectionLogic = new SelectionLogic();
                }
                return _selectionLogic;
            }
        }
    }
}
