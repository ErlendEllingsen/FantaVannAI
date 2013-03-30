using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fantaVannAI.Game_Data;

namespace fantaVannAI
{
    public class ai_logic
    {
        #region Definitions
        #region movement
        public enum movements
        {
            UP,
            DOWN,
            RIGHT_UP,
            RIGHT_DOWN,
            LEFT_UP,
            LEFT_DOWN
        }

        public Dictionary<movements, int> movementPrice = new Dictionary<movements, int>() { 
            {movements.UP, 10},
            {movements.DOWN, 10},
            {movements.RIGHT_UP, 14},
            {movements.RIGHT_DOWN, 14},
            {movements.LEFT_UP, 14},
            {movements.LEFT_DOWN, 14}
        };
        #endregion

        #region pathFinding
        List<Tuple<int, int>> finalPath;

        List<Tuple<int, int>> path_current;
        List<Tuple<int, int>> path_blackList;

        Tuple<int, int> path_lastTile;
        Tuple<int, int> path_currentTile;
        List<Tuple<int, int>> path_final;
        #endregion
        #endregion

        #region functions
        public void pathFinding_process(map processMap, tile initPos, tile targetPos)
        {
            path_currentTile = initPos.tile_pos;
            path_lastTile = path_currentTile;   

            Boolean path_found = false;
            Boolean dokill = false;
            

            List<movements> availableActions = new List<movements>() { movements.UP, movements.DOWN, movements.RIGHT_UP, movements.RIGHT_DOWN, movements.LEFT_UP, movements.LEFT_DOWN};

            /*
                Movement costs:
             *       up   
             *    14  10   14
             *      \  /          
             *       |              
             *      / \ 
             *   14  10  14
             *      down
             */


            while ((!path_found) && (!dokill))
            {
                Dictionary<movements, Tuple<int, int>> availableDirections = new Dictionary<movements, Tuple<int, int>>()
                {
                    {movements.UP, new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2)},
                    {movements.DOWN, new Tuple<int, int>(path_currentTile.Item1 + 1, path_currentTile.Item2)},
                    {movements.LEFT_UP, new Tuple<int, int>(path_currentTile.Item1 + 1, path_currentTile.Item2 + 1)},
                    {movements.LEFT_DOWN, new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2 + 1)},
                    {movements.RIGHT_DOWN, new Tuple<int, int>(path_currentTile.Item1 + 1, path_currentTile.Item2 - 1)},
                    {movements.RIGHT_DOWN, new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2 - 1)}
                };

                //remove invalid tiles
                foreach (KeyValuePair<movements, Tuple<int, int>> pair in availableDirections)
                {
                    //remove invalid tiles
                    if (!processMap.map_data.ContainsKey(pair.Value))
                    {
                        path_blackList.Add(pair.Value);
                        availableActions.Remove(pair.Key);
                         Console.WriteLine("[PathFinder] - Removed invalid tile at (" + Convert.ToString(pair.Value.Item1) + "," + Convert.ToString(pair.Value.Item2) + ")");
                    }

                    //remove blacklisted tiles
                    if (path_blackList.Contains(pair.Value))
                    {
                        availableActions.Remove(pair.Key);
                        Console.WriteLine("[PathFinder] - Removed blacklisted tile at (" + Convert.ToString(pair.Value.Item1) + "," + Convert.ToString(pair.Value.Item2) + ")");
                    }

                    //remove unwalkable tiles
                    tile analyzeTile = processMap.map_data[pair.Value];
                    if ((analyzeTile.tile_type == tiles.tileType.VOID) || (analyzeTile.tile_type == tiles.tileType.SPAWN) || (analyzeTile.tile_type == tiles.tileType.ROCK))
                    {
                        availableActions.Remove(pair.Key);
                         Console.WriteLine("[PathFinder] - Removed unwalkable tile at (" + Convert.ToString(pair.Value.Item1) + "," + Convert.ToString(pair.Value.Item2) + ")");
                    }
                }
                
                

                /*
                    FIND TILES
                 */


                //prioritize y axis before x axis.
                Boolean foundYTile = true; //is set to false if not
                Boolean foundXTile = false;

