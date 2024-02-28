using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#nullable enable

namespace TSKT.Tweens
{
    public class SoundVolume : Task
    {
        readonly AudioSource targetAudio;
        float? to;
        float? from;
        Func<float, float, float, float> function = EasingFunction.Linear;

        public SoundVolume(AudioSource target, float duration, bool scaledTime) : base(target.gameObject, default, duration, scaledTime: scaledTime)
        {
            targetAudio = target;
        }

        public SoundVolume To(float value)
        {
            to = value;
            return this;
        }

        public SoundVolume From(float value)
        {
            from = value;
            return this;
        }

        public SoundVolume Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        protected override void Apply()
        {
            if (!to.HasValue)
            {
                to = targetAudio.volume;
            }
            if (!from.HasValue)
            {
                from = targetAudio.volume;
            }

            targetAudio.volume = function.Invoke(from.Value, to.Value, NormalizedElapsedTime);
        }
        public SoundVolume RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() =>
            {
                Halt();
            });

            return this;
        }
    }
}
