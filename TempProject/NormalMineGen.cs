using IsekaiTechnologies.FacCord.Core.DAL;
using IsekaiTechnologies.FacCord.Core.DAL.EntityFramework;
using IsekaiTechnologies.FacCord.Core.Mine;
using IsekaiTechnologies.FacCord.Core.Models.Equipments;
using IsekaiTechnologies.FacCord.Core.Models.Mines;
using IsekaiTechnologies.FacCord.Core.Models.Storages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace TempProject
{
    [TestGallery("Standrand mine generation", 
        Description = "This is a demonstrasion about how mine generation work in action",
        LastModified = "11:47 PM, 31th July, 2020",
        CreatedAt = "11:47 PM, 31th July, 2020")]
    class NormalMineGen : ITestGallery
    {
        public void Run(string[] args)
        {
            Task.Run(async () =>
            {
                Console.Clear();

                Mine mine = new Mine(new Prospector() { RangeX = 100, RangeY = 100 }, 1, new Block() { Item = new Item() { Id = 0 }, Hardness = -1, IsSolid = false });
                mine.MaterialCounts = new Dictionary<Block, int>()
                {
                    [new Block() { Item = new Item() { Id = 1 } }] = 100,
                    [new Block() { Item = new Item() { Id = 2 } }] = 100,
                    [new Block() { Item = new Item() { Id = 3 } }] = 100,
                    [new Block() { Item = new Item() { Id = 4 } }] = 9700 - 9,
                };

                Console.Write("     ");
                for (int i = 0; i < 100; i++)
                {
                    Console.Write($"{i,2}");
                }
                Console.WriteLine("\n");

                Dictionary<long, int> a = new Dictionary<long, int>()
                {
                    [0] = 0,
                    [1] = 0,
                    [2] = 0,
                    [3] = 0,
                    [4] = 0,
                };

                Stopwatch sw = new Stopwatch();
                sw.Start();
                await mine.GenerateMap();
                sw.Stop();
                ComputeTime = sw.ElapsedMilliseconds;

                for (int h = 0; h < mine.Height; h++)
                {
                    Console.Write($"{h,2}  ");
                    for (int w = 0; w < mine.Width; w++)
                    {
                        if (mine[w, h] == null)
                        {
                            Console.Write("? ");
                            continue;
                        }
                        else if (mine[w, h].Item.Id == 4)
                            Console.Write($"  ");
                        else
                            Console.Write($"{mine[w, h].Item.Id,2}");

                        a[mine[w, h].Item.Id]++;

                    }
                    Console.WriteLine();
                }

                foreach (var item in a)
                {
                    Console.WriteLine(item.Value);
                }
            }).GetAwaiter().GetResult();
        }

        public long ComputeTime { get; set; }
        public string Report() => $"Computation time: {ComputeTime}ms!";
    }
}
