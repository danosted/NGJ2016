using Assets.Code.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Common.BaseClasses
{
    public class CellBase : MonoBehaviour
    {
        public string Name { get; set; }
        public SupplyBase Ressource { get; set; }

        #region General Properties
        [SerializeField]
        private CellBase _neighbourUp;
        public CellBase NeighbourUp
        {
            get { return _neighbourUp; }
            set { _neighbourUp = value; }
        }

        [SerializeField]
        private CellBase _neighbourDown;
        public CellBase NeighbourDown
        {
            get { return _neighbourDown; }
            set { _neighbourDown = value; }
        }

        [SerializeField]
        private CellBase _neighbourRight;
        public CellBase NeighbourRight
        {
            get { return _neighbourRight; }
            set { _neighbourRight = value; }
        }

        [SerializeField]
        private CellBase _neighbourLeft;
        public CellBase NeighbourLeft
        {
            get { return _neighbourLeft; }
            set { _neighbourLeft = value; }
        }

        [SerializeField]
        private SupplyBase _supply;
        public SupplyBase Supply
        {
            get { return _supply; }
            set { _supply = value; }
        }
        #endregion

        #region Graph Properties
        private int? _distance;
        public int? Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        [SerializeField]
        private CellBase _previous;
        public CellBase Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }

        private HashSet<CellBase> _neighBours;
        public HashSet<CellBase> NeighBours
        {
            get
            {
                if (_neighBours == null)
                {
                    _neighBours = new HashSet<CellBase>();
                }
                return _neighBours;
            }
            set { _neighBours = value; }
        }

        [SerializeField]
        private CellBase _nextCellTowardsGoal;
        public CellBase NextCellTowardsGoal
        {
            get
            {
                return _nextCellTowardsGoal;
            }
            set { _nextCellTowardsGoal = value; }
        }
        #endregion

        #region Selection
        void OnMouseDown()
        {
            LogicCollection.Instance.SelectionLogic.EntitySelected(this);
        }
        #endregion
    }
}
