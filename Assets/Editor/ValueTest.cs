#nullable enable
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using NUnit.Framework;
using TSKT;
using UnityEngine.TestTools;

namespace TSKT
{
    public class ValueTest
    {
        [UnityTest]
        public IEnumerator Test()
        {
            var value = Tween.Value(1f, false);
            yield return value.UniTask.ToCoroutine();
            Assert.That(value.Finished);
        }

        [UnityTest]
        public IEnumerator CancelTest()
        {
            var source = new CancellationTokenSource();
            var value = Tween.Value(1f, false)
                .RegisterCancellationToken(source.Token);
            yield return null;
            source.Cancel();

            yield return value.UniTask.ToCoroutine();
            Assert.That(value.Finished);
        }

        [UnityTest]
        public IEnumerator AwaitableTest()
        {
            var value = Tween.Value(1f, false);

            var v = value.Awaitable.GetAwaiter();
            while (!v.IsCompleted)
            {
                yield return null;
            }
            Assert.That(value.Finished);
        }
    }
}