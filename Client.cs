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
        UdpClient client = null;
        string serverAddress = null;
        string localAddress = null;
        int port = 43;

        public void findServer()
        {
            try
            {
                client = new UdpClient();
                client.Client.SendTimeout = 1000;
                client.Client.ReceiveTimeout = 1000;

                localAddress = fetchIPAddress();
                Console.WriteLine("Client IP: " + localAddress);

                var serverEp = new IPEndPoint(IPAddress.Any, port);

                client.EnableBroadcast = true;
                var data = Encoding.ASCII.GetBytes("Xiangqi? " + localAddress);
                client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, port));

                var serverResponseData = client.Receive(ref serverEp);
                var serverResponse = Encoding.ASCII.GetString(serverResponseData);

                Console.WriteLine("Got Response: " + serverResponse + " from " + serverEp);
                String[] splitResponse = serverResponse.Split(" ");
                string serverIPAddress;
                if (splitResponse[0] == "Xiangqi.")
                {
                    serverAddress = splitResponse[1];
                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Runtime Error Detected: " + e.ToString());
            }
        }

        //client sends last move to server to update it
        public void updateServer(String move)
        {
            client = new UdpClient();
            client.Client.SendTimeout = 1000;
            client.Client.ReceiveTimeout = 1000;

            var serverEp = new IPEndPoint(IPAddress.Parse(serverAddress), port);

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
            client = new UdpClient();
            client.Client.SendTimeout = 1000;
            client.Client.ReceiveTimeout = 1000;

            var serverEp = new IPEndPoint(IPAddress.Parse(serverAddress), port);

            var data = Encoding.ASCII.GetBytes("UpdateClient");
            client.Send(data, data.Length, serverEp);

            var serverResponseData = client.Receive(ref serverEp);
            var serverResponse = Encoding.ASCII.GetString(serverResponseData);

            return serverResponse;
            client.Close();
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
