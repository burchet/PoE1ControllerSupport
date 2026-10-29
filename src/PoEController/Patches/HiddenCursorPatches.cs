using HarmonyLib;
using PoEController.Input;
using UnityEngine;

namespace PoEController.Patches
{
    /// <summary>
    /// <c>GameCursor.Update</c> shows the cursor again every frame while any window is open
    /// (<c>UIWindowManager.AnyWindowShowing</c>), so hiding it with <c>SetShowCursor</c> doesn't last in conversations.
    /// While the mod has the cursor hidden, every cursor the game sets becomes its own invisible one. The game sets
    /// its cursor anew each frame, so the gauntlet is back as soon as the mod lets go.
    /// </summary>
    [Compat.Feature(Compat.Features.HiddenCursorWindow)]
    [HarmonyPatch(typeof(Cursor), nameof(Cursor.SetCursor), typeof(Texture2D), typeof(Vector2), typeof(CursorMode))]
    internal static class HiddenCursorPatches
    {
        private static void Prefix(ref Texture2D __0, ref Vector2 __1)
        {
            try
            {
                GameCursor cursor = GameCursor.Instance;
                if (DeviceTransitions.CursorHiddenByMod && cursor != null && cursor.PrefabCursors != null)
                {
                    __0 = cursor.PrefabCursors.InvisibleCursor;
                    __1 = Vector2.zero;
                }
            }
            catch (System.Exception e)
            {
                Compat.Features.Off(Compat.Features.HiddenCursorWindow, "error: " + e);
            }
        }
    }
}
