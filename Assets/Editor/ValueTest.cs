#nullable enable
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TSKT;
using UnityEngine;

namespace TSKT
{
    public class ValueTest
    {
        [Test]
        public async Task Test()
        {
            var value = Tween.Value(1f, false);
            await value.Awaitable;
            Assert.That(value.Finished);
            Assert.IsFalse(value.Halted);
        }

        [Test]
        public void CancelTest()
        {
            var source = new CancellationTokenSource();
            var value = Tween.Value(1f, false)
                .RegisterCancellationToken(source.Token);
            source.Cancel();
            Assert.That(value.Finished);
            Assert.IsTrue(value.Halted);
        }
    }
}