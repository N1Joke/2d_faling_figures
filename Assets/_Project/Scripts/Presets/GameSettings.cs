using UnityEngine;

namespace Presets
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public int countFigures = 15;
        [field: SerializeField] public int maxVariants = 27;
    }
}