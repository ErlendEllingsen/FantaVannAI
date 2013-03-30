using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fantaVannAI.Game_Data
{
    public class tiles
    {
        public static Dictionary<String, tileType> tileTypes = new Dictionary<string, tileType>() { 
            {"G", tileType.GRASS},
            {"V", tileType.VOID},
            {"S", tileType.SPAWN},
            {"E", tileType.EXPLODIUM},
            {"R", tileType.RUBIDIUM},
            {"C", tileType.SCRAP},
            {"O", tileType.ROCK}
        };

        public enum tileType
        {
            GRASS,
            VOID,
            SPAWN,
            EXPLODIUM,
            RUBIDIUM,
            SCRAP,
            ROCK
        }
    }

    public class tile
    {

        public Tuple<int, int> tile_pos;
        public tiles.tileType tile_type;

        public tile(int y, int x, tiles.tileType tileType)
        {
            //coordinate formula: (y,x). Check map.cs for explanation.
            tile_pos = new Tuple<int,int>(y, x);
            tile_type = tileType;
        }
    }
}
