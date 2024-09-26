using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sys = Cosmos.System;


namespace SimpleOS
{
    public class Kernel : Sys.Kernel
    {
        private const string rootLoginFail = "Cannot login as root: Access denied.";
        private DateTime lastInputTime;
        private string currentDirectory = "/";
        private readonly Dictionary<string, string> files = new Dictionary<string, string>();
        private readonly Dictionary<string, HashSet<string>> directories = new Dictionary<string, HashSet<string>>();
        public string username = "LiveCD";
        public static string currentUser = "LiveCD";
        public string password = "live";
        public static string version = "0.6 Slimy";
        public static string osASCIILogo = "  ___ _            _      ___  ___ \r\n / __(_)_ __  _ __| |___ / _ \\/ __|\r\n \\__ \\ | '  \\| '_ \\ / -_) (_) \\__ \\\r\n |___/_|_|_|_| .__/_\\___|\\___/|___/\r\n             |_|                   ";
        public static string osName = "SimpleOS";
        string warningASCII = " ___       __   ________  ________  ________   ___  ________   ________     \r\n|\\  \\     |\\  \\|\\   __  \\|\\   __  \\|\\   ___  \\|\\  \\|\\   ___  \\|\\   ____\\    \r\n\\ \\  \\    \\ \\  \\ \\  \\|\\  \\ \\  \\|\\  \\ \\  \\\\ \\  \\ \\  \\ \\  \\\\ \\  \\ \\  \\___|    \r\n \\ \\  \\  __\\ \\  \\ \\   __  \\ \\   _  _\\ \\  \\\\ \\  \\ \\  \\ \\  \\\\ \\  \\ \\  \\  ___  \r\n  \\ \\  \\|\\__\\_\\  \\ \\  \\ \\  \\ \\  \\\\  \\\\ \\  \\\\ \\  \\ \\  \\ \\  \\\\ \\  \\ \\  \\|\\  \\ \r\n   \\ \\____________\\ \\__\\ \\__\\ \\__\\\\ _\\\\ \\__\\\\ \\__\\ \\__\\ \\__\\\\ \\__\\ \\_______\\\r\n    \\|____________|\\|__|\\|__|\\|__|\\|__|\\|__| \\|__|\\|__|\\|__| \\|__|\\|_______|\r\n                                                                            \r\n                                                                            \r\n                                                                            ";
        string osBuild = "0272";
        string osNamenoUppercaseLetters = "simpleos";
        string channel = "beta";
        bool guiEnabled = false;
        bool rootAllowed = true;
        bool ACPIenabled = true;
        string settingsPath = "/sys/config.ini";
        string kernelpanic_dummy = "DUMMY_KERNEL_PANIC";
        string kernelpanic_forcebomb = "FORCE_BOMB_ACTIVATED";
        string kernelpanic_memorystackoverflow = "MEMORY_STACK_OVERFLOW";
        string kernelpanic_driverinitfailed = "DRIVER_EXCEPTION_INITIALIZATION_FAILED";
        string kernelpanic_memorycorruption = "MEMORY_CORRUPTED";
        string kernelpanic_kernelexception = "KERNEL_EXCEPTION";
        string kernelpanic_bootconfigurationisincorrect = "INCORRECT_BOOT_CONFIGURATION";
        string kernelpanic_gpufailure = "GPU_FAILURE";

        private void askUsername()
        {
            Console.WriteLine("Username:");
            string userInput = Console.ReadLine();
            if (userInput != username)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("E: ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("User is not found!\n");
                askUsername();
            }
            else
            {
                askPassword();
            }
        }

