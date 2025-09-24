using System;
using Spectre.Console;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Spectre;


namespace PBS6
{
    internal class Program
    {
        // [Var]
        static int select;
        
        static int playerHP = 100;
        static string playerName;
        static int playerDamage;

        static int enemyNameNum;
        static string enemyName2;
        static string enemyName;
        static int enemyHP;
        static int enemyDamage;
        static bool stunned;

        // Default Color
        static int txtColor = 4;

        static int winCount = 0;
        static int playerKills = 0;
        static int playerDeaths = 0;

        static Random playerDamageGen = new Random();
        static Random enemyDamageGen = new Random();
        static Random enemyNameGen = new Random();
        static Random enemyHPGen = new Random();

        static int bashUsesLeft = 3; // Set your desired limit

        static void MenuText()
        {

            Console.WriteLine(@"
    ██████╗  ██████╗ ██╗  ██╗███████╗███╗   ███╗ ██████╗ ███╗   ██╗
    ██╔══██╗██╔═══██╗██║ ██╔╝██╔════╝████╗ ████║██╔═══██╗████╗  ██║
    ██████╔╝██║   ██║█████╔╝ █████╗  ██╔████╔██║██║   ██║██╔██╗ ██║
    ██╔═══╝ ██║   ██║██╔═██╗ ██╔══╝  ██║╚██╔╝██║██║   ██║██║╚██╗██║
    ██║     ╚██████╔╝██║  ██╗███████╗██║ ╚═╝ ██║╚██████╔╝██║ ╚████║
    ╚═╝      ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═══╝
    ---------------------------------------------------------------                                                           
    [1] Start
    [2] Settings
    [3] Stats
    [4] Quit");
            
        }

        // XXXXXX

        static void SettingsText()
        {
            Console.WriteLine(string.Format(@"
    Current Text Color: {0}
    [1] White
    [2] Green
    [3] Cyan
    [4] Red
    [5] Pink
    [6] Black

    [8] Back to menu", txtColor));
        }

        static void Stats()
        {
            Console.WriteLine(string.Format(@"
    Wins: {0}
    Kills: {1}
    Deaths: {2}", winCount, playerKills, playerDeaths));
            Console.ReadKey();
        }

        static void TurnText()
        {
            
            Console.WriteLine($@"
    {enemyName} the Baller
    HP: {enemyHP}
    
    
    {playerName}
    HP: {playerHP}
    -----------
    [1] Attack
    [2] Bag
    [3] Run");
        }

        static void AttackText()
        {
            Console.WriteLine($@"
    -------[Attack]-------
    [1] Sword (7-14 Damage)
    [2] Bash (6-12 Damage) [{bashUsesLeft} left]
    [3] Back");
        }

        static void BagText()
        {
            Console.WriteLine(@"
    --------[Bag]--------
    [1] Potion (10 Health)
    [2] Back");
        }

        static void Bag()
        {
            Console.Clear();
            TurnText();
            BagText();

            int bagSelect;
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine();

            if (char.IsDigit(keyInfo.KeyChar))
            {
                bagSelect = Convert.ToInt32(keyInfo.KeyChar.ToString());

                switch (bagSelect)
                {
                    case 1:

                        Console.Clear();
                        TurnText();
                        BagText();
                        Console.WriteLine($@"
    You gained 10 health!");

                        playerHP += 10;

                        Thread.Sleep(1000);
                        break;

                    case 2:
                        Console.Clear();
                        PlayerTurn();
                        break;

                    case 3:
                        playerHP = 0;
                        break;

                    case 4:
                        enemyHP = 0;
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine(@"
    Invalid input. Please enter a digit.
    ------------------------------------");
                Thread.Sleep(1000);
                Console.Clear();
                Bag();
            }
        }

        static void Attack()
        {
            Console.Clear();
            TurnText();
            AttackText();

            int attackSelect;
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine();

            if (char.IsDigit(keyInfo.KeyChar))
            {
                attackSelect = Convert.ToInt32(keyInfo.KeyChar.ToString());

                switch (attackSelect)
                {
                    case 1:
                        playerDamage = playerDamageGen.Next(7, 15);
                        Console.Clear();
                        TurnText();
                        AttackText();
                        Console.WriteLine($@"
    You dealt {playerDamage} damage!");
                        enemyHP -= playerDamage;
                        Thread.Sleep(1000);
                        break;

                    case 2:
                        int bashChance = playerDamageGen.Next(1, 101); // 1-100
                        if (bashChance <= 60) // 60% chance to hit
                        {
                            playerDamage = playerDamageGen.Next(6, 13);
                            Console.Clear();
                            TurnText();
                            AttackText();
                            Console.WriteLine($@"
    You dealt {playerDamage} damage!");
                            stunned = true;
                            enemyHP -= playerDamage;
                        }
                        else
                        {
                            Console.Clear();
                            TurnText();
                            AttackText();
                            Console.WriteLine(@"
    Bash missed!");
                        }
                        Thread.Sleep(1000);
                        Console.Clear();
                        break;

                    case 3:
                        Console.Clear();
                        PlayerTurn();
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine(@"
    Invalid input. Please enter a digit.
    ------------------------------------");
                Thread.Sleep(1000);
                Console.Clear();
                Attack();
            }
        }

        static void PlayerTurn()
        {
            while (playerHP > 0 && enemyHP > 0)
            {
                TurnText();

                int turnSelect;
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.WriteLine();

                if (char.IsDigit(keyInfo.KeyChar))
                {
                    turnSelect = Convert.ToInt32(keyInfo.KeyChar.ToString());

                    switch (turnSelect)
                    {
                        case 1:
                            Attack();
                            Console.Clear();
                            EnemyDeathChecker();
                            break;

                        case 2:
                            Bag();
                            EnemyDeathChecker();
                            break;

                        case 3:
                            Console.Clear();
                            Console.WriteLine(@"
    You ran away!
    -------------");
                            Thread.Sleep(1000);
                            Console.Clear();
                            MainMenu();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(@"
    Invalid input. Please enter a digit.
    ------------------------------------");
                    Thread.Sleep(1000);
                    Console.Clear();
                    PlayerTurn();
                }

            }
            if (playerHP < 1)
            {
                playerDeaths += 1;
                Console.Clear();
                Console.WriteLine(@"
    You Lost...
    -----------");
                Thread.Sleep(1000);
            }
            
            
                
        }

        static void EnemyTurn()
        {
            if (enemyHP < 1)
            {
                // Enemy is dead, do nothing
                return;
            }

            if (stunned == false)
            {
                Console.Clear();
                enemyDamage = enemyDamageGen.Next(10, 16);
                playerHP -= enemyDamage;
                Console.WriteLine($@"
    {enemyName} dealt {enemyDamage}!
    --------------------------------");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                Console.WriteLine($@"
    {enemyName} is stunned!
    -----------------------");
                stunned = false;
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        static void EnemyDeathChecker()
        {
            if (enemyHP < 1)
            {
                winCount += 1;
                playerKills += 1;
                Console.Clear();
                Console.WriteLine(@"
    You Won!!!
    ----------");
                Thread.Sleep(1000);
            }
            else
            {
                EnemyTurn();
            }
        }

        static void Settings()
        {
            SettingsText();

            int settingsSelect;
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine();

            if (char.IsDigit(keyInfo.KeyChar))
            {
                settingsSelect = Convert.ToInt32(keyInfo.KeyChar.ToString());

                switch (settingsSelect)
                {
                    case 1:
                        txtColor = 1;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;

                    case 2:
                        txtColor = 2;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;

                    case 3:
                        txtColor = 3;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;

                    case 4:
                        txtColor = 4;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;

                    case 5:
                        txtColor = 5;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;

                    case 6:
                        txtColor = 6;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        break;

                    default:
                        Console.WriteLine("Invalid Color...");
                        break;

                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine(@"
Invalid input. Please enter a digit.
------------------------------------");
                Thread.Sleep(1000);
                Console.Clear();
                Settings();
            }
        }

                static void EnemyNameSelection()
                {
                    enemyName = "";

                    enemyNameNum = enemyNameGen.Next(1, 5);
                    switch (enemyNameNum)
                    {
                        case 1:
                            enemyName = "Jimmy Neutron";
                            break;

                        case 2:
                            enemyName = "Goongel Slipee";
                            break;

                        case 3:
                            enemyName = "Dave the Magical Cheese Wizard";
                            break;

                        case 4:
                            enemyName = "Jerry";
                            break;

                    }
                    

                }

                static void Game()
                {

                    if (playerName == "")
                    {
                        Console.Write(@"
    Player Name: ");
                        playerName = Console.ReadLine();
                    }
                   
                    enemyHP = enemyHPGen.Next(25, 81);
                    EnemyNameSelection();
                    bashUsesLeft = 3; // Reset bash uses for each new enemy
                    Console.Clear();
                    PlayerTurn();
                }

        

        private static void MainMenu()
        {
            if (txtColor == 1) { Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black; }
            else if (txtColor == 2) { Console.ForegroundColor = ConsoleColor.Green; Console.BackgroundColor = ConsoleColor.Black; }
            else if (txtColor == 3) { Console.ForegroundColor = ConsoleColor.Cyan; Console.BackgroundColor = ConsoleColor.Black; }
            else if (txtColor == 4) { Console.ForegroundColor = ConsoleColor.DarkRed; Console.BackgroundColor = ConsoleColor.Black; }
            else if (txtColor == 5) { Console.ForegroundColor = ConsoleColor.Magenta; Console.BackgroundColor = ConsoleColor.Black; }
            else if (txtColor == 6) { Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.White; }

            
            Thread.Sleep(1000);
            Console.Clear();
            MenuText();

            int select;
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine(); // move to the next line

            if (char.IsDigit(keyInfo.KeyChar))
            {
                select = Convert.ToInt32(keyInfo.KeyChar.ToString());

                switch (select)
                {
                    case 1:
                        Console.Clear();
                        Game();
                        Console.Clear();
                        MainMenu();
                        break;

                    case 2:
                        Console.Clear();
                        Settings();
                        Console.Clear();
                        MainMenu();
                        break;

                    case 3:
                        Console.Clear();
                        Stats();
                        Console.Clear();
                        MainMenu();
                        break;

                    case 4:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid Input...");
                        Thread.Sleep(1000);
                        break;
                }
            }
            else
            {
                Console.WriteLine(@"
    Invalid input. Please enter a digit.
    ------------------------------------");
                Console.Clear();
            }

        }

        static void Main(string[] args)
        {
            playerName = "";
            Console.Clear();
            MainMenu();
        }

        
    }
}
