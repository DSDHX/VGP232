using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponLib
{
    public class Weapon
    {
        public enum WeaponType
        {
            Sword, Polearm, Claymore, Catalyst, Bow, None
        }

        public Weapon() {}

        // Name,Type,Rarity,BaseAttack
        public string Name { get; set; }
        public WeaponType Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }
        public string Image { get; set; }
        public string SecondaryStat { get; set; }
        public string Passive { get; set; }

        /// <summary>
        /// The Comparator function to check for name
        /// </summary>
        /// <param name="left">Left side Weapon</param>
        /// <param name="right">Right side Weapon</param>
        /// <returns> -1 (or any other negative value) for "less than", 0 for "equals", or 1 (or any other positive value) for "greater than"</returns>
        public static int CompareByName(Weapon left, Weapon right)
        {
            return string.Compare(left.Name, right.Name, StringComparison.Ordinal);
        }

        // TODO: add sort for each property: (DONE)

        /// <summary>
        /// CompareByType
        /// </summary>
        public static int CompareByType(Weapon left, Weapon right)
        {
            return left.Type.CompareTo(right.Type);
        }

        /// <summary>
        /// CompareByRarity
        /// </summary>
        public static int CompareByRarity(Weapon left, Weapon right)
        {
            return left.Rarity.CompareTo(right.Rarity);
        }

        /// <summary>
        /// CompareByBaseAttack
        /// </summary>
        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            return left.BaseAttack.CompareTo(right.BaseAttack);
        }

        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formated string</returns>
        public override string ToString()
        {
            // TODO: construct a comma seperated value string (DONE)
            // Name,Type,Rarity,BaseAttack
            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }

        public static bool TryPrase(string rawData, out Weapon weapon)
        {
            weapon = null;

            if (string.IsNullOrWhiteSpace(rawData))
            {
                return false;
            }

            string[] values = rawData.Split(',');

            //7 columns based on data2.csv
            if (values.Length != 7)
            {
                return false;
            }

            try
            {
                string name = values[0].Trim();
                if (!Enum.TryParse(values[1].Trim(), true, out WeaponType type))
                {
                    type = WeaponType.None;
                }

                string image = values[2].Trim();
                if (!int.TryParse(values[3].Trim(), out int rarity))
                {
                    return false;
                }
                if (!int.TryParse(values[4].Trim(), out int attack))
                {
                    return false;
                }
                string secondaryStat = values[5].Trim();
                string passive = values[6].Trim();

                weapon = new Weapon()
                {
                    Name = name,
                    Type = type,
                    Image = image,
                    Rarity = rarity,
                    BaseAttack = attack,
                    SecondaryStat = secondaryStat,
                    Passive = passive
                };

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}