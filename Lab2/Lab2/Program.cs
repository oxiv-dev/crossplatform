//variant 72
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Lab2
{
    static class Palindromes
    {
        public static List<string> list;
        public static bool found;
    }
    
    internal class Program
    {
        public static bool IsPalindrome(string sub)
        {
            var result = true;
            for (int i = 0; i < (int)sub.Length / 2; i++)
            {
                if (sub[i] != sub[sub.Length - i - 1])
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public static void FindPalindromes(string subset, string place = "none")
        {
            switch (subset.Length)
            {
                case 1:
                    Palindromes.list.Add(subset);
                    break;
                case 0:
                    return;
                default:
                {
                    for (var i = subset.Length; i > 0; i--)
                    {
                        Palindromes.found = false;
                        for (var j = 0; j + i <= subset.Length; j++)
                        {
                            var toCheck = subset.Substring(j, i);
                            if (IsPalindrome(toCheck))
                            {
                                Palindromes.found = true;
                                Palindromes.list.Add(toCheck);
                                var parts = subset.Split(new [] { toCheck }, StringSplitOptions.RemoveEmptyEntries);
                                if (parts.Length > 0)
                                {
                                    FindPalindromes(parts[0], "before");
                                    if (parts.Length > 1)
                                    {
                                        FindPalindromes(parts[1], "after");
                                    }
                                }
                                break;
                            }
                        }
                        if (Palindromes.found) break;
                    }
                    break;
                }
            }
        }
        public static void Run(string input, string output)
        {
            string line;
            Palindromes.list = new List<string>();
            var fileStream = new FileStream(@"input.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                line = streamReader.ReadLine();

            }

            if (line == null)
            {
                Console.WriteLine("File must be non-empty!");
            }
            else
            {
                FindPalindromes(line);
                Console.WriteLine($"{Palindromes.list.Count}");
                Palindromes.list.Sort((a, b) => a.Length.CompareTo(b.Length));
            }
            var writeStream = new FileStream(@"output.txt", FileMode.Create, 
                FileAccess.ReadWrite, FileShare.None);
            using (var streamWriter= new StreamWriter(writeStream, Encoding.UTF8))
            {
                
                streamWriter.WriteLine(Palindromes.list.Count);
                foreach (var s in Palindromes.list)
                {
                    Console.WriteLine(s);
                    streamWriter.WriteLine(s);
                }
            }
        }
        
        public static void Main(string[] args)
        {
            string input = "INPUT2.txt";
            string output = "OUTPUT.txt";
            Run(input, output);
        }
    }
}