#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TSKT.Tweens
{
    public class PositionAndRotation : Task
    {
        public PositionAndRotation(GameObject target, float duration, bool scaledTime) : base(target, default, duration, scaledTime: scaledTime)
        {
            transform = target.transform;
        }

        bool local;
        (Vector3 position, Quaternion rotation)? to;
        (Vector3 position, Quaternion rotation)? from;
        Func<float, float, float, float> function = EasingFunction.Cubic.EaseOut;
        readonly Transform transform;

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
                    transform.GetLocalPositionAndRotation(out var pos, out var r);
                    To(pos, r);
                }
                else
                {
                    transform.GetPositionAndRotation(out var pos, out var r);
                    To(pos, r);
                }
            }
            if (!from.HasValue)
            {
                if (local)
                {
                    transform.GetLocalPositionAndRotation(out var pos, out var r);
                    From(pos, r);
                }
                else
                {
                    transform.GetPositionAndRotation(out var pos, out var r);
                    From(pos, r);
                }
            }

            var t = function.Invoke(0f, 1f, NormalizedElapsedTime);
            var rotation = Quaternion.LerpUnclamped(from!.Value.rotation, to!.Value.rotation, t);
            var position = Vector3.LerpUnclamped(from!.Value.position, to!.Value.position, t);

            if (local)
            {
                transform.SetLocalPositionAndRotation(position, rotation);
            }
            else
            {
                transform.SetPositionAndRotation(position, rotation);
            }
        }
        public new PositionAndRotation RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(Halt);

            return this;
        }
    }
}
