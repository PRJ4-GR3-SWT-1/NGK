using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace server{
    class Server{
        static void Main(string[] args){

            //derived from https://riptutorial.com/csharp/example/32222/basic-udp-client
            bool done = false;
            int listenPort = 9000;
            using(UdpClient listener = new UdpClient(listenPort))
            {
                Console.WriteLine("UDP client started");
                
                
                while(!done)
                {
                    IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
                    Console.WriteLine("Starting listening");
                    byte[] receivedData = listener.Receive(ref listenEndPoint);

                    Console.WriteLine("Received broadcast message from client {0}", listenEndPoint.ToString());

                    Console.WriteLine("Decoded data is:");
                    string recievedText=Encoding.ASCII.GetString(receivedData);
                    recievedText=recievedText.ToLower();
                    Console.WriteLine(recievedText); //should be "U" or "L" sent from  client
                    
                    string text;
                    if(recievedText=="u") {text = "Uptime: " + File.ReadAllText( "/proc/uptime" );}//From https://stackoverflow.com/a/42110779
                    else if(recievedText=="l"){ text ="Load avg: " + File.ReadAllText( "/proc/loadavg" );}
                    else text ="Sorry, could not parse command :/ ";
                    
                    Console.WriteLine("Replying with: " + text);
                    listener.Send(Encoding.ASCII.GetBytes(text),text.Length,listenEndPoint);

                    //string iptext=listenEndPoint.ToString();
                    //string[] arr=iptext.Split(":");
                    //sendData(text, listenEndPoint);
                    
                    

                }
            }
        } 

       static void sendData(string text, IPEndPoint ep){
            byte[] data = Encoding.ASCII.GetBytes(text);
            try
            {
                using (var client = new UdpClient())
                {
                    Console.WriteLine("Created UdpClient for sending");
                    //client.Connect(ep);
                    Console.WriteLine("Connected?");
                    client.Send(data, data.Length);
                    Console.WriteLine("Data transmitted to ep =" + ep.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
        }
    }
}