using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Features
{
    using BlazorState;
    using LuckyDraw.Model;
    using MediatR;

    public partial class LuckyDrawState
    {
        public class SetNextPrizeAction : IAction
        {
            public Prize Prize { get; set; }
        }        
    }
}
