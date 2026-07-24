using BepInEx;
using BepInEx.Unity.IL2CPP;
using ClassicUs.Manactor;
using ClassicUs.ManuAPI;
using HarmonyLib;

namespace ClassicUs.ManuAPI.ModExample
{
    /// <summary>
    /// Small, self-contained example mod: the Ghostling role can toggle invisibility.
    /// It demonstrates a virtual role, a CustomAbility and a host-authoritative Manactor RPC.
    /// </summary>
    [BepInPlugin(Guid, "ManuAPI Mod Example", Version)]
    [BepInDependency(ManactorPlugin.Guid)]
    [BepInDependency(ManuAPIPlugin.Guid)]
    public sealed class ExamplePlugin : BasePlugin
    {
        public const string Guid = "classicus.manuapi.modexample";
        public const string Version = "1.0.0";

        public override void Load()
        {
            // Registers this mod for Manactor networking.
            ManactorAPI.Register("ManuAPI.ModExample", Version);
            // Makes the cloak RPC methods callable by every client.
            ManactorAPI.RegisterRpcMethods(typeof(CloakSystem));
            // Adds a safe custom role without injecting a native IL2CPP role.
            RoleRegistry.RegisterVirtual(new GhostlingRole());

            // Connects the HUD, meeting and animation callbacks below.
            var harmony = new Harmony(Guid);
            harmony.PatchAll(typeof(ExamplePlugin).Assembly);
            Log.LogInfo("ManuAPI Mod Example loaded.");
        }

        // Returns true when the player currently has the example role.
        public static bool IsGhostling(PlayerControl player) =>
            player != null && player.Data != null && RoleRegistry.IsAssigned(player, GhostlingRole.Id);
    }
}
