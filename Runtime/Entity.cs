using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;

namespace TSKT.Tweens
{
    public abstract class Entity
    {
        protected GameObject Target { get; private set; }
        readonly float startedTime;
        readonly float duration;
        readonly bool scaledTime;
        UniTaskCompletionSource completion;
        public bool Halted { get; private set; }

        public Entity(GameObject target, float duration, bool scaledTime)
        {
            this.duration = duration;
            this.scaledTime = scaledTime;
            Target = target;

            if (scaledTime)
            {
                startedTime = Time.time;
            }
            else
            {
                startedTime = Time.realtimeSinceStartup;
            }

            UniTask.DelayFrame(0, PlayerLoopTiming.PostLateUpdate)
                .ContinueWith(_ => Update())
                .Forget();
        }

        async UniTask Update()
        {
            while (true)
            {
                if (!Halted && Target)
                {
                    Apply();
                }

                if (Finished)
                {
                    break;
                }
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            completion?.TrySetResult();
        }

        abstract protected void Apply();

        public bool Finished
        {
            get
            {
                return Halted
                    || (ElapsedTime >= duration)
                    || !Target
                    || !Target.activeInHierarchy;
            }
        }

        protected float NormalizdElapsedTime
        {
            get
            {
                if (duration == 0f)
                {
                    return 1f;
                }

                return Mathf.Clamp01(ElapsedTime / duration);
            }
        }

        float ElapsedTime
        {
            get
            {
                if (scaledTime)
                {
                    return Time.time - startedTime;
                }
                else
                {
                    return Time.realtimeSinceStartup - startedTime;
                }
            }
        }


        public UniTask UniTask
        {
            get
            {
                if (completion == null)
                {
                    completion = new UniTaskCompletionSource();
                }
                return completion.Task;
            }
        }

        public void Halt()
        {
            Halted = true;
        }
    }
}
