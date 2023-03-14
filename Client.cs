using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace Xiangqi
{
    public class Client
    {
        public string otherAddress = null;
        string myAddress = null;
        int port = 43;
        bool haltProcess = false;
        string lastMove = null;
        int currentTurn = -1;
        UdpClient server = null;

        public void findServer()
        {
            try
            {
                UdpClient client = new UdpClient();
                client.Client.SendTimeout = 1000;
                client.Client.ReceiveTimeout = 1000;

                myAddress = fetchIPAddress();
                Console.WriteLine("Client IP: " + myAddress);

                var otherEp = new IPEndPoint(IPAddress.Any, port);

                client.EnableBroadcast = true;
                var data = Encoding.ASCII.GetBytes("Xiangqi? " + myAddress);
                client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, port));

                var serverResponseData = client.Receive(ref otherEp);
                var serverResponse = Encoding.ASCII.GetString(serverResponseData);

                Console.WriteLine("Got Response: " + serverResponse + " from " + otherEp);
                String[] splitResponse = serverResponse.Split(" ");
                if (splitResponse[0] == "Xiangqi.")
                {
                    if (splitResponse[1] != myAddress)
                    {
                        otherAddress = splitResponse[1];
                        var response = Encoding.ASCII.GetBytes("Xiangqi. " + myAddress);
                        server.Send(response, response.Length, otherEp);
                    }
                }
                client.Close();
            }
            catch (Exception e)
            {
                //Console.WriteLine("Runtime Error Detected: " + e.ToString());
                //Console.WriteLine("");
            }
        }

        public void runServer()
        {
            if(server != null)
            {
                server.Close();
                server = null;
            }
            myAddress = fetchIPAddress();
            server = new UdpClient(port);

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
                    if (splitRequest[1] != myAddress){
                        otherAddress = splitRequest[1];
                        var response = Encoding.ASCII.GetBytes("Xiangqi. " + myAddress);
                        server.Send(response, response.Length, clientEp);
                    }
                }
                //send last move to other
                else if (splitRequest[0] == "FetchLastMove")
                {
                    var response = Encoding.ASCII.GetBytes(lastMove);
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

        //client sends last move to server to update it
        public void updateServer(String move)
        {
            UdpClient client = new UdpClient();
            client.Client.SendTimeout = 1000;
            client.Client.ReceiveTimeout = 1000;

            var serverEp = new IPEndPoint(IPAddress.Parse(otherAddress), port);

            var data = Encoding.ASCII.GetBytes("UpdateServer " + move);
            client.Send(data, data.Length, serverEp);

            var serverResponseData = client.Receive(ref serverEp);
            var serverResponse = Encoding.ASCII.GetString(serverResponseData);

            if (serverResponse == "Server updated.")
            {
                Console.WriteLine("Server updated.");
            }
            client.Close();
        }

        public string updateClient()
        {
            UdpClient client = new UdpClient();
            client.Client.SendTimeout = 1000;
            client.Client.ReceiveTimeout = 1000;

            var serverEp = new IPEndPoint(IPAddress.Parse(otherAddress), port);

            var data = Encoding.ASCII.GetBytes("UpdateClient");
            client.Send(data, data.Length, serverEp);

            var serverResponseData = client.Receive(ref serverEp);
            var serverResponse = Encoding.ASCII.GetString(serverResponseData);

            client.Close();
            return serverResponse;
        }

        //requests the current game turn from the server
        public int turnUpdate()
        {
            UdpClient client = new UdpClient();
            client.Client.SendTimeout = 1000;
            client.Client.ReceiveTimeout = 1000;

            var serverEp = new IPEndPoint(IPAddress.Parse(otherAddress), port);

            var data = Encoding.ASCII.GetBytes("UpdateTurn");
            client.Send(data, data.Length, serverEp);
            
            var serverResponseData = client.Receive(ref serverEp);
            var serverResponse = Encoding.ASCII.GetString(serverResponseData);

            return int.Parse(serverResponse);
        }

        public string fetchIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No IPv4 addresses detected");
        }
    }
}