        public void Cosfetch()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("CosFetch - an open source Neofetch Clone for Cosmos");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"< CPU Model: {CPU.GetCPUBrandString()} >");
            Console.WriteLine($"< RAM Amount: {GCImplementation.GetAvailableRAM()} >");
            Console.WriteLine($"< RAM in use: {GCImplementation.GetUsedRAM()} >");
            Console.WriteLine($"< Current User: LiveCD >");
            Console.WriteLine($"Sudo Permissions: " + rootAllowed);
            Console.WriteLine($"< Operating System: " + osName + " >");
            Console.WriteLine($"< OS Version: " + version + " >");
        }

        private void askPassword()
        {
            Console.WriteLine("Password:");
            string passInput = Console.ReadLine();
            if (passInput != password)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("E: ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Incorrect password!\n");
                askUsername();
            }
            else
            {
                Console.WriteLine("Logging in...");
            }
        }
        protected override void BeforeRun()
        {
            Console.WriteLine("Loading module 'RootCheck' (1 / 1)");
            Thread.Sleep(200);

            if (rootAllowed == true)
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(" Core - Root access");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(warningASCII);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("This SimpleOS setup has root access. This may can give access to your RAM and CPU, causing CPU to halt, otherwise trigger an exception.\nAttackers can also use root to corrupt your memory and make computer");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" COMPLETELY UNUSABLE");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(", otherwise - fill the memory fully.\n\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Boot process will continue after pressing any key...");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
            Console.Write("[Cosmos] Decompressing kernel from SimpleOS.bin.xz...");
            Thread.Sleep(50);
            Console.Write("Ok!\n");
            Console.Clear();
            Console.WriteLine("Loading module 'ivp4' (1 / 5)");
            Thread.Sleep(100);
            Console.Clear();
            Console.WriteLine("Loading module 'ramfs' (2 / 5)");
            Thread.Sleep(100);
            Console.Clear();
            Console.WriteLine("Loading module 'ipv6' (3 / 5)");
            Console.Clear();
            Console.WriteLine("Loading module 'watchdog' (4 / 5)");
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Loading module '" + osNamenoUppercaseLetters + "-core' (5 / 5)");
            Thread.Sleep(300);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorVisible = true;
            Thread.Sleep(100);
            Console.WriteLine("[" + osName + "-Core] Welcome to " + osName + " " + version + ", in " + channel + " channel.\n" + "Build " + osBuild);
            Console.WriteLine("[" + osName + "-Core] Launching date and time module...");
            Console.WriteLine("Loading additional module 'datetime', which was triggered by '" + osNamenoUppercaseLetters + "-core' service... (1 / 1)");
            lastInputTime = DateTime.Now;
            Console.WriteLine("[" + osName + "-Core] Additional module 'datetime' loaded");
            // Initializing RAMFS file system
            InitializeFileSystem();

            // Messages from services
            Console.WriteLine("[ipv4] Connecting to Ethernet service...Ok!\n[ipv6] Connecting to Ethernet service...Ok!\n[WatchDog] Enabling watchdog timer within 1 minute...");
            Thread.Sleep(500);
            Console.WriteLine("[RAMFS] File system storaging set to ram://");
            Console.WriteLine("Read the login info carefully:\nUsername is " + username + ".\nPassword is " + password + ".");
            Console.WriteLine("[" + osName + "-Core] If keyboard doesn't work, make sure you have an compatible PS/2 keyboard.");
            askUsername();
            // Login message
            Console.WriteLine("Welcome, " + username + ".");
            Console.Clear();
            // THE SIMPLEOS ASCII LOGO!!!
            Console.WriteLine(osASCIILogo);
            if (channel != "stable") // If the OS channel is not stable
            {
                // Giving warning
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(osName + "-Core gives an warning: ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Note that this OS is in " + channel + " testing, so it \ncould be buggy and unstable.\n\n\n");
            }
        }



        private void sudo()
        {
            if (rootAllowed != false)
            {
                GetPromptROOT(); // not working lol
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("E: ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("User is not found!");
            }
            else
            {
                Console.WriteLine(rootLoginFail);
            }
        }

        protected override void Run()
        {
            CheckScreensaver();

            Console.Write($"{GetPrompt()} ");
            string input = Console.ReadLine();
            lastInputTime = DateTime.Now;
            string[] args = input.Split(' ');

            string command = args[0].ToLower();
            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;

                case "bomb":
                    KernelPanic(kernelpanic_forcebomb);
                    break;

                case "cosfetch":
                    Cosfetch();
                    break;

                case "sudo":
                    sudo();
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "rm /":
                    Console.WriteLine("Removing folder /sys...\nTime left: 5 days");
                    Thread.Sleep(5);
                    KernelPanic(kernelpanic_memorystackoverflow);
                    break;

                case "echo":
                    EchoCommand(args, input);
                    break;

                case "curl":
                    CurlCommand(args);
                    break;

                case "wget":
                    WgetCommand(args);
                    break;

                case "ls":
                case "dir":
                    ListFiles();
                    break;

                case "cat":
                    CatCommand(args);
                    break;

                case "reboot":
                    Console.WriteLine("Targetted 'Force reboot'");
                    Thread.Sleep(100);
                    Sys.Power.Reboot();
                    break;

                case "beep":
                    Console.Beep();
                    break;

                case "screensaver":
                    StartScreensaver();
                    break;

                case "clock":
                    Console.WriteLine("Current time: " + DateTime.Now.ToString("HH:mm:ss"));
                    break;

                case "date":
                    Console.WriteLine("Current date: " + DateTime.Now.ToString("yyyy-MM-dd"));
                    break;

                case "cd":
                    Console.WriteLine("cd: To avoid glitches and bugs in cd, this build (" + osBuild + ") uses RAMFS module.");
                    ChangeDirectory(args);
                    break;

                case "exit":
                    poweroff();
                    break;

                case "panic":
                    KernelPanic(kernelpanic_dummy);
                    break;

                case "whoami":
                    showUsername();
                    break;

                case "mousedrv.mod":
                    mousedrv();
                    break;

                case "legacypanic":
                    LegacyKernelPanic("Dummy legacy kernel panic");
                    break;

                case "mgp":
                    startGUI();
                    break;

                case "logoff":
                    logoff();
                    break;

                default:
                    if (command.EndsWith(".app"))
                    {
                        RunAppFile(command);
                    }
                    else
                    {
                        Console.Beep();
                        Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
                    }
                    break;
            }
        }

        private void showUsername()
        {
            Console.WriteLine(username);
        }

        private void poweroff()
        {
            Console.WriteLine("");
            Console.WriteLine("Targetted 'Force system power off'");
            Thread.Sleep(100);
            Sys.Power.Shutdown();
            Console.WriteLine("If your device did not turn off, it means it does not support ACPI.\nPlease, hold the power button until your computer shuts down.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Additionally, we will try sending an ACPI power off signal.\nThis will begin after 3 seconds...");
            Thread.Sleep(3000);
            ACPI.Shutdown();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Seems like we are still unable to shut down your device.\n\nPlease, hold power button.");
        }

        private string GetPrompt()
        {
            return $"{osName} - {currentDirectory}$";
        }

        private string GetPromptROOT()
        {
            return $"{osName} - {currentDirectory}#";
        }

        private void mousedrv()
        {
            Console.WriteLine("Loading module 'mousedrv'... (1 / 1)");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("E: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("The GUI is not loaded yet!\n");
        }

        private void ShowHelp()
        {
            Console.WriteLine("Available commands for " + osName + ":");
            Console.WriteLine(" - help: Display this help message");
            Console.WriteLine(" - clear: Clear the console screen");
            Console.WriteLine(" - echo <text>: Output the specified text");
            Console.WriteLine(" - curl <url>: Fetch and display content from the URL (Currently not supported)");
            Console.WriteLine(" - wget <url> <filename>: Download a file from the URL and save it (Currently not supported)");
            Console.WriteLine(" - ls: List files in the current directory");
            Console.WriteLine(" - dir: Equivalent to 'ls'");
            Console.WriteLine(" - cosfetch: Fetch the system and computer information");
            Console.WriteLine(" - cat <filename>: Show the content of the specified file");
            Console.WriteLine(" - reboot: Restart the operating system");
            Console.WriteLine(" - sudo: Execute a command with 'root' privileges");
            Console.WriteLine(" - screensaver: Manually activate the screensaver");
            Console.WriteLine(" - clock: Display the current time");
            Console.WriteLine(" - logoff: Log out from currently logged in user");
            Console.WriteLine(" - date: Show the current date");
            Console.WriteLine(" - whoami: Display your current user profile");
            Console.WriteLine(" - cd <directory>: Change the working directory");
            Console.WriteLine(" - mgp: Start MGP desktop environment");
            Console.WriteLine(" - beep: Emit a beep sound");
            Console.WriteLine(" - exit: Shut down your computer");
        }

        private void EchoCommand(string[] args, string input)
        {
            if (args.Length > 1)
            {
                string echoText = input.Substring(5);
                Console.WriteLine(echoText);
            }
            else
            {
                Console.Beep();
                Console.WriteLine("Error: No text provided for echo.");
            }
        }

        private void CurlCommand(string[] args)
        {
            if (args.Length > 1)
            {
                string url = args[1];
                Console.WriteLine("Attempting to fetch URL: " + url);
                Console.Beep();
                Console.WriteLine("Error: Networking is not supported in this environment.");
            }
            else
            {
                Console.Beep();
                Console.WriteLine("Error: No URL provided.");
            }
        }

        private void WgetCommand(string[] args)
        {
            if (args.Length > 2)
            {
                string url = args[1];
                string filename = args[2];
                Console.WriteLine("Attempting to download: " + url);
                Console.Beep();
                Console.WriteLine("Error: Networking is not supported in this environment.");
            }
            else
            {
                Console.Beep();
                Console.WriteLine("Error: URL or filename not provided.");
            }
        }

        private void startGUI()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.Beep(100, 100);
            Console.Beep(500, 100);
            Console.Beep(700, 100);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.White;
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.Clear();
            }
        }

        private void ListFiles()
        {
            if (directories.ContainsKey(currentDirectory))
            {
                Console.WriteLine("Directories:");
                foreach (var dir in directories[currentDirectory])
                {
                    Console.WriteLine("[DIR] " + dir);
                }

                Console.WriteLine("Files:");
                foreach (var file in files.Keys.Where(f => files[f].StartsWith(currentDirectory)))
                {
                    Console.WriteLine(file);
                }
            }
            else
            {
                Console.Beep();
                Console.WriteLine("Error: Directory not found.");
            }
        }

        private void CatCommand(string[] args)
        {
            if (args.Length > 1)
            {
                string fileName = args[1];
                if (files.ContainsKey(fileName) && files[fileName].StartsWith(currentDirectory))
                {
                    Console.WriteLine(files[fileName]);
                }
                else
                {
                    Console.Beep();
                    Console.WriteLine("Error: File not found or not in current directory. Do you have access to the file?");
                }
            }
            else
            {
                Console.Beep();
                Console.WriteLine("Error: No file specified.");
            }
        }

        private void ChangeDirectory(string[] args)
        {
            if (args.Length > 1)
            {
                string newDir = args[1];
                if (newDir == "/")
                {
                    currentDirectory = "/";
                }
                else if (directories.ContainsKey(newDir))
                {
                    currentDirectory = newDir;
                }
                else
                {
                    Console.Beep();
                    Console.WriteLine("Error: Directory not found. Do you have access to the folder?");
                }
            }
            else
            {
                Console.Beep();
                Console.WriteLine("Error: No directory specified.");
            }
        }

        private void RunAppFile(string fileName)
        {
            Console.Beep();
            Console.WriteLine($"Error: Unable to run {fileName}: .app files are not supported in this environment.");
        }

        private void CheckScreensaver()
        {
            if ((DateTime.Now - lastInputTime).TotalSeconds > 30)
            {
                StartScreensaver();
            }
        }

        private static void StartScreensaver()
        {
            Console.WriteLine("Screensaver was removed in this build due to bugs.\nThe command will be removed in future.");
        }

        private void logoff()
        {
            askUsername();
        }

        private void InitializeFileSystem()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Beginning file system copying...");
            Console.ForegroundColor = ConsoleColor.Black;
            Thread.Sleep(1000);
            directories.Add("/", new HashSet<string> { "sys", "home", "bin", "usr" });
            Console.WriteLine("Copying /sys");
            Console.WriteLine("Copying /sys/version.txt");
            Thread.Sleep(300);
            Console.WriteLine("Copying /sys/config");
            Thread.Sleep(300);
            Console.WriteLine("Copying /sys/config/users.xml");
            Thread.Sleep(300);
            Console.WriteLine("Copying /sys/config/access.xml");
            Thread.Sleep(300);
            directories.Add("/sys", new HashSet<string>());
            directories.Add("/home", new HashSet<string>());
            Thread.Sleep(300);
            Console.WriteLine("Copying /home");
            Thread.Sleep(300);
            Console.WriteLine("Copying /home/LiveCD");
            Thread.Sleep(300);
            Console.WriteLine("Copying /home/LiveCD/.userdata");
            directories.Add("/bin", new HashSet<string>());
            Thread.Sleep(300);
            Console.WriteLine("Copying /bin");
            Thread.Sleep(300);
            Console.WriteLine("Copying /bin/shell");
            Thread.Sleep(500);
            Console.WriteLine("Copying /bin/shellcmds");
            Thread.Sleep(1000);
            directories.Add("/usr", new HashSet<string>());
            Thread.Sleep(100);
            Console.WriteLine("Copying /usr");

            files.Add("/sys/version.txt", osName + " v" + version + channel);
            Thread.Sleep(300);
            Console.WriteLine("Copying /mousedrv.mod");
            Thread.Sleep(10);
            files.Add("/mousedrv.mod", "File content was hidden to reduce lags.");
        }

        private void KernelPanic(string errorMessage)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.Beep();
            Console.WriteLine("A problem has been detected and " + osName + " has been shut down to prevent damage");
            Console.Beep();
            Console.WriteLine("to your computer.");
            Console.Beep();
            Console.WriteLine();
            Console.Beep();
            Console.WriteLine($"STOP: {errorMessage}");
            Console.Beep();
            Console.WriteLine();
            Console.Beep();
            Console.WriteLine("If this is the first time you've seen this error screen, restart your computer.");
            Console.Beep();
            Console.WriteLine("If this screen appears again, follow these steps:");
            Console.Beep();
            Console.WriteLine();
            Console.Beep();
            Console.WriteLine("Check to make sure any new hardware or software is properly installed.");
            Console.Beep();
            Console.WriteLine("If this is a new installation, ask your hardware or software manufacturer");
            Console.Beep();
            Console.WriteLine("for any " + osName + " updates you might need.");
            Console.Beep();
            Console.WriteLine();
            Console.Beep();
            Console.WriteLine("If problems continue, disable or remove any newly installed hardware or software.");
            Console.Beep();
            Console.WriteLine("Disable BIOS memory options such as caching or shadowing.");
            Console.Beep();
            Console.WriteLine();
            Console.Beep();
            Console.WriteLine("Technical information:");
            Console.Beep();
            Console.WriteLine();
            Console.Beep();
            Console.WriteLine("*** STOP: 0x0000007B (0xF79B2524, 0xC0000034, 0x00000000, 0x00000000)");
            Console.Beep();
            Console.WriteLine();
            Console.Beep();
            Console.WriteLine("Kernel panic - not syncing: " + errorMessage);
            Console.Beep();
            Console.WriteLine("Press Ctrl+Alt+Del to restart.");
            Console.Beep();
            CPU.Halt();
            CPU.DisableInterrupts();
            CPU.GetEndOfKernel();
            CPU.Halt();
            Console.Beep();
            while (true)
            {
                Console.Beep();
            }
        }
        private void LegacyKernelPanic(string errormsgLegacy)
        {
            Console.Clear();

            while (true)
            {
                Console.Write("KERNEL PANIC!! " + errormsgLegacy + " \n");
            }
        }
    }
}
