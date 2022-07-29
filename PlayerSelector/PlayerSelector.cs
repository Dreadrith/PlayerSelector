using MelonLoader;
using UnityEngine;
using ABI_RC.Core.Player;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Networking.IO.Social;
using ABI_RC.Core.Savior;

[assembly: MelonInfo(typeof(PlayerSelector.PlayerSelector), "PlayerSelector", "1.0.0", "Dreadrith", "https://github.com/Dreadrith/PlayerSelector")] 
[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]

namespace PlayerSelector
{
    public class PlayerSelector : MelonMod
    {
        public static MelonPreferences_Entry<bool> enabled;
        public static MelonPreferences_Entry<bool> autoShowMenu;
        public override void OnApplicationStart()
        {
            var category = MelonPreferences.CreateCategory("PlayerSelector", "Player Selector");
            enabled = category.CreateEntry("Enabled", true, "Enable Player Selector");
            autoShowMenu = category.CreateEntry("AutoShowMenu", true, "Auto Show Menu");
        }

        public override void OnUpdate()
        {
            if (!enabled.Value) return;
            if (MetaPort.Instance.isUsingVr) return;
            if (!Input.GetMouseButton(1) || !Input.GetMouseButtonUp(0)) return;

            if (Physics.Raycast(Camera.current.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                var collider = hit.collider;

                if (collider.isTrigger)
                {
                    var descriptor = collider.GetComponent<PlayerDescriptor>();
                    if (descriptor != null)
                    {
                        MelonLogger.Msg($"Requesting User Details: {descriptor.userName}");
                        Users.ShowDetails(descriptor.ownerId);
                        if (autoShowMenu.Value) ViewManager.Instance.UiStateToggle(true);
                    }
                }
            }

        }
    }
}