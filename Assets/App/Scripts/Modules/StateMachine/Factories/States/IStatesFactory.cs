using System;
using App.Scripts.Modules.StateMachine.States.General;

namespace App.Scripts.Modules.StateMachine.Factories.States
{
    public interface IStatesFactory
    {
        State GetState(Type type);
    }
}