                if (targetPos.tile_pos.Item1 > path_currentTile.Item1)
                {
                    //target y axis is greater.
                    //following moves should be made: up, left_up, right_up 

                    if (availableDirections.ContainsKey(movements.UP)) {
                        Console.WriteLine("[PathFinder] - Found walkable UP tile (" + Convert.ToString(path_currentTile.Item1 - 1) + "," + Convert.ToString(path_currentTile.Item2) + ")");
                        path_currentTile = new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2);
                    }
                    else if (availableDirections.ContainsKey(movements.LEFT_UP))
                    {
                        Console.WriteLine("[PathFinder] - Found walkable LEFT UP tile (" + Convert.ToString(path_currentTile.Item1 - 1) + "," + Convert.ToString(path_currentTile.Item2 + 1) + ")");
                        path_currentTile = new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2 + 1);
                    }
                    else if (availableDirections.ContainsKey(movements.RIGHT_UP))
                    {
                        Console.WriteLine("[PathFinder] - Found walkable RIGHT UP tile (" + Convert.ToString(path_currentTile.Item1 - 1) + "," + Convert.ToString(path_currentTile.Item2 - 1) + ")");
                        path_currentTile = new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2 - 1);
                    }
                    else
                    {
                        foundYTile = false;
                    }

                    path_current.Add(path_currentTile);
                    path_lastTile = path_currentTile;
                }
                else if (targetPos.tile_pos.Item1 < path_currentTile.Item1)
                {
                    //target y axis is lower.
                    //following moves should be made: down, left_down, right_down
                    if (availableDirections.ContainsKey(movements.DOWN))
                    {
                        Console.WriteLine("[PathFinder] - Found walkable DOWN tile (" + Convert.ToString(path_currentTile.Item1 + 1) + "," + Convert.ToString(path_currentTile.Item2) + ")");
                        path_currentTile = new Tuple<int, int>(path_currentTile.Item1 + 1, path_currentTile.Item2);
                    }
                    else if (availableDirections.ContainsKey(movements.LEFT_UP))
                    {
                        Console.WriteLine("[PathFinder] - Found walkable LEFT DOWN tile (" + Convert.ToString(path_currentTile.Item1 + 1) + "," + Convert.ToString(path_currentTile.Item2 + 1) + ")");
                        path_currentTile = new Tuple<int, int>(path_currentTile.Item1 + 1, path_currentTile.Item2 + 1);
                    }
                    else if (availableDirections.ContainsKey(movements.RIGHT_UP))
                    {
                        Console.WriteLine("[PathFinder] - Found walkable RIGHT DOWN tile (" + Convert.ToString(path_currentTile.Item1 + 1) + "," + Convert.ToString(path_currentTile.Item2 - 1) + ")");
                        path_currentTile = new Tuple<int, int>(path_currentTile.Item1 + 1, path_currentTile.Item2 - 1);
                    }
                    else
                    {
                        foundYTile = false;
                    }

                    path_current.Add(path_currentTile);
                    path_lastTile = path_currentTile;

                }

                if (!foundYTile)
                {
                    //try to find XTILE
                    if (targetPos.tile_pos.Item2 > path_currentTile.Item2)
                    {
                        //target x axis is greater.
                        //following moves should be made: left up, left down, down, up


                        if (availableDirections.ContainsKey(movements.LEFT_UP))
                        {
                            Console.WriteLine("[PathFinder] - Found walkable LEFT UP tile (" + Convert.ToString(path_currentTile.Item1 - 1) + "," + Convert.ToString(path_currentTile.Item2 + 1) + ")");
                            path_currentTile = new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2 + 1);
                        }
                        else if (availableDirections.ContainsKey(movements.LEFT_DOWN))
                        {
                            Console.WriteLine("[PathFinder] - Found walkable LEFT DOWN tile (" + Convert.ToString(path_currentTile.Item1 + 1) + "," + Convert.ToString(path_currentTile.Item2 + 1) + ")");
                            path_currentTile = new Tuple<int, int>(path_currentTile.Item1 + 1, path_currentTile.Item2 + 1);
                        }
                        else if (availableDirections.ContainsKey(movements.RIGHT_UP))
                        {
                            Console.WriteLine("[PathFinder] - Found walkable RIGHT UP tile (" + Convert.ToString(path_currentTile.Item1 - 1) + "," + Convert.ToString(path_currentTile.Item2 - 1) + ")");
                            path_currentTile = new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2 - 1);
                        }
                        else
                        {
                            foundXTile = false;
                        }

                        path_current.Add(path_currentTile);
                        path_lastTile = path_currentTile;

                    }
                    else if (targetPos.tile_pos.Item2 < path_currentTile.Item2)
                    {
                        //target x axis is lower.right up, right down, down, up

                        if (availableDirections.ContainsKey(movements.RIGHT_UP))
                        {
                            Console.WriteLine("[PathFinder] - Found walkable RIGHT UP tile (" + Convert.ToString(path_currentTile.Item1 - 1) + "," + Convert.ToString(path_currentTile.Item2 - 1) + ")");
                            path_currentTile = new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2 - 1);
                        }
                        else if (availableDirections.ContainsKey(movements.RIGHT_DOWN))
                        {
                            Console.WriteLine("[PathFinder] - Found walkable RIGHT DOWN tile (" + Convert.ToString(path_currentTile.Item1 + 1) + "," + Convert.ToString(path_currentTile.Item2 - 1) + ")");
                            path_currentTile = new Tuple<int, int>(path_currentTile.Item1 + 1, path_currentTile.Item2 - 1);
                        }
                        else if (availableDirections.ContainsKey(movements.DOWN))
                        {
                            Console.WriteLine("[PathFinder] - Found walkable DOWN tile (" + Convert.ToString(path_currentTile.Item1 + 1) + "," + Convert.ToString(path_currentTile.Item2) + ")");
                            path_currentTile = new Tuple<int, int>(path_currentTile.Item1 + 1, path_currentTile.Item2);
                        }
                        else if (availableDirections.ContainsKey(movements.UP))
                        {
                            Console.WriteLine("[PathFinder] - Found walkable UP tile (" + Convert.ToString(path_currentTile.Item1 - 1) + "," + Convert.ToString(path_currentTile.Item2) + ")");
                            path_currentTile = new Tuple<int, int>(path_currentTile.Item1 - 1, path_currentTile.Item2);
                        }
                        else
                        {
                            foundXTile = false;
                        }

                        path_current.Add(path_currentTile);
                        path_lastTile = path_currentTile;
                    }
                }

                if ((!foundYTile) && (!foundXTile))
                {
                    if (path_current.Contains(path_currentTile)) path_current.Remove(path_currentTile);
                    path_blackList.Add(path_currentTile);
                    path_currentTile = path_lastTile;
                }
                
            }

         }
        #endregion
    }
}
