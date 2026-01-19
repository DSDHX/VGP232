using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Assignment2a
{
    public class WeaponCollection : List<Weapon>, IPeristence
    {
        public bool Load(string filename)
        {
            this.Clear();

            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            {
                return false;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string header = reader.ReadLine();

                    while (reader.Peek() > -1)
                    {
                        string line = reader.ReadLine();
                        if (Weapon.TryPrase(line, out Weapon weapon))
                        {
                            this.Add(weapon);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
                return false;
            }
        }

        public bool Save(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("Name, Type, Image, Rarity, BaseAttack, SecondaryStat, Passive");

                    foreach (var weapon in this)
                    {
                        writer.WriteLine(weapon.ToString());
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
                return false;
            }
        }

        public int GetHighestBaseAttack()
        {
            if (this.Count == 0)
            {
                return 0;
            }

            int max = this[0].BaseAttack;
            for (int i = 1; i < this.Count; i++)
            {
                if (this[i].BaseAttack > max)
                {
                    max = this[i].BaseAttack;
                }
            }

            return max;
        }

        public int GetLowestBaseAttack()
        {
            if (this.Count == 0)
            {
                return 0;
            }

            int min = this[0].BaseAttack;
            for (int i = 1; i < this.Count; i++)
            {
                if (this[i].BaseAttack < min)
                {
                    min = this[i].BaseAttack;
                }
            }

            return min;
        }

        public List<Weapon> GetAllWeaponsOfType(Weapon.WeaponType type)
        {
            List<Weapon> filtered = new List<Weapon>();
            foreach (var w in this)
            {
                if (w.Type == type)
                {
                    filtered.Add(w);
                }
            }

            return filtered;
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            List<Weapon> filtered = new List<Weapon>();
            foreach (var w in this)
            {
                if (w.Rarity == stars)
                {
                    filtered.Add(w);
                }
            }

            return filtered;
        }

        public void SortBy(string columName)
        {
            if (string.IsNullOrEmpty(columName))
            {
                this.Sort(Weapon.CompareByName);
                return;
            }

            switch (columName.ToLower())
            {
                case "type":
                    this.Sort(Weapon.CompareByType);
                    break;
                case "rarity":
                    this.Sort(Weapon.CompareByRarity);
                    break;
                case "baseattack":
                    this.Sort(Weapon.CompareByBaseAttack);
                    break;
                case "name":
                default:
                    this.Sort(Weapon.CompareByName);
                    break;
            }
        }
    }
}