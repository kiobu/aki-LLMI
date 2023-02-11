﻿using Aki.Reflection.Patching;
using Aki.Reflection.Utils;
using System.Linq;
using System.Reflection;
using EFT;
using Comfort.Common;

namespace LightkeeperDoor
{
    class LLMIPatch : ModulePatch
    {
        private static string LocalPlayer() => GamePlayerOwner.MyPlayer.ProfileId;
        protected override MethodBase GetTargetMethod()
        {
            var desiredType = PatchConstants.EftTypes.Single(x => x.Name == "GameWorld");
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            var desiredMethod = desiredType.GetMethod("OnGameStarted", flags);

            Logger.LogDebug($"{this.GetType().Name} Type: {desiredType?.Name}");
            Logger.LogDebug($"{this.GetType().Name} Method: {desiredMethod?.Name}");

            return desiredMethod;
        }
        [PatchPrefix]
        private static bool PatchPrefix()
        {
            // Local player.
            var player = LocalPlayer();

            // For now, player is always allowed to enter Lightkeeper's room.
            var status = true;

            // Set access status for local player.
            var doorAccess = Singleton<GameWorld>.Instance.BufferZoneController.bufferZoneContainer_0.InnerZone;
            doorAccess.ChangePlayerAccessStatus(player, status);

            return true;
        }
    }
}