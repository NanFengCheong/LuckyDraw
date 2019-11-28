using LuckyDraw.Class;
using LuckyDraw.Data;
using LuckyDraw.Features;
using LuckyDraw.Model;
using MediatR;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Pages
{
    public class Helper
    {
        private readonly IJSRuntime JSRuntime;
        private readonly LuckyDrawService LuckyDrawService;
        private readonly IMediator Mediator;

        public Helper(IJSRuntime jsRuntime, LuckyDrawService luckyDrawService, IMediator mediator)
        {
            JSRuntime = jsRuntime;
            LuckyDrawService = luckyDrawService;
            Mediator = mediator;
        }

        public async Task<Prize> GetNextPrize()
        {
            var nextPrize = await LuckyDrawService.GetNextPrizeAsync();
            return nextPrize;
        }

        public async Task<LuckyDrawResult> StartSingleLuckyDrawResult()
        {
            LuckyDrawResult result = await LuckyDrawService.GetNextLuckyDrawAsync();
            await JSRuntime.InvokeVoidAsync("setSingleWinner", "winnerWwid", "winnerName", result.Winner.WWID, result.Winner.Name);
            return result;
        }

        public async Task<Prize> SetSinglePrizeWinner(LuckyDrawState luckyDrawState)
        {
            await LuckyDrawService.SetPrizeWinner(luckyDrawState.result.Prize.PrizeID, luckyDrawState.result.Winner.WWID);
            await LuckyDrawService.SetPrizeCollected(luckyDrawState.result.Prize.PrizeID);
            var nextPrize = await LuckyDrawService.GetNextPrizeAsync();
            return nextPrize;
        }

        public async Task<LuckyDrawResult> StartMultiLuckyDraw()
        {
            LuckyDrawResult result = await LuckyDrawService.GetNextLuckyDrawAsync(10);
            var winnerResult = result.Winners.Select((s, i) =>
            {
                return new
                {
                    winnerWwidId = "winnerWwid-" + i.ToString(),
                    winnerNameId = "winnerName-" + i.ToString(),
                    winnerWwid = result.Winners[i].WWID,
                    winnerName = result.Winners[i].Name
                };
            });

            await JSRuntime.InvokeVoidAsync("setMultiWinner", winnerResult);
            return result;
        }

        public async Task<List<Prize>> SetMultiPrizeWinner(LuckyDrawState luckyDrawState)
        {
            for (int i = 0; i < luckyDrawState.result.Winners.Count; i++)
            {
                await LuckyDrawService.SetPrizeWinner(luckyDrawState.result.Prizes[i].PrizeID, luckyDrawState.result.Winners[i].WWID);
            }
            var nextPrize = await LuckyDrawService.GetNextPrizeAsync(10);
            return nextPrize;
        }

        public async Task RunNormalAnimation()
        {            
            await JSRuntime.InvokeVoidAsync("runNormalAnimation", null);
        }

        public async Task RunDrawAnimation()
        {            
            await JSRuntime.InvokeVoidAsync("runDrawAnimation", null);
        }
    }
}