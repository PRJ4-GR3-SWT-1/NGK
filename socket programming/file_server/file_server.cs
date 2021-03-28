using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace tcp
{
	class file_server
	{
		/// <summary>
		/// The PORT
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// Opretter en socket.
		/// Venter på en connect fra en klient.
		/// Modtager filnavn
		/// Finder filstørrelsen
		/// Kalder metoden sendFile
		/// Lukker socketen og programmet
 		/// </summary>
		private file_server ()
		{
			TcpListener serverSocket=null;
			TcpClient clientSocket=null;
			try{
			//SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
			
			//TcpListener serverSocket = new TcpListener(PORT);
			 serverSocket = new TcpListener(IPAddress.Any,PORT);


			
			 clientSocket = default(TcpClient);
			
			serverSocket.Start();

			while(true){
				Console.WriteLine(" >> Server Started listening for request");
				clientSocket = serverSocket.AcceptTcpClient();
				Console.WriteLine($" Forbindelse til {clientSocket.ToString()}");
				NetworkStream networkStream = clientSocket.GetStream();
				//byte[] bytesFrom = new byte[1000];
				Console.WriteLine((int)clientSocket.ReceiveBufferSize);
               
				string dataFromClient = LIB.readTextTCP ( networkStream);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

				long length =LIB.check_File_Exists ( dataFromClient);
				
                Console.WriteLine(" >> Data from client - " + dataFromClient);

                string serverResponse = length.ToString();
                if(length<1) serverResponse="Filen kunne ikke lokaliseres";
				LIB.writeTextTCP(networkStream,serverResponse);
                 Console.WriteLine(" >> " + serverResponse);
				 if(length>0) sendFile(dataFromClient, length, networkStream);
				
			}
	}
	catch(SocketException e)
    {
      Console.WriteLine("SocketException: {0}", e);
    }
    finally
    {
       // Stop listening for new clients.
	   clientSocket.Close();
    }
			
		
	
		}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// The filename.
		/// </param>
		/// <param name='fileSize'>
		/// The filesize.
		/// </param>
		/// <param name='io'>
		/// Network stream for writing to the client.
		/// </param>
		private void sendFile (String fileName, long fileSize, NetworkStream io)
		{
			using (BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open,FileAccess.Read)))
        	{
			byte[] chunk=new byte[1000];
			//var base64 = Convert.ToBase64String(fileContents);
			int i;
			for( i=0; i<fileSize-1000;i+=1000)
			{
				Console.WriteLine("Readning and sending. count="+i.ToString());
				reader.BaseStream.Seek(i, SeekOrigin.Begin);
    			reader.Read(chunk, 0, 1000);
				io.Write(chunk, 0, 1000);
			}
			Console.WriteLine("Readning and sending. count="+i.ToString());
			reader.BaseStream.Seek(i, SeekOrigin.Begin);
			reader.Read(chunk,0,(int)(fileSize%1000));
			io.Write(chunk, 0, (int)(fileSize%1000));
			}
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Server starts...");
			var file_server = new file_server();
			
		}
	}
}
