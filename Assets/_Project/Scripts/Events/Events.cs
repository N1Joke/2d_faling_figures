using Tools.Extensions;
using UniRx;

namespace Assets._Project.Scripts.Events
{
    public struct Events
    {
        public ReactiveProperty<bool> canClick;
        public ReactiveEvent onRestart;
        public ReactiveEvent onBarClear;
        public ReactiveEvent onWinLevel;
    }
}
