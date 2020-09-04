using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace StatePriceCalculator
{
    public class Server
    {
        public void Start()
        {
            TcpListener ServerListen = new TcpListener(IPAddress.Loopback, 7);
            ServerListen.Start();

            TcpClient socket = ServerListen.AcceptTcpClient();

            using (socket)
            {
                DoClient(socket);
            }
            Console.ReadKey();
        }
        public double NumberOfItems { get; set; }
        public string StateCode { get; set; }
        public void DoClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();

            StreamReader streamReader = new StreamReader(ns);
            StreamWriter streamWriter = new StreamWriter(ns);

            while (true)
            {
                double priceAfterDiscount;
                double priceAfterStateTax;
                string[] informations = new string[2];

                try
                {

                    string line = streamReader.ReadLine();
                    informations = line.Split(", ");
                    if (informations.Length > 1 && informations.Length < 3)
                    {
                        try
                        {
                            NumberOfItems = Convert.ToDouble(informations[0]);
                        }
                        catch (FormatException)
                        {
                            streamWriter.WriteLine("Please use the right format, fx: 10, UT");
                            streamWriter.Flush();
                            continue;
                        }

                        StateCode = informations[1];
                        priceAfterDiscount = CalculatePrice.GetDiscountRate(NumberOfItems);
                        priceAfterStateTax = CalculatePrice.CalculateStateTax(StateCode, priceAfterDiscount);
                        if (CalculatePrice.IsStateCodeWrong == true)
                        {
                            streamWriter.WriteLine("Please use one of the available state codes provided above.");
                            streamWriter.Flush();
                            continue;
                        }
                        Console.WriteLine(line);
                        streamWriter.WriteLine("Total price after discount and taxes have been applied: " + string.Format("{0:0.00}", priceAfterStateTax) + "$");
                        streamWriter.Flush();
                        
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(informations[0]))
                        {
                            continue;
                        }
                        streamWriter.WriteLine("Please provide us with the correct details. fx: 10, UT");
                        streamWriter.Flush();
                        continue;
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("Connection to the client was closed");
                    return;
                }

                // wont work :((( check later
                streamWriter.WriteLine("Would you like to purchase more items? Y / N");
                streamWriter.Flush();
                string answer = streamReader.ReadLine();
                if (answer == "Y")
                {
                    continue;
                }
                if (answer == "N")
                {
                    Environment.Exit(0);
                }
            }


        }
    }
}
