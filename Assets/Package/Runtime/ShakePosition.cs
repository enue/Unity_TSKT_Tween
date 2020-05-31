using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSKT.Tweens
{
    public class ShakePosition : Task
    {
        Vector3 amount;
        bool local;
        Vector3? initialPosition;

        public ShakePosition(GameObject target, float duration, bool scaledTime) : base(target, duration, scaledTime: scaledTime)
        {
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
                    initialPosition = Target.transform.localPosition;
                }
                else
                {
                    initialPosition = Target.transform.position;
                }
            }

            var size = 1f - NormalizdElapsedTime;
            var offset = new Vector3(
                UnityEngine.Random.Range(-size * amount.x, size * amount.x),
                UnityEngine.Random.Range(-size * amount.y, size * amount.y),
                UnityEngine.Random.Range(-size * amount.z, size * amount.z));

            if (local)
            {
                Target.transform.localPosition = initialPosition.Value + offset;
            }
            else
            {
                Target.transform.position = initialPosition.Value + offset;
            }
        }
    }
}
