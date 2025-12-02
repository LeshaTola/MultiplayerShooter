using System.Collections.Generic;
using System.Linq;
#if Game_Push
using GamePush;
#endif
#if RUSTORE
using RuStore.PayClient;
#endif
using UnityEngine;

namespace App.Scripts.Modules.OtherPlatforms
{
    public class IsPurchaseAvailable : MonoBehaviour
    {
#if Game_Push 
        [SerializeField] private List<Platform> _notWorkingPlatforms;
#endif

        [SerializeField] private GameObject _closedPanel;

        private void Awake()
        {
#if Game_Push
            if (GP_Payments.IsPaymentsAvailable() && _notWorkingPlatforms.All(x => GP_Platform.Type() != x))
            {
                return;
            }
            
            ClosePanel();
#elif RUSTORE
            
            RuStorePayClient.Instance.GetPurchaseAvailability(
                onFailure: (error) =>
                {
                    Debug.LogError(error.description);
                    ClosePanel();
                },
                onSuccess: (result) =>
                {
                    if (!result.isAvailable)
                    {
                        ClosePanel();
                    }
                });
#else
            return;
#endif

        }
        
        private void ClosePanel()
        {
            gameObject.SetActive(false);
            if (_closedPanel)
                _closedPanel.SetActive(true);
        }

    }
    
}