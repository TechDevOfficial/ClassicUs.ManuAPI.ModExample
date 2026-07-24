using HarmonyLib;

namespace ClassicUs.ManuAPI.ModExample
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.FixedUpdate))]
    internal static class HudManager_FixedUpdate_ExamplePatch
    {
        private static void Postfix(HudManager __instance)
        {
            // Updates the button and ends cloak effects when their timer expires.
            CloakAbilityHolder.Tick(__instance);
            CloakSystem.Tick();
        }
    }

    [HarmonyPatch(typeof(PlayerPhysics), "HandleAnimation")]
    internal static class PlayerPhysics_HandleAnimation_ExamplePatch
    {
        // Vanilla animation may re-enable renderers, so apply the cloak again afterwards.
        private static void Postfix(PlayerPhysics __instance) => CloakSystem.Reapply(__instance != null ? __instance.myPlayer : null);
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    internal static class MeetingHud_Start_ExamplePatch
    {
        private static void Postfix()
        {
            // Meetings always cancel the example ability.
            CloakSystem.Reset();
            CloakAbilityHolder.Reset();
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    internal static class HudManager_Start_ExamplePatch
    {
        private static void Postfix()
        {
            CloakSystem.Reset();
            CloakAbilityHolder.Reset();
        }
    }
}
