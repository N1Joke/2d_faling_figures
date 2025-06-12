using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class FigureView : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer animal { get; private set; }
        [field: SerializeField] public SpriteRenderer shape { get; private set; }
        [field: SerializeField] public SpriteRenderer colorShape { get; private set; }
        [field: SerializeField] public SpriteRenderer behaviorIcon { get; private set; }
        [field: SerializeField] private Rigidbody2D _rigidbody;

        private FigureSpecialBehavior _specialBehavior;
        private FigureConfig _figureConfig;
        private BehaviorFigureConfig _behaviorFigureConfig;

        public FigureConfig FigureConfig => _figureConfig;
        public Rigidbody2D Rigidbody => _rigidbody;
        public BehaviorFigureConfig BehaviorFigureConfig => _behaviorFigureConfig;

        public void SetConfig(BehaviorFigureConfig behaviorFigureConfig)
        {
            _behaviorFigureConfig = behaviorFigureConfig;
            ApplySpecialProperties();
        }

        public void Setup(FigureConfig config, Sprite animalSprite, Color color)
        {
            _figureConfig = config;
            animal.sprite = animalSprite;
            colorShape.color = color;            
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _specialBehavior = GetComponent<FigureSpecialBehavior>();
        }

        private void ApplySpecialProperties()
        {
            switch (_behaviorFigureConfig.figureType)
            {
                case FigureType.Heavy:
                    SetupHeavyFigure();
                    break;
                case FigureType.Sticky:
                    SetupStickyFigure();
                    break;
            }
        }

        private void SetupHeavyFigure()
        {
            _rigidbody.mass *= _behaviorFigureConfig.heavyMultiplier;
            colorShape.color = Color.Lerp(colorShape.color, Color.black, 0.3f);
            behaviorIcon.gameObject.SetActive(true);
            behaviorIcon.sprite = _behaviorFigureConfig.icon;
        }

        private void SetupStickyFigure()
        {
            if (_specialBehavior == null)
                _specialBehavior = gameObject.AddComponent<StickyFigureBehavior>();

            var color = colorShape.color;
            color.a = 0.8f;
            colorShape.color = color;
            behaviorIcon.gameObject.SetActive(true);
            behaviorIcon.sprite = _behaviorFigureConfig.icon;
        }
    }
}
