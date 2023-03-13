using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Xiangqi
{
    public class Server
    {
        public static int port = 43;
        public static string localAddress = null;
        public static string clientAddress = null;
        public static List<String> moveLog = new List<String>();

        static void Main(string[] args)
        {
            runServer();
        }

        static public void runServer()
        {
            localAddress = getLocalIPAddress();
            var server = new UdpClient(port);

            while (true)
            {
                Console.WriteLine("Server started listening... ");
                var clientEp = new IPEndPoint(IPAddress.Any, port);
                var clientRequestData = server.Receive(ref clientEp);
                var clientRequest = Encoding.ASCII.GetString(clientRequestData);

                Console.WriteLine("Recieved request: " + clientRequest + " from: " + clientEp);
                string[] splitRequest = clientRequest.Split(" ");

                //client and server exchange IP addresses
                if (splitRequest[0] == "Xiangqi?")
                {
                    clientAddress = splitRequest[1];
                    var response = Encoding.ASCII.GetBytes("Xiangqi. " + localAddress);
                    server.Send(response, response.Length, clientEp);
                }
                //client updates the server with it's last move
                if (splitRequest[0] == "UpdateServer")
                {
                    moveLog.Add(splitRequest[1]);
                    Console.WriteLine("Server updated.");
                    var response = Encoding.ASCII.GetBytes("Server updated.");
                    server.Send(response, response.Length, clientEp);
                }
                //server sends last move to client
                if (splitRequest[0] == "UpdateClient")
                {
                    var response = Encoding.ASCII.GetBytes(moveLog[moveLog.Count - 1]);
                    server.Send(response, response.Length, clientEp);
                }
            }
        }

        public static string getLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Could not find local IP address");
        }
    }
}
