using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

namespace TSKT.Tweens
{
    public class Manipulator<T> : Task
        where T : UnityEngine.Component
    {
        Action<float, T>? action;
        readonly T target;

        public Manipulator(T target, float duration, bool scaledTime) : base(target.gameObject, duration, scaledTime: scaledTime)
        {
            this.target = target;
        }
        public Manipulator<T> Action(Action<float, T> value)
        {
            action = value;
            return this;
        }

        protected override void Apply()
        {
            action?.Invoke(NormalizdElapsedTime, target);
        }
    }
}
