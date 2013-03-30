using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fantaVannAI
{
    public static class constants
    {
        //game states
        public const int gamestate_error = -1; //something went wrong
        public const int gamestate_preHandshake = 0; //client has not yet sent handshake.
        public const int gamestate_posthandShakeWaiting = 1; //client has sent handshake. Waiting for server reply
        public const int gamestate_waitForInitGameState = 2; //waiting for initial gamestate to be sent.
        public const int gamestate_chooseEquipment = 3; //the AI shal now select the proper equipment.
        public const int gamestate_equipmentSelectionSent = 4; //the equipment selection has been sent. Waiting for further details.
 
    }
}
