using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckyDraw.Features
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using BlazorState;
    using BlazorState.Features.JavaScriptInterop;
    using MediatR;

    public partial class LuckyDrawState
    {
        public class StartSingleLuckyDrawHandler : ActionHandler<StartSingleLuckyDrawAction>
        {
            private readonly JsonRequestHandler JsonRequestHandler;
            public StartSingleLuckyDrawHandler(IStore aStore) : base(aStore) { }

            LuckyDrawState CounterState => Store.GetState<LuckyDrawState>();

            public override Task<Unit> Handle(StartSingleLuckyDrawAction aIncrementCountAction, CancellationToken aCancellationToken)
            {                
                CounterState.Count = CounterState.Count + 1;
                return Unit.Task;
            }
        }
    }
}
