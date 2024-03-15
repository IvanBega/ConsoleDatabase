using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace ArmyDatabase
{
    internal class Interface
    {
        private List<Account> accounts;
        private Account current;
        public Interface()
        {
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            if (!File.Exists("database.json"))
            {
                accounts = new List<Account>();
                return;
            }

            accounts = Serializer.ReadAsJsonFormat<List<Account>>("database.json");
            Account.UpdateBans(accounts);
        }
        public void Start()
        {
            ConsoleInterface();
        }

        private void ConsoleInterface()
        {
            string selection = "";
            PrintOptions();
            bool flag = true;
            while (flag)
            {
                selection = Console.ReadLine();
                switch (selection)
                {
                    case "0":
                        PrintOptions();
                        break;
                    case "1":
                        ChooseRandom();
                        break;
                    case "11":
                        DisplayCurrent();
                        break;
                    case "2":
                        ChooseByOrg();
                        break;
                    case "3":
                        BannedCurrent();
                        break;
                    case "4":
                        ChooseByName();
                        break;
                    case "5":
                        AddAccount();
                        break;
                    case "55":
                        AddMultipleAccounts();
                        break;
                    case "6":
                        ModifyByName();
                        break;
                    case "7":
                        ModifyCurrent();
                        break;
                    case "8":
                        DisplayStatistics();
                        break;
                    case "9":
                        DisplayAccounts();
                        break;
                    case "99":
                        DisplayByArmy();
                        break;
                    case "10":
                        Serialize();
                        flag = false;
                        break;
                    case "111":
                        DeleteByName();
                        break;
                    default:
                        Console.WriteLine("Could not recognize operation");
                        break;
                }
            }
        }

        private void DisplayCurrent()
        {
            Console.WriteLine(current);
        }

        private void DeleteByName()
        {
            Console.Write("Enter nick or part of the nick: ");
            string nick = Console.ReadLine();
            Account account = Account.GetByName(accounts, nick);
            if (account == null)
            {
                Console.WriteLine("Could not find account! Returning to main menu");
                return;
            }
            Console.Write("{0} | Are you sure you want to delete this account?(1 - yes, 0 - no): ", account);
            string option = Console.ReadLine();
            if (option.Equals("1"))
            {
                accounts.Remove(account);
                Console.WriteLine("Successfuly removed {0} from database!", account);
            }
            else
            {
                Console.WriteLine("Canceled operation!");
            }
        }

        private void AddMultipleAccounts()
        {
            Console.Write("Enter army ID for the group: ");
            string option = Console.ReadLine();
            bool isNumber = int.TryParse(option, out int armyId);
            if (!isNumber || armyId > 6 || armyId < 0)
            {
                Console.WriteLine("Could not recornize army ID. Returning to main menu.");
                return;
            }
            Console.WriteLine("Army - {0}. Enter data in format nick:pass. Enter 0 to finish", (Group)armyId);
            string input = Console.ReadLine();
            while (!input.Equals("0"))
            {
                string[] data = input.Split(":");
                if (data[0].Length == 0 || data[1].Length == 0)
                {
                    Console.WriteLine("Entered incorrect data, try again...");
                    continue;
                }

                Account account = new(data[0], data[1], (Group)armyId);
                accounts.Add(account);
                Console.WriteLine("Added account " + account);
                input = Console.ReadLine();
            }
            Console.WriteLine("Finished adding accounts. Exiting to main menu");

        }

        private void DisplayByArmy()
        {
            DisplayArmyID();
            string option = Console.ReadLine();
            bool isNumber = int.TryParse(option, out int result);
            if (!isNumber)
            {
                Console.WriteLine("Could not recornize operation");
                return;
            }
            List<Account> toPrint = (from acc in accounts
                                     where acc.ArmyType == (Group)result select acc).ToList();
            foreach (Account acc in toPrint)
                Console.WriteLine(acc);
        }

        private void ModifyCurrent()
        {
            if (current == null)
            {
                Console.WriteLine("Could not find account!");
                return;
            }

            ModificationOptions(current);
        }

        private void ModificationOptions(Account account)
        {
            Console.WriteLine(account);
            Console.WriteLine("Select operation for the account: ");
            bool flag = true;
            string option = "";
            while (flag)
            {
                Console.WriteLine("0 - exit\n1 - edit pass\n2 - edit army\n3 - unban\n4 - erase description\n5 - add description");
                option = Console.ReadLine();
                flag = false;
                switch (option)
                {
                    case "0":
                        Console.WriteLine("Moving to main mane\n");
                        break;
                    case "1":
                        Console.Write("Enter new password: ");
                        string pass = Console.ReadLine();
                        account.Password = pass;
                        break;
                    case "2":
                        DisplayArmyID();
                        int opt = int.Parse(Console.ReadLine());
                        account.ArmyType = (Group)opt;
                        break;
                    case "3":
                        account.Banned = false;
                        account.DaysUntilUnban = 0;
                        account.BannedUntil = DateTime.MinValue;
                        break;
                    case "4":
                        account.Description = "";
                        break;
                    case "5":
                        Console.Write("Enter description: ");
                        string desc = Console.ReadLine();
                        account.Description = desc;
                        break;
                    default:
                        flag = true;
                        Console.WriteLine("Could not recognize operation");
                        break;
                }
            }
            if (option == null || option.Equals("0")) return;
            Console.WriteLine("Your modified account: ");
            Console.WriteLine(account);
        }

        private void DisplayStatistics()
        {
            int total = accounts.Where(acc => acc.ArmyType != Group.SPECIAL).Count();
            int totalAvailable = accounts.Where(acc=> acc.DaysUntilUnban <= 0 && acc.ArmyType != Group.SPECIAL).Count();

            int army = accounts.Where(acc=> acc.ArmyType == Group.SV || acc.ArmyType == Group.VMF || acc.ArmyType == Group.VVS).Count();
            int armyAvailable = accounts.Where(acc => acc.DaysUntilUnban <= 0 && (acc.ArmyType == Group.SV || acc.ArmyType == Group.VMF || acc.ArmyType == Group.VVS)).Count();

            int family = accounts.Where(acc => acc.ArmyType == Group.FAMILY).Count();
            int familyAvailable = accounts.Where(acc => acc.ArmyType == Group.FAMILY && acc.DaysUntilUnban <= 0).Count();

            int kpz = accounts.Where(acc => acc.ArmyType == Group.KPZ).Count();
            int none = accounts.Where(acc => acc.ArmyType == Group.NONE).Count();

            Console.WriteLine("Total - {0}, available - {2}, banned - {1}", total, total-totalAvailable, totalAvailable);
            Console.WriteLine("Army total - {0}, available - {2}, banned - {1}", army, army-armyAvailable, armyAvailable);
            Console.WriteLine("Family total - {0}, available {2}, banned - {1}", family, family-familyAvailable, familyAvailable);
            Console.WriteLine("None - {0}, KPZ - {1}", none, kpz);

        }

        private void ChooseByName()
        {
            Console.Write("Enter nick: ");
            string nick = Console.ReadLine();
            Account account = Account.GetByName(accounts, nick);
            if (account == null)
            {
                Console.WriteLine("Could not find account!");
                return;
            }
            current = account;
            Console.WriteLine("Current account is {0}", account);
        }
        private void ModifyByName()
        {
            Console.Write("Enter nick: ");
            string nick = Console.ReadLine();
            Account account = Account.GetByName(accounts, nick);
            if (account == null)
            {
                Console.WriteLine("Could not find account!");
                return;
            }
            ModificationOptions(account);
        }

        private void DisplayArmyID()
        {
            Console.Write("Enter ID (0 - SV, 1 - VVS, 2 - VMF, 3 - UNKNOWN, 4 - NONE, 5 - KPZ, 6 - FAMILY, 7 - SPECIAL): ");
        }
        private void ChooseByOrg()
        {
            DisplayArmyID();
            int result = int.Parse(Console.ReadLine());
            Account res = (from acc in accounts where acc.ArmyType == (Group)result && acc.Banned == false select acc).FirstOrDefault();
            if (res == null)
            {
                Console.WriteLine("Could not find account!");
                return;
            }
            current = res;
            Console.WriteLine("\n-------------------");
            Console.WriteLine("CURRENT ACCOUNT: ");
            Console.WriteLine(res);
        }

        private void AddAccount()
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.Write("Enter pass: ");
            string pass = Console.ReadLine();
            DisplayArmyID();
            int armyId = int.Parse(Console.ReadLine());
            Group army = (Group)armyId;
            Console.Write("Is account banned (0/1)?:");
            string banned = Console.ReadLine();
            DateTime bannedUntil = DateTime.MinValue;
            Account a;
            if (banned == "1")
            {
                Console.Write("Until what date?: ");
                bannedUntil = DateTime.Parse(Console.ReadLine());
                a = new Account(name, pass, army, bannedUntil);
            }
            else
            { 
                a = new Account(name, pass, army); 
            }
            Console.Write("Is account correct - " + a + " ?(0/1)");
            string result = Console.ReadLine();
            if (result != "1")
            {
                Console.WriteLine("CANCELLING....");
                return;
            }
            accounts.Add(a);
        }
        private void UpdateBannedAccount(Account account, int days)
        {
            account.SetBanned(days);
        }
        private void UpdateBannedAccount()
        {
            Console.WriteLine("Enter name of account");
            string name = Console.ReadLine();
            Account? a = Account.GetByName(accounts, name);
            if (a == null)
            {
                Console.WriteLine("Error! Could not find account by " + "\"" + name + "\"");
                return;
            }
            Console.WriteLine("Number of days the account got banned: ");
            int days = int.Parse(Console.ReadLine());
            
        }

        private void PrintOptions()
        {
            //Console.WriteLine("1 - print all accounts\n2 - set account banned\n3 - exit\n4 - add account");
            Console.WriteLine("0 - menu\n1 - choose random, 11 - display current \n2 - choose by army\n3 - banned chosen\n4 - choose by name\n5 - add by name, 55 - bulk add" +
                "\n6 - modify by name\n7 - modify current\n8 - statistics\n9 - print all\n10 - exit\n111 - delete");
        }
        private void DisplayAccounts()
        {
            foreach (Account account in accounts)
            {
                Console.WriteLine(account);
            }
        }
        public void Serialize()
        {
            Serializer.SaveAsJsonFormat(accounts, "database.json");
        }
        private void ChooseRandom()
        {
            Account? result = (from acc in accounts where (acc.Banned == false &&
                              acc.ArmyType != Group.NONE) select acc).FirstOrDefault();
            if (result == null)
            {
                Console.WriteLine("Could not find account!");
                return;
            }
            current = result;
            Console.WriteLine("\n-------------------");
            Console.WriteLine("CURRENT ACCOUNT: ");
            Console.WriteLine(result);
        }
        private void BannedCurrent()
        {
            if (current == null)
            {
                Console.WriteLine("No current account!");
                return;
            }
            Console.Write("For how many days did account got banned?: ");
            int days = int.Parse(Console.ReadLine());
            current.SetBanned(days);
        }
    }
}
