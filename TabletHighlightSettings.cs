using System.Windows.Forms;
using System.Drawing;
using ExileCore2;
using ExileCore2.Shared.Attributes;
using ExileCore2.Shared.Interfaces;
using ExileCore2.Shared.Nodes;
using Newtonsoft.Json;

namespace TabletHighlight;

public class TabletHighlightSettings : ISettings
{
  public ToggleNode Enable { get; set; } = new ToggleNode(false);

    [Menu("General Settings")]
    public GeneralSettings General { get; set; } = new GeneralSettings();
    
    [Menu("Graphics, Colors, and Font Settings")]    
    public GraphicSettings Graphics { get; set; } = new GraphicSettings();
}


[Submenu(CollapsedByDefault = false)]
public class GeneralSettings
{
    //Mandatory setting to allow enabling/disabling your plugin
    public ToggleNode Enable { get; set; } = new ToggleNode(false);

    [Menu("Minimum increased item rarity")]
    public RangeNode<int> MinRarity { get; set; } = new RangeNode<int>(10, 10, 20);

    [Menu("Minimum increased item quantity")]
    public RangeNode<int> MinQuantity { get; set; } = new RangeNode<int>(10, 10, 20);

    [Menu("Minimum increased map quantity")]
    public RangeNode<int> MinMapQuantity { get; set; } = new RangeNode<int>(10, 10, 20);

    [Menu("Minimum irradiated maps in range")]
    public RangeNode<int> MinIrradiatedMaps { get; set; } = new RangeNode<int>(5, 5, 10);

    [Menu("Minimum breach maps in range")]
    public RangeNode<int> MinBreachMaps { get; set; } = new RangeNode<int>(5, 5, 10);

    [Menu("Minimum delirium maps in range")]
    public RangeNode<int> MinDeliriumMaps { get; set; } = new RangeNode<int>(5, 5, 10);

    [Menu("Minimum expedition maps in range")]
    public RangeNode<int> MinExpeditionMaps { get; set; } = new RangeNode<int>(5, 5, 10);

    [Menu("Minimum ritual maps in range")]
    public RangeNode<int> MinRitualMaps { get; set; } = new RangeNode<int>(5, 5, 10);

    [Menu("Minimum boss maps in range")]
    public RangeNode<int> MinBossMaps { get; set; } = new RangeNode<int>(5, 5, 10);

    [Menu("Custom mod group 1", "List of mods to highlight, separated with ','.\nLocate them by alt-clicking on item and hovering over affix tier ('U') on the right")]
    public TextNode CustomModGroup1 { get; set; } = new TextNode("");

    [Menu("Custom mod group 2", "List of mods to highlight, separated with ','.\nLocate them by alt-clicking on item and hovering over affix tier ('U') on the right")]
    public TextNode CustomModGroup2 { get; set; } = new TextNode("");

    [Menu("Custom mod group 3", "List of mods to highlight, separated with ','.\nLocate them by alt-clicking on item and hovering over affix tier ('U') on the right")]
    public TextNode CustomModGroup3 { get; set; } = new TextNode("");

    [JsonIgnore]
    public ButtonNode ReloadCustomModGroups { get; set; } = new ButtonNode();

}

[Submenu(CollapsedByDefault = false)]
public class GraphicSettings {
    //HIGHLIGHT COLOR
    [Menu("Rarity highlight color", "Highlight color for tablets meeting the minimum increased item rarity")]
    public ColorNode RarityHighlightColor { get; set; } = new ColorNode(Color.Green);

    [Menu("Quantity highlight color", "Highlight color for tablets meeting the minimum increased item rarity")]
    public ColorNode QuantityHighlightColor { get; set; } = new ColorNode(Color.Cyan);

    [Menu("Map quantity highlight color", "Highlight color for tablets meeting the minimum increased item rarity")]
    public ColorNode MapQuantityHighlightColor { get; set; } = new ColorNode(Color.Red);

    [Menu("Custom group 1 highlight color", "Highlight color for tablets matching one of the mods defined in custom group 1")]
    public ColorNode CustomGroup1HighlightColor { get; set; } = new ColorNode(Color.White);

    [Menu("Custom group 2 highlight color", "Highlight color for tablets matching one of the mods defined in custom group 2")]
    public ColorNode CustomGroup2HighlightColor { get; set; } = new ColorNode(Color.Yellow);

    [Menu("Custom group 3 highlight color", "Highlight color for tablets matching one of the mods defined in custom group 3")]
    public ColorNode CustomGroup3HighlightColor { get; set; } = new ColorNode(Color.Pink);

    //HIGHLIGHT SETTINGS
    [Menu("Highlight Style", "1 = Border; 2 = Filled box/dot")]
    public RangeNode<int> HighlightStyle { get; set; } = new RangeNode<int>(1, 1, 2);

    [Menu("Border Highlight Thickness", "Thickness of the border for highlighted tablets")]
    public RangeNode<int> BorderThickness { get; set; } = new RangeNode<int>(3, 1, 5);

    [Menu("Box Highlight Rounding", "Rounding of the box for highlighted tablets")]
    public RangeNode<int> BannedBoxRounding { get; set; } = new RangeNode<int>(55, 1, 60);

    [Menu("Font Size Settings")]
    public FontSizeSettings FontSize { get; set; } = new FontSizeSettings();
}

[Submenu(CollapsedByDefault = true)]
public class FontSizeSettings {
    //FONT SIZE (Needs modifications as text scales from origin point and doesn't change position accordingly)
    [Menu("Mods Font Size", "Size of the font for mod values shown directly on the tablet item")]
    public RangeNode<float> ModsFontSizeMultiplier { get; set; } = new RangeNode<float>(1.0f, 0.5f, 2f);

    [Menu("Affected Maps Font Size", "Size of the font for how many maps are affected in range (e.g. number of breaches added in range)")]
    public RangeNode<float> AffectedMapsFontSizeMultiplier { get; set; } = new RangeNode<float>(1.0f, 0.5f, 2f);
}
