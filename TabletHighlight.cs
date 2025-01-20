using ExileCore2;
using ExileCore2.PoEMemory.Components;
using ExileCore2.PoEMemory.MemoryObjects;
using System.Linq;
using System.Numerics;
using System.Drawing;
using System;
using System.Collections.Generic;
using RectangleF = ExileCore2.Shared.RectangleF;
using ExileCore2.Shared.Nodes;
using ExileCore2.PoEMemory.Elements.InventoryElements;
using Microsoft.VisualBasic.Logging;


namespace TabletHighlight;

public class TabletHighlight : BaseSettingsPlugin<TabletHighlightSettings>
{
    private IngameState InGameState => GameController.IngameState;
    private List<string> CustomModGroup1;
    private List<string> CustomModGroup2;
    private List<string> CustomModGroup3;

     

    public override bool Initialise()
    {
        Settings.General.ReloadCustomModGroups.OnPressed += LoadCustomModGroups;
        LoadCustomModGroups();
        return base.Initialise();
    }

    public override void Render()
    {
        IList<TabletItem> tablets = [];

        var stashPanel = InGameState.IngameUi.StashElement;
        var stashPanelGuild = InGameState.IngameUi.GuildStashElement;
        var inventoryPanel = InGameState.IngameUi.InventoryPanel;

        bool isQuadTab = false;


        // Run if inventory panel is opened
        if (inventoryPanel.IsVisible) {
            // Add stash items
            if (stashPanel.IsVisible && stashPanel.VisibleStash != null) {
                if (stashPanel.VisibleStash.TotalBoxesInInventoryRow == 24) {
                    isQuadTab = true;
                }
                foreach (var item in stashPanel.VisibleStash.VisibleInventoryItems) {
                    TryAddTablet(item, tablets);
                }
            }
            else if (stashPanelGuild.IsVisible && stashPanelGuild != null) {
                if (stashPanelGuild.VisibleStash.TotalBoxesInInventoryRow == 24) {
                    isQuadTab = true;
                }
                foreach (var item in stashPanelGuild.VisibleStash.VisibleInventoryItems) {
                    TryAddTablet(item, tablets);
                }
            } 

            // Add inventory items
            var inventoryItems = GameController.IngameState.ServerData.PlayerInventories[0].Inventory.InventorySlotItems;
            foreach (var item in inventoryItems) {
                TryAddTablet(item, tablets);
            }

            foreach (var tablet in tablets) {
                var itemMods = tablet.mods;
                var bbox = tablet.rect;

                int iiq = 0;
                int iir = 0;
                int imq = 0;
                int affectedMaps = 0;

                bool custom1 = false;
                bool custom2 = false;
                bool custom3 = false;

                // Iterate through the mods
                foreach (var mod in itemMods.ItemMods) {
                    // Find mods
                    switch (mod.Name) {
                        case "TowerDroppedItemRarityIncrease":
                            iir += mod.Values[0];
                            break;
                        case "TowerDroppedItemQuantityIncrease":
                            iiq += mod.Values[0];
                            break;
                        case "TowerMapDroppedMapsIncrease":
                            imq += mod.Values[0];
                            break;
                        case "TowerAddIrradiatedToMapsImplicit":
                            affectedMaps += mod.Values[0];
                            break;
                        case "TowerAddBreachToMapsImplicit":
                            affectedMaps += mod.Values[0];
                            break;
                        case "TowerAddDeliriumToMapsImplicit":
                            affectedMaps += mod.Values[0];
                            break;
                        case "TowerAddRitualToMapsImplicit":
                            affectedMaps += mod.Values[0];
                            break;
                        case "TowerAddExpeditionToMapsImplicit":
                            affectedMaps += mod.Values[0];
                            break;
                        case "TowerAddMapBossesToMapsImplicit":
                            affectedMaps += mod.Values[0];
                            break;
                    }

                    if (CustomModGroup1.Count > 0) {
                        foreach (var customMod in CustomModGroup1) {
                            if (mod.DisplayName.Contains(customMod, StringComparison.OrdinalIgnoreCase)) {
                                custom1 = true;
                                break;
                            }
                        }
                    }

                    if (CustomModGroup2.Count > 0) {
                        foreach (var customMod in CustomModGroup2) {
                            if (mod.DisplayName.Contains(customMod, StringComparison.OrdinalIgnoreCase)) {
                                custom2 = true;
                                break;
                            }
                        }
                    }

                    if (CustomModGroup3.Count > 0) {
                        foreach (var customMod in CustomModGroup3) {
                            if (mod.DisplayName.Contains(customMod, StringComparison.OrdinalIgnoreCase)) {
                                custom3 = true;
                                break;
                            }
                        }
                    }
                }

                // Drawing stats text
                if (tablet.location == ItemLocation.Inventory || (tablet.location == ItemLocation.Stash && !isQuadTab)) {

                    // Stats
                    // SetTextScale doesn't scale well we need to change origin point or add x:y placement modifications depending on scale
                    using (Graphics.SetTextScale(Settings.Graphics.FontSize.ModsFontSizeMultiplier)) {
                        Graphics.DrawText(iir.ToString(), new Vector2(bbox.Left + 5, bbox.Top + 4), ExileCore2.Shared.Enums.FontAlign.Left);
                        Graphics.DrawText(iiq.ToString(), new Vector2(bbox.Left + 5, bbox.Top + 6 + (10 * Settings.Graphics.FontSize.ModsFontSizeMultiplier)), ExileCore2.Shared.Enums.FontAlign.Left);
                        Graphics.DrawText(imq.ToString(), new Vector2(bbox.Left + 5, bbox.Bottom - 4 - (15 * Settings.Graphics.FontSize.AffectedMapsFontSizeMultiplier)), ExileCore2.Shared.Enums.FontAlign.Left);
                    }

                    // number of maps affected
                    // SetTextScale doesn't scale well we need to change origin point or add x:y placement modifications depending on scale
                    using (Graphics.SetTextScale(Settings.Graphics.FontSize.AffectedMapsFontSizeMultiplier)) {
                        Graphics.DrawText(affectedMaps.ToString(), new Vector2(bbox.Right - 5, bbox.Top + 4), ExileCore2.Shared.Enums.FontAlign.Right);
                    }
                }

                // Check if min affected maps is met

                if (tablet.baseComponent.Name == TabletType.Irradiated && affectedMaps < Settings.General.MinIrradiatedMaps) {
                    continue;
                }
                else if (tablet.baseComponent.Name == TabletType.Breach && affectedMaps < Settings.General.MinBreachMaps) {
                    continue;
                }
                else if (tablet.baseComponent.Name == TabletType.Delirium && affectedMaps < Settings.General.MinDeliriumMaps) {
                    continue;
                }
                else if (tablet.baseComponent.Name == TabletType.Ritual && affectedMaps < Settings.General.MinRitualMaps) {
                    continue;
                }
                else if (tablet.baseComponent.Name == TabletType.Expedition && affectedMaps < Settings.General.MinExpeditionMaps) {
                    continue;
                }
                else if (tablet.baseComponent.Name == TabletType.Boss && affectedMaps < Settings.General.MinBossMaps) {
                    continue;
                }

                // Drawing highlight
                if (iiq >= Settings.General.MinQuantity) {
                    switch (Settings.Graphics.HighlightStyle) {
                        case 1:
                            DrawBorderHighlight(bbox, Settings.Graphics.QuantityHighlightColor, Settings.Graphics.BorderThickness);
                            break;
                        case 2:
                            DrawBoxHighlight(bbox, Settings.Graphics.QuantityHighlightColor, Settings.Graphics.BannedBoxRounding.Value);
                            break;
                    }
                }
                else if (iir >= Settings.General.MinRarity) {
                    switch (Settings.Graphics.HighlightStyle) {
                        case 1:
                            DrawBorderHighlight(bbox, Settings.Graphics.RarityHighlightColor, Settings.Graphics.BorderThickness);
                            break;
                        case 2:
                            DrawBoxHighlight(bbox, Settings.Graphics.RarityHighlightColor, Settings.Graphics.BannedBoxRounding.Value);
                            break;
                    }
                }
                else if (imq >= Settings.General.MinMapQuantity) {
                    switch (Settings.Graphics.HighlightStyle) {
                        case 1:
                            DrawBorderHighlight(bbox, Settings.Graphics.MapQuantityHighlightColor, Settings.Graphics.BorderThickness);
                            break;
                        case 2:
                            DrawBoxHighlight(bbox, Settings.Graphics.MapQuantityHighlightColor, Settings.Graphics.BannedBoxRounding.Value);
                            break;
                    }
                }
                else if (custom1) {
                    switch (Settings.Graphics.HighlightStyle) {
                        case 1:
                            DrawBorderHighlight(bbox, Settings.Graphics.CustomGroup1HighlightColor, Settings.Graphics.BorderThickness);
                            break;
                        case 2:
                            DrawBoxHighlight(bbox, Settings.Graphics.CustomGroup1HighlightColor, Settings.Graphics.BannedBoxRounding.Value);
                            break;
                    }
                }
                else if (custom2) {
                    switch (Settings.Graphics.HighlightStyle) {
                        case 1:
                            DrawBorderHighlight(bbox, Settings.Graphics.CustomGroup2HighlightColor, Settings.Graphics.BorderThickness);
                            break;
                        case 2:
                            DrawBoxHighlight(bbox, Settings.Graphics.CustomGroup2HighlightColor, Settings.Graphics.BannedBoxRounding.Value);
                            break;
                    }
                }
                else if (custom3) {
                    switch (Settings.Graphics.HighlightStyle) {
                        case 1:
                            DrawBorderHighlight(bbox, Settings.Graphics.CustomGroup3HighlightColor, Settings.Graphics.BorderThickness);
                            break;
                        case 2:
                            DrawBoxHighlight(bbox, Settings.Graphics.CustomGroup3HighlightColor, Settings.Graphics.BannedBoxRounding.Value);
                            break;
                    }
                }



            }
        }
    }

