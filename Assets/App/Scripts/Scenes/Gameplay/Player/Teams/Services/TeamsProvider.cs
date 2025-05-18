using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.Gameplay.Player.Configs;
using App.Scripts.Scenes.Gameplay.Player.Teams.Configs;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player.Teams.Services
{
    public class TeamsProvider : MonoBehaviourPun
    {
        [SerializeField] private TeamsConfig _teamsConfig;

        private Dictionary<string, List<Player>> _teams;

        private void Awake()
        {
            _teams = new Dictionary<string, List<Player>>();
            foreach (var config in _teamsConfig.TeamConfigs)
            {
                _teams[config.Name] = new List<Player>();
            }
        }

        public void RPCChangeTeam(Player player, string teamName)
        {
            photonView.RPC(nameof(ChangeTeam), RpcTarget.All, player.photonView.ViewID, teamName);
        }

        public void ChangeTeam(int viewId, string teamName)
        {
            var player = GetPlayerByViewId(viewId);
            if (player == null) return;

            var newTeam = GetTeamConfigByName(teamName);
            if (newTeam == null) return;

            RemovePlayerFromCurrentTeam(player);

            if (player.Team != newTeam)
            {
                player.SetTeam(newTeam);
            }

            AddPlayerToTeam(player, newTeam);
        }

        public List<Player> GetTeamPlayers(string teamName)
        {
            return _teams.TryGetValue(teamName, out var players) ? players : new List<Player>();
        }
        
        private Player GetPlayerByViewId(int viewId)
        {
            var photonView = PhotonNetwork.GetPhotonView(viewId);
            return photonView != null ? photonView.GetComponent<Player>() : null;
        }

        private TeamConfig GetTeamConfigByName(string teamName)
        {
            return _teamsConfig.TeamConfigs.FirstOrDefault(t => t.Name == teamName);
        }

        private void RemovePlayerFromCurrentTeam(Player player)
        {
            if (player.Team != null && _teams.TryGetValue(player.Team.Name, out var teamList))
            {
                teamList.Remove(player);
            }
        }

        private void AddPlayerToTeam(Player player, TeamConfig team)
        {
            if (!_teams.ContainsKey(team.Name))
            {
                _teams[team.Name] = new List<Player>();
            }

            _teams[team.Name].Add(player);
        }

    }
}
