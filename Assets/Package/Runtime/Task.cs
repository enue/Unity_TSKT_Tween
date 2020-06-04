using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace TSKT.Tweens
{
    public abstract class Task
    {
        public enum FinishType
        {
            Completed,
            Halted,
            DisbaledGameObject,
            DestroyedGameObject,
        }

        protected GameObject Target { get; private set; }
        readonly float startedTime;
        readonly float duration;
        readonly bool scaledTime;
        UniTaskCompletionSource<FinishType> completion;
        public bool Halted { get; private set; }

        public Task(GameObject target, float duration, bool scaledTime)
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

            Cysharp.Threading.Tasks.UniTask.DelayFrame(0, PlayerLoopTiming.PostLateUpdate)
                .ContinueWith(() => Update())
                .Forget();
        }

        async UniTask Update()
        {
            while (true)
            {
                if (Halted)
                {
                    completion?.TrySetResult(FinishType.Halted);
                    break;
                }
                if (!Target)
                {
                    completion?.TrySetResult(FinishType.DestroyedGameObject);
                    break;
                }
                if (!Target.activeInHierarchy)
                {
                    completion?.TrySetResult(FinishType.DisbaledGameObject);
                    break;
                }

                Apply();

                if (ElapsedTime >= duration)
                {
                    completion?.TrySetResult(FinishType.Completed);
                    break;
                }
                await Cysharp.Threading.Tasks.UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
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


        public UniTask<FinishType> UniTask
        {
            get
            {
                if (completion == null)
                {
                    completion = new UniTaskCompletionSource<FinishType>();
                }
                return completion.Task;
            }
        }

        public void Halt()
        {
            Halted = true;
            completion?.TrySetResult(FinishType.Halted);
        }
    }
}
