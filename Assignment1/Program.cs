using System;
using System.Collections.Generic;
using System.IO;

// TODO: Fill in your name and student number. (DONE)
// Assignment 1
// NAME: Haoxi Dong
// STUDENT NUMBER: 2325946062

namespace Assignment1
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Variables and flags
            string inputFile = string.Empty;
            string outputFile = string.Empty;
            bool appendToFile = false;
            bool displayCount = false;
            bool sortEnabled = false;
            string sortColumnName = string.Empty;

            // The results to be output to a file or to the console
            List<Weapon> results = new List<Weapon>();

            for (int i = 0; i < args.Length; i++)
            {
                // h or --help for help to output the instructions on how to use it
                if (args[i] == "-h" || args[i] == "--help")
                {
                    Console.WriteLine("-i <path> or --input <path> : loads the input file path specified (required)");
                    Console.WriteLine("-o <path> or --output <path> : saves result in the output file path specified (optional)");

                    // TODO: include help info for count (DONE)
                    Console.WriteLine("-c or --count : displays the number of entries in the input file (optional).");

                    // TODO: include help info for append (DONE)
                    Console.WriteLine("-a or --append : " +
                                      "enables append mode when writing to an existing output file (optional).");

                    // TODO: include help info for sort (DONE)
                    Console.WriteLine("-s <column name> or --sort <column name> : " +
                                      "outputs the results sorted by column name (optional).");

                    break;
                }
                else if (args[i] == "-i" || args[i] == "--input")
                {
                    // Check to make sure there's a second argument for the file name.
                    if (args.Length > i + 1)
                    {
                        // stores the file name in the next argument to inputFile
                        ++i;
                        inputFile = args[i];

                        if (string.IsNullOrEmpty(inputFile))
                        {
                            // TODO: print no input file specified. (DONE)
                            Console.WriteLine("No input file specified.");
                        }
                        else if (!File.Exists(inputFile))
                        {
                            // TODO: print the file specified does not exist. (DONE)
                            Console.WriteLine("The file specified does not exist.");
                        }
                        else
                        {
                            // This function returns a List<Weapon> once the data is parsed.
                            results = Parse(inputFile);
                        }
                    }
                }
                else if (args[i] == "-s" || args[i] == "--sort")
                {
                    // TODO: set the sortEnabled flag and see if the next argument is set for the column name (DONE)
                    sortEnabled = true;
                    // TODO: set the sortColumnName string used for determining if there's another sort function. (DONE)
                    if (args.Length > i + 1 && !args[i + 1].StartsWith("-"))
                    {
                        ++i;
                        sortColumnName = args[i];
                    }
                }
                else if (args[i] == "-c" || args[i] == "--count")
                {
                    displayCount = true;
                }
                else if (args[i] == "-a" || args[i] == "--append")
                {
                    // TODO: set the appendToFile flag (DONE)
                    appendToFile = true;
                }
                else if (args[i] == "-o" || args[i] == "--output")
                {
                    // validation to make sure we do have an argument after the flag
                    if (args.Length > i + 1)
                    {
                        // increment the index.
                        ++i;
                        string filePath = args[i];
                        if (string.IsNullOrEmpty(filePath))
                        {
                            // TODO: print No output file specified. (DONE)
                            Console.WriteLine("No output file specified.");
                        }
                        else
                        {
                            // TODO: set the output file to the outputFile (DONE)
                            outputFile = filePath;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The argument Arg[{0}] = [{1}] is invalid", i, args[i]);
                }
            }

            if (sortEnabled)
            {
                // TODO: add implementation to determine the column name to trigger a different sort. (Hint: column names are the 4 properties of the weapon class)
                
                // print: Sorting by <column name> e.g. BaseAttack
                if (string.IsNullOrEmpty(sortColumnName))
                {
                    Console.WriteLine("Sorting by Name (Default)");
                    results.Sort(Weapon.CompareByName);
                }
                else
                {
                    Console.WriteLine("Sorting by {0}.", sortColumnName);

                    switch (sortColumnName.ToLower())
                    {
                        case "type":
                            results.Sort(Weapon.CompareByType);
                            break;
                        case "rarity":
                            results.Sort(Weapon.CompareByRarity);
                            break;
                        case "baseattack":
                            results.Sort(Weapon.CompareByBaseAttack);
                            break;
                        case "name":
                        default:
                            results.Sort(Weapon.CompareByName);
                            break;
                    }
                }
            }

            if (displayCount)
            {
                Console.WriteLine("There are {0} entries", results.Count);
            }

            if (results.Count > 0)
            {
                if (!string.IsNullOrEmpty(outputFile))
                {
                    FileStream fs;

                    // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
                    if (appendToFile && File.Exists((outputFile)))
                    {
                        fs = File.Open(outputFile, FileMode.Append);
                    }
                    else
                    {
                        fs = File.Open(outputFile, FileMode.Create);
                    }

                    // opens a stream writer with the file handle to write to the output file.
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        // Hint: use writer.WriteLine
                        // TODO: write the header of the output "Name,Type,Rarity,BaseAttack" (DONE)
                        if (!appendToFile || fs.Position == 0)
                        {
                            writer.WriteLine("Name,Type,Rarity,BaseAttack");
                        }

                        // TODO: use the writer to output the results. (DONE)
                        foreach (var weapon in results)
                        {
                            writer.WriteLine(weapon.ToString());
                        }

                        // TODO: print out the file has been saved. (DONE)
                        Console.WriteLine("Output saved to {0}", outputFile);
                    }
                }
                else
                {
                    // prints out each entry in the weapon list results.
                    for (int i = 0; i < results.Count; i++)
                    {
                        Console.WriteLine(results[i]);
                    }
                }
            }

            Console.WriteLine("Done!");
        }

        /// <summary>
        /// Reads the file and line by line parses the data into a List of Weapons
        /// </summary>
        /// <param name="fileName">The path to the file</param>
        /// <returns>The list of Weapons</returns>
        public static List<Weapon> Parse(string fileName)
        {
            // TODO: implement the streamreader that reads the file and appends each line to the list
            // note that the result that you get from using read is a string, and needs to be parsed 
            // to an int for certain fields i.e. HP, Attack, etc.
            // i.e. int.Parse() and if the results cannot be parsed it will throw an exception
            // or can use int.TryParse() 

            // streamreader https://msdn.microsoft.com/en-us/library/system.io.streamreader(v=vs.110).aspx
            // Use string split https://msdn.microsoft.com/en-us/library/system.string.split(v=vs.110).aspx

            List<Weapon> output = new List<Weapon>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                // Skip the first line because header does not need to be parsed.
                // Name,Type,Rarity,BaseAttack

                string header = reader.ReadLine();

                // The rest of the lines looks like the following:
                // Skyward Blade,Sword,5,46
                while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    
                    string[] values = line.Split(',');

                    // TODO: validate that the string array the size expected. (DONE)
                    if (values.Length == 4)
                    {
                        Weapon weapon = new Weapon();

                        weapon.Name = values[0].Trim();
                        weapon.Type = values[1].Trim();

                        // TODO: use int.Parse or TryParse for stats/number values. (DONE)
                        if (int.TryParse(values[2], out int rarity))
                        {
                            weapon.Rarity = rarity;
                        }

                        if (int.TryParse(values[3], out int attack))
                        {
                            weapon.BaseAttack = attack;
                        }

                        // TODO: Add the Weapon to the list (DONE)
                        output.Add(weapon);
                    }
                }
            }

            return output;
        }
    }
}
