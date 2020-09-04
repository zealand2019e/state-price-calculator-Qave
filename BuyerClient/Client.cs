using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace BuyerClient
{
    public class Client
    {
        public void Start()
        {
            try
            {
                using (TcpClient socket = new TcpClient("localhost", 7))
                {
                    NetworkStream ns = socket.GetStream();
                    StreamReader streamReader = new StreamReader(ns);
                    StreamWriter streamWriter = new StreamWriter(ns);
                    Console.WriteLine("Please provide following details regarding your purchase seperated by comma, in the following order:\nNumber of items, StateCode. For example: 10, UT");
                    Console.WriteLine("Available state codes: UT, NV, TX, AL, CA.");
                    while (true)
                    {
                        try
                        {
                            string ClientInfo = Console.ReadLine();
                            streamWriter.WriteLine(ClientInfo);
                            streamWriter.Flush();
                            string line = streamReader.ReadLine();
                            Console.WriteLine(line);
                        }
                        catch (IOException)
                        {
                            Console.WriteLine("Connection to the server cannot be made, is it running?");
                            return;
                        }
                    }
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("No connection could be made to the server.");
                return;
            }
        }
    }
}
