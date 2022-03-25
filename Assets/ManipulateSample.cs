using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TSKT
{
    public class ManipulateSample : MonoBehaviour
    {
        [SerializeField]
        GameObject target;

        void Start()
        {
            var x = new List<float>();
            var y = new List<float>();
            var z = new List<float>();
            var t = new List<float>();
            x.Add(target.transform.position.x);
            y.Add(target.transform.position.y);
            z.Add(target.transform.position.z);
            t.Add(0f);
            for (int i = 0; i < 100; ++i)
            {
                x.Add(Random.Range(-5f, 5f));
                y.Add(Random.Range(-5f, 5f));
                z.Add(Random.Range(-5f, 5f));
                t.Add(i + 1);
            }
            x.Add(target.transform.position.x);
            y.Add(target.transform.position.y);
            z.Add(target.transform.position.z);
            t.Add(t.Count + 1);

            var fx = new Spline(t.Zip(x, (t, x) => (t, x)).ToArray());
            var fy = new Spline(t.Zip(y, (t, y) => (t, y)).ToArray());
            var fz = new Spline(t.Zip(z, (t, z) => (t, z)).ToArray());

            foreach (var it in t)
            {
                var a = it - 0.00001f;
                var b = it + 0.00001f;
                UnityEngine.Assertions.Assert.AreNotEqual(b, a);
                UnityEngine.Assertions.Assert.AreApproximatelyEqual(fx.Velocity(a), fx.Velocity(b), 0.001f);
            }

            Tween.Manipulate(target.transform, duration: (float)t[^1])
                .Action((_time, target) =>
                {
                    var p = new Vector3(
                        fx.Evaluate(_time * fx.Duration),
                        fy.Evaluate(_time * fy.Duration),
                        fz.Evaluate(_time * fz.Duration));
                    target.position = p;
                });
        }

        void Test()
        {
            var s = new Spline(
                (time: 0f, value: 0f),
                (time: 1f, value: 1f),
                (time: 2f, value: 1f),
                (time: 3f, value: 1f),
                (time: 4f, value: 1f),
                (time: 5f, value: 0f));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(s.Evaluate(1f), 1f);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(s.Velocity(1f), s.Velocity(1f - Mathf.Epsilon));
        }
    }
}