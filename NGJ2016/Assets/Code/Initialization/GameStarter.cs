using Assets.Code.BusinessLogic;
using Assets.Code.Common;
using Assets.Code.Common.BaseClasses;
using Assets.Code.PrefabAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Initialization
{
    public class GameStarter : MonoBehaviour
    {
        void Awake()
        {
            var man = ManagerCollection.Instance.GetManager(Constants.CharacterManagerName);
            var pl = man.GetPrefabFromType<CharacterBase>();
            pl.Init();
            ManagerCollection.Instance.MovementHandler.MoveCharacterContinuous(pl);

            LogicCollection.Instance.CellLogic.CreateStandardCellGrid(3, 3);
            var cells = ManagerCollection.Instance.GetManager(Constants.CellManagerName).GetAllActiveObjects<CellBase>();
            cells[2].Supply = ManagerCollection.Instance.GetManager(Constants.SupplyManagerName).GetPrefabFromType<SupplyBase>();
            cells[2].Supply.transform.position = cells[2].transform.position; 
        }
    }
}
