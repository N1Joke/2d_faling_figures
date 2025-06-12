using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public abstract class FigureSpecialBehavior : MonoBehaviour
    {
        protected FigureView figureView;

        protected virtual void Start()
        {
            figureView = GetComponent<FigureView>();
        }
    }
}
