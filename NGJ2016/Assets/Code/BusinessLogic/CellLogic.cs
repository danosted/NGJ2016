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
    public class Room
    {
        public int width { get; set; }
        public int height { get; set; }
        public List<CellBase> cells { get; set; }
    }
    public class CellLogic
    {
        #region Constructor
        public ManagerBase CellManager { get; private set; }
        //public List<CellBase> cells;
        public enum Compass
        {
            North = 1,
            East = 2,
            South = 3,
            West = 4,
        };

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
        public List<CellBase> CreateStandardCellGrid(int width, int height, int widthOffset = 0, int heightOffset = 0)
        {
            var cells = new List<CellBase>(width * height);
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    CellBase cell;
                    var rnd = new System.Random();
                    // Boarders
                    if (w == 0 || w == width - 1 || h == height - 1 || h == 0)
                    {
                        cell = CreateAndPlaceCellInGrid(w + widthOffset, h + heightOffset, cells, CellType.BlockedCell);
                    }
                    else
                    {
                        // Start Cell
                        if (w == 1 && h == height / 2)
                        {
                            cell = CreateAndPlaceCellInGrid(w + widthOffset, h + heightOffset, cells, CellType.StartCell);
                        }
                        else
                        {
                            // End cell
                            if (w == width - 1 && h == height / 2)
                            {
                                cell = CreateAndPlaceCellInGrid(w + widthOffset, h + heightOffset, cells, CellType.EndCell);
                            }
                            else
                            {
                                // Normal cell
                                cell = CreateAndPlaceCellInGrid(w + widthOffset, h + heightOffset, cells, CellType.NormalCell);
                            }
                        }

                    }
                    //SetCellNeighboursIterative(cell, cells, w+ widthOffset, h+ heightOffset, width, height);
                }
            }
            return cells;
        }

        public void CreateMap()
        {
            var mapLength = 4;
            var rnd = new System.Random();
            var width = rnd.Next(20, 30);
            var height = rnd.Next(10, 20);
            var widthPrev = 0;
            var heightPrev = 0;
            
            var widthOffset = 0;
            var heightOffset = 0;


            var a = CreateRandomRoom(width, height, rnd.Next(5, 10), rnd.Next(5, 10));
            var exitDirection = CreateRandomExit(a);
            for (int i = 0; i < mapLength; i++)
            {
                widthPrev = width;
                heightPrev = height;
                switch (exitDirection)
                {
                    case Compass.North:
                        width = rnd.Next(20, 30);
                        height = rnd.Next(10, 20);
                        widthOffset += (int)Math.Round((widthPrev - width) / 2.0);
                        heightOffset += heightPrev;
                        break;
                    case Compass.South:
                        width = rnd.Next(20, 30);
                        height = rnd.Next(10, 20);
                        widthOffset += (int)Math.Round((widthPrev - width) / 2.0);
                        heightOffset += -height;
                        break;
                    case Compass.West:
                        width = rnd.Next(20, 30);
                        height = rnd.Next(10, 20);
                        widthOffset += -width;
                        heightOffset += (int)Math.Round((heightPrev - height) / 2.0); ;
                        break;
                    case Compass.East:
                        width = rnd.Next(20, 30);
                        height = rnd.Next(10, 20);
                        widthOffset += widthPrev;
                        heightOffset += (int)Math.Round((heightPrev - height) / 2.0); ;
                        break;
                }
                var b = CreateRandomRoom(width, height, rnd.Next(5, 10), rnd.Next(5, 10), widthOffset, heightOffset);
                exitDirection = CreateRandomExit(b);
            }
        }

        public Compass CreateRandomExit(Room room)
        {
            var rnd = new System.Random();
            Compass direction = (Compass)Enum.GetValues(typeof(Compass)).GetValue(rnd.Next(4));
            var i = 0;
            switch (direction)
            {
                case Compass.North:
                    i = room.cells.Count() - rnd.Next(2, room.width - 2);
                    break;
                case Compass.South:
                    i = rnd.Next(2, room.width - 2);

                    break;
                case Compass.East:
                    i = room.width * rnd.Next(2, room.height - 2) - 1;

                    break;
                case Compass.West:
                    i = room.width * rnd.Next(2, room.height - 2);
                    break;
            };
            CellManager.RecyclePrefab(room.cells[i].Wall.gameObject);
            return direction;

        }
        public Room CreateRandomRoom(int width, int height, int blockCount, int supplyCount, int widthOffset = 0, int heightOffset = 0)
        {
            var grid = CreateStandardCellGrid(width, height, widthOffset, heightOffset);
            //var startCell = CellManager.StartCell;
            //var endCell = CellManager.EndCell;
            while (blockCount > 0)
            {
                for (var w = 1; w < width - 1; w++)
                {
                    var randomHeight = UnityEngine.Random.Range(0, height - 1);
                    var cell = grid.Find(c => c.transform.position.x == w+ widthOffset && c.transform.position.y == randomHeight+ heightOffset);
                    RecycleCell(cell);
                    cell = CreateAndPlaceCellInGrid(w + widthOffset, randomHeight + heightOffset, grid, CellType.BlockedCell);
                    SetCellNeighbours(cell, w + widthOffset, randomHeight + heightOffset, width + widthOffset, height + heightOffset);
                    blockCount--;
                    if (blockCount <= 0) break;
                }
            }
            while (supplyCount > 0)
            {
                for (var w = 1; w < width - 1; w++)
                {
                    var randomHeight = UnityEngine.Random.Range(0, height - 1);
                    var cell = grid.Find(c => c.transform.position.x == w + widthOffset && c.transform.position.y == randomHeight + heightOffset);

                    if (cell.Wall == null)
                    {
                        cell.Supply = ManagerCollection.Instance.GetManager(Constants.SupplyManagerName).GetPrefabFromType<SupplyBase>();
                        cell.Supply.transform.position = new Vector3(w + widthOffset, randomHeight + heightOffset, 0f);
                        supplyCount--;
                        if (supplyCount <= 0) break;
                    }
                }
            }
            var room = new Room { width = width, height = height, cells = grid };
            return room;
        }
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
            // Checks if for Blocked CellType
            if (cellType == CellType.BlockedCell)
            {
                cell.Wall = ManagerCollection.Instance.GetManager(Constants.WallManagerName).GetPrefabFromType<WallBase>();
                cell.Wall.transform.position = new Vector3(w, h, 0f);

            }
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
