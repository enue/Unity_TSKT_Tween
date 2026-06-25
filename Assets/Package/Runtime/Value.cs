#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using R3;

namespace TSKT.Tweens
{
    public class Value : Task
    {
        float to;
        float from;
        Func<float, float, float, float> function = EasingFunction.Linear;
        Action<float>? callback;
        R3.Subject<float>? subject;

        public Value(float duration, bool scaledTime, GameObject? target, CancellationToken cancellationToken)
            : base(target, cancellationToken, duration, scaledTime: scaledTime)
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

        public R3.Observable<float> Observable
        {
            get
            {
                if (subject == null)
                {
                    subject = new();
                    DisposeAsync().LogExceptionsAndForget();
                }
                return subject;

                async Awaitable DisposeAsync()
                {
                    await Await();
                    subject?.Dispose();
                    subject = null;
                }
            }
        }
        public Value Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        protected override void Apply()
        {
            var value = function.Invoke(from, to, NormalizedElapsedTime);
            callback?.Invoke(value);
            subject?.OnNext(value);
        }
        public new Value RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(Halt);

            return this;
        }
    }

    public class Value<T> : Task
    {
        T state;
        float to;
        float from;
        Func<float, float, float, float> function = EasingFunction.Linear;
        Action<float, T>? callback;
        R3.Subject<float>? subject;

        public Value(float duration, bool scaledTime, GameObject? target, CancellationToken cancellationToken)
            : base(target, cancellationToken, duration, scaledTime: scaledTime)
        {
        }

        public Value<T> To(float value)
        {
            to = value;
            return this;
        }

        public Value<T> From(float value)
        {
            from = value;
            return this;
        }

        public Value<T> Subscribe(T state, Action<float, T> callback)
        {
            this.state = state;
            this.callback += callback;
            return this;
        }

        public R3.Observable<float> Observable
        {
            get
            {
                if (subject == null)
                {
                    subject = new();
                    Observe().LogExceptionsAndForget();
                }
                return subject;

                async Awaitable Observe()
                {
                    await Await();
                    subject.OnCompleted();
                    subject = null;
                }
            }
        }

        public Value<T> Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        protected override void Apply()
        {
            var value = function.Invoke(from, to, NormalizedElapsedTime);
            callback?.Invoke(value, state);
            subject?.OnNext(value);
        }
        public new Value<T> RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(Halt);

            return this;
        }
    }
}
