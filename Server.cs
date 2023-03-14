using Newtonsoft.Json;
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
        public int port = 43;
        public string localAddress = null;
        public string clientAddress = null;
        public List<String> moveLog = new List<String>();
        public int currentTurn = -1;
        public bool connected = false;

        public void runServer()
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
                else if (splitRequest[0] == "UpdateServer")
                {
                    moveLog.Add(splitRequest[1]);
                    Console.WriteLine("Server updated.");
                    var response = Encoding.ASCII.GetBytes("Server updated.");
                    server.Send(response, response.Length, clientEp);
                }
                //server sends last move to client
                else if (splitRequest[0] == "UpdateClient")
                {
                    var response = Encoding.ASCII.GetBytes(moveLog[moveLog.Count - 1]);
                    server.Send(response, response.Length, clientEp);
                    currentTurn = -currentTurn;
                }
                else if (splitRequest[0] == "UpdateTurn")
                {
                    var response = Encoding.ASCII.GetBytes(currentTurn.ToString());
                    server.Send(response, response.Length, clientEp);
                    Console.WriteLine("spam time");
                }
                else
                {
                    var response = Encoding.ASCII.GetBytes("Unknown Command");
                    server.Send(response, response.Length, clientEp);
                }
            }
        }

        public string getLocalIPAddress()
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
