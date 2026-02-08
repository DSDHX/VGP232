using System;
using System.Collections.Generic;
using System.IO;

// TODO: Fill in your name and student number. (DONE)
// Assignment 2a
// NAME: Haoxi Dong
// STUDENT NUMBER: 2343873

namespace Assignment2a
{
    internal class MainClass
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
            WeaponCollection results = new WeaponCollection();

            for (int i = 0; i < args.Length; i++)
            {
                // h or --help for help to output the instructions on how to use it
                if (args[i] == "-h" || args[i] == "--help")
                {
                    Console.WriteLine("-i <path> or --input <path> : loads the input file path specified (required)");
                    Console.WriteLine(
                        "-o <path> or --output <path> : saves result in the output file path specified (optional)");
                    Console.WriteLine("-c or --count : displays the number of entries in the input file (optional).");
                    Console.WriteLine("-a or --append : " +
                                      "enables append mode when writing to an existing output file (optional).");
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
                            Console.WriteLine("No input file specified.");
                        }
                        else if (!File.Exists(inputFile))
                        {
                            Console.WriteLine("The file specified does not exist.");
                        }
                        else
                        {
                            results.Load(inputFile);
                        }
                    }
                }
                else if (args[i] == "-s" || args[i] == "--sort")
                {
                    sortEnabled = true;
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
                    appendToFile = true;
                }
                else if (args[i] == "-o" || args[i] == "--output")
                {
                    if (args.Length > i + 1)
                    {
                        ++i;
                        outputFile = args[i];
                        if (string.IsNullOrEmpty(outputFile))
                        {
                            Console.WriteLine("No output file specified.");
                        }
                    }
                }
            }

            if (sortEnabled)
            {
                Console.WriteLine("Sorting by {0}.", string.IsNullOrEmpty(sortColumnName) ? "Name" : sortColumnName);
                results.SortBy(sortColumnName);
            }

            if (displayCount)
            {
                Console.WriteLine("There are {0} entries", results.Count);
            }

            if (results.Count > 0)
            {
                if (!string.IsNullOrEmpty(outputFile))
                {
                    if (appendToFile)
                    {
                        try
                        {
                            bool fileExists = File.Exists(outputFile);
                            using (StreamWriter writer = new StreamWriter(outputFile, true))
                            {
                                if (!fileExists || new FileInfo(outputFile).Length == 0)
                                {
                                    writer.WriteLine("Name, Type, Image, Rarity, BaseAttack, SecondaryStat, Passive");
                                }

                                foreach (var weapon in results)
                                {
                                    writer.WriteLine(weapon.ToString());
                                }
                            }

                            Console.WriteLine("Output appended to {0}", outputFile);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error appending file: " + ex.Message);
                        }
                    }
                    else
                    {
                        if (results.Save(outputFile))
                        {
                            Console.WriteLine("Output saved to {0}", outputFile);
                        }
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
    }
}