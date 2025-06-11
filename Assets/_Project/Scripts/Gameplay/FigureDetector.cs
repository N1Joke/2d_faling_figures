using Core;
using GUI;
using Tools.Extensions;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class FigureDetector : BaseDisposable
    {
        private Camera _camera;
        private IFigureContainer _figureContainer;        

        public FigureDetector(IFigureContainer figureContainer)
        {
            _figureContainer = figureContainer;
            _camera = Camera.main;

            AddDispose(ReactiveExtensions.StartUpdate(Update));
        }

        private void Update()
        {
            if (!Application.isMobilePlatform)
            {
                if (Input.GetMouseButtonDown(0))
                    TryClickFigure(Input.mousePosition);
            }
            else if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPos = touch.position;

                if (touch.phase == TouchPhase.Began)
                    TryClickFigure(touchPos);
            }
        }

        private void TryClickFigure(Vector2 screenPos)
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(screenPos);
            Vector2 point = new Vector2(worldPos.x, worldPos.y);

            Collider2D hit = Physics2D.OverlapPoint(point);

            if (hit != null && hit.gameObject.TryGetComponent(out FigureView figureView) )
            {
                if (_figureContainer.CanAddFigure && _figureContainer.AddFigure(figureView) == FigureAddResult.Success)
                    GameObject.Destroy(figureView.gameObject);
            }
        }
    }
}
