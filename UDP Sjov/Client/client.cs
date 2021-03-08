using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace client{
class Client{
static void Main(string[] args){
    byte[] data = Encoding.ASCII.GetBytes("Hello World");
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
            Console.WriteLine("Data transmitted");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}
}
}