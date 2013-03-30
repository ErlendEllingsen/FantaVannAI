using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace fantaVannAI
{
    /*
     *  Written by Erlend Aspelund Matre Ellingsen
     *  In co-operation with Kevin <<>>.
     *  Written 2013.
     *  
     * 
     *  Third party libraries used:
     *  - http://james.newtonking.com JSON
     *  
     *  
     */

    class mainProcessor
    {
        #region Definitions
            //network is moved to its own class. network.cs
        #region general


            public static Process currentProc = Process.GetCurrentProcess();
            public static int protocol_revision = 1;

            #region game
            public static int game_state;
            public static Object game_map;
            public static Object game_initServerData;
            public static Object game_lastServerData;

            #region player_me
            #endregion

            #region player_enemies
            public static Object[] game_enemies;
            #endregion

            #endregion
            #region extra
            public static Random rand = new Random();
            #endregion
        #endregion

        #region AI stuff
        #region general

        #endregion
        #region pathFinding

        #endregion
        #region gameTactic
        #endregion
        #endregion
        

        #endregion

        #region Functions

        #region Freq funcs
        public static string Split(string msg, string delim, int part)
        {
            try
            {
                for (int a = 0; a < part; a++)
                {
                    msg = msg.Substring(msg.IndexOf(delim) + delim.Length);
                }
                if (msg.IndexOf(delim) == -1) return msg;
                return msg.Substring(0, msg.IndexOf(delim));
            }
            catch { return "split-error"; }
        }
        #endregion
        #region System funcs
        #endregion
        #region Extra funcs
        public static void doDie(string msg)
        {
            Console.WriteLine("[ERROR] " + msg + " GameState: " + Convert.ToString(game_state));
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static void sendInfo(string msg)
        {
            Console.WriteLine("[INFO] " + msg);
        }
        #endregion

        #endregion

        static void Main(string[] args)
        {

            //init game loop.
            Boolean connection = network.connection_establish();
            if (!connection) doDie("Connection failed.");

            sendInfo("Connection OK. Starting AI.");
            network.connection_keeper();
        }
    }
}
 