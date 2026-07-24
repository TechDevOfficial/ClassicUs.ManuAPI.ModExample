using System.Collections.Generic;
using ClassicUs.Manactor;
using UnityEngine;

namespace ClassicUs.ManuAPI.ModExample
{
    internal static class CloakSystem
    {
        // First RPC asks the host; second RPC tells all clients the approved result.
        private const string RequestRpc = "classicus.example.RequestCloak";
        private const string StartRpc = "classicus.example.StartCloak";
        private static readonly Dictionary<byte, float> EndsAt = new();

        public static bool IsCloaked(PlayerControl player) => player != null && player.Data != null && EndsAt.ContainsKey(player.Data.PlayerId);

        public static void RequestStart(byte playerId, float seconds)
        {
            // The host owns the final state so clients cannot disagree.
            if (AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost) Start(playerId, seconds, true);
            else ManactorAPI.SendRpcMethod(RequestRpc, playerId, seconds);
        }

        [ManactorRpc(RequestRpc)]
        private static void OnRequest(byte senderId, byte playerId, float seconds)
        {
            if (AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost) Start(playerId, seconds, true);
        }

        [ManactorRpc(StartRpc)]
        private static void OnStart(byte senderId, byte playerId, float seconds)
        {
            if (AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost) Start(playerId, seconds, false);
        }

        private static void Start(byte playerId, float seconds, bool broadcast)
        {
            // Save the end time locally and immediately refresh the player visuals.
            EndsAt[playerId] = Time.time + seconds;
            Apply(FindPlayer(playerId), true);
            if (broadcast) ManactorAPI.SendRpcMethod(StartRpc, playerId, seconds);
        }

        public static void Tick()
        {
            // Keeps the player invisible while active, then restores the normal model.
            if (EndsAt.Count == 0) return;
            var expired = new List<byte>();
            foreach (var pair in EndsAt)
            {
                var player = FindPlayer(pair.Key);
                if (Time.time >= pair.Value) expired.Add(pair.Key);
                else Apply(player, true);
            }
            foreach (var id in expired)
            {
                EndsAt.Remove(id);
                Apply(FindPlayer(id), false);
            }
        }

        public static void Reapply(PlayerControl player)
        {
            if (IsCloaked(player)) Apply(player, true);
        }

        public static void Reset()
        {
            foreach (var id in EndsAt.Keys) Apply(FindPlayer(id), false);
            EndsAt.Clear();
        }

        private static void Apply(PlayerControl player, bool cloaked)
        {
            // Hide/show the visible vanilla pieces; movement and colliders stay untouched.
            if (player == null) return;
            if (player.MyPhysics != null && player.MyPhysics.rend != null) player.MyPhysics.rend.enabled = !cloaked;
            if (player.HatRenderer != null) player.HatRenderer.SetEnabled(!cloaked);
            if (player.nameText != null) player.nameText.gameObject.SetActive(!cloaked);
            if (player.CurrentPet != null) player.CurrentPet.Visible = !cloaked;
        }

        private static PlayerControl FindPlayer(byte playerId)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
                if (player != null && player.Data != null && player.Data.PlayerId == playerId) return player;
            return null;
        }
    }
}
