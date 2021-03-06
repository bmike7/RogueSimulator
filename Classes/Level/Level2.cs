using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSimulator.Classes.Entity;

namespace RogueSimulator.Classes.Level
{
    public class Level2 : BaseLevel
    {
        private enum TileType
        {
            NONE,
            GROUND,
            LEFTWALL,
            LEFTWALL_GROUND_LEVEL,
            LEFT_TOP_CORNER,
            RIGHTWALL,
            RIGHTWALL_GROUND_LEVEL,
            RIGHT_TOP_CORNER,
            BLOCK,
            LEFTSIDE_FLOATING_BLOCK,
            RIGHTSIDE_FLOATING_BLOCK,
        }

        private readonly Dictionary<TileType, Rectangle> _tileTypes = new Dictionary<TileType, Rectangle> {
            {TileType.GROUND, new Rectangle(90, 30, 30, 30)},
            {TileType.LEFTWALL, new Rectangle(64, 56, 30, 30)},
            {TileType.LEFTWALL_GROUND_LEVEL, new Rectangle(257, 273, 30, 30)},
            {TileType.LEFT_TOP_CORNER, new Rectangle(64, 32, 30, 30)},
            {TileType.RIGHTWALL, new Rectangle(115, 56, 30, 30)},
            {TileType.RIGHTWALL_GROUND_LEVEL, new Rectangle(193, 273, 30, 30)},
            {TileType.RIGHT_TOP_CORNER, new Rectangle(114, 32, 30, 30)},
            {TileType.BLOCK, new Rectangle(160, 32, 30, 30)},
            {TileType.LEFTSIDE_FLOATING_BLOCK, new Rectangle(128, 128, 30, 30)},
            {TileType.RIGHTSIDE_FLOATING_BLOCK, new Rectangle(192, 128, 30, 30)}
        };

        private const int NUMBER_OF_LINES = 5;
        private const int NUMBER_OF_COLUMNS = 50;
        private readonly int[,] _levelDesign = new int[NUMBER_OF_LINES, NUMBER_OF_COLUMNS]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,10,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,10,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,8,0,8,0,0,0,0,0,0,0,0,0,0,0,0,4,7,0,0,0,0,0,0,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-1,0},
            {0,0,0,0,0,4,7,0,0,8,0,0,0,0,0,0,0,0,0,8,0,0,2,5,0,0,0,0,0,9,10,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,3,6,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,3,6,1,1,0,0,0,0,0,0,0,0,8,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };

        public Level2(Game1 game)
            : base(
                game: game,
                texture: Utility.LoadTexture(game, "SpriteSheets/Tileset/jungleTileSet"),
                background: Utility.LoadTexture(game, "SpriteSheets/Background/background"),
                portalTexture: Utility.LoadTexture(game, FinisherPortal.ASSET_NAME),
                size: NUMBER_OF_COLUMNS * Tile.SIZE
            )
        { }

        public override void Create()
        {
            for (int line = 0; line < NUMBER_OF_LINES; line++)
            {
                for (int block = 0; block < NUMBER_OF_COLUMNS; block++)
                {
                    int startY = _viewport.Height - NUMBER_OF_LINES * Tile.SIZE;
                    Vector2 position = new Vector2(block * Tile.SIZE, line * Tile.SIZE + startY);

                    if (_levelDesign[line, block] == -1)
                        FinisherPortal = new FinisherPortal(_portalTexture, position);
                    else
                    {
                        TileType type = (TileType)_levelDesign[line, block];

                        if (type != TileType.NONE)
                        {
                            Tile newTile = new Tile(_texture, position, _tileTypes[type]);
                            _tiles.Add(newTile);
                        }
                    }
                }
            }
            if (Characters.Count < 1)
                createEnemies();
        }

        private void createEnemies()
        {
            Texture2D goblinTexture = Utility.LoadTexture(_game, Goblin.ASSET_NAME);
            Texture2D skeletonTexture = Utility.LoadTexture(_game, Skeleton.ASSET_NAME);

            Characters.Add(new Goblin(goblinTexture, new Vector2(100, 200)));
            Characters.Add(new Goblin(goblinTexture, new Vector2(800, 200)));
            Characters.Add(new Skeleton(skeletonTexture, new Vector2(1900, 200)));
        }
    }
}
