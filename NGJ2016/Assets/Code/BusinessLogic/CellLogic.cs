using Assets.Code.Common;
using Assets.Code.Common.BaseClasses;
using Assets.Code.Common.Enums;
using Assets.Code.PrefabAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.BusinessLogic
{
    public class CellLogic
    {
        #region Constructor
        public ManagerBase CellManager { get; private set; }
        public CellLogic()
        {
            CellManager = ManagerCollection.Instance.GetManager(Constants.CellManagerName);
        }
        #endregion

        #region Helpers
        public HashSet<CellBase> GetNeighbourCellsWithoutCells(CellBase cell, List<CellBase> excludeCells)
        {
            return new HashSet<CellBase>(cell.NeighBours.Where(n => !excludeCells.Contains(n)));
        }

        //public CellType GetCellTypeFromCell(CellBase cell)
        //{
        //    var cellType = cell.GetType();
        //    if (cellType == typeof(StartCell))
        //    {
        //        return CellType.StartCell;
        //    }
        //    if (cellType == typeof(EndCell))
        //    {
        //        return CellType.EndCell;
        //    }
        //    if (cellType == typeof(NormalCell))
        //    {
        //        return CellType.NormalCell;
        //    }
        //    if (cellType == typeof(BlockedCell))
        //    {
        //        return CellType.BlockedCell;
        //    }
        //    var msg = string.Format(LogUtil.FormatClassAndMethodNames(ClassName, "GetCellTypeFromCell") + "CellType not recognized '{0}'.", cellType);
        //    throw new UnityException(msg);
        //}
        #endregion

        #region Create Grid
        public List<CellBase> CreateStandardCellGrid(int width, int height)
        {
            var cells = new List<CellBase>(width * height);
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    CellBase cell;
                    if (w == 0 && h == height / 2)
                    {
                        // Start cell
                        cell = CreateAndPlaceCellInGrid(w, h, cells, CellType.StartCell);
                    }
                    else if (w == width - 1 && h == height / 2)
                    {
                        // End cell
                        cell = CreateAndPlaceCellInGrid(w, h, cells, CellType.EndCell);
                    }
                    else
                    {
                        // Normal cell
                        cell = CreateAndPlaceCellInGrid(w, h, cells, CellType.NormalCell);
                    }
                    SetCellNeighboursIterative(cell, cells, w, h, width, height);
                }
            }
            return cells;
        }

        //public List<CellBase> CreateRandomCellGrid(int width, int height, int blockCount)
        //{
        //    var cellManager = ManagerCollection.Instance.CellManager;
        //    var grid = CreateStandardCellGrid(width, height);
        //    var startCell = cellManager.StartCell;
        //    var endCell = cellManager.EndCell;
        //    while (blockCount > 0)
        //    {
        //        for (var w = 1; w < width - 1; w++)
        //        {
        //            var randomHeight = Random.Range(0, height - 1);
        //            var cell = grid.Find(c => c.transform.position.x == w && c.transform.position.y == randomHeight);
        //            if (cell == null) { throw new UnityException("Cell was not found."); }
        //            if (!LogicCollection.Instance.GraphLogic.IsPathAvailableOnCellBlocked(cell)) continue;
        //            if (cell == startCell || cell == endCell)
        //            {
        //                Debug.LogError("Cell was Start or End Cell - NOT GOOD.");
        //            }
        //            RecycleCell(cell);
        //            cell = CreateAndPlaceCellInGrid(w, randomHeight, grid, cellManager, CellType.BlockedCell);
        //            SetCellNeighbours(cell, w, randomHeight, width, height);
        //            blockCount--;
        //            if (blockCount <= 0) break;
        //        }
        //    }
        //    LogicCollection.Instance.GraphLogic.SetShortestPathFromCellToGoalFromCells(cellManager.StartCell, cellManager.EndCell, grid);
        //    return grid;
        //}
        #endregion

        #region Recycling
        private void RecycleCell(CellBase cell)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            // Remove from grid
            cells.Remove(cell);

            // Neighbour up
            RemoveCellFromNeighbours(cell, cell.NeighbourUp);

            // Neighbour down
            RemoveCellFromNeighbours(cell, cell.NeighbourDown);

            // Neighbour right
            RemoveCellFromNeighbours(cell, cell.NeighbourRight);

            // Neighbour left
            RemoveCellFromNeighbours(cell, cell.NeighbourLeft);

            // Recycle
            CellManager.RecyclePrefab(cell.gameObject);
        }

        private void RemoveCellFromNeighbours(CellBase cellToRemove, CellBase cellRef)
        {
            if (cellRef == null) return;
            if (!cellRef.NeighBours.Contains(cellToRemove)) return;
            cellRef.NeighBours.Remove(cellToRemove);
        }
        #endregion

        #region Private methods
        private CellBase CreateAndPlaceCellInGrid(int w, int h, List<CellBase> cells, CellType cellType)
        {
            var cell = CellManager.GetPrefabFromType<CellBase>();
            cell.transform.position = new Vector3(w, h, 0f);
            cells.Add(cell);
            return cell;
        }

        private void SetCellNeighbours(CellBase cell, int w, int h, int width, int height)
        {
            // For all left-most
            if (w == 0 && 0 < h)
            {
                SetBelowCellRelation(cell, w, h);
                SetAboveCellRelation(cell, w, h);
                SetRightCellRelation(cell, w, h);
            }

            // For all bottom-most  TODO 1 (DRO)
            if (0 < w && h == 0)
            {
                SetLeftCellRelation(cell, w, h);
                SetAboveCellRelation(cell, w, h);
                SetRightCellRelation(cell, w, h);
            }

            // For all right-most
            if (w == width - 1 && 0 < h)
            {
                SetBelowCellRelation(cell, w, h);
                SetLeftCellRelation(cell, w, h);
                SetAboveCellRelation(cell, w, h);
            }

            // For all top-most
            if (0 < w && h == height - 1)
            {
                SetBelowCellRelation(cell, w, h);
                SetLeftCellRelation(cell, w, h);
                SetRightCellRelation(cell, w, h);
            }

            // For all in the middle
            if (0 < h && 0 < w && h < height - 1 && w < width - 1)
            {
                SetBelowCellRelation(cell, w, h);
                SetLeftCellRelation(cell, w, h);
                SetAboveCellRelation(cell, w, h);
                SetRightCellRelation(cell, w, h);
            }

            // Pathfinding
            cell.Distance = int.MaxValue;
            cell.Previous = null;

        }

        private void SetRightCellRelation(CellBase cell, int w, int h)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            var rightCell = cells.Find(c => c.transform.position.x == w + 1 && c.transform.position.y == h);
            cell.NeighbourRight = rightCell;
            cell.NeighBours.Add(rightCell);

            rightCell.NeighbourLeft = cell;
            rightCell.NeighBours.Add(cell);
        }

        private void SetLeftCellRelation(CellBase cell, int w, int h)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            var leftCell = cells.Find(c => c.transform.position.x == w - 1 && c.transform.position.y == h);
            cell.NeighbourLeft = leftCell;
            cell.NeighBours.Add(leftCell);

            leftCell.NeighbourRight = cell;
            leftCell.NeighBours.Add(cell);
        }

        private void SetBelowCellRelation(CellBase cell, int w, int h)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            var belowCell = cells.Find(c => c.transform.position.x == w && c.transform.position.y == h - 1);
            cell.NeighbourDown = belowCell;
            cell.NeighBours.Add(belowCell);

            belowCell.NeighbourUp = cell;
            belowCell.NeighBours.Add(cell);
        }

        private void SetAboveCellRelation(CellBase cell, int w, int h)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            var aboveCell = cells.Find(c => c.transform.position.x == w && c.transform.position.y == h + 1);
            cell.NeighbourUp = aboveCell;
            cell.NeighBours.Add(aboveCell);

            aboveCell.NeighbourDown = cell;
            aboveCell.NeighBours.Add(cell);
        }

        private void SetCellNeighboursIterative(CellBase cell, List<CellBase> cells, int w, int h, int width, int height)
        {
            // For all left-most
            if (w == 0 && 0 < h)
            {
                var belowCell = cells[width * (h - 1) + w];
                cell.NeighbourDown = belowCell;
                cell.NeighBours.Add(belowCell);

                belowCell.NeighbourUp = cell;
                belowCell.NeighBours.Add(cell);
            }

            // For all bottom-most
            if (0 < w && h == 0)
            {
                var leftCell = cells[width * h + (w - 1)];
                cell.NeighbourLeft = leftCell;
                cell.NeighBours.Add(leftCell);

                leftCell.NeighbourRight = cell;
                leftCell.NeighBours.Add(cell);
            }

            // For all top right
            if (0 < h && 0 < w)
            {
                var belowCell = cells[width * (h - 1) + w];
                cell.NeighbourDown = belowCell;
                cell.NeighBours.Add(belowCell);

                belowCell.NeighbourUp = cell;
                belowCell.NeighBours.Add(cell);

                var leftCell = cells[width * h + (w - 1)];
                cell.NeighbourLeft = leftCell;
                cell.NeighBours.Add(leftCell);

                leftCell.NeighbourRight = cell;
                leftCell.NeighBours.Add(cell);
            }

            // Pathfinding
            cell.Distance = int.MaxValue;
            cell.Previous = null;
        }
        #endregion
    }
}
