using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ExtremelySimpleLogger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Data.Content;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using TinyLife;
using TinyLife.Actions;
using TinyLife.Emotions;
using TinyLife.Mods;
using TinyLife.Objects;
using TinyLife.Utilities;
using TinyLife.World;
using TinyLife.Tools;
using Action = TinyLife.Actions.Action;
using MLEM.Misc;

namespace PartyNightKit;

public class PartyNightKit : Mod
{

    // the logger that we can use to log info about this mod
    public static Logger Logger { get; private set; }

    // visual data about this mod
    public override string Name => "Party Night Kit";
    public override string Description => "Party all night! - By Gindew v1.1";
    public override string IssueTrackerUrl => "https://x.com/RedGindew";
    public override string TestedVersionRange => "[0.45.0]";
    private Dictionary<Point, TextureRegion> uiTextures;
    public override TextureRegion Icon => this.uiTextures[new Point(0, 0)];


    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info)
    {
        PartyNightKit.Logger = logger;
        texturePacker.Add(new UniformTextureAtlas(content.Load<Texture2D>("UITex"), 8, 8), r => this.uiTextures = r, 1, true);
    }

    public override void AddGameContent(GameImpl game, ModInfo info)
    {
        FurnitureType.Register(new FurnitureType.TypeSettings("PartyNightKit.PartyBalloons", new Point(1, 1), ObjectCategory.Nothing, 250, new ColorScheme[] { ColorScheme.Modern, ColorScheme.Modern, ColorScheme.Modern, ColorScheme.White })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.Modern, ColorScheme.Modern, ColorScheme.Modern, ColorScheme.White){Defaults = new int[] { 13, 9, 8, 0 }, PreviewName = "PartyNightKit.PartyBalloons"},
            DefaultRotation = MLEM.Maths.Direction2.Up
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("PartyNightKit.PartyRecordPlayer", new Point(1, 1), ObjectCategory.Nothing, 250, new ColorScheme[] { ColorScheme.SimpleWood, ColorScheme.Grays, ColorScheme.White })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.LivingRoom),
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.Grays, ColorScheme.White){Defaults = new int[] { 0, 6, 0 }, PreviewName = "PartyNightKit.PartyRecordPlayer"},
            DefaultRotation = MLEM.Maths.Direction2.Up
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("PartyNightKit.PartyIcicleLight", new Point(1, 1), ObjectCategory.WallHanging | ObjectCategory.NonColliding, 120, new ColorScheme[] { ColorScheme.White, ColorScheme.Modern })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.White, ColorScheme.Modern){Defaults = new int[] { 0, 10 }, PreviewName = "PartyNightKit.PartyIcicleLight"},
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("PartyNightKit.PartyWallBanner", new Point(1, 1), ObjectCategory.WallHanging | ObjectCategory.NonColliding, 120, new ColorScheme[] { ColorScheme.White, ColorScheme.Modern })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Decoration),
            Colors = new ColorSettings(ColorScheme.White, ColorScheme.Modern){Defaults = new int[] { 0, 11 }, PreviewName = "PartyNightKit.PartyWallBanner"},
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
    }

    public override IEnumerable<string> GetCustomFurnitureTextures(ModInfo info)
    {
        yield return "PartyBalloons";
        yield return "PartyRecordPlayer";
        yield return "PartyIcicleLight";
        yield return "PartyWallBanner";
    }
}