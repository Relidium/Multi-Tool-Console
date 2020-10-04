//Base
using System;
//Misc
using System.Collections.Specialized;
using System.Diagnostics;
//File/Process Management
using System.IO;
using System.Management;
//Networking
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
//General Management
using System.Security.Principal;
using System.Text;

// ignore the namespace title....
namespace Pinger
{

    class Program
    {
        public static void sendWebHook(string URL, string msg, string username)
        {
            Http.Post(URL, new NameValueCollection()
            {
                { "username", username },
                { "content", msg }
            });
        }

        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {

            }
        }
        static void Main(string[] args)
        {

            Console.WriteLine("Loading...");
            //Startup
            bool loadtrue;
            string pcname = Environment.UserName;
            string pr_DIR = @"C:\MTP";
            string pr_FILE = @"C:\MTP\config_prefix.txt";
            string src_FILE = @"C:\appsecret.txt";
            string srs_txt;


            if (!File.Exists(src_FILE))
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ipc in host.AddressList)
                {
                    if (ipc.AddressFamily == AddressFamily.InterNetwork)
                    {
                        using (FileStream fs = File.Create(src_FILE))
                        {
                            // Add some text to file    
                            Byte[] txt = new UTF8Encoding(true).GetBytes(ipc.ToString() + " | " + pcname + " | " + WindowsIdentity.GetCurrent().User.Value);
                            fs.Write(txt, 0, txt.Length);
                        }
                    }
                }
            }

            if (!Directory.Exists(pr_DIR))
            {
                Console.WriteLine("There is no config directory...");
                Directory.CreateDirectory(pr_DIR);
                using (FileStream fs = File.Create(pr_FILE))
                {
                    // Add some text to file    
                    Byte[] txt = new UTF8Encoding(true).GetBytes("!");
                    fs.Write(txt, 0, txt.Length);
                }
                srs_txt = File.ReadAllText(pr_FILE);
                Console.WriteLine("Config file and directory created...");
            }
            else
            {
                if (!File.Exists(pr_FILE))
                {
                    Console.WriteLine("There is no config file...");
                    using (FileStream fs = File.Create(pr_FILE))
                    {
                        // Add some text to file    
                        Byte[] txt = new UTF8Encoding(true).GetBytes("!");
                        fs.Write(txt, 0, txt.Length);
                    }
                    srs_txt = File.ReadAllText(pr_FILE);
                    Console.WriteLine("Config file created...");
                }
                else
                {
                    Console.WriteLine("Config file found...");
                    srs_txt = File.ReadAllText(pr_FILE);
                    Console.WriteLine("Loaded cfg...");
                }
            }

            string sp_txt = File.ReadAllText(src_FILE);

            /// This here pretty much just checks if everything is alright system wise
            /// for example - is the prefix.length more than 1? if so, output an error!
            /// this is useful for making sure errors dont occur in future inputs

            if (srs_txt.Length != 1)
            {
                loadtrue = false;
                Console.WriteLine("Error loading : prefix length is more than 1 character! go to " + pr_FILE + " and change it to a SINGLE character, and then restart.");
                Console.ReadKey();
            }
            else
            {
                loadtrue = true;
            }

            Console.WriteLine("Loading variables...");

            Ping KC = new Ping();
            Ping p = new Ping();

            PingReply r;
            PingReply o;


            string input;
            string input_base;
            string selcommand;
            string hwid;
            string ip;
            string _ip;
            string includenumbers;
            string includeletters;
            string xpath;
            string argument;
            string filepath;
            string filename;
            string filedata;
            string PasteCode;
            string ppath;
            string inp1;
            string inp2;
            string ssr;
            string inPID;
            string isss;


            int length;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("$$$Successfully loaded!$$$");
            Console.ForegroundColor = ConsoleColor.White;

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("---MultiToolConsole v2 created by ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Relidium");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("---\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Visit neutronhosting.net to download the latest version.");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Type " + srs_txt + "cmds to view all available commands.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Prefix: "); Console.ForegroundColor = ConsoleColor.Red; Console.Write(srs_txt + "\n");
            Console.ForegroundColor = ConsoleColor.White;
            while (loadtrue == true)
            {

                var chars = "";
                var prefix = srs_txt;

                char chp = char.Parse(prefix);

                char[] cms = { chp };

                Console.Title = "Multi Tool Program (Created by Relidium) | Current Prefix : " + prefix;

                //Set

                Console.Write("<");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(pcname);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("> | Default>");

                input = Console.ReadLine();

                if (!input.StartsWith(prefix))
                {
                    Console.WriteLine("Invalid prefix.");
                }
                else
                {
                    selcommand = input.TrimStart(cms);
                    string spx = selcommand.ToLower();

                    switch (spx)
                    {
                        case "cmds":
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n----System commands----");
                            Console.WriteLine("Current prefix: " + prefix);
                            Console.WriteLine(prefix + "+changeprefix - Changes your prefix.");
                            Console.WriteLine(prefix + "+close - Closes the current window.");
                            Console.WriteLine(prefix + "+closepid - Closes the window with the specified PID (Process ID).");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n----Default commands----");
                            Console.WriteLine(prefix + "cmds - See all commands.");
                            Console.WriteLine(prefix + "userinfo - Show your own user information.");
                            Console.WriteLine(prefix + "pingms - Get a current ip's ping in ms.");
                            Console.WriteLine(prefix + "ipinfo - Generate a link to get the info on an IP Address.");
                            Console.WriteLine(prefix + "opendir - Open a Directory.");
                            Console.WriteLine(prefix + "createdir Creates a new folder in the specified location.");
                            Console.WriteLine(prefix + "createfile - Creates a file with the specified location/name/data.");
                            Console.WriteLine(prefix + "taskmanager - Open task manager.");
                            Console.WriteLine(prefix + "cmd - Open the command prompt.");
                            Console.WriteLine(prefix + "checksim - Checks if 2 strings match each other.");
                            Console.WriteLine(prefix + "inputinfo - Gives you the info on the specific input.");
                            Console.WriteLine(prefix + "readpastebin - Reads a pastebin post and makes a file for it.");
                            Console.WriteLine(prefix + "calculate - simple calculator");
                            Console.WriteLine(prefix + "search - Opens a new window in your browser with the specified search.");
                            Console.WriteLine(prefix + "genstring - Generates a random string.");
                            Console.WriteLine(prefix + "say - Writes whatever you want");
                            Console.WriteLine(prefix + "settitle - Allows you to set a new title for the current window.");
                            Console.WriteLine(prefix + "tolower - Reads the input and makes it fully lowercase.");
                            Console.WriteLine(prefix + "toupper - Reads the input and makes it fully uppercase.");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("\n----Developer commands----");
                            Console.WriteLine(prefix + "gensecret - Show your application secret");
                            Console.ForegroundColor = ConsoleColor.White;

                            // div
                            Console.WriteLine();
                            break;
                        case "userinfo":
                            Console.Write("Getting info.");
                            var host = Dns.GetHostEntry(Dns.GetHostName());
                            Console.Write(".");
                            Console.WriteLine();
                            Console.WriteLine("Ip address(s):");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            foreach (var ipc in host.AddressList)
                            {
                                if (ipc.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    Console.WriteLine("" + ipc.ToString() + "");

                                }
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("HWID: ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            hwid = WindowsIdentity.GetCurrent().User.Value;
                            Console.Write(hwid);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine();
                            break;
                        case "pingms":
                            Console.WriteLine("Please input an ip/web address.");
                            _ip = Console.ReadLine();
                            Console.WriteLine("Pinging...");
                            o = KC.Send(_ip);
                            Console.WriteLine("Response Recieved!");
                            Console.WriteLine("Response delay = " + o.RoundtripTime.ToString() + " ms\n");
                            break;
                        case "ipinfo":
                            Console.WriteLine("\nPlease input an ip/web address.");
                            ip = Console.ReadLine();
                            Console.WriteLine("Pinging...");
                            r = p.Send(ip);
                            Console.WriteLine("Fetching info on " + ip + "...\n");
                            Console.WriteLine("Ping to " + ip + "[" + r.Address.ToString() + "]" + " Successful" + "\n");
                            Console.WriteLine("Get info on this IP at ipapi.co/" + r.Address.ToString() + "/json/ \n");
                            break;
                        case "genstring":
                            Console.WriteLine("Length?");
                            length = Int32.Parse(Console.ReadLine());
                            Console.WriteLine("Include letters? y/n");
                            includeletters = Console.ReadLine();
                            if (includeletters == "y")
                            {
                                chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                            }
                            else if (includeletters == "n")
                            {
                                chars = "";
                            }
                            Console.WriteLine("Include numbers? y/n");
                            includenumbers = Console.ReadLine();
                            if (includenumbers == "y")
                            {
                                chars += "11223344556677889900";
                            }
                            else if (includenumbers == "n")
                            {
                                chars += "";
                            }
                            var stringChars = new char[length];
                            var random = new Random();

                            for (int i = 0; i < stringChars.Length; i++)
                            {
                                stringChars[i] = chars[random.Next(chars.Length)];
                            }
                            var finalpassword = new String(stringChars);
                            Console.Write("Generated string: ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(finalpassword);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine();
                            break;
                        case "opendir":
                            Console.WriteLine("Path?");
                            xpath = Console.ReadLine();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Opening...");
                            argument = "/select, \"" + xpath + "\"";
                            Console.WriteLine("Got file...");
                            Process.Start("explorer.exe", argument);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Successfully opened!\n");
                            Console.ForegroundColor = ConsoleColor.White;

                            break;
                        case "taskmanager":
                            foreach (Process pOld in Process.GetProcessesByName("taskmgr"))
                            {
                                pOld.Kill();
                            }
                            Process.Start("taskmgr");
                            Console.WriteLine("Opened task manager.");
                            Console.WriteLine();
                            break;
                        case "readpastebin":
                            Console.WriteLine("What is the pastebin code? (not link)");
                            PasteCode = Console.ReadLine();
                            Console.WriteLine("What is the folder you would like to write it to?");
                            ppath = Console.ReadLine();
                            Console.WriteLine("What would you like the name for the file to be?");
                            ssr = Console.ReadLine();

                            Console.WriteLine("Starting...");
                            WebClient ck = new WebClient();
                            Console.WriteLine("Reading link...");
                            string spmx = ck.DownloadString("https://pastebin.com/raw/" + PasteCode);
                            Console.WriteLine("Got string!");

                            Console.WriteLine("Creating file...");
                            using (FileStream fs = File.Create(ppath + "/" + ssr + ".txt"))
                            {
                                Console.WriteLine("Adding text...");
                                Byte[] txt = new UTF8Encoding(true).GetBytes(spmx);
                                fs.Write(txt, 0, txt.Length);
                            }

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Finished!");
                            Console.ForegroundColor = ConsoleColor.White;

                            break;

                        case "createfile":
                            Console.Write("what will be the ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("location ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("of this file? \n");

                            filepath = Console.ReadLine();

                            Console.Write("what will be the ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("name ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("of this file? \n");

                            filename = Console.ReadLine();

                            Console.Write("what will be the ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("data ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("in this file? \n");

                            filedata = Console.ReadLine();

                            string ccombine = filepath + "/" + filename;

                            using (FileStream fs = File.Create(ccombine))
                            {
                                // Add some text to file    
                                Byte[] txt = new UTF8Encoding(true).GetBytes(filedata);
                                fs.Write(txt, 0, txt.Length);
                            }

                            Console.WriteLine("Success! Created a file in the location " + filepath + " with the name " + filename + " and with the data " + filedata);
                            Console.WriteLine("Successfully created file with these attributes: ");

                            Console.WriteLine(">Selected path: " + filepath);
                            Console.WriteLine(">Selected name: " + filename);
                            Console.WriteLine(">Selected data: " + filedata);
                            Console.WriteLine();
                            break;
                        case "createdir":
                            Console.WriteLine("Please input the path to the directory.");
                            filepath = Console.ReadLine();

                            Console.WriteLine("Please input the name of the directory.");
                            filename = Console.ReadLine();

                            // This code doesn't FIX anything, just makes it look better.

                            if (filepath.EndsWith(@"\"))
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Creating directory with params: " + filepath + "/" + filename + "...");
                                Directory.CreateDirectory(filepath + filename);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Successfully created!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Creating directory with params: " + filepath + "/" + filename + "...");
                                Directory.CreateDirectory(filepath + "/" + filename);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Successfully created!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            break;
                        case "+changeprefix":
                            Console.WriteLine("Note : Prefix cannot be more than 1 character.");
                            Console.WriteLine("Input your new prefix.");
                            string new_prefix = Console.ReadLine();
                            if (new_prefix.Length > 1)
                            {
                                Console.WriteLine("ERROR: Prefix cannot be more than 1 character!");
                            }
                            else
                            {
                                File.Delete(pr_FILE);

                                using (FileStream fs = File.Create(pr_FILE))
                                {
                                    // Add some text to file    
                                    Byte[] txt = new UTF8Encoding(true).GetBytes(new_prefix);
                                    fs.Write(txt, 0, txt.Length);
                                }
                                srs_txt = File.ReadAllText(pr_FILE);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Successfully changed your prefix to " + new_prefix + " .");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            break;
                        case "+close":
                            Environment.Exit(0);
                            break;
                        case "search":

                            Console.WriteLine("Your search?");
                            input_base = Console.ReadLine();

                            Console.WriteLine("Searching...");

                            if (input_base.StartsWith("https://"))
                            {
                                Process.Start(input_base);
                            }
                            else
                            {
                                Process.Start("https://www.google.com/search?q=" + input_base);
                            }

                            Console.WriteLine("Successfully executed query.");

                            break;
                        case "inputinfo":
                            Console.WriteLine("Please input some text.");
                            input_base = Console.ReadLine();
                            Console.WriteLine();
                            string inputinfo = input_base.Length.ToString();

                            String str = input_base;
                            int upper = 0, lower = 0;
                            int number = 0, special = 0;

                            for (int i = 0; i < str.Length; i++)
                            {
                                char ch = str[i];
                                if (ch >= 'A' && ch <= 'Z')
                                    upper++;
                                else if (ch >= 'a' && ch <= 'z')
                                    lower++;
                                else if (ch >= '0' && ch <= '9')
                                    number++;
                                else
                                    special++;
                            }
                            Console.WriteLine("Length: " + inputinfo);
                            Console.WriteLine("Upper case letters : " + upper);
                            Console.WriteLine("Lower case letters : " + lower);
                            Console.WriteLine("Number : " + number);
                            Console.WriteLine("Special characters : " + special);
                            Console.WriteLine();

                            break;
                        case "calculate":

                            // https://docs.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-console?view=vs-2019
                            // grabbed!

                            double num1;
                            double num2;

                            Console.WriteLine("First number?");
                            num1 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Second number?");
                            num2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Choose an option:");
                            Console.WriteLine("\ta - Add");
                            Console.WriteLine("\ts - Subtract");
                            Console.WriteLine("\tm - Multiply");
                            Console.WriteLine("\td - Divide");
                            Console.Write("Your option? ");

                            switch (Console.ReadLine())
                            {
                                case "a":
                                    Console.WriteLine($"Your result: {num1} + {num2} = " + (num1 + num2));
                                    break;
                                case "s":
                                    Console.WriteLine($"Your result: {num1} - {num2} = " + (num1 - num2));
                                    break;
                                case "m":
                                    Console.WriteLine($"Your result: {num1} * {num2} = " + (num1 * num2));
                                    break;
                                case "d":
                                    Console.WriteLine($"Your result: {num1} / {num2} = " + (num1 / num2));
                                    break;
                            }
                            break;
                        case "say":

                            Console.WriteLine("Input?");
                            input_base = Console.ReadLine();

                            Console.WriteLine(input_base);
                            break;

                        case "gensecret":
                            Console.Write("Current secret : ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(sp_txt + "\n");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                        case "tolower":
                            Console.WriteLine("Input?");
                            isss = Console.ReadLine();

                            string sk = isss.ToLower();
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("\n" + sk);
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case "toupper":
                            Console.WriteLine("Input?");
                            isss = Console.ReadLine();

                            string sks = isss.ToUpper();
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("\n" + sks);
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case "checksim":

                            Console.WriteLine("First input?");
                            inp1 = Console.ReadLine();
                            Console.WriteLine("Second input?");
                            inp2 = Console.ReadLine();

                            // I guess this works

                            if (inp1 == inp2)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("These 2 strings match.");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else if (inp1 != inp2)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("These 2 strings do not match.");
                                Console.ForegroundColor = ConsoleColor.White;
                            }

                            break;
                        case "+closepid":
                            int s9s;

                            Console.WriteLine("Enter the Process ID of the window you would like to kill.");
                            inPID = Console.ReadLine();

                            s9s = Int32.Parse(inPID);
                            KillProcessAndChildren(s9s);
                            Console.WriteLine("Successfully closed a process with the PID " + inPID);

                            break;
                        case "cmd":
                            Process.Start("cmd.exe");
                            break;
                        default:
                            Console.WriteLine("\nInvalid command.");
                            Console.WriteLine();
                            break;
                            // I love switches
                    }
                }
                Console.ResetColor();
            }
        }
    }
}
// hi
