using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace fantaVannAI
{
    public class packets
    {
        #region Connection packets
        //connection packets
        public static packet packet_connection_handshake = new packet(new Dictionary<String, String>() { {"message", "connect"}, {"revision", Convert.ToString(mainProcessor.protocol_revision)}, {"name", ("FantaVann" + Convert.ToString(mainProcessor.rand.Next(0,99)))}});
        //game packets
        public static packet packet_game_selectEquipment = new packet(new Dictionary<String, String>() { {"message", "loadout"}, {"primary-weapon", "laser"}, {"secondary-weapon", "mortar"} });
        #endregion
    }
}
