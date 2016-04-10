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
        public System.Random rnd = new System.Random();
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
                    // Boarders
                    if (w == 0 || w == width - 1 || h == height - 1 || h == 0)
                    {
                        cell = CreateAndPlaceCellInGrid(w + widthOffset, h + heightOffset, cells, CellType.BlockedCell);
                    }
                    else
                    {
                        // Normal cell
                        cell = CreateAndPlaceCellInGrid(w + widthOffset, h + heightOffset, cells, CellType.NormalCell);
                                        }
                    SetCellNeighbours(cell, w, h, width, height, widthOffset,heightOffset);
                }
            }
            return cells;
        }

        public List<CellBase> CreateGoal(int width, int height, int widthOffset = 0, int heightOffset = 0)
        {
            var cells = new List<CellBase>(width * height);
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    CellBase cell;
                    // Boarders
                    if (w == 0 || w == width - 1 || h == height - 1 || h == 0)
                    {
                        cell = CreateAndPlaceCellInGrid(w + widthOffset, h + heightOffset, cells, CellType.BlockedCell);
                    }
                    else
                    {
                        if (w == 1 && h == 1)
                        {
                            cell = CreateAndPlaceCellInGrid(w + widthOffset, h + heightOffset, cells, CellType.EndCell);
                        }
                        else
                        {
                            // Normal cell
                            cell = CreateAndPlaceCellInGrid(w + widthOffset, h + heightOffset, cells, CellType.NormalCell);
                        }

                    }
                    SetCellNeighbours(cell, w, h, width, height, widthOffset, heightOffset);
                }
            }
            return cells;
        }

        public void CreateMap()
        {
            var mapLength = 10;
            var width = rnd.Next(20, 30);
            var height = rnd.Next(10, 20);
            var widthPrev = 0;
            var heightPrev = 0;
            var widthOffsetPrev = 0;
            var heightOffsetPrev = 0;
            
            var widthOffset = 0;
            var heightOffset = 0;
            Compass exitDirection;
            CellBase exitCell;
            var maxAttempts = 100;
            var nextRoomWallSize = 0;


            var map = CreateRandomRoom(width, height, rnd.Next(5, 10), rnd.Next(5, 10));
            //exitDirection = (Compass)Enum.GetValues(typeof(Compass)).GetValue(rnd.Next(4));
            //var mapPrev = map;

            while (mapLength > 0 && maxAttempts > 0)
            {
                maxAttempts--;
                widthPrev = width;
                heightPrev = height;
                widthOffsetPrev = widthOffset;
                heightOffsetPrev = heightOffset;
                exitDirection = (Compass)Enum.GetValues(typeof(Compass)).GetValue(rnd.Next(4));

                if (mapLength == 1)
                {
                    width = 3;
                    height = 3;
                }
                else
                {
                    width = rnd.Next(10, 20);
                    height = rnd.Next(10, 20);
                }

                switch (exitDirection)
                {
                    case Compass.North:
                        widthOffset += (int)Math.Round((widthPrev - width) / 2.0);
                        heightOffset += heightPrev;
                        nextRoomWallSize = width;
                        break;
                    case Compass.South:
                        widthOffset += (int)Math.Round((widthPrev - width) / 2.0);
                        heightOffset += -height;
                        nextRoomWallSize = width;
                        break;
                    case Compass.West:
                        widthOffset += -width;
                        heightOffset += (int)Math.Round((heightPrev - height) / 2.0);
                        nextRoomWallSize = height;
                        break;
                    case Compass.East:
                        widthOffset += widthPrev;
                        heightOffset += (int)Math.Round((heightPrev - height) / 2.0);
                        nextRoomWallSize = height;
                        break;
                };
                if (TestNewRoomArea(width, height, widthOffset, heightOffset) == false)
                {
                    widthOffset = widthOffsetPrev;
                    heightOffset = heightOffsetPrev;
                    width = widthPrev;
                    height = heightPrev;
                    continue;
                }
  
                // Laver en random udgang
                CreateRandomExit(map, exitDirection, nextRoomWallSize,out exitCell);
                //mapPrev = map;
                if (mapLength > 1)
                {
                    map = CreateRandomRoom(width, height, rnd.Next(5, 10), rnd.Next(5, width), widthOffset, heightOffset);
                }
                else
                {
                    switch (exitDirection)
                    {
                        case Compass.North:
                            widthOffset = (int) exitCell.transform.position.x - 1;
                            heightOffset = (int)exitCell.transform.position.y + 1;
                            break;
                        case Compass.South:
                            widthOffset = (int)exitCell.transform.position.x - 1;
                            heightOffset = (int)exitCell.transform.position.y - 3;
                            break;
                        case Compass.West:
                            widthOffset = (int)exitCell.transform.position.x - 3;
                            heightOffset = (int)exitCell.transform.position.y - 1;
                            break;
                        case Compass.East:
                            widthOffset = (int)exitCell.transform.position.x + 1;
                            heightOffset = (int)exitCell.transform.position.y - 1;
                            break;
                    };
                    CreateRandomRoom(width, height, 0, 0, widthOffset, heightOffset,true);
                }
                //CreateBranchRooms(mapPrev,widthOffset,heightOffset);
                switch (exitDirection)
                {
                    case Compass.North:
                        CreateEntrance(exitCell.NeighbourUp);
                        break;
                    case Compass.South:
                        CreateEntrance(exitCell.NeighbourDown);
                        break;
                    case Compass.West:
                        CreateEntrance(exitCell.NeighbourLeft);
                        break;
                    case Compass.East:
                        CreateEntrance(exitCell.NeighbourRight);
                        break;
                };
                mapLength--;

            }
        }

       
        public bool TestNewRoomArea(int width, int height,int widthOffset, int heightOffset)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            for (int i = 0; i < width; i++)
            {
                // Checks bottom boarder
                if(cells.Find(c => c.transform.position.x == widthOffset+i && c.transform.position.y == heightOffset) != null)
                {
                    return false;
                }
                // Check top boader
                if (cells.Find(c => c.transform.position.x == widthOffset + i && c.transform.position.y == heightOffset+height-1) != null)
                {
                    return false;
                }
            }
            for(int i = 0; i < height; i++)
            {
                // Checks west boarder
                if (cells.Find(c => c.transform.position.x == widthOffset && c.transform.position.y == heightOffset+i) != null)
                {
                    return false;
                }
                // Checks east boarder
                if (cells.Find(c => c.transform.position.x == widthOffset + width-1 && c.transform.position.y == heightOffset + i) != null)
                {
                    return false;
                }
            }
            return true;
        }
        public void CreateEntrance(CellBase cell)
        {
            CellManager.RecyclePrefab(cell.Wall.gameObject);
        }
        
        public void CreateRandomExit(Room room, Compass direction, int nextRoomWallSize, out CellBase exitCell)
        {
            var i = 0;
            var offsetForExit = 0;
            var random = 0;
            Debug.Log("Exit. Width = " + room.width + " - Height = " + room.height + " - nextRoomWallSize = " + nextRoomWallSize + " - Direction = " + direction.ToString());
            switch (direction)
            {
                case Compass.North:
                    offsetForExit = (Math.Abs(room.width - nextRoomWallSize) / 2)+3;
                    if (offsetForExit > room.width - offsetForExit)
                    {
                        random = offsetForExit;
                    }
                    else
                    {
                        random = rnd.Next(offsetForExit, room.width - offsetForExit);
                    }
                    i = room.cells.Count() - random;
                    break;
                case Compass.South:
                    offsetForExit = (Math.Abs(room.width - nextRoomWallSize) / 2) + 2;
                    if (offsetForExit > room.width - offsetForExit)
                    {
                        random = offsetForExit;
                    }
                    else
                    {
                        random = rnd.Next(offsetForExit, room.width - offsetForExit);
                    }
                    i = random;
                    break;
                case Compass.East:
                    offsetForExit = (Math.Abs(room.height - nextRoomWallSize) / 2) + 2;
                    if (offsetForExit > room.height - offsetForExit)
                    {
                        random = offsetForExit;
                    }
                    else
                    {
                        random = rnd.Next(offsetForExit, room.height - offsetForExit);
                    }
                    i = room.width * (random) - 1;
                    break;
                case Compass.West:
                    offsetForExit = (Math.Abs(room.height - nextRoomWallSize) / 2) + 2;
                    if (offsetForExit > room.height - offsetForExit)
                    {
                        random = offsetForExit;
                    }
                    else
                    {
                        random = rnd.Next(offsetForExit, room.height - offsetForExit);
                    }
                    i = room.width * random;
                    break;
            };
            Debug.Log("Random = "+random+" - i = "+i);
            CellManager.RecyclePrefab(room.cells[i].Wall.gameObject);
            exitCell = room.cells[i];

        }
        public Room CreateRandomRoom(int width, int height, int blockCount, int supplyCount, int widthOffset = 0, int heightOffset = 0, bool GoalArea = false)
        {
            var grid = new List<CellBase>();
            if (!GoalArea)
            {
                grid = CreateStandardCellGrid(width, height, widthOffset, heightOffset);
            }else
            {
                grid = CreateGoal(width, height, widthOffset, heightOffset);
            }
            //var startCell = CellManager.StartCell;
            //var endCell = CellManager.EndCell;

            //while (blockCount > 0)
            //{
            //    for (var w = 1; w < width - 1; w++)
            //    {
            //        var randomHeight = UnityEngine.Random.Range(2, height - 1);
            //        var cell = grid.Find(c => c.transform.position.x == w + widthOffset && c.transform.position.y == randomHeight + heightOffset);
            //        RecycleCell(cell);
            //        cell = CreateAndPlaceCellInGrid(w + widthOffset, randomHeight + heightOffset, grid, CellType.BlockedCell);
            //        SetCellNeighbours(cell, w + widthOffset, randomHeight + heightOffset, widthOffset, heightOffset);
            //        blockCount--;
            //        if (blockCount <= 0) break;
            //    }
            //}
            while (supplyCount > 0)
            {
                for (var w = 1; w < width - 1; w++)
                {
                    var randomHeight = UnityEngine.Random.Range(2, height - 1);
                    var cell = grid.Find(c => c.transform.position.x == w + widthOffset && c.transform.position.y == randomHeight + heightOffset);

                    if (cell.Wall == null)
                    {
                        cell.Supply = ManagerCollection.Instance.GetManager(Constants.SupplyManagerName).GetRandomPrefabFromType<SupplyBase>();
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
            var cell = CellManager.GetRandomPrefabFromType<CellBase>();
            cell.transform.position = new Vector3(w, h, 0f);
            // Checks if for Blocked CellType
            if (cellType == CellType.BlockedCell)
            {
                cell.Wall = ManagerCollection.Instance.GetManager(Constants.WallManagerName).GetPrefabFromType<WallBase>();
                cell.Wall.transform.position = new Vector3(w, h, 0f);

            };
            if (cellType == CellType.EndCell)
            {
                cell.Toilet = ManagerCollection.Instance.GetManager(Constants.ToiletManager).GetPrefabFromType<ToiletBase>();
                cell.Toilet.transform.position = new Vector3(w,h,0f);
            }
            cells.Add(cell);
            return cell;
        }

        private void SetCellNeighbours(CellBase cell, int w, int h, int width, int height, int widthOffset = 0, int heightOffset = 0)
        {
            SetBelowCellRelation(cell, w + widthOffset, h + heightOffset);
            SetLeftCellRelation(cell, w + widthOffset, h + heightOffset);
            SetAboveCellRelation(cell, w + widthOffset, h + heightOffset);
            SetRightCellRelation(cell, w + widthOffset, h + heightOffset);

            // Pathfinding
            cell.Distance = int.MaxValue;
            cell.Previous = null;

        }

        private void SetRightCellRelation(CellBase cell, int w, int h)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            var rightCell = cells.Find(c => c.transform.position.x == w + 1 && c.transform.position.y == h);
            if (rightCell == null)
                return;
            cell.NeighbourRight = rightCell;
            cell.NeighBours.Add(rightCell);

            rightCell.NeighbourLeft = cell;
            rightCell.NeighBours.Add(cell);
        }

        private void SetLeftCellRelation(CellBase cell, int w, int h)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            var leftCell = cells.Find(c => c.transform.position.x == w - 1 && c.transform.position.y == h);
            if (leftCell == null)
                return;
            cell.NeighbourLeft = leftCell;
            cell.NeighBours.Add(leftCell);

            leftCell.NeighbourRight = cell;
            leftCell.NeighBours.Add(cell);
        }

        private void SetBelowCellRelation(CellBase cell, int w, int h)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            var belowCell = cells.Find(c => c.transform.position.x == w && c.transform.position.y == h - 1);
            if (belowCell == null)
            {
                return;
            }
            cell.NeighbourDown = belowCell;
            cell.NeighBours.Add(belowCell);

            belowCell.NeighbourUp = cell;
            belowCell.NeighBours.Add(cell);
        }

        private void SetAboveCellRelation(CellBase cell, int w, int h)
        {
            var cells = CellManager.GetAllActiveObjects<CellBase>();
            var aboveCell = cells.Find(c => c.transform.position.x == w && c.transform.position.y == h + 1);
            if (aboveCell == null)
                return;
            cell.NeighbourUp = aboveCell;
            cell.NeighBours.Add(aboveCell);

            aboveCell.NeighbourDown = cell;
            aboveCell.NeighBours.Add(cell);
        }
        #endregion
    }
}
