using System;
using UnityEngine;

namespace Interfaces
{
    public interface IToggle
    {
        event Action<bool> OnToggle;
        Transform transform { get; }
        void InitialState();
    }
}