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

    public partial class LuckyDrawState
    {
        public class SetLuckyDrawResultAction : IAction
        {
            public LuckyDrawResult LuckyDrawResult { get; set; }
        }        
    }
}
