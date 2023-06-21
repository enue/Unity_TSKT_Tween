#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace TSKT.Tweens
{
    public abstract class Task : ITask
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
        public float Duration { get; private set; }
        readonly bool scaledTime;
        UniTaskCompletionSource<FinishType>? completion;
        public bool Halted { get; private set; }

        public Task(GameObject target, float duration, bool scaledTime)
        {
            UnityEngine.Assertions.Assert.IsTrue(target.activeInHierarchy, "game object must be active : " + target.name);
            Duration = duration;
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

            Update().Forget();
        }

        async UniTask Update()
        {
            while (true)
            {
                await Cysharp.Threading.Tasks.UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

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

                if (ElapsedTime >= Duration)
                {
                    completion?.TrySetResult(FinishType.Completed);
                    break;
                }
            }
        }

        protected abstract void Apply();

        public bool Finished
        {
            get
            {
                return Halted
                    || (ElapsedTime >= Duration)
                    || !Target
                    || !Target.activeInHierarchy;
            }
        }

        public float NormalizdElapsedTime
        {
            get
            {
                if (Duration == 0f)
                {
                    return 1f;
                }

                return Mathf.Clamp01(ElapsedTime / Duration);
            }
        }

        public float ElapsedTime
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
                completion ??= new UniTaskCompletionSource<FinishType>();
                return completion.Task;
            }
        }

        public Awaitable<FinishType> Awaitable
        {
            get
            {
                return GetAwaitable();

                async Awaitable<FinishType> GetAwaitable()
                {
                    return await UniTask;
                }
            }
        }

        public void Halt()
        {
            Halted = true;
            completion?.TrySetResult(FinishType.Halted);
        }
    }
}
