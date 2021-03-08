using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace client{
    class Client{
        static void UdpReceive(){
            int listenPort = 9001;
            using(UdpClient listener = new UdpClient(listenPort))
            {
                Console.WriteLine("UDP listen client started");
                IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
            
                Console.WriteLine("Starting listening");
                byte[] receivedData = listener.Receive(ref listenEndPoint);

                Console.WriteLine("Received broadcast message from client {0}", listenEndPoint.ToString());

                Console.WriteLine("Decoded data is:");
                string recievedText=Encoding.ASCII.GetString(receivedData);
                Console.WriteLine(recievedText); //should be "Uptime" sent from above client
               
            }
        }
        static void Main(string[] args){
             //derived from https://riptutorial.com/csharp/example/32222/basic-udp-client

            byte[] data = Encoding.ASCII.GetBytes("u");
            string ipAddress = "10.0.0.1";
            int sendPort = 9000;
            try
            {
                using (var client = new UdpClient())
                {
                    Console.WriteLine("Created UdpClient");
                    IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), sendPort);
                    Console.WriteLine("Created IpEndPoint");
                    client.Connect(ep);
                    Console.WriteLine("Connected?");
                    client.Send(data, data.Length);
                    Console.WriteLine("Data transmitted ep =" + ep.ToString());

                    /*IPEndPoint receiveEP = null;
                    byte[] receivedData = client.Receive(ref receiveEP);
                    string recievedText=Encoding.ASCII.GetString(receivedData);
                    Console.WriteLine(recievedText);*/

                    
                    UdpReceive();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
        }
    }
}