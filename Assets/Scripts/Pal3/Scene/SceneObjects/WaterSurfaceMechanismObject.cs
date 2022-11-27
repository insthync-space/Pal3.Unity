﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2022, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Scene.SceneObjects
{
    using System;
    using System.Collections;
    using Command;
    using Command.SceCommands;
    using Common;
    using Core.Animation;
    using Core.DataReader.Nav;
    using Core.DataReader.Scn;
    using Core.GameBox;
    using Core.Services;
    using Data;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [ScnSceneObject(ScnSceneObjectType.WaterSurfaceMechanism)]
    public class WaterSurfaceMechanismObject : SceneObject
    {
        private SceneManager _sceneManager;
        private WaterSurfaceMechanismObjectController _surfaceMechanismObjectController;
        
        public WaterSurfaceMechanismObject(ScnObjectInfo objectInfo, ScnSceneInfo sceneInfo)
            : base(objectInfo, sceneInfo)
        {
            _sceneManager = ServiceLocator.Instance.Get<SceneManager>();
        }
        
        public override GameObject Activate(GameResourceProvider resourceProvider, Color tintColor)
        {
            if (Activated) return GetGameObject();
            GameObject sceneGameObject = base.Activate(resourceProvider, tintColor);
            _surfaceMechanismObjectController = sceneGameObject.AddComponent<WaterSurfaceMechanismObjectController>();
            _surfaceMechanismObjectController.Init(this);
            
            UpdateTileMapIfConditionApply(true);
            
            return sceneGameObject;
        }
        
        public override void Interact(bool triggerredByPlayer)
        {
            if (!IsInteractableBasedOnTimesCount()) return;

            if (_surfaceMechanismObjectController != null)
            {
                _surfaceMechanismObjectController.Interact();
            }
        }

        public override void Deactivate()
        {
            UpdateTileMapIfConditionApply(false);
            
            if (_surfaceMechanismObjectController != null)
            {
                Object.Destroy(_surfaceMechanismObjectController);
            }
            
            base.Deactivate();
        }

        // 仙三霹雳堂水面机关对TileMap地砖通过性的特殊处理:
        // * 当水面机关开启之前，需要将地砖类型为土壤的地砖设置为不可通过
        // * 当水面机关开启之后，需要将地砖类型为土壤的地砖设置为可通过
        private void UpdateTileMapIfConditionApply(bool setSoilFloorAsObstacle)
        {
            #if PAL3
            if (string.Equals(SceneInfo.CityName, "m09", StringComparison.OrdinalIgnoreCase))
            {
                _sceneManager.GetCurrentScene()
                    .GetTilemap()
                    .MarkFloorKindAsObstacle(NavFloorKind.Soil, setSoilFloorAsObstacle);
            }
            #endif
        }
    }

    internal class WaterSurfaceMechanismObjectController : MonoBehaviour
    {
        private const float WATER_ANIMATION_DURATION = 4f;
        
        private WaterSurfaceMechanismObject _surfaceMechanismObject;
        
        public void Init(WaterSurfaceMechanismObject surfaceMechanismObject)
        {
            _surfaceMechanismObject = surfaceMechanismObject;
        }
        
        public void Interact()
        {
            StartCoroutine(InteractInternal());
        }

        private IEnumerator InteractInternal()
        {
            Vector3 finalPosition = gameObject.transform.position;
            finalPosition.y = GameBoxInterpreter.ToUnityYPosition(_surfaceMechanismObject.ObjectInfo.Parameters[0]);
            
            CommandDispatcher<ICommand>.Instance.Dispatch(new PlaySfxCommand("wc007", 1));
            
            yield return AnimationHelper.MoveTransform(gameObject.transform,
                finalPosition,
                WATER_ANIMATION_DURATION,
                AnimationCurveType.Sine);
            
            _surfaceMechanismObject.ChangeActivationState(false);
        }
    }
}