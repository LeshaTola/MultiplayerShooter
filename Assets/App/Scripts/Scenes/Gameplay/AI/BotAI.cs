using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using Module.AI.Resolver;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.AI
{
    public class BotAI : MonoBehaviourPun
    {
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Transform WeaponHolder { get; private set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }

        private Weapon _weapon;
        private IActionResolver _actionResolver;

        private PlayerProvider _playerProvider;
        private NavMeshPath _path;
        private int _currentCornerIndex;

        [Inject]
        public void Constructor(PlayerProvider playerProvider)
        {
            Debug.Log("Injected");
            _playerProvider = playerProvider;
        }

        private void Start()
        {
            Debug.Log("Bot Activated");
            _path = new NavMeshPath();
        }

        /*public void Initialize(IActionResolver actionResolver, Weapon weapon)
        {
            _actionResolver = actionResolver;
            _weapon = weapon;
            Health.OnDied += OnDied;
        }*/

        private void Update()
        {
            /*var action = _actionResolver.GetBestAction();
            action?.Execute();*/
            if (Input.GetKeyDown(KeyCode.G))
            {
                NavMesh.CalculatePath(transform.position,
                    _playerProvider.Player.transform.position,
                    NavMesh.AllAreas, _path);
                Debug.Log(_path.corners.Length);
                _currentCornerIndex = 0;
            }
            
            if (_path == null || _path.corners.Length == 0 || _currentCornerIndex >= _path.corners.Length)
                return;

            var targetPos = _path.corners[_currentCornerIndex];
            var direction = (targetPos - transform.position).normalized;
            
            if (Vector3.Distance(transform.position, targetPos) < 0.2f)
            {
                _currentCornerIndex++;
                return;
            }
            
            float angle = Vector3.SignedAngle(
                transform.forward,                            // текущее направление
                direction,         // направление к цели
                Vector3.up                                    // ось поворота (обычно вверх)
            );
            Debug.Log(angle);
            if(angle > 5)
                PlayerMovement.MoveCamera(new Vector2(angle*Time.deltaTime,0));
            else
            {
                PlayerMovement.MoveCamera(Vector2.zero);
                PlayerMovement.Move(Vector3.forward);
            }
        }

        private void OnDied()
        {
            PhotonNetwork.Destroy(gameObject);
            Health.OnDied -= OnDied;
        }
        
        private void OnDrawGizmos()
        {
            if (_path == null || _path.corners.Length < 2)
                return;

            Gizmos.color = Color.green;
            for (int i = 0; i < _path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(_path.corners[i], _path.corners[i + 1]);
                Gizmos.DrawSphere(_path.corners[i], 0.1f);
            }

            // Последняя точка
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_path.corners[^1], 0.15f);
        }
    }
}