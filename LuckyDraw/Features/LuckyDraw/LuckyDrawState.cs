using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Features
{
    using BlazorState;
    using LuckyDraw.Class;
    using LuckyDraw.Model;
    using MediatR;

    public partial class LuckyDrawState : State<LuckyDrawState>
    {
        public int Count { get; private set; }
        public LuckyDrawResult result;
        public Prize nextPrize;
        public List<Prize> nextPrizes;

        public string NextPrizeName
        {
            get
            {
                if (nextPrize != null)
                {
                    return nextPrize.PrizeName;
                }
                else
                {
                    return nextPrizes.Max(m => m.PrizeName) + " - " + nextPrizes.Min(m => m.PrizeName);
                }
            }
        }

        public void ResetLuckyDrawResult()
        {
            result = new LuckyDrawResult();
        }

        public override void Initialize()
        {
            Count = 3;
        }
    }
}