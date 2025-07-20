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
        private Vector3 _targetPos;

        private float _timer;
        
        [Inject]
        public void Constructor(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        private void Start()
        {
            _path = new NavMeshPath();
        }

        public void Initialize(IActionResolver actionResolver, Weapon weapon)
        {
            _actionResolver = actionResolver;
            _weapon = weapon;
            Health.OnDied += OnDied;
        }

        private void Update()
        {
            var action = _actionResolver.GetBestAction();
            action?.Execute();
            _timer += Time.deltaTime;
            
            if (_timer >= 0.5f)
            {
                NavMesh.CalculatePath(transform.position,
                    _playerProvider.Player.transform.position,
                    NavMesh.AllAreas, _path);
                _currentCornerIndex = 1;
                _timer = 0;
            }

            var direction = (_targetPos - transform.position);
            var targetDir = _playerProvider.Player.transform.position - transform.position;
            targetDir.y = 0;
            RotateToTargetPosition(targetDir);
            
            if (_path == null || _path.corners.Length == 0)
                return;
            
            if (_currentCornerIndex >= _path.corners.Length)
            {
                PlayerMovement.Move(Vector3.zero);
                return;
            }
            
            _targetPos = _path.corners[_currentCornerIndex];
            MoveToTargetPosition(direction.normalized);
        }

        private void MoveToTargetPosition(Vector3 direction)
        {
            if (Vector3.Distance(transform.position, _targetPos) < 5f)
            {
                // Debug.Log($"Corner Index UP: {_currentCornerIndex}");
                _currentCornerIndex++;
                return;
            }
            var local = transform.InverseTransformDirection(direction);
            PlayerMovement.Move(new(local.x, local.z));
        }

        private void RotateToTargetPosition(Vector3 direction)
        {
            /*float angle = Vector3.SignedAngle(
                direction,
                transform.forward,
                Vector3.up
            );

            Debug.Log(angle);
            PlayerMovement.MoveCamera(angle >= 5 ? new Vector2(angle* 5 * Time.deltaTime, 0) : Vector2.zero);*/
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                40 * Time.deltaTime
            );
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