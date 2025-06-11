using UnityEngine;

namespace GUI
{
    public class GUIView : MonoBehaviour
    {
        [field: SerializeField] public FigureContainer figureContainer { get; private set; }
    }
}