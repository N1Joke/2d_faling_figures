using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class GUIView : MonoBehaviour
    {
        [field: SerializeField] public FigureContainer figureContainer { get; private set; }
        [field: SerializeField] public Button restartBtn { get; private set; }
        [field: SerializeField] public Panel winPanel { get; private set; }
        [field: SerializeField] public Panel losePanel { get; private set; }
    }
}