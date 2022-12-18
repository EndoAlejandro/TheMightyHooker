using System.Collections;
using Pooling;
using UnityEngine;

namespace Interfaces
{
    public interface IToggleChild
    {
        IToggle Toggle { get; }
        PoolAfterSeconds ToggleFx { get; }
        bool State { get; }
        void OnToggle(bool value);
    }
}