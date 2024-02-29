using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#nullable enable

namespace TSKT.Tweens
{
    public class Scale : Task
    {
        public Scale(GameObject target, float duration, bool scaledTime, System.Threading.CancellationToken destroyCancellationToken)
            : base(target, destroyCancellationToken, duration, scaledTime: scaledTime)
        {
            transform = target.transform;
        }

        Vector3? to;
        Vector3? from;
        Func<float, float, float, float> function = EasingFunction.Cubic.EaseOut;
        readonly Transform transform;
        public Scale To(Vector3 to)
        {
            this.to = to;
            return this;
        }

        public Scale From(Vector3 from)
        {
            this.from = from;
            return this;
        }

        public Scale Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        protected override void Apply()
        {
            if (!to.HasValue)
            {
                To(transform.localScale);
            }
            if (!from.HasValue)
            {
                From(transform.localScale);
            }
            var x = function.Invoke(from!.Value.x, to!.Value.x, NormalizedElapsedTime);
            var y = function.Invoke(from.Value.y, to.Value.y, NormalizedElapsedTime);
            var z = function.Invoke(from.Value.z, to.Value.z, NormalizedElapsedTime);

            transform.localScale = new Vector3(x, y, z);
        }
        public new Scale RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(Halt);

            return this;
        }
    }
}
