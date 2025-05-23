﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2025, Jiaqi (0x7c13) Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Game.Command.Extensions
{
    using Core.Command;

    [AvailableInConsole]
    public sealed class SceneSaveGlobalObjectSwitchStateCommand : ICommand
    {
        public SceneSaveGlobalObjectSwitchStateCommand(string cityName,
            string sceneName,
            int objectId,
            byte switchState)
        {
            CityName = cityName;
            SceneName = sceneName;
            ObjectId = objectId;
            SwitchState = switchState;
        }

        public string CityName { get; }
        public string SceneName { get; }
        public int ObjectId { get; }
        public byte SwitchState { get; }
    }
}