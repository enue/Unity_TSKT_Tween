using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#nullable enable

namespace TSKT.Tweens
{
    public class ShakePosition : Task
    {
        Vector3 amount;
        bool local;
        Vector3? initialPosition;
        readonly Transform transform;

        public ShakePosition(GameObject target, float duration, bool scaledTime, System.Threading.CancellationToken destroyCancellationToken)
            : base(target, destroyCancellationToken, duration, scaledTime: scaledTime)
        {
            transform = target.transform;
        }

        public ShakePosition Amount(Vector3 value)
        {
            amount = value;
            return this;
        }

        public ShakePosition Amount(float value)
        {
            amount = new Vector3(value, value, value);
            return this;
        }

        public ShakePosition Local(bool value)
        {
            local = value;
            return this;
        }

        protected override void Apply()
        {
            if (!initialPosition.HasValue)
            {
                if (local)
                {
                    initialPosition = transform.localPosition;
                }
                else
                {
                    initialPosition = transform.position;
                }
            }

            var size = 1f - NormalizedElapsedTime;
            var offset = new Vector3(
                UnityEngine.Random.Range(-size * amount.x, size * amount.x),
                UnityEngine.Random.Range(-size * amount.y, size * amount.y),
                UnityEngine.Random.Range(-size * amount.z, size * amount.z));

            if (local)
            {
                transform.localPosition = initialPosition.Value + offset;
            }
            else
            {
                transform.position = initialPosition.Value + offset;
            }
        }
        public new ShakePosition RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(Halt);

            return this;
        }
    }
}
