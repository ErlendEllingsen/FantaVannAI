using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

using fantaVannAI.Game_Data;

namespace fantaVannAI
{
    public class network
    {

        #region definitions
        //network variables
        public static TcpClient serverConnection = new TcpClient();
        public static string server_ip = "127.0.0.1"; public static int server_port = 54321;

        //network stream
        public static NetworkStream serverStream;
        public static StreamReader serverStreamReader;
        public static StreamWriter serverStreamWriter;
        public static string serverStreamReader_currentLine;

        //connection
        public static int serverConnection_status;
        public static Boolean serverConnection_keepAlive = true;
        #endregion

        #region ConnectionMethods - ESTABLISHMENT
        public static Boolean connection_establish()
        {
            try
            {
                mainProcessor.game_state = constants.gamestate_preHandshake;

                network.serverConnection = new TcpClient(network.server_ip, network.server_port);
                network.serverStream = network.serverConnection.GetStream();
                network.serverStreamReader = new StreamReader(network.serverStream);
                network.serverStreamWriter = new StreamWriter(network.serverStream);
                

                //Send connection request to game server. (HANDSHAKE 1/2)
               // packet connectionPacket = new packet(new connection_handshake_send());
                Boolean sendHandshake = packets.packet_connection_handshake.sendPacket(); //connectionPacket.sendPacket(); 
                if (!sendHandshake) { mainProcessor.doDie("Error upon handshake. Exiting connection. 1/2"); return false; }

                mainProcessor.game_state = constants.gamestate_posthandShakeWaiting;
                mainProcessor.sendInfo("Handshake sent.");

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region connection
        public static void connection_keeper()
        {
            try
            {
                while (network.serverConnection_keepAlive)
                {
                    Thread.Sleep(100); //sleep for 1/10 second, await new server data 
                    while ((serverStreamReader_currentLine = serverStreamReader.ReadLine()) != null)
                    {
                        Console.WriteLine(serverStreamReader_currentLine);

                        if (!serverStreamReader_currentLine.Contains("message"))
                        {
                            mainProcessor.doDie("Invalid data received from server.");
                        }


                        //connection logic
                        Dictionary<Object, Object> serverData = JsonConvert.DeserializeObject<Dictionary<Object, Object>>(serverStreamReader_currentLine);
                        string messageContent = (string)serverData["message"];

                        switch (mainProcessor.game_state)
                        {
                            case constants.gamestate_preHandshake:
                                //got nothing to do 
                                break;
                            case constants.gamestate_posthandShakeWaiting:
                                if (messageContent != "connect")
                                {
                                    mainProcessor.doDie("Invalid data received from server. Expecting connection data");
                                }

                                if (serverData["status"].GetType() != typeof(Boolean)) mainProcessor.doDie("Connection answer is not boolean.");
                                if ((Boolean)serverData["status"] == false) mainProcessor.doDie("Handshake rejected by server.");

                                mainProcessor.sendInfo("Handshake accepted by server.");
                                mainProcessor.game_state = constants.gamestate_waitForInitGameState;

                                break;
                            case constants.gamestate_waitForInitGameState:
                                //expecting gamestate data
                                if (messageContent != "gamestate") mainProcessor.doDie("Expected gamestate. Got " + messageContent + ".");
                                mainProcessor.game_initServerData = serverData;

                                mainProcessor.sendInfo("Selecting gear.");

                                packet selectEquipment = packets.packet_game_selectEquipment;
                                selectEquipment.sendPacket();

                                mainProcessor.game_state = constants.gamestate_equipmentSelectionSent;
                                break;
                            case constants.gamestate_equipmentSelectionSent:

                                //if (messageContent != "gamestate") mainProcessor.doDie("Expected gamestate. Got " + messageContent + ".");

                                if (messageContent == "gamestate")
                                {
                                    mapHandling.parseMap(serverData);
                                    //map = 
                                    //mapHandling.parseMap(serverData["map"]);
                                }
                                mainProcessor.sendInfo("Waiting");
                                break;
                            default:
                                network.serverConnection_keepAlive = false; //kill connection
                                break;
                        }
                    }
                }

                network.serverConnection.Close();
                network.serverStreamReader.Close();
                network.serverStreamWriter.Close();

                mainProcessor.doDie("Connection quit by programs.");
            }
            catch (Exception ex)
            {
                mainProcessor.doDie("Network error, another part closed the connection. " + ex.Message);
            }
        }
        #endregion
    }
}
