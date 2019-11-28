using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Features
{
    using BlazorState;
    using MediatR;

    public partial class LuckyDrawState
    {
        public class StartSingleLuckyDrawAction : IAction
        {
            //public int Amount { get; set; }
        }        
    }
}
