#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#if R3_SUPPORT
using R3;
#endif

namespace TSKT.Tweens
{
    public class Value : Task
    {
        float to;
        float from;
        Func<float, float, float, float> function = EasingFunction.Linear;
        Action<float>? callback;
#if R3_SUPPORT
        R3.Subject<float>? subject;
#endif

        public Value(float duration, bool scaledTime, GameObject? target = null, CancellationToken destroyCancellationToken = default)
            : base(target, destroyCancellationToken, duration, scaledTime: scaledTime)
        {
        }

        public Value To(float value)
        {
            to = value;
            return this;
        }

        public Value From(float value)
        {
            from = value;
            return this;
        }

        public Value Subscribe(Action<float> callback)
        {
            this.callback += callback;
            return this;
        }

#if R3_SUPPORT
        public R3.Observable<float> Observable
        {
            get
            {
                if (subject == null)
                {
                    subject = new();
                    _ = DisposeAsync();
                }
                return subject;

                async Awaitable DisposeAsync()
                {
                    await Awaitable;
                    subject?.Dispose();
                    subject = null;
                }
            }
        }
#endif

        public Value Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        protected override void Apply()
        {
            var value = function.Invoke(from, to, NormalizedElapsedTime);
            callback?.Invoke(value);
#if R3_SUPPORT
            subject?.OnNext(value);
#endif
        }
        public new Value RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(Halt);

            return this;
        }
    }
}
