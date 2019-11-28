using LuckyDraw.Class;
using LuckyDraw.Context;
using LuckyDraw.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Data
{
    public class LuckyDrawService
    {
        private LuckyDrawContext _LuckyDrawContext;

        public LuckyDrawService(LuckyDrawContext luckyDrawContext)
        {
            _LuckyDrawContext = luckyDrawContext;
        }

        public async Task<LuckyDrawResult> GetNextLuckyDrawAsync()
        {
            Prize prize = await GetNextPrizeAsync();
            if (prize != null)
            {
                try
                {
                    List<string> existingWinners = await GetExistingWinnerAsync(true);
                    List<Employee> potentialWinners = await GetPotentialWinnersAsync(existingWinners, 1);

                    var winner = potentialWinners.First();

                    return new LuckyDrawResult
                    {
                        Prize = prize,
                        Winner = winner
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<LuckyDrawResult> GetNextLuckyDrawAsync(int winnerCount = 10)
        {
            List<Prize> prizes = await GetNextPrizeAsync(10);
            if (prizes != null)
            {
                try
                {
                    List<string> existingWinners = await GetExistingWinnerAsync(false);
                    List<Employee> potentialWinners = await GetPotentialWinnersAsync(existingWinners, winnerCount);

                    return new LuckyDrawResult
                    {
                        Prizes = prizes,
                        Winners = potentialWinners
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task SetPrizeWinner(int prizeID, string wwwid)
        {
            var prize = await _LuckyDrawContext.Prizes.FindAsync(prizeID);
            prize.WWID = wwwid;
            prize.DrawTime = DateTimeOffset.Now;
            await _LuckyDrawContext.SaveChangesAsync();
        }

        public async Task SetPrizeCollected(int prizeID)
        {
            var prize = await _LuckyDrawContext.Prizes.FindAsync(prizeID);
            prize.CollectTime = DateTimeOffset.Now;
            await _LuckyDrawContext.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetPotentialWinnersAsync(List<string> existingWinners, int potentialWinnerCount = 3)
        {
            var potentialWinners = await _LuckyDrawContext.Employees
                                             .Where(w => !existingWinners.Contains(w.WWID))
                                             .ToListAsync();
            return potentialWinners.OrderBy(r => Guid.NewGuid())
                                             .Take(potentialWinnerCount)
                                             .ToList();
        }

        public async Task<List<string>> GetExistingWinnerAsync(bool isPrizeCollected)
        {
            return await _LuckyDrawContext.Prizes.Where(w => !string.IsNullOrWhiteSpace(w.WWID)).Select(s => s.WWID).ToListAsync();
            //if (isPrizeCollected)
            //{
            //    return await _LuckyDrawContext.Prizes.Where(w => !string.IsNullOrWhiteSpace(w.WWID) && w.CollectTime != null).Select(s => s.WWID).ToListAsync();
            //}
            //else
            //{
            //    return await _LuckyDrawContext.Prizes.Where(w => !string.IsNullOrWhiteSpace(w.WWID)).Select(s => s.WWID).ToListAsync();
            //}
        }

        public async Task<Prize> GetNextPrizeAsync()
        {
            //get next uncollected item
            return await _LuckyDrawContext.Prizes.Where(w => w.CollectTime == null).OrderBy(o => o.Order).FirstOrDefaultAsync();
        }

        public async Task<List<Prize>> GetNextPrizeAsync(int count = 10)
        {
            // get next undrawed prize of count
            return await _LuckyDrawContext.Prizes.Where(w => w.WWID == null).OrderBy(o => o.Order).Take(10).ToListAsync();
        }

        public async Task<Prize> ResetLuckyDrawAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Prize>> GetWinners()
        {
            return await _LuckyDrawContext.Prizes
                .Include(i => i.Employee)
                .Where(w => w.WWID != null)
                //.OrderByDescending(o => o.DrawTime)
                .ToListAsync();
        }
    }
}