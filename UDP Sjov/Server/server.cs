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
                    //Receiving message:
                    IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
                    Console.WriteLine("Starting listening");
                    byte[] receivedData = listener.Receive(ref listenEndPoint);

                    Console.WriteLine("Received broadcast message from client " + listenEndPoint.ToString());

                    Console.WriteLine("Decoded data is:");
                    string recievedText=Encoding.ASCII.GetString(receivedData);
                    recievedText=recievedText.ToLower();
                    Console.WriteLine(recievedText); //should be "U" or "L" sent from  client
                    
                    //Finished Receiving. Starting sending:
                    string returnText;
                    if(recievedText=="u") {returnText = "Uptime: " + File.ReadAllText( "/proc/uptime" );}//From https://stackoverflow.com/a/42110779
                    else if(recievedText=="l"){ returnText ="Load avg: " + File.ReadAllText( "/proc/loadavg" );}
                    else returnText ="Sorry, could not parse command :/ ";
                    
                    Console.WriteLine("Replying with: " + returnText);
                    listener.Send(Encoding.ASCII.GetBytes(returnText),returnText.Length,listenEndPoint);

                }
            }
        } 

       
    }
}