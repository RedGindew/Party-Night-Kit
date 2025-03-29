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
    public override string Description => "Party all night! - By Gindew";
    public override string IssueTrackerUrl => "https://x.com/RedGindew";
    public override string TestedVersionRange => "[0.47.0]";
    private Dictionary<Point, TextureRegion> uiTextures, Floor;
    private Dictionary<Point, TextureRegion> openShirt;
    public override TextureRegion Icon => this.uiTextures[new Point(0, 0)];

    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info)
    {
        PartyNightKit.Logger = logger;
        texturePacker.Add(new UniformTextureAtlas(content.Load<Texture2D>("UITex"), 8, 8), r => this.uiTextures = r, 1, true);
        texturePacker.Add(new UniformTextureAtlas(content.Load<Texture2D>("Tiles"), 6, 4), r => this.Floor = r, 1, true, true);
        texturePacker.Add(new UniformTextureAtlas(content.Load<Texture2D>("OpenShirt"), 4, 7), r => this.openShirt = r, 1, true, true);
    }

    public override void AddGameContent(GameImpl game, ModInfo info)
    {
        var arcadeCategory = ObjectCategory.AddFlag<ObjectCategory>("ArcadeMachine");
        ActionType.Register(new ActionType.TypeSettings("PartyNightKit.ArcadeMachineAction", arcadeCategory, typeof(ArcadeMachineAction))
        {
            Ai =
            {
                CanDoRandomly = true,
                PassivePriority = _=> 30,
                SolvedNeeds = [NeedType.Entertainment]
            },
            CanExecute = ActionType.IsNotEmotional(EmotionType.Sad)
        });
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
            Colors = new ColorSettings(ColorScheme.SimpleWood, ColorScheme.Grays, ColorScheme.White){Defaults = new int[] { 1, 6, 0 }, PreviewName = "PartyNightKit.PartyRecordPlayer"},
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
            Colors = new ColorSettings(ColorScheme.White, ColorScheme.Modern){Defaults = new int[] { 0, 13 }, PreviewName = "PartyNightKit.PartyWallBanner"},
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("PartyNightKit.NeonSign", new Point(1, 1), ObjectCategory.WallHanging | ObjectCategory.NonColliding, 120, new ColorScheme[] { ColorScheme.Modern })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Lighting),
            Colors = new ColorSettings(ColorScheme.Modern){Defaults = new int[] { 11 }, PreviewName = "PartyNightKit.NeonSign"},
            DefaultRotation = MLEM.Maths.Direction2.Right
        });
        FurnitureType.Register(new FurnitureType.TypeSettings("PartyNightKit.ArcadeMachine", new Point(1, 1), arcadeCategory, 250)
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.LivingRoom),
            Colors = new ColorSettings(ColorScheme.SheetMetal, ColorScheme.Modern, ColorScheme.Modern, ColorScheme.Modern){Defaults = new int[] { 0, 12, 8, 8 }, PreviewName = "PartyNightKit.ArcadeMachine"},
            DefaultRotation = MLEM.Maths.Direction2.Right,
            ConstructedType = typeof(ScreenObject)
        });

        ScreenObject.ScreenContentOverrides.Add((furniture => furniture.Type.HasCategory(arcadeCategory), (furniture, content) => "PartyNightKit.ArcadeMachine" + content));
        
        FurnitureType.Register(new FurnitureType.TypeSettings("PartyNightKit.DiscoBall", new Point(1, 1), ObjectCategory.Lamp | ObjectCategory.SmallObject | ObjectCategory.NonColliding, 50, new ColorScheme[] { ColorScheme.White })
        {
            Icon = this.Icon,
            Tab = (FurnitureTool.Tab.Lighting),
            Colors = new ColorSettings(ColorScheme.White){ Defaults = new int[] { 0 } },
            ElectricityRating = 1,
            LightSettings = {
                CreateLights = f => [
                    new Light(f.Map, f.Position, f.Floor, Light.CircleTexture, new Vector2(6, 8), Color.GhostWhite) {
                        VisualWorldOffset = new Vector2(0.5F)
                    }
                ]
            }
        });
        Tile.Register("PartyNightKit.FunkyCarpet", 20, this.Floor, new Point(0, 0), new ColorScheme[] { ColorScheme.Modern, ColorScheme.Modern }, 0, true, Tile.Category.None, this.Icon);

        Clothes.Register(new Clothes("PartyNightKit.OpenShirt", ClothesLayer.Shirt, this.openShirt, new Point(0, 0), 100, ClothesIntention.Everyday, StylePreference.Neutral, ColorScheme.WarmDarkMutedPastels) { Icon = this.Icon });
    }

    public override IEnumerable<string> GetCustomFurnitureTextures(ModInfo info)
    {
        yield return "PartyBalloons";
        yield return "PartyRecordPlayer";
        yield return "PartyIcicleLight";
        yield return "PartyWallBanner";
        yield return "DiscoBall";
        yield return "NeonSign";
        yield return "ArcadeMachine";
    }
}