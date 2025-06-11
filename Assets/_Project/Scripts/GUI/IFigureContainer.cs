using Assets._Project.Scripts.Gameplay;

namespace GUI
{
    public interface IFigureContainer
    {
        public void Clear();
        public FigureAddResult AddFigure(FigureView figureView);
        public bool CanAddFigure { get; }
    }
}