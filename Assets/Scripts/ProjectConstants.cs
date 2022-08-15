using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectConstants 
{
    public static class vars
    {
        public static readonly string SWORD_ATTACK = "SHit1";
        public static readonly string PUNCH_ATTACK = "Punch1";
    }
    public static string[] combatAnims = {"Punch1", "Punch2", "Punch3", "Punch4", "Punch5" };
   public static class UI
    {
        public static readonly Dictionary<Menu, string> MenuAddresses = new Dictionary<Menu, string> 
        {
            {Menu.MaingGame, "" },
            {Menu.Settings, "" },
        };

        public enum Menu
        {
            MaingGame,
            Menu,
            Settings,
        }
    }
}
