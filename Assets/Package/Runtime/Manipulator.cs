using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#nullable enable

namespace TSKT.Tweens
{
    public class Manipulator<T> : Task
        where T : UnityEngine.Component
    {
        Action<ManipulatingHandler<T>>? action;
        readonly T target;

        public Manipulator(T target, float duration, bool scaledTime) : base(target.gameObject, duration, scaledTime: scaledTime)
        {
            this.target = target;
        }
        public Manipulator<T> Action(Action<ManipulatingHandler<T>> value)
        {
            action = value;
            return this;
        }

        protected override void Apply()
        {
            action?.Invoke(new ManipulatingHandler<T>(this, target));
        }
        public Manipulator<T> RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() =>
            {
                Halt();
            });

            return this;
        }
    }
    public readonly struct ManipulatingHandler<T>
    {
        readonly Task task;
        public T Target { get; }
        public float NormalziedElapsedTime => task.NormalizdElapsedTime;
        public float Duration => task.Duration;
        public float ElapsedTime => task.ElapsedTime;

        public ManipulatingHandler(Task task, T target)
        {
            this.task = task;
            Target = target;
        }
    }
}
