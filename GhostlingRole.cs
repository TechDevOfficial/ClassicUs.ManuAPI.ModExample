using ClassicUs.ManuAPI;
using UnityEngine;

namespace ClassicUs.ManuAPI.ModExample
{
    internal sealed class GhostlingRole : CustomCrewmateRole
    {
        // Network-stable identifier used by RoleRegistry.
        public const string Id = "classicus.example.Ghostling";

        public override string DisplayName => "Ghostling";
        public override string RoleTypeName => Id;
        public override string Description => "Become invisible for a few seconds.";
        public override string DescriptionShort => "Press Cloak to disappear.";
        public override Color TeamColor => new(0.55f, 0.85f, 1f, 1f);
        public override string EjectionText(string playerName) => playerName + " was a Ghostling.";
    }
}