    private static void TryAddTablet(ServerInventory.InventSlotItem item, IList<TabletItem> targetList) {
        try {
            if (!item.Item.IsValid || string.IsNullOrEmpty(item.Item.Metadata) || !item.Item.Metadata.Contains("TowerAugment")) {
                return;
            }
            Base baseComponent = item.Item.GetComponent<Base>();
            Mods modComponent = item.Item.GetComponent<Mods>();
            RectangleF rect = item.GetClientRect();
            if (baseComponent == null && modComponent == null) {
                throw new Exception("Base or mods component is null", new NullReferenceException());
            }
            targetList.Add(new TabletItem(baseComponent, modComponent, rect, ItemLocation.Inventory));
        } catch (Exception ex) {
            Logger.Log.Warning($"Could not add tablet due to {ex})");
        }
    }

    private static void TryAddTablet(NormalInventoryItem item, IList<TabletItem> targetList) {
        try {
            if (!item.Item.IsValid || string.IsNullOrEmpty(item.Item.Metadata) || !item.Item.Metadata.Contains("TowerAugment")) {
                return;
            }
            Base baseComponent = item.Item.GetComponent<Base>();
            Mods modComponent = item.Item.GetComponent<Mods>();
            RectangleF rect = item.GetClientRectCache;
            if (baseComponent == null && modComponent == null) {
                throw new Exception("Base or mods component is null", new NullReferenceException());
            }
            targetList.Add(new TabletItem(baseComponent, modComponent, rect, ItemLocation.Stash));
        } catch (Exception ex) {
            Logger.Log.Warning($"Could not add tablet due to {ex})");
        }
    }

