using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System.Reflection;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace TombstoneHelper
{
    [BepInPlugin(PluginGUID, "TombstoneHelper", "{VERSION}")]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
    internal class TombstoneHelperPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com.userstorm.tombstonehelper";

        private CustomLocalization Localization;

        private static FieldInfo m_interactMaskField = AccessTools.Field(typeof(Player), "m_interactMask");

        private ConfigEntry<float> InteractionRange;
        private ConfigEntry<KeyboardShortcut> ShortcutConfig;
        private ButtonConfig ShortcutButton;

        private StatusEffect TombstoneNearbyStatusEffect;
        private bool HasTombstoneNearbyStatusEffect = false;
        private float CheckForNearbyTombstoneTimeDifference = 0f;

        private void Awake()
        {
            Assets.Init();

            CreateConfigValues();
            AddInputs();
            AddLocalizations();
            AddStatusEffects();
        }
        
        private void Update()
        {
            HandleInput();

            CheckForNearbyTombstoneTimeDifference += Time.deltaTime;

            if (CheckForNearbyTombstoneTimeDifference > 0.2f)
            {
                CheckForNearbyTombstone();

                CheckForNearbyTombstoneTimeDifference = 0f;
            }
        }

        private void CreateConfigValues()
        {
            Config.SaveOnConfigSet = true;

            ShortcutConfig = Config.Bind(
                "General",
                "Open tombstone",
                new KeyboardShortcut(KeyCode.E, KeyCode.LeftShift),
                new ConfigDescription("Open tombstone key combination")
            );

            InteractionRange = Config.Bind(
                "General",
                "Interaction range",
                5.0f,
                new ConfigDescription(
                    "Interaction range",
                    new AcceptableValueRange<float>(0.1f, 10f)
                )
            );
        }

        private void AddInputs()
        {
            ShortcutButton = new ButtonConfig
            {
                Name = "OpenTombstone",
                ShortcutConfig = ShortcutConfig,
                HintToken = "$open_tombstone"
            };
            InputManager.Instance.AddButton(PluginGUID, ShortcutButton);
        }

        private void AddLocalizations()
        {
            Localization = new CustomLocalization();
            LocalizationManager.Instance.AddLocalization(Localization);

            Localization.AddJsonFile("English", Assets.English);
            Localization.AddJsonFile("German", Assets.German);
        }

        private void AddStatusEffects()
        {
            TombstoneNearbyStatusEffect = ScriptableObject.CreateInstance<StatusEffect>();
            TombstoneNearbyStatusEffect.name = "TombstoneNearbyStatusEffect";
            TombstoneNearbyStatusEffect.m_name = "$tombstone_nearby_effect_name";
            TombstoneNearbyStatusEffect.m_icon = Assets.Tombstone;
            TombstoneNearbyStatusEffect.m_startMessageType = MessageHud.MessageType.Center;
            TombstoneNearbyStatusEffect.m_startMessage = "$tombstone_nearby_effect_start";
        }

        private void CheckForNearbyTombstone()
        {
            var player = Player.m_localPlayer;

            if (player == null)
            {
                return;
            }

            TombStone tombStone = FindTombstone();

            if (tombStone != null && !HasTombstoneNearbyStatusEffect)
            {
                player.GetSEMan().AddStatusEffect(TombstoneNearbyStatusEffect, true, 0, 0f);

                HasTombstoneNearbyStatusEffect = true;
            }
            else if (tombStone == null && HasTombstoneNearbyStatusEffect)
            {
                player.GetSEMan().RemoveStatusEffect(TombstoneNearbyStatusEffect, true);

                HasTombstoneNearbyStatusEffect = false;
            }
        }

        private TombStone FindTombstone()
        {
            var player = Player.m_localPlayer;
            var interactMask = (int)m_interactMaskField.GetValue(player);
            var colliders = Physics.OverlapSphere(player.transform.position, this.InteractionRange.Value, interactMask);

            foreach (var collider in colliders)
            {
                if (collider?.gameObject?.GetComponentInParent<TombStone>() is TombStone nearbyTombStone)
                {
                    return nearbyTombStone;
                }
            }

            return null;
        }

        private void HandleInput()
        {
            var player = Player.m_localPlayer;

            if (ZInput.instance == null || player == null)
            {
                return;
            }

            if (ShortcutButton != null && ZInput.GetButtonDown(ShortcutButton.Name))
            {
                TombStone tombStone = FindTombstone();

                if (tombStone != null)
                {
                    player.Interact(tombStone.gameObject, false, false);

                    // prevent the "use" button from closing the tombstone immediately
                    ZInput.ResetButtonStatus("Use");
                    ZInput.ResetButtonStatus("JoyUse");
                }
                else if (MessageHud.instance != null && MessageHud.instance.m_msgQeue.Count == 0)
                {
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "$no_tombstone_nearby");
                }
            }
        }
    }
}
