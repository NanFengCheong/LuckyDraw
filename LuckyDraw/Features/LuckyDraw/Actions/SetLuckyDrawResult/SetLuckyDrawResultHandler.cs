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
        public class SetLuckyDrawResultHandler : ActionHandler<SetLuckyDrawResultAction>
        {
            public SetLuckyDrawResultHandler(IStore aStore) : base(aStore) { }

            LuckyDrawState LuckyDrawState => Store.GetState<LuckyDrawState>();

            public override Task<Unit> Handle(SetLuckyDrawResultAction action, CancellationToken aCancellationToken)
            {
                LuckyDrawState.result = action.LuckyDrawResult;
                LuckyDrawState.result.Prize = action.LuckyDrawResult.Prize;
                LuckyDrawState.result.Winner = action.LuckyDrawResult.Winner;
                return Unit.Task;
            }
        }
    }
}
