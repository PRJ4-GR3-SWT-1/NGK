using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace client{
    class Client{
        static void Main(string[] args){
             //derived from https://riptutorial.com/csharp/example/32222/basic-udp-client
            if(args.Length<2) {
				Console.WriteLine("Husk argumenter ... *Skuffet*");
				return ;
            }

            byte[] data = Encoding.ASCII.GetBytes(args[1]);
            string ipAddress = args[0];
            int sendPort = 9000;
            try
            {
                using (var client = new UdpClient())
                {
                    //Sending message:
                    Console.WriteLine("Created UdpClient");
                    IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), sendPort);
                    Console.WriteLine("Created IpEndPoint");
                    client.Connect(ep);//Can be omitted by adding ep as a third parameter in client.send();
                    Console.WriteLine("Connected?");
                    client.Send(data, data.Length);
                    Console.WriteLine("Data transmitted to server with ep:" + ep.ToString());
                    //Finished sending. Starting recieve:
                    byte[] receivedData = client.Receive(ref ep);
                    Console.WriteLine("Data recieved: ");
                    string recievedText=Encoding.ASCII.GetString(receivedData);
                    Console.WriteLine(recievedText);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
        }
    }
}