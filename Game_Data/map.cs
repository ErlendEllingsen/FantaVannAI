using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fantaVannAI.Game_Data
{
    public class map
    {
        #region explanation
        /*
         *  This is how the developers of the game presented the map. I found it quite unnecessary hard to develop with, so i turned it abit around.
         * 
         *  This is the map they present:
         *  -------------------------------
                         J-koordinat                       K-koordinat
              \                               /
               \                             /
                \                _____      /
                 \         <0>  /     \  <0>
                  \            /       \
                   \     ,----(   0,0   )----.
                    <1> /      \       /      \ <1>
                 _____ /  1,0   \_____/  0,1   \_____
            <2> /      \        /     \        /     \  <2>
               /        \      /       \      /       \
              (   2,0    )----(   1,1   )----(   0,2   )
               \        /      \       /      \       /
                \_____ /  2,1   \_____/  1,2   \_____/
                       \        /     \        /
                        \      /       \      /
                         `----(   2,2   )----'
                       .       \       /      .
                      .         \_____/        .
                     .                          .
         *  
         *         Y(J)
         * _____    _____    ______
         * /     \  /     \  /      \
         * | 0,2 | |  0,1  | | 0,0   |
         * \_____/  \______/  \______/
         *  
         *  _____    _____    ______
         * /     \  /     \  /      \
         * |  1,2 | | 1,1  | |  1,0  |  X(K)
         * \_____/  \______/  \______/
         *  
         *  _____    _____    ______
         * /     \  /     \  /      \
         * | 2,2  | | 2,1  | | 2,0   |
         * \_____/  \______/  \______/
         *  
            ------------------------------
         * This is how it should have been
         *          Y (J) 
         * 
         *  0,2     0,1     0,0
         *  1,2     1,1     1,0     X (K)
         *  2,2     2,1     2,0
         *  
         * So in the code, i'll just my own system. As it is much simpler and easier to understand. It basicly contains the same data, except its not rotated.
         *
         * !!! IMPORTANT !!!
         *      When dealing with Tiles, it's important to remember the fact that:
         *          - Due to gameMaker's fucked up logic, the Y-coordinate has to come before the X-coordinate.
         *      (2,6) means Y = 2, X = 6. Which also means J = 2, K = 6.
         */
        #endregion
        /*
         * Most important:
         * x = k  
         * y = j 
         * cordinate: (y,x) not (x,y)
        */

        public map(int lengthX, int lengthY, Dictionary<Tuple<int, int>, tile> mapData)
        {
            length_x = lengthX; length_y = lengthY; map_data = mapData;
        }
 
        public int length_x = 0;
        public int length_y = 0;
        public Dictionary<Tuple<int, int>, tile> map_data;

    }
}
