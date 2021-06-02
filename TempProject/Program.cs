using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace TempProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TestGalleryInfo> testGalleries = new List<TestGalleryInfo>();
            foreach (var item in Assembly.GetEntryAssembly().DefinedTypes)
            {
                if (item.GetInterfaces().Contains(typeof(ITestGallery)))
                {
                    var info = new TestGalleryInfo();
                    info.Type = item;

                    var attInfo = item.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(TestGalleryAttribute));
                    if (attInfo != null)
                    {
                        info.Name = (string) attInfo.ConstructorArguments[0].Value;

                        foreach (var attItem in attInfo.NamedArguments)
                        {
                            switch (attItem.MemberName)
                            {
                                case "Description":
                                    info.Description = (string)attItem.TypedValue.Value;
                                    break;
                                case "CreatedAt":
                                    info.CreatedAt = (string)attItem.TypedValue.Value;
                                    break;
                                case "LastModified":
                                    info.LastModified = (string)attItem.TypedValue.Value;
                                    break;
                            }
                        }
                    }

                    testGalleries.Add(info);
                }
            }

            Console.WriteLine("Welcome to MineCord temp project. All tests, demonstrations are saved here.");
            Console.WriteLine("Note: GUI version might be implement for specific tests");

            List<TestGalleryInfo> filterResult = testGalleries.Where(x => true).ToList();
            string searchedKeyword = "";
            while (true)
            {
                Console.Clear();
                Console.WriteLine(string.IsNullOrEmpty(searchedKeyword) ? "List of tests/demonstrations: " : $"Search tests/demonstrations for {searchedKeyword}:");

                for (int i = 0; i < filterResult.Count; i++)
                {
                    Console.WriteLine($"{i, -3}:{filterResult[i].Name} Created at: {filterResult[i].CreatedAt} Last Modified: {filterResult[i].LastModified}");
                    Console.WriteLine($"    Typed full name: {filterResult[i].Type.FullName}");
                    Console.WriteLine($"    Description: {(string.IsNullOrEmpty(filterResult[i].Description) ? filterResult[i].Description : "No description provided!")}");
                    Console.WriteLine();
                }
                Console.WriteLine("Enter a number to run the specified test, keywords in quotes to search");

                string input = Console.ReadLine();
                int choice;
                if (int.TryParse(input, out choice))
                {
                    if (choice < 0 || choice >= filterResult.Count)
                    {
                        Console.WriteLine($"Invalid input: Please choose from 0 - {filterResult.Count - 1}!");
                    }
                    else
                    {
                        Exec(filterResult[choice], args);
                    }
                }
                else
                {
                    filterResult = testGalleries.Where(x => x.Name.Contains(input, StringComparison.OrdinalIgnoreCase) || x.Description.Contains(input, StringComparison.OrdinalIgnoreCase)).ToList();
                    searchedKeyword = input;
                }
            }
        }

        // return if the user want to exit
        private static bool Exec(TestGalleryInfo testGalleryInfo, string[] args)
        {
            Console.Clear();
            Console.WriteLine("------------------ Tests/demonstation infomation ------------------");
            Console.WriteLine($"{testGalleryInfo.Name} Created at: {testGalleryInfo.CreatedAt} Last Modified: {testGalleryInfo.LastModified}");
            Console.WriteLine($"Typed full name: {testGalleryInfo.Type.FullName}");
            Console.WriteLine($"Description: {testGalleryInfo.Description}");
            Console.WriteLine("------------------ Running in 3 seconds ------------------");
            Thread.Sleep(3000);

            while (true)
            {
                Console.Clear();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                var test = (Activator.CreateInstance(testGalleryInfo.Type) as ITestGallery);
                test.Run(args);
                sw.Stop();
                Console.WriteLine("\n--------------------------------------------------------------");
                Console.WriteLine($"Tests/demonstation executed in {sw.ElapsedMilliseconds}ms!");
                var rp = test.Report();
                if (string.IsNullOrEmpty(rp))
                {
                    Console.WriteLine("This test does not provide any additional information.");
                }
                else
                {
                    Console.WriteLine("The below text is addtional information provided by the test/demonstation: ");
                    Console.WriteLine(rp);
                }

                Console.WriteLine("Enter: \"list\" to return gallery list, \"exit\" to quit the application, others to repeat this test/demonstration");
                switch (Console.ReadLine().ToLower())
                {
                    case "list":
                    case "l":
                        return false;
                    case "exit":
                    case "e":
                    case "x":
                        return true;
                }
            }
        }
    }
}
