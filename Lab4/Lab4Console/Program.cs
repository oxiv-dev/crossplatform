using LabsLibrary;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;

namespace Lab4Console
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "Lab4",
                Description = "CPP Console app for lab work 4",
            };

            app.HelpOption(inherited: true);

            app.Command("version", versionCmd =>
            {
                versionCmd.Description = "Console application info";
                versionCmd.OnExecute(() =>
                {
                    Console.WriteLine("Author: Tkach Viktoria");
                    Console.WriteLine("Version: 1.0.0");
                });
            });

            app.Command("run", runCmd =>
            {
                runCmd.Description = "Run lab by key";

                var inputPath = runCmd.Option("-i", "Path to input file", CommandOptionType.SingleValue);
                var outputPath = runCmd.Option("-o", "Path to output file", CommandOptionType.SingleValue);

                // Set default values to input/output variables
                var defaultPath = LoadLabPath();
                inputPath.DefaultValue = Path.Combine(defaultPath, "INPUT.TXT");
                outputPath.DefaultValue = Path.Combine(defaultPath, "OUTPUT.txt");
                

                runCmd.OnExecute(() =>
                {
                    Console.WriteLine("\nLab command not specified");
                    runCmd.ShowHelp();
                });
                
                runCmd.Command("Lab1", lab1Cmd =>
                {
                    lab1Cmd.Description = "Run lab work 1";
                    lab1Cmd.OnExecute(() =>
                    {
                        Console.WriteLine("Lab1 was started with the next file: " + inputPath.Value());
                        LabsLibrary.Lab1.Run(inputPath.Value(), outputPath.Value());
                        Console.WriteLine("\nLab1 result saved to the next file: " + outputPath.Value());
                    });
                });

                runCmd.Command("Lab2", lab1Cmd =>
                {
                    lab1Cmd.Description = "Run lab work 2";
                    lab1Cmd.OnExecute(() =>
                    {
                        Console.WriteLine("Lab2 was started with the next file: " + inputPath.Value());
                        LabsLibrary.Lab2.Run(inputPath.Value(), outputPath.Value());
                        Console.WriteLine("\nLab2 result saved to the next file: " + outputPath.Value());
                    });
                });

                runCmd.Command("Lab3", lab1Cmd =>
                {
                    lab1Cmd.Description = "Run lab work 3";
                    lab1Cmd.OnExecute(() =>
                    {
                        Console.WriteLine("Lab3 was started with the next file: " + inputPath.Value());
                        LabsLibrary.Lab3.Run(inputPath.Value(), outputPath.Value());
                        Console.WriteLine("\nLab3 result saved to the next file: " + outputPath.Value());
                    });
                });

            });

            app.Command("set-path", setPathCmd => 
            {
                setPathCmd.Description = "Change standart directory LAB_PATH";

                var path = setPathCmd.Argument("path", "path to input and output folder");

                setPathCmd.OnExecute(() =>
                {
                    if (path.Value != null)
                        File.WriteAllText("LAB_PATH", path.Value);
                    else
                        setPathCmd.ShowHelp();
                });

                setPathCmd.Command("default", defaultCmd => 
                {
                    defaultCmd.Description = "Set LAB_PATH to default value";
                    defaultCmd.OnExecute(() =>
                    {
                        File.WriteAllText("LAB_PATH", "Data");
                    });
                });
            });

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 1;
            });
            return app.Execute(args);
        }

        private static string LoadLabPath()
        {
            if (File.Exists("LAB_PATH"))
            {
                return File.ReadAllText("LAB_PATH");
            }
            else
            {
                File.WriteAllText("LAB_PATH", "");
                return "";
            }
        }

		public string OnExecute(string lab, string[] strings)
		{
            try
            {
                if (lab == "Lab1")
                {
                    File.WriteAllText("input1.txt", strings[0]);
                    Main(new[] { "run", "Lab1" });
                    return File.ReadAllText("output1.txt");
                }
                else if (lab == "Lab2")
                {
                    File.WriteAllText("input2.txt", strings[0]);
                    Main(new[] { "run", "Lab2" });
                    return File.ReadAllText("output2.txt");
                }
                else if (lab == "Lab3")
                {
                    File.WriteAllText("INPUT.txt", strings[0]);
                    Main(new[] { "run", "Lab3" });
                    return File.ReadAllText("OUTPUT.txt");
                }
                else
                    return "Wrong lab name";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
		}
	}
}
