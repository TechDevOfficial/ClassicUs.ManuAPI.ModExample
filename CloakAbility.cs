using ClassicUs.ManuAPI;
using UnityEngine;

namespace ClassicUs.ManuAPI.ModExample
{
    internal sealed class CloakAbility : CustomAbility
    {
        // The button uses the standard kill-button sprite: no asset setup is needed.
        protected override string Name => "GhostlingCloak";
        protected override float Cooldown => 12f;
        protected override Vector3 DistanceFromEdge => AbilityButtonGrid.SlotA;
        protected override Sprite CreateIcon(Sprite original) => original;

        protected override bool IsVisible() => ExamplePlugin.IsGhostling(PlayerControl.LocalPlayer) &&
                                               PlayerControl.LocalPlayer.Data != null &&
                                               !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanActivate() => !CloakSystem.IsCloaked(PlayerControl.LocalPlayer);

        protected override void OnActivate()
        {
            // Ask the host to start the cloak for every client.
            var local = PlayerControl.LocalPlayer;
            if (local != null && local.Data != null) CloakSystem.RequestStart(local.Data.PlayerId, 5f);
        }
    }

    internal static class CloakAbilityHolder
    {
        // A single ability instance is refreshed every HUD frame.
        private static readonly CloakAbility Ability = new();
        public static void Tick(HudManager hud) => Ability.Tick(hud);
        public static void Reset() => Ability.Reset();
    }
}
