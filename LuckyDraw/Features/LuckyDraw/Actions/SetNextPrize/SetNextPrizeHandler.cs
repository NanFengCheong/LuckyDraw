using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Features
{
    using System.Threading;
    using System.Threading.Tasks;
    using BlazorState;
    using MediatR;

    public partial class LuckyDrawState
    {
        public class SetNextPrizeHandler : ActionHandler<SetNextPrizeAction>
        {
            public SetNextPrizeHandler(IStore aStore) : base(aStore) { }

            LuckyDrawState LuckyDrawState => Store.GetState<LuckyDrawState>();

            public override Task<Unit> Handle(SetNextPrizeAction action, CancellationToken aCancellationToken)
            {
                LuckyDrawState.nextPrize = action.Prize;
                return Unit.Task;
            }
        }
    }
}
