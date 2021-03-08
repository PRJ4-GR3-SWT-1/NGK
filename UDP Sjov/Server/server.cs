using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace server{
    class Server{
        static void Main(string[] args){
            bool done = false;
            int listenPort = 9000;
            using(var listener = new UdpClient(listenPort))
            {
                IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
                while(!done)
                {
                    byte[] receivedData = listener.Receive(ref listenPort);

                    Console.WriteLine("Received broadcast message from client {0}", listenEndPoint.ToString());

                    Console.WriteLine("Decoded data is:");
                    Console.WriteLine(Encoding.ASCII.GetString(receivedData)); //should be "Hello World" sent from above client
                }
            }
        }   
    }
}