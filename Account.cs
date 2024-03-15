using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyDatabase
{
    internal class Account
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Banned { get; set; }
        public Army ArmyType { get; set; }
        public DateTime BannedUntil { get; set; }
        public int DaysUntilUnban { get; set; }
        public String Description { get; set; }
        public Account(string name, string password, Army armyType = Army.UNKNOWN, DateTime? bannedUntil = null, String desc = "")
        {
            Name = name;
            Password = password;
            ArmyType = armyType;
            Description = desc;
            if (bannedUntil == null)
            {
                BannedUntil = DateTime.MinValue;
                Banned = false;
                DaysUntilUnban = 0;
            }
            else
            {
                BannedUntil = (DateTime)bannedUntil;
                DaysUntilUnban = -1 * (DateTime.Now.AddHours(8) - BannedUntil).Days + 1;
                Banned = true;
            }
        }
        public static Account? GetByName(List<Account> list, string name)
        {
            foreach(Account account in list)
            {
                if (account.Name.ToLower().Contains(name.ToLower()))
                {
                    return account;
                }
            }
            return null;
        }
        public override string ToString()
        {
            string result = Name + ":" + Password + " | " + ArmyType.ToString();
            string desc = "";
            if (Description != null && Description.Length > 0)
            {
                desc = " | " + Description;
            }
            if (!Banned) 
                return result + desc;
            return result + " | banned for " + DaysUntilUnban + " days" + desc;
        }
        public void SetBanned(int days)
        {
            DateTime now = DateTime.Now.AddHours(8);
            DateTime bannedUntil = new DateTime(now.Year, now.Month, now.Day);
            //bannedUntil.AddDays(days);
            Banned = true;
            BannedUntil = bannedUntil.AddDays(days);
            DaysUntilUnban = days;
        }
        public static void UpdateBans(List<Account> list)
        {
            DateTime now = DateTime.Now.AddHours(8);
            foreach (Account account in list)
            {
                int days = (now - account.BannedUntil).Days - 1;
                if (days >= 0)
                {
                    account.Banned = false;
                    account.DaysUntilUnban = 0;
                }
                else
                {
                    account.Banned = true;
                    account.DaysUntilUnban = -days;
                }
            }
        }
       
    }
}
