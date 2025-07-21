using App.Scripts.Modules.AI.Actions;
using App.Scripts.Modules.AI.Factories;
using App.Scripts.Modules.AI.Resolver;
using App.Scripts.Scenes.Gameplay.Effectors;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.AI
{
    public class BotAI : MonoBehaviourPun, IEntity, IEntityMovement
    {
        [field: SerializeField] public Health Health { get; private set; }

        [field: SerializeField] public Transform WeaponHolder { get; private set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }

        public PhotonView PhotonView => photonView;
        public IEntityMovement Movement => this;

        private IActionResolver _actionResolver;

        public Transform Target { get; private set; }
        /*private NavMeshPath _path;
        private float _timer;
        private int _currentCornerIndex;
        private Vector3 _finalTargetPos;*/

        public Weapon Weapon { get; private set; }

        [Inject]
        public void Constructor(IActionResolver actionResolver, IActionsFactory actionsFactory)
        {
            _actionResolver = actionResolver;
            _actionResolver.Init(actionsFactory.GetAllActions());
        }

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                enabled = false;
            }
            /*var context = GetComponent<GameObjectContext>();
            if(context.Container!=null)
                context.Container.Inject(this);*/
        }

        public void Initialize(Weapon weapon)
        {
            Weapon = weapon;
            RPCConnectWeapon(weapon);
            // _path = new NavMeshPath();
            Health.OnDied += OnDied;
        }

        private void Update()
        {
            if (_actionResolver == null)
            {
                return;
            }

            var action = _actionResolver.GetBestAction();
            action?.Execute();
            Debug.Log(action);
            /*HandlePathProviding();
            HandleMovement();*/
        }

        public void SetTarget(Transform target)
        {
            // if (Vector3.Distance(_finalTargetPos, targetPos) <= 5f)
            // {
            //     return;
            // }
            
            Target = target;
            Agent.SetDestination(Target.position);

            /*_finalTargetPos = targetPos;
            Debug.Log($"FinalPos {_finalTargetPos}");*/
            //RecalculatePath();
        }


        /*private void HandlePathProviding()
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.5f)
            {
                RecalculatePath();
            }
        }*/

        /*private void HandleMovement()
        {
            var direction = (_targetPos - transform.position);
            var targetDir = _finalTargetPos - transform.position;
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

        private void RecalculatePath()
        {
            Debug.Log("RecalculatePath");
            NavMesh.CalculatePath(transform.position,
                _finalTargetPos,
                NavMesh.AllAreas, _path);
            _currentCornerIndex = 1;
            _timer = 0;
        }*/

        /*private void MoveToTargetPosition(Vector3 direction)
        {
            if (Vector3.Distance(transform.position, _targetPos) < StopDistance)
            {
                _currentCornerIndex++;
                return;
            }
            var local = transform.InverseTransformDirection(direction);
            PlayerMovement.Move(new(local.x, local.z));
        }

        private void RotateToTargetPosition(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                40 * Time.deltaTime
            );
        }*/

        private void OnDied()
        {
            Health.OnDied -= OnDied;
        }

        public void AddForce(Vector3 force)
        {
            Debug.Log("Пока не работет");
        }

        private void RPCConnectWeapon(Weapon weapon)
        {
            photonView.RPC(nameof(ConnectWeapon), 
                RpcTarget.AllBuffered,
                photonView.ViewID,
                weapon.photonView.ViewID);
        }

        [PunRPC]
        public void ConnectWeapon(int botId, int weaponId)
        {
            var weaponObject = PhotonView.Find(weaponId).gameObject;
            var botAI = PhotonView.Find(botId).GetComponent<BotAI>();
            var weaponTransform = weaponObject.transform;
            weaponTransform.SetParent(botAI.WeaponHolder);
        }
        
        /*public void RPCSetActive(bool active)
        {
            photonView.RPC(nameof(SetActiveBot), RpcTarget.All, active);
        }

        [PunRPC]
        public void SetActiveBot(bool active)
        {
            gameObject.SetActive(active);
        }*/
    }
}