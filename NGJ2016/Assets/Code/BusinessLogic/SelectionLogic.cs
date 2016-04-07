using Assets.Code.Common.BaseClasses;
using UnityEngine;
namespace Assets.Code.BusinessLogic
{
    public class SelectionLogic
    {

        #region Properties
        private object _currentSelection;
        public object CurrentSelection
        {
            get
            {
                return _currentSelection;
            }
            set
            {
                _currentSelection = value;
            }
        }
        #endregion

        internal void EntitySelected(object selectedEntity)
        {
            var msg = string.Format("Selected. Entity '{0}'.", selectedEntity);
            Debug.Log(msg);
            if(selectedEntity == null)
            {
                msg = string.Format("Deselected.");
                Debug.Log(msg);
                CurrentSelection = null;
                return;
            }
            var entityType = selectedEntity.GetType();
            if (entityType.IsSubclassOf(typeof(CellBase)))
            {
                var cell = (CellBase)selectedEntity;
                if (cell == null)
                {
                    msg = string.Format("Cell is null.");
                    Debug.LogError(msg);
                }
                CellSelected(cell);
            }
        }

        private void CellSelected(CellBase cell)
        {
            var msg = string.Format("Not implemented.");
            Debug.LogWarning(msg);
        }

    }
}
