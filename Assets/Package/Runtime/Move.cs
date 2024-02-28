#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TSKT.Tweens
{
    public class Move : Task
    {
        public Move(GameObject target, float duration, bool scaledTime) : base(target, default, duration, scaledTime: scaledTime)
        {
            this.target = target.transform;
        }

        readonly Transform target;
        bool anchoredPosition;
        bool local;
        float? toX;
        float? toY;
        float? toZ;

        float? fromX;
        float? fromY;
        float? fromZ;

        Func<float, float, float, float> function = EasingFunction.Cubic.EaseOut;

        public Move To(Vector3 position)
        {
            toX = position.x;
            toY = position.y;
            toZ = position.z;
            return this;
        }

        public Move ToX(float value)
        {
            toX = value;
            return this;
        }
        public Move ToY(float value)
        {
            toY = value;
            return this;
        }
        public Move ToZ(float value)
        {
            toZ = value;
            return this;
        }

        public Move From(Vector3 position)
        {
            fromX = position.x;
            fromY = position.y;
            fromZ = position.z;
            return this;
        }
        public Move FromX(float value)
        {
            fromX = value;
            return this;
        }
        public Move FromY(float value)
        {
            fromY = value;
            return this;
        }
        public Move FromZ(float value)
        {
            fromZ = value;
            return this;
        }

        public Move Local(bool value)
        {
            local = value;
            return this;
        }

        public Move AnchoredPosition(bool value)
        {
            anchoredPosition = value;
            return this;
        }

        public Move Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        protected override void Apply()
        {
            if (!toX.HasValue
                && !toY.HasValue
                && !toZ.HasValue)
            {
                if (anchoredPosition)
                {
                    To(((RectTransform)target).anchoredPosition3D);
                }
                else if (local)
                {
                    To(target.localPosition);
                }
                else
                {
                    To(target.position);
                }
            }
            if (!fromX.HasValue
                && !fromY.HasValue
                && !fromZ.HasValue)
            {
                if (anchoredPosition)
                {
                    From(((RectTransform)target).anchoredPosition3D);
                }
                else if (local)
                {
                    From(target.localPosition);
                }
                else
                {
                    From(target.position);
                }
            }

            Vector3 position;
            if (anchoredPosition)
            {
                position = ((RectTransform)target).anchoredPosition3D;
            }
            else if (local)
            {
                position = target.localPosition;
            }
            else
            {
                position = target.position;
            }

            if (toX.HasValue && fromX.HasValue)
            {
                position.x = function.Invoke(fromX.Value, toX.Value, NormalizedElapsedTime);
            }
            if (toY.HasValue && fromY.HasValue)
            {
                position.y = function.Invoke(fromY.Value, toY.Value, NormalizedElapsedTime);
            }
            if (toZ.HasValue && fromZ.HasValue)
            {
                position.z = function.Invoke(fromZ.Value, toZ.Value, NormalizedElapsedTime);
            }

            if (anchoredPosition)
            {
                ((RectTransform)target).anchoredPosition3D = position;
            }
            else if (local)
            {
                target.localPosition = position;
            }
            else
            {
                target.position = position;
            }
        }
        public Move RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() =>
            {
                Halt();
            });

            return this;
        }
    }
}
