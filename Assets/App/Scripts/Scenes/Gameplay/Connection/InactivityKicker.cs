using App.Scripts.Modules.StateMachine;
using App.Scripts.Scenes.Gameplay.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Zenject;

public class InactivityKicker : MonoBehaviour
{
    [SerializeField] private float _inactivityTimeout = 60f;
    
    private float _currentTimer;
    private bool _isPlayerActive = true;
    private StateMachine _stateMachine;

    [Inject]
    public void Construct(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    
    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            ResetTimer();
        }

        _currentTimer -= Time.deltaTime;

        if (_currentTimer <= 0f && _isPlayerActive)
        {
            KickForInactivity();
        }
    }

    private void ResetTimer()
    {
        _currentTimer = _inactivityTimeout;
        _isPlayerActive = true;
    }

    private void KickForInactivity()
    {
        _isPlayerActive = false;
        _stateMachine.ChangeState<LeaveMatch>().Forget();
    }
}