using System;
using System.IO;
using System.Net.Sockets;

namespace FDRPConnect
{
    public class FDRP
    {
        private TcpClient client;
        public string res = String.Empty;

        public FDRP() { }

        public string fnConnectResult(string sNetIP, int iPORT_NUM)
        {
            try
            {
                client = new TcpClient(sNetIP, iPORT_NUM);
                return "Connection Succeeded";
            }
            catch (Exception ex)
            {
                return "Server is not active.  Please start server and try again.      " + ex.ToString();
            }
        }

        public void fnPacket(string sInfo)
        {
            SendData("" + sInfo);
        }

        private void SendData(string data)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.Write(data + (char)13 + (char)10);
            writer.Flush();
        }
    }
}