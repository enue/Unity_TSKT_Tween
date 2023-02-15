#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT.Tweens
{
    public class PositionAndRotation : Task
    {
        public PositionAndRotation(GameObject target, float duration, bool scaledTime) : base(target, duration, scaledTime: scaledTime)
        {
        }

        bool local;
        (Vector3 position, Quaternion rotation)? to;
        (Vector3 position, Quaternion rotation)? from;
        Func<float, float, float, float> function = EasingFunction.Cubic.EaseOut;

        public PositionAndRotation To(Vector3 position, Quaternion rotation)
        {
            to = (position, rotation);
            return this;
        }

        public PositionAndRotation From(Vector3 position, Quaternion rotation)
        {
            from = (position, rotation);
            return this;
        }

        public PositionAndRotation Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        public PositionAndRotation Local(bool value)
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
                    Target.transform.GetLocalPositionAndRotation(out var pos, out var r);
                    To(pos, r);
                }
                else
                {
                    Target.transform.GetPositionAndRotation(out var pos, out var r);
                    To(pos, r);
                }
            }
            if (!from.HasValue)
            {
                if (local)
                {
                    Target.transform.GetLocalPositionAndRotation(out var pos, out var r);
                    From(pos, r);
                }
                else
                {
                    Target.transform.GetPositionAndRotation(out var pos, out var r);
                    From(pos, r);
                }
            }

            var t = function.Invoke(0f, 1f, NormalizdElapsedTime);
            var rotation = Quaternion.LerpUnclamped(from!.Value.rotation, to!.Value.rotation, t);
            var position = Vector3.LerpUnclamped(from!.Value.position, to!.Value.position, t);

            if (local)
            {
                Target.transform.SetLocalPositionAndRotation(position, rotation);
            }
            else
            {
                Target.transform.SetPositionAndRotation(position, rotation);
            }
        }
    }
}
