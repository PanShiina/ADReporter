using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProcessManipulation
{
    class Program
    {
        // texte acceuil
        private const string home = "Listes de recherche disponible:\n" +
            "\t1 - afficher les groupes\n" +
            "\t2 - afficher les partages de fichier actif sur un serveur\n" +
            "\t3 - Afficher les accès à un partage\n" +
            "\t4 - quiter le program\n";
        // variable
        static string serverName = "";
        static string choice = "";
        
       

        private static void CommandSender(string command)
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal,


                }
            };
            process.Start();

            while(!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                Console.WriteLine($"{line}");
            }

        }
        static void Main()
        {
            var serveurList = new List<string>()
            {
                "srv01",
                "srvcao"

            };

            Console.Clear();
            Console.WriteLine(home);
            Console.Write("Entrez votre choix: ");

            choice = Console.ReadLine();
            if(choice != "")
            {
                Console.Clear();
                switch(choice)
                {
                    case "1":
                        Console.WriteLine("Commande en cours d'execution");
                        CommandSender("Get-ADGroup -Filter * | Select-Object Name");
                        Console.ReadKey();
                        Main();
                        break;
                    case "2":
                        Console.Write("Veueillez entrez le nom du serveur: ");
                        serverName = Console.ReadLine();
                        CommandSender($"get-WmiObject -class Win32_Share -ComputerName {serverName} -Amended  |Select-Object Name, Path");
                        Console.ReadKey();
                        Main();
                        break;
                    case "3":
                        Console.WriteLine("chargement des partages");
                        foreach(string serveur in serveurList)
                        {
                            Console.WriteLine($"{serveur} :");
                            CommandSender($"get-WmiObject -class Win32_Share -ComputerName {serveur} |Select-Object Name, Path | Format-Table");

                        }
                        Console.ReadKey();
                        Main();
                        break;
                    case "4":
                        Console.WriteLine("à bientôt !!");
                        Thread.Sleep(2000);
                        break;
                    default:
                        Main();
                        break;
                }
                
            }else
            {
                Console.Clear();
                Console.WriteLine("aucun choix n'a été fais !! veuillez recommencer");
                Main();
            }
            
        }
    }
}

