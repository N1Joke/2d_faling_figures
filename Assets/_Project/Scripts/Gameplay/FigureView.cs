using Tools.Extensions;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class FigureView : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer animal { get; private set; }
        [field: SerializeField] public SpriteRenderer shape { get; private set; }

        private FigureConfig _figureConfig;
        public FigureConfig FigureConfig => _figureConfig;

        public void Setup(FigureConfig config, Sprite animalSprite, Color color)
        {
            _figureConfig = config;
            animal.sprite = animalSprite;
            shape.color = color;
        }
    }
}
