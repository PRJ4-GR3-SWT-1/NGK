using System;
using System.IO;
using System.Net;
using System.Net.Sockets;


//using '../LIB/lib.cs';

namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client (string[] args)
		{
			if(args.Length<2) {
				Console.WriteLine("Husk argumenter ... *Skuffet*");
				return ;}
			string ipString=args[0];
			
			string requestedFile = args[1];
			
			
			System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

			Console.WriteLine("Client Started");
            clientSocket.Connect(ipString, PORT);
			Console.WriteLine("Client Connected");
            NetworkStream serverStream = clientSocket.GetStream();
            Console.WriteLine("Trying to recieve file");
			receiveFile(requestedFile,serverStream);
		}

		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile (String fileName, NetworkStream io)
		{
			LIB.writeTextTCP(io,  fileName+"$");
			string lenghtString = LIB.readTextTCP (io);
			
			long fileSize;
			try
			{
				 fileSize=long.Parse(lenghtString);
			}
			catch (System.FormatException)
			{
				Console.WriteLine(lenghtString);
				return ;
				
			}
			
			if (fileSize<1) {
				Console.WriteLine("Vi er stødt på en fejl. længden er rapporteret som 0 :(");
				return ;}
			Console.WriteLine("length= " +lenghtString);

				if(File.Exists(fileName))
				{
					File.Delete(fileName);
				}

				//https://stackoverflow.com/a/2398471
				 using (var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None))
    				using (var bw = new BinaryWriter(fileStream))
					{
						int i;
						for( i=0; i<fileSize-1000;i+=1000){
							byte[] bytearray=new byte[1000];
							io.Read(bytearray,0,1000);
							Console.WriteLine(i.ToString());
							bw.Write(bytearray);
							Console.WriteLine($"Wrote: {bytearray[900]}\n");
						}
						byte[] lastChunk=new byte[1000];
						io.Read(lastChunk,0,1000);
						Console.WriteLine(i.ToString());
						

						bw.Write(lastChunk,0,(int)fileSize%1000);
						Console.WriteLine($"Wrote: {lastChunk[900]}\n");
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
			
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