    private static List<string> ParseModifiers(string input) {
        List<string> mods = new List<string>();
        List<string> parsedInput = input
            .Split(",")
            .Select(x => x.Trim().ToLower())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        if (parsedInput != null && parsedInput.Count > 0) {
            mods = parsedInput;
        }
        return mods;
    }

    private void LoadCustomModGroups() {
        CustomModGroup1 = ParseModifiers(Settings.General.CustomModGroup1.Value);
        CustomModGroup2 = ParseModifiers(Settings.General.CustomModGroup2.Value);
        CustomModGroup3 = ParseModifiers(Settings.General.CustomModGroup3.Value);
    }

    private void DrawBorderHighlight(RectangleF rect, ColorNode color, int thickness)
    {
        int scale = thickness - 1;
        int innerX = (int)rect.X + 1 + (int)(0.5 * scale);
        int innerY = (int)rect.Y + 1 + (int)(0.5 * scale);
        int innerWidth = (int)rect.Width - 1 - scale;
        int innerHeight = (int)rect.Height - 1 - scale;
        RectangleF scaledFrame = new RectangleF(innerX, innerY, innerWidth, innerHeight);
        Graphics.DrawFrame(scaledFrame, color, thickness);
    }

    private void DrawBoxHighlight(RectangleF rect, ColorNode color, int rounding)
    {
        int innerX = (int)rect.X + 1 + (int)(0.5 * rounding);
        int innerY = (int)rect.Y + 1 + (int)(0.5 * rounding);
        int innerWidth = (int)rect.Width - 1 - rounding;
        int innerHeight = (int)rect.Height - 1 - rounding;
        RectangleF scaledBox = new RectangleF(innerX, innerY, innerWidth, innerHeight);
        Graphics.DrawBox(scaledBox, color, rounding);
    }
}
