using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Xiangqi
{
    public class Client
    {
        UdpClient client = null;
        string address = null;

        public void findServer()
        {
            try
            {
                client = new UdpClient();
                string address = null;
                int port = 43;

                string localIP = fetchIPAddress();
                Console.WriteLine("This Device's IP is: " + localIP);

                string IPBase1 = localIP.Split('.')[0] + "." + localIP.Split('.')[1] + "." + localIP.Split('.')[2] + ".";

                var serverEp = new IPEndPoint(IPAddress.Any, 43);

                client.EnableBroadcast = true;
                var data = Encoding.ASCII.GetBytes("Test data");
                client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 43));

                var serverResponseData = client.Receive(ref serverEp);
                var serverResponse = Encoding.ASCII.GetString(serverResponseData);

                Console.WriteLine("Got Response: " + serverResponse + " from " + serverEp);

                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Runtime Error Detected: " + e.ToString());
            }
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
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                try
                {
                    //connection variables
                    TcpClient client = new TcpClient();
                    string address = null;
                    int port = 43;

                    client.ReceiveTimeout = 1000;
                    client.SendTimeout = 1000;

                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach(var ip in host.AddressList)
                    {
                        client.Connect(ip.ToString(), port);
                    }

                    //for(int i = 0; i <= 255; i++)
                    //{
                        //address = 
                        //client.Connect(address, port);
                    //}

                    StreamWriter sw = new StreamWriter(client.GetStream());
                    StreamReader sr = new StreamReader(client.GetStream());
                    //httpCheck enables different displaying of server responses if a http request is sent
                    bool httpCheck = false;
                    //HTTP 0.9 requests
                    if (Array.Exists(args, x => x == "-h9"))
                    {
                        httpCheck = true;
                        if (args.Length == 2)
                        {
                            args = args.Where(e => e != "-h9").ToArray();
                            sw.WriteLine("GET /" + args[0]);
                        }
                        if (args.Length == 3)
                        {
                            args = args.Where(e => e != "-h9").ToArray();
                            sw.Write("PUT /" + args[0] + "\r\n\r\n" + args[1] + "\r\n");
                        }
                    }
                    //HTTP 1.0 requests
                    else if (Array.Exists(args, x => x == "-h0"))
                    {
                        httpCheck = true;
                        if (args.Length == 2)
                        {
                            args = args.Where(e => e != "-h0").ToArray();
                            sw.WriteLine("GET /?" + args[0] + " HTTP/1.0");
                            sw.WriteLine("");
                        }
                        if (args.Length == 3)
                        {
                            args = args.Where(e => e != "-h0").ToArray();
                            sw.Write("POST /" + args[0] + " HTTP/1.0\r\nContent-Length: " + args[1].Length + "\r\n\r\n"
                                + args[1] + "\r\n");
                        }
                    }
                    //HTTP 1.1 requests
                    else if (Array.Exists(args, x => x == "-h1"))
                    {
                        httpCheck = true;
                        if (args.Length == 2)
                        {
                            args = args.Where(e => e != "-h1").ToArray();
                            sw.Write("GET /?name=" + args[0] + " HTTP/1.1\r\nHost: " + address + "\r\n\r\n");
                        }
                        if (args.Length == 3)
                        {
                            args = args.Where(e => e != "-h1").ToArray();
                            string content = "name=" + args[0] + "&location=" + args[1];
                            sw.Write("POST / HTTP/1.1\r\nHost: " + address + "\r\nContent-Length: " + content.Length.ToString() +
                                "\r\n\r\nname=" + args[0] + "&location=" + args[1]);
                        }
                    }
                    //handles server responses if the request was HTTP
                    if (httpCheck == true)
                    {
                        sw.Flush();
                        string line;
                        string response = "";
                        //reads the response line by line until end of stream or closing html tags
                        while (sr.Peek() > -1)
                        {
                            line = sr.ReadLine();
                            response = (response + line + "\r\n");
                            if (line == "</html>")
                            {
                                break;
                            }
                        }
                        string headerSplitter = "\r\n\r\n";
                        //splits the body from the head of the http response
                        int numLines = response.Split("\r\n").Length;
                        string[] splitResponse = response.Split("\r\n", numLines);
                        if (splitResponse[0] == ("HTTP/0.9 200 OK") || splitResponse[0] == ("HTTP/1.0 200 OK") || splitResponse[0] == ("HTTP/1.1 200 OK"))
                        {
                            if (numLines == 5)
                            {
                                Console.Write(args[0] + " is " + splitResponse[3]);
                            }
                            else
                            {
                                Console.WriteLine(args[0] + " location changed to be " + args[1]);
                            }
                        }
                        else if (splitResponse[0] == ("HTTP/0.9 404 Not Found") || splitResponse[0] == ("HTTP/1.0 404 Not Found") || splitResponse[0] == ("HTTP/1.1 404 Not Found"))
                        {
                            Console.WriteLine("ERROR: no entries found");
                        }
                        else
                        {
                            string body = response.Substring(response.IndexOf(headerSplitter) + headerSplitter.Length);
                            Console.Write(args[0] + " is " + body);
                        }
                    }
                    //default whois response handling
                    else
                    {
                        //sending requests to the server
                        if (args.Length == 1)
                        {
                            sw.Write(args[0]);
                        }
                        else if (args.Length == 2)
                        {
                            sw.Write(args[0] + " " + args[1]);
                        }

                        sw.Flush();
                        string response = sr.ReadToEnd();

                        if (args.Length == 1)
                        {
                            if (response == "ERROR: no entries found\r\n")
                            {
                                Console.WriteLine(response);
                            }
                            //valid person
                            else
                            {
                                Console.WriteLine(args[0] + " is " + response);
                            }
                        }
                        else if (args.Length == 2)
                        {
                            //change user location
                            if (response == "OK\r\n")
                            {
                                Console.WriteLine(args[0] + " location changed to be " + args[1]);
                            }
                        }
                        else
                        {
                            Console.WriteLine(response);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Runtime Error Detected: " + e.ToString());
                }
            }
            else
            {
                Console.WriteLine("Argument Error");
            }
        }
    }
}
