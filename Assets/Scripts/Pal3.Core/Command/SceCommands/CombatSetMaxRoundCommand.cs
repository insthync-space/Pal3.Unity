﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2025, Jiaqi (0x7c13) Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Core.Command.SceCommands
{
    [SceCommand(82, "设置战斗的最大回合数")]
    public sealed class CombatSetMaxRoundCommand : ICommand
    {
        public CombatSetMaxRoundCommand(int maxRound)
        {
            MaxRound = maxRound;
        }

        public int MaxRound { get; }
    }
}