using System;

namespace Interfaces
{
    public interface IToggle
    {
        event Action<bool> OnToggle;
    }
}