#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TSKT.Tweens
{
    public class Value : Task
    {
        public delegate void CallbackAction(float time);

        float to;
        float from;
        Func<float, float, float, float> function = EasingFunction.Linear;
        Action<float>? callback;

        public Value(float duration, bool scaledTime) : base(null, default, duration, scaledTime: scaledTime)
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

        public Value Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        protected override void Apply()
        {
            callback?.Invoke(function.Invoke(from, to, NormalizedElapsedTime));
        }
        public new Value RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(Halt);

            return this;
        }
    }
}
