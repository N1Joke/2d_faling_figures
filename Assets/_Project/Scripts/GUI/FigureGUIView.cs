using Assets._Project.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.GUI
{
    public class FigureGUIView : MonoBehaviour
    {
        [field: SerializeField] public Image animal { get; private set; }
        [field: SerializeField] public Image shape { get; private set; }

        private FigureConfig _config;
        public FigureConfig Config => _config;

        public void Setup(FigureConfig figureConfig, Sprite shape, Sprite animal, Color color)
        {
            _config = figureConfig; 
            this.animal.sprite = animal;
            this.shape.sprite = shape;
            this.shape.color = color;
        }
    }
}
