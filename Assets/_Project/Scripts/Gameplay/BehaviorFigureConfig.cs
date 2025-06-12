using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    [System.Serializable]
    public class BehaviorFigureConfig
    {
        public FigureType figureType = FigureType.Normal;
        public float heavyMultiplier = 2f;
        public float stickyRange = 1.5f;
        public float stickyForce = 10f;
        public Sprite icon;
    }
}
