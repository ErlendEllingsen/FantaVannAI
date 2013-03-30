using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace fantaVannAI
{
    public class packet
    {
        public string packetContent = "";

        #region Set packet data
        public packet(Object input = null)
        {
            if (!(input == null))
            {
                setPacket(input);
            }
        }

        public Boolean setPacket(Object input)
        {
            try 
            {
                packetContent = JsonConvert.SerializeObject(input, Formatting.None);
                return true;
            } catch
            {
                return false;
            }
        }
        #endregion

        public Boolean sendPacket()
        {
            try
            {
                network.serverStreamWriter.WriteLine(this.packetContent);
                network.serverStreamWriter.Flush();

                Console.WriteLine(this.packetContent);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("[ERROR] While sending packet " + packetContent + Environment.NewLine + "Error: " + ex.InnerException.Message);
                return false;
            }
        }
    }
}
