using System;
using UnityEngine;

namespace App.Scripts.Features.Input
{
    public interface IGameInputProvider
    {
        event Action<int> OnNumber;
        event Action OnPause;
        event Action OnEnter;
        event Action OnR;
        event Action OnE;
        event Action OnTabPerformed;
        event Action OnTabCanceled;
        event Action OnLeftMouseStarted;
        event Action OnLeftMouseCanceled;
        event Action OnRightMouseStarted;
        event Action OnRightMouseCanceled;
        event Action OnSpace;
        event Action<float> OnScrollWheel;
        Vector2 GetMovementNormalized();
        Vector2 GetMouseLook();
    }
}