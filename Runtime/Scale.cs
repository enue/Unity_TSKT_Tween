using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT.Tweens
{
    public class Scale : Task
    {
        public Scale(GameObject target, float duration, bool scaledTime) : base(target, duration, scaledTime: scaledTime)
        {
        }

        Vector3? to;
        Vector3? from;
        Func<float, float, float, float> function = EasingFunction.Cubic.EaseOut;

        public Scale To(Vector3 position)
        {
            to = position;
            return this;
        }

        public Scale From(Vector3 position)
        {
            from = position;
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
                To(Target.transform.localScale);
            }
            if (!from.HasValue)
            {
                From(Target.transform.localScale);
            }

            var x = function.Invoke(from.Value.x, to.Value.x, NormalizdElapsedTime);
            var y = function.Invoke(from.Value.y, to.Value.y, NormalizdElapsedTime);
            var z = function.Invoke(from.Value.z, to.Value.z, NormalizdElapsedTime);

            Target.transform.localScale = new Vector3(x, y, z);
        }
    }
}
