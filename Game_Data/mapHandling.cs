using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace fantaVannAI.Game_Data
{
    public static class mapHandling
    {
        public static void parseMap(Dictionary<Object, Object> serverData)
        {
            try
            {
                if (!(serverData.ContainsKey("map"))) mainProcessor.doDie("serverData does not contain map data. Cannot parse.");

                //make new map object for new data.
                Dictionary<Tuple<int, int>, tile> newMapData = new Dictionary<Tuple<int,int>,tile>();

                Dictionary<Object, Object> mapData = JsonConvert.DeserializeObject<Dictionary<Object, Object>>(Convert.ToString(serverData["map"]));

                int yLength = Convert.ToInt32(mapData["j-length"]);
                int xLength = Convert.ToInt32(mapData["k-length"]);
                string actualMapData = Convert.ToString(mapData["data"]);
                
                //parse through the raw map data received from the server.

                int splitIndex = 0;
                for (int currColumn = 1; currColumn < (yLength + 1); currColumn++)
                {
                    for (int currRow = 1; currRow < (xLength + 1); currRow++)
                    {
                        splitIndex++; 

                        //get current tile
                        string tileType = mainProcessor.Split(mainProcessor.Split(actualMapData, "\"", splitIndex), "\"", 0);
                        tile currentTile = new tile(currColumn, currRow, tiles.tileTypes[tileType]);

                        //add tile to new map
                        Tuple<int, int> tileCoordinates = new Tuple<int, int>(currColumn, currRow);
                        newMapData.Add(tileCoordinates, currentTile);

                        splitIndex++; //jump to correct split point
                    }
                }

                //map is now parsed.
                mainProcessor.game_map = new map(xLength, yLength, newMapData);
                mainProcessor.sendInfo("Map has been parsed.");
            }
            catch (Exception ex)
            {
                mainProcessor.doDie("Unable to parse map. " + ex.Message);
            }
        }
    }
}
