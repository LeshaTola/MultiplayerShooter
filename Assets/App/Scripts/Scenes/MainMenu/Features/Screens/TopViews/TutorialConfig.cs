using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.TopViews
{
    [CreateAssetMenu(fileName = "TutorialConfig", menuName = "Configs/Tutorial/Input")]
    public class TutorialConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite RuTutor { get; private set; }
        [field: SerializeField] public Sprite EnTutor { get; private set; }

        [field: Header("Mobile")] 
        [field: SerializeField] public Sprite RuMobileTutor { get;  private set;}
        [field: SerializeField] public Sprite EnMobileTutor { get; private set; }
    }
}