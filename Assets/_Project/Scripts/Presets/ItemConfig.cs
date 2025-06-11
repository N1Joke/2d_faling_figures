using Assets._Project.Scripts.Gameplay;
using GUI;
using UnityEngine;

namespace Assets._Project.Scripts.Presets
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "ScriptableObjects/ItemConfig", order = 1)]
    public class ItemConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite[] animalSprites { get; private set; }
        [field: SerializeField] public Color[] colors { get; private set; }
        [field: SerializeField] public FigureView[] shapes { get; private set; }
    }
}
