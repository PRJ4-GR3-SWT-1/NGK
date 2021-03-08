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
            if(args.Length<2) {
				Console.WriteLine("Husk argumenter ... *Skuffet*");
				return ;}

            byte[] data = Encoding.ASCII.GetBytes(args[1]);
            string ipAddress = args[0];
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
                    Console.WriteLine("Data transmitted to server with ep:" + ep.ToString());

                    //IPEndPoint receiveEP = new IPEndPoint(IPAddress.Any, sendPort);
                    byte[] receivedData = client.Receive(ref ep);
                    Console.WriteLine("Data recieved: ");
                    string recievedText=Encoding.ASCII.GetString(receivedData);
                    Console.WriteLine(recievedText);

                    
                    //UdpReceive();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
        }
    }
}