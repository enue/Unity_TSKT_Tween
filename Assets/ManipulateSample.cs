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
            for (int i = 0; i < 2; ++i)
            {
                x.Add(Random.Range(-5f, 5f));
                y.Add(Random.Range(-5f, 5f));
                z.Add(Random.Range(-5f, 5f));
                t.Add(i + 1);
            }
            x.Add(target.transform.position.x);
            y.Add(target.transform.position.y);
            z.Add(target.transform.position.z);
            t.Add(3);

            var fx = CubicFunction.Solve4Points((t[0], x[0]), (t[1], x[1]), (t[2], x[2]), (t[3], x[3]));
            var fy = CubicFunction.Solve4Points((t[0], y[0]), (t[1], y[1]), (t[2], y[2]), (t[3], y[3]));
            var fz = CubicFunction.Solve4Points((t[0], z[0]), (t[1], z[1]), (t[2], z[2]), (t[3], z[3]));

            Tween.Manipulate(target.transform, duration: (float)t[^1])
                .Action(_ =>
                {
                    var p = new Vector3(
                        fx.Evaluate(_.NormalizedElapsedTime * 3f),
                        fy.Evaluate(_.NormalizedElapsedTime * 3f),
                        fz.Evaluate(_.NormalizedElapsedTime * 3f));
                    _.Target.position = p;
                });
        }
    }
}