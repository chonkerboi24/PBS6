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
        static Player player = new Player();
        static Enemy enemy = new Enemy();

        static int enemyNameNum;

        // Default Color
        static int txtColor = 4;

        static int winCount = 0;
        static Random playerDamageGen = new Random();
        static Random enemyDamageGen = new Random();
        static Random enemyNameGen = new Random();
        static Random enemyHPGen = new Random();

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
    Deaths: {2}", winCount, player.Kills, player.Deaths));
            Console.ReadKey();
        }

        static void TurnText()
        {
            
            Console.WriteLine($@"
    {enemy.Name} the Baller
    HP: {enemy.HP}
    
    
    {player.Name}
    HP: {player.HP}
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
    [2] Bash (6-12 Damage) [{player.BashUsesLeft} left]
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

                        player.HP += 10;

                        Thread.Sleep(1000);
                        break;

                    case 2:
                        Console.Clear();
                        PlayerTurn();
                        break;

                    case 3:
                        player.HP = 0;
                        break;

                    case 4:
                        enemy.HP = 0;
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
                        player.Damage = playerDamageGen.Next(7, 15);
                        Console.Clear();
                        TurnText();
                        AttackText();
                        Console.WriteLine($@"
    You dealt {player.Damage} damage!");
                        enemy.HP -= player.Damage;
                        Thread.Sleep(1000);
                        break;

                    case 2:
                        int bashChance = playerDamageGen.Next(1, 101); // 1-100
                        if (bashChance <= 60) // 60% chance to hit
                        {
                            player.Damage = playerDamageGen.Next(6, 13);
                            Console.Clear();
                            TurnText();
                            AttackText();
                            Console.WriteLine($@"
    You dealt {player.Damage} damage!");
                            enemy.Stunned = true;
                            enemy.HP -= player.Damage;
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
            while (player.HP > 0 && enemy.HP > 0)
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
            if (player.HP < 1)
            {
                player.Deaths += 1;
                Console.Clear();
                Console.WriteLine(@"
    You Lost...
    -----------");
                Thread.Sleep(1000);
            }
            
            
                
        }

        static void EnemyTurn()
        {
            if (enemy.HP < 1)
            {
                // Enemy is dead, do nothing
                return;
            }

            if (enemy.Stunned == false)
            {
                Console.Clear();
                enemy.Damage = enemyDamageGen.Next(10, 16);
                player.HP -= enemy.Damage;
                Console.WriteLine($@"
    {enemy.Name} dealt {enemy.Damage}!
    --------------------------------");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                Console.WriteLine($@"
    {enemy.Name} is stunned!
    -----------------------");
                enemy.Stunned = false;
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        static void EnemyDeathChecker()
        {
            if (enemy.HP < 1)
            {
                winCount += 1;
                player.Kills += 1;
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
                    enemy.Name = "";

                    enemyNameNum = enemyNameGen.Next(1, 5);
                    switch (enemyNameNum)
                    {
                        case 1:
                            enemy.Name = "Jimmy Neutron";
                            break;

                        case 2:
                            enemy.Name = "Goongel Slipee";
                            break;

                        case 3:
                            enemy.Name = "Dave the Magical Cheese Wizard";
                            break;

                        case 4:
                            enemy.Name = "Jerry";
                            break;

                    }
                    

                }

                static void Game()
                {

                    if (player.Name == "")
                    {
                        Console.Write(@"
    Player Name: ");
                        player.Name = Console.ReadLine();
                    }
                   
                    enemy.HP = enemyHPGen.Next(25, 81);
                    EnemyNameSelection();
                    player.BashUsesLeft = 3; // Reset bash uses for each new enemy
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
            player = new Player();
            enemy = new Enemy();
            Console.Clear();
            MainMenu();
        }

        
    }
}
