using LuckyDraw.Class;
using LuckyDraw.Context;
using LuckyDraw.Data;
using LuckyDraw.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LuckDraw.XUnitTest
{
    public class LuckyDrawServiceTest
    {
        private List<Prize> Prizes { get; set; }
        private List<Employee> Employees { get; set; }

        public LuckyDrawServiceTest()
        {
            Prizes = new List<Prize> {
                    new Prize
                    {
                        PrizeName = "Prize1",
                        Order =1
                    },
                    new Prize
                    {
                        PrizeName = "Prize2",
                        Order =2
                    },
                    new Prize
                    {
                        PrizeName = "Prize3",
                        Order =3
                    },
                    new Prize
                    {
                        PrizeName = "Prize4",
                        Order =4,
                        WWID="Employee1"
                    },
                    new Prize
                    {
                        PrizeName = "Prize5",
                        Order =5
                    }
                };

            Employees = Enumerable.Range(1, 10)
                                  .Select(s =>
                                    {
                                        return new Employee
                                        {
                                            WWID = "Employee" + s,
                                            Name = "Employee" + s
                                        };
                                    }).ToList();
        }

        [Fact]
        public async void GetExistingWinnerAsyncTest()
        {
            // Setup
            var options = new DbContextOptionsBuilder<LuckyDrawContext>()
               .UseInMemoryDatabase(databaseName: "GetExistingWinnerAsyncTest")
               .Options;

            // Arrange
            using (var context = new LuckyDrawContext(options))
            {
                var prizes = Prizes;
                await context.Prizes.AddRangeAsync(prizes);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new LuckyDrawContext(options))
            {
                var service = new LuckyDrawService(context);
                var existingWinners = await service.GetExistingWinnerAsync();
                Assert.Equal(1, existingWinners.Count);
                Assert.Equal("Employee1", existingWinners.First());
            }
        }

        [Fact]
        public async void GetPotentialWinnersAsyncTest()
        {
            // Setup
            var options = new DbContextOptionsBuilder<LuckyDrawContext>()
               .UseInMemoryDatabase(databaseName: "GetPotentialWinnersAsyncTest")
               .Options;

            // Arrange
            using (var context = new LuckyDrawContext(options))
            {               
                await context.Prizes.AddRangeAsync(Prizes);
                await context.Employees.AddRangeAsync(Employees);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new LuckyDrawContext(options))
            {
                var service = new LuckyDrawService(context);
                var existingWinners = await service.GetExistingWinnerAsync();
                var potentialWinners = await service.GetPotentialWinnersAsync(existingWinners, 5);
                Assert.Equal(5, potentialWinners.Count);
                Assert.DoesNotContain("Employee1", potentialWinners.Select(s=>s.WWID));
            }
        }

        [Fact]
        public async void GetNextPrizeAsyncTest()
        {
            // Setup
            var options = new DbContextOptionsBuilder<LuckyDrawContext>()
               .UseInMemoryDatabase(databaseName: "GetNextPrizeAsyncTest")
               .Options;

            // Arrange
            using (var context = new LuckyDrawContext(options))
            {
                await context.Prizes.AddRangeAsync(Prizes);
                await context.Employees.AddRangeAsync(Employees);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new LuckyDrawContext(options))
            {
                var service = new LuckyDrawService(context);
                var nextPrize = await service.GetNextPrizeAsync();
                Assert.Equal("Prize1", nextPrize.PrizeName);
            }
        }

        [Fact]
        public async void GetNextLuckyDrawAsync()
        {
            // Setup
            var options = new DbContextOptionsBuilder<LuckyDrawContext>()
               .UseInMemoryDatabase(databaseName: "GetNextPrizeAsyncTest")
               .Options;

            LuckyDrawResult LuckyDrawResult = new LuckyDrawResult();

            // Arrange
            using (var context = new LuckyDrawContext(options))
            {
                await context.Prizes.AddRangeAsync(Prizes);
                await context.Employees.AddRangeAsync(Employees);
                await context.SaveChangesAsync();
                var service = new LuckyDrawService(context);
                LuckyDrawResult = await service.GetNextLuckyDrawAsync(10);
            }

            // Assert
            using (var context = new LuckyDrawContext(options))
            {
                var service = new LuckyDrawService(context);
                var existingWinners = await service.GetExistingWinnerAsync();
                var potentialWinners = await service.GetPotentialWinnersAsync(existingWinners, 10);

                Assert.DoesNotContain(LuckyDrawResult.Winner.WWID, potentialWinners.Select(s => s.WWID));
                Assert.Equal(8, potentialWinners.Count);
                Assert.Equal(LuckyDrawResult.Winner.WWID, context.Prizes.First(f => f.PrizeID == LuckyDrawResult.Prize.PrizeID).WWID);
                Assert.Equal(3, context.Prizes.Where(w => w.WWID == null).Count());
            }
        }
    }
}
