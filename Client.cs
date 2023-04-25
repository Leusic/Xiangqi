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
        //address of the other player
        public string otherAddress = null;

        public string myAddress = null;

        public int port = 43;

        //true when the server/client has been stopped
        public bool haltProcess = false;

        //stores the last move, used for detecting when a new move has been taken
        public string lastMove = null;
        public int currentTurn = -1;
        public int myTeam = 0;
        public UdpClient server = null;
        public bool gameBegun = false;

        //sends out a udp broadcast of "Xiangqi?" + the player's own address, any other player on the network will respond to with "Xiangqi." if they are also looking for a game
        public void findPlayer()
        {
            UdpClient client = new UdpClient();
            try
            {

                client.Client.SendTimeout = 1000;
                client.Client.ReceiveTimeout = 1000;

                myAddress = fetchIPAddress();

                var otherEp = new IPEndPoint(IPAddress.Any, port);

                client.EnableBroadcast = true;
                var data = Encoding.ASCII.GetBytes("Xiangqi? " + myAddress);
                client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, port));

                var serverResponseData = client.Receive(ref otherEp);
                var serverResponse = Encoding.ASCII.GetString(serverResponseData);

                String[] splitResponse = serverResponse.Split(" ");
                if (splitResponse[0] == "Xiangqi.")
                {
                    if (splitResponse[1] != myAddress)
                    {
                        otherAddress = splitResponse[1];
                        var response = Encoding.ASCII.GetBytes("Xiangqi. " + myAddress);
                        client.Send(response, response.Length, otherEp);
                    }
                }
            }
            catch (Exception e)
            {
            }
            client.Close();
        }

        //both players exchange random team selections until both select different teams, these become their teams
        public void assignTeams()
        {
            UdpClient client = new UdpClient();
            //prevents both teams generating randomly and thus stopping coherence of teams
            if (myTeam == 0)
            {

                var teams = new[] { "-11", "1-1" }; // red/black or black/red , the one assigning teams picks the left team
                Random r = new Random();
                String randTeamsString = teams[r.Next(teams.Length)];

                if((randTeamsString == "-11"))
                {
                    myTeam = -1;
                }
                else
                {
                    myTeam = 1;
                }

                var otherEp = new IPEndPoint(IPAddress.Parse(otherAddress), port);
                var data = Encoding.ASCII.GetBytes("AssignTeam " + randTeamsString);
                client.Send(data, data.Length, otherEp);

                var serverResponseData = client.Receive(ref otherEp);
                var serverResponse = Encoding.ASCII.GetString(serverResponseData);

                if (serverResponse == "OK")
                {
                    gameBegun = true;
                }
            }
            client.Close();
        }

        //runs a udp server that waits for requests from the other player
        public void runServer()
        {
            //removes any previously running server to prevent multiple servers on one machine
            if (server != null)
            {
                server.Close();
                server = null;
                return;
            }
            myAddress = fetchIPAddress();
            server = new UdpClient(port);

            lastMove = "-";

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
                    if ((splitRequest[1] != myAddress) && (gameBegun == false)){
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
                    var response = Encoding.ASCII.GetBytes("OK");
                    lastMove = splitRequest[1];
                    server.Send(response, response.Length, clientEp);
                }
                //ping, used for checking connection status
                else if (splitRequest[0] == "ping")
                {
                    var response = Encoding.ASCII.GetBytes("OK");
                    server.Send(response, response.Length, clientEp);
                }
                //sets player team
                else if (splitRequest[0] == "AssignTeam") // red/black or black/red , the one recieving the teams picks the right team
                {
                    if (splitRequest[1] == "-11")
                    {
                        myTeam = 1;
                    }
                    else
                    {
                        myTeam = -1;
                    }
                    var response = Encoding.ASCII.GetBytes("OK");
                    server.Send(response, response.Length, clientEp);
                }
                //unknown request
                else
                {
                    var response = Encoding.ASCII.GetBytes("Unknown Command");
                    server.Send(response, response.Length, clientEp);
                }
                if(haltProcess == true)
                {
                    server.Close();
                    break;
                }
            }
        }

        //pings server to check if other player is still available
        public bool checkConnection()
        {
            UdpClient client = new UdpClient();
            try
            {
                client.Client.SendTimeout = 2000;
                client.Client.ReceiveTimeout = 2000;
                var serverEp = new IPEndPoint(IPAddress.Parse(otherAddress), port);
                var data = Encoding.ASCII.GetBytes("ping");
                client.Send(data, data.Length, serverEp);

                var serverResponseData = client.Receive(ref serverEp);
            }
            catch(SocketException ex)
            {
                //if error was caused by a timeout
                if (ex.ErrorCode == 10060)
                {
                    Console.WriteLine("timed out");
                    haltProcess = true;
                    return false;
                }
            }
            client.Close();
            return true;
        }

        //client sends last move to server to update it
        public void sendTurn(string moveCode)
        {
            UdpClient client = new UdpClient();
            client.Client.SendTimeout = 2000;
            client.Client.ReceiveTimeout = 2000;

            var serverEp = new IPEndPoint(IPAddress.Parse(otherAddress), port);

            var data = Encoding.ASCII.GetBytes("UpdateTurn " + moveCode);
            client.Send(data, data.Length, serverEp);

            var serverResponseData = client.Receive(ref serverEp);
            var serverResponse = Encoding.ASCII.GetString(serverResponseData);
            client.Close();
        }

        //fetches ip address of machine
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
