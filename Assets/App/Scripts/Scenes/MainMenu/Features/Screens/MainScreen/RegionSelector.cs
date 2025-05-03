using System;
using App.Scripts.Modules.Localization;
using UnityEngine;
using Photon.Pun;
using TMPro;
using YG;
using Zenject;

public class RegionDropdownHandler : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _regionDropdown;

    private readonly string[] _regionCodes = new string[] { "ru", "eu", "us", "asia" };
    private ILocalizationSystem _localizationSystem;

    [Inject]
    public void Constructor(ILocalizationSystem localizationSystem)
    {
        _localizationSystem = localizationSystem;
    }
    
    public void Initialize()
    {
        SetupDropdown();
        _localizationSystem.OnLanguageChanged += SetupDropdown;
        _regionDropdown.onValueChanged.AddListener(OnRegionSelected);
    }

    private void SetupDropdown()
    {
        _regionDropdown.ClearOptions();

        foreach (var region in _regionCodes)
        {
            _regionDropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedRegionName(region)));
        }
        
        SetInitialRegion();
    }

    private string GetLocalizedRegionName(string regionCode)
    {
        if (_localizationSystem.Language == "ru")
        {
            switch (regionCode)
            {
                case "ru": return "Россия";
                case "eu": return "Европа";
                case "us": return "США";
                case "asia": return "Азия";
                default:
                    OnRegionSelected(1);
                    return "Европа";
            }
        }
        else
        {
            switch (regionCode)
            {
                case "ru": return "Russia";
                case "eu": return "Europe";
                case "us": return "USA";
                case "asia": return "Asia";
                default:
                    OnRegionSelected(1);
                    return "Europe";
            }
        }
    }

    private void SetInitialRegion()
    {
        if (PhotonNetwork.CloudRegion != null)
        {
            for (int i = 0; i < _regionCodes.Length; i++)
            {
                if (_regionCodes[i].Equals(PhotonNetwork.CloudRegion, System.StringComparison.OrdinalIgnoreCase))
                {
                    _regionDropdown.SetValueWithoutNotify(i);
                    _regionDropdown.captionText.text = GetLocalizedRegionName(_regionCodes[i]);
                    return;
                }
            }
        }

        _regionDropdown.value = 1;
        _regionDropdown.captionText.text = GetLocalizedRegionName(_regionCodes[1]);
    }

    private void OnRegionSelected(int index)
    {
        if (index < 0 || index >= _regionCodes.Length) return;

        string selectedRegion = _regionCodes[index];
        Debug.Log($"Selected region: {selectedRegion}");

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.ConnectToRegion(selectedRegion);
        }
        else
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = selectedRegion;
        }
        
        YG2.saves.Region = selectedRegion;
        YG2.SaveProgress();
    }

    private void OnDestroy()
    {
        _localizationSystem.OnLanguageChanged -= SetupDropdown;
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public string Region = String.Empty;
    }
}