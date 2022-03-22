using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
#nullable enable

namespace TSKT.Tweens
{
    public interface ITask
    {
        bool Finished { get; }
        void Halt();
        bool Halted { get; }
    }
}
