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
            var characterManager = ManagerCollection.Instance.GetManager(Constants.CharacterManagerName);
            var pl = characterManager.GetPrefabFromType<PlayerBase>();
            pl.Init();
            var pukeManager = ManagerCollection.Instance.GetManager(Constants.PukeManager);
            var pk = pukeManager.GetPrefabFromType<PukeBase>();
            pk.SourceCharacter = pl;
            var fartManager = ManagerCollection.Instance.GetManager(Constants.FartManagerName);
            var fa = fartManager.GetPrefabFromType<FartBase>();
            fa.SourceCharacter = pl;

            
            var npc = characterManager.GetPrefabFromType<NpcBase>();
            npc.Init();

            ManagerCollection.Instance.MovementHandler.MoveCharacterContinuous(npc);

            ManagerCollection.Instance.MovementHandler.MoveCharacterContinuous(pl);
            ManagerCollection.Instance.ProjectileHandler.CheckForPukeAndFartContinous(pk,fa);
            //LogicCollection.Instance.CellLogic.CreateStandardCellGrid(3, 3);
            LogicCollection.Instance.CellLogic.CreateMap();
            ManagerCollection.Instance.UIHandler.SetCharacter(pl);

            
            //var cells = ManagerCollection.Instance.GetManager(Constants.CellManagerName).GetAllActiveObjects<CellBase>();
            //cells[2].Supply = ManagerCollection.Instance.GetManager(Constants.SupplyManagerName).GetPrefabFromType<SupplyBase>();
            //cells[2].Supply.transform.position = cells[2].transform.position; 
        }
    }
}
