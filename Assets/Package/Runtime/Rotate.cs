using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#nullable enable

namespace TSKT.Tweens
{
    public class Rotate : Task
    {
        public Rotate(GameObject target, float duration, bool scaledTime) : base(target, duration, scaledTime: scaledTime)
        {
        }

        bool local;
        Quaternion? to;
        Quaternion? from;
        Func<float, float, float, float> function = EasingFunction.Cubic.EaseOut;

        public Rotate To(Quaternion to)
        {
            this.to = to;
            return this;
        }

        public Rotate From(Quaternion from)
        {
            this.from = from;
            return this;
        }

        public Rotate Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        public Rotate Local(bool value)
        {
            local = value;
            return this;
        }

        protected override void Apply()
        {
            if (!to.HasValue)
            {
                if (local)
                {
                    To(Target.transform.localRotation);
                }
                else
                {
                    To(Target.transform.rotation);
                }
            }
            if (!from.HasValue)
            {
                if (local)
                {
                    From(Target.transform.localRotation);
                }
                else
                {
                    From(Target.transform.rotation);
                }
            }

            var t = function.Invoke(0f, 1f, NormalizdElapsedTime);
            var rotation = Quaternion.LerpUnclamped(from!.Value, to!.Value, t);

            if (local)
            {
                Target.transform.localRotation = rotation;
            }
            else
            {
                Target.transform.rotation = rotation;
            }
        }
        public Rotate RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() =>
            {
                Halt();
            });

            return this;
        }
    }
}
