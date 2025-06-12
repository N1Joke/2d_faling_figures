using Assets._Project.Scripts.Events;
using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.GUI;
using Assets.Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static Assets._Project.Scripts.Gameplay.FigureSpawner;

namespace GUI
{
    public class FigureContainer : MonoBehaviour, IFigureContainer
    {
        [SerializeField] private Transform[] _slots;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private FigureGUIView _prefab;

        private List<FigureGUIView> _figures = new List<FigureGUIView>();
        private Events _events;
        private Camera _camera;
        private int _countAnimations;

        public bool CanAddFigure => _countAnimations == 0;

        public void Construct(Events events)
        {
            _events = events;
            _events.onRestart.SubscribeWithSkip(Clear).AddTo(this);
        }

        private void Awake()
        {
            _camera = Camera.main;

            for (int i = 0; i < _slots.Length; i++)            
                _figures.Add(null);            
        }

        public FigureAddResult AddFigure(FigureView figureView)
        {
            int freeSlotIndex = FindFirstFreeSlot();
            if (freeSlotIndex == -1)
            {
                _events.onLoseLevel?.Notify();
                return FigureAddResult.Loose;
            }

            AddFigureToSlot(figureView.FigureConfig, freeSlotIndex, figureView.shape.sprite, figureView.animal.sprite, figureView.shape.color, figureView.transform.position);

            StartCoroutine(WaitAndCheckMatches());           

            return FigureAddResult.Success;
        }

        private IEnumerator WaitAndCheckMatches()
        {
            yield return new WaitUntil(() => CanAddFigure);

            bool foundMatches = RemoveMatches();

            if (foundMatches)
                CompactFigures();

            if (IsContainerEmpty())
                _events.onBarClear?.Notify();
            else
            {
                int freeSlotIndex = FindFirstFreeSlot();
                if (freeSlotIndex == -1)
                    _events.onLoseLevel?.Notify();
            }
        }

        private int FindFirstFreeSlot()
        {
            for (int i = 0; i < _figures.Count; i++)
            {
                if (_figures[i] == null)                
                    return i;                
            }
            return -1;
        }

        private void AddFigureToSlot(FigureConfig config, int slotIndex, Sprite shape, Sprite animal, Color color, Vector3 worldStartPosition)
        {
            FigureGUIView guiView = Instantiate(_prefab, _parent);
            guiView.Setup(config, shape, animal, color);

            Vector3 screenPosition = _camera.WorldToScreenPoint(worldStartPosition);           
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parent,
                screenPosition,
                null,
                out Vector2 canvasPosition
            );

            guiView.transform.localPosition = canvasPosition;

            Vector3 targetPosition = _slots[slotIndex].localPosition;
            _countAnimations++;
            guiView.transform.DOLocalMove(targetPosition, 0.5f)
                .SetEase(Ease.OutQuart)
                .OnComplete(() =>
                {
                    _countAnimations--;
                    guiView.transform.SetParent(_slots[slotIndex]);
                    guiView.transform.localPosition = Vector3.zero;

                    if (_countAnimations == 0)
                        _events.canClick.Value = false;
                });

            _figures[slotIndex] = guiView;
        }

        private bool RemoveMatches()
        {
            bool foundMatches = false;

            for (int i = 0; i < _figures.Count; i++)
            {
                if (_figures[i] == null) 
                    continue;

                FigureConfig baseConfig = _figures[i].Config;
                List<int> matchingIndices = new List<int> { i };

                for (int j = i + 1; j < _figures.Count; j++)
                {
                    if (_figures[j] == null) 
                        continue;

                    if (baseConfig.animalId == _figures[j].Config.animalId &&
                        baseConfig.colorId == _figures[j].Config.colorId &&
                        baseConfig.shapeId == _figures[j].Config.shapeId)
                    {
                        matchingIndices.Add(j);
                    }
                }

                if (matchingIndices.Count >= 3)
                {
                    RemoveFiguresAt(matchingIndices.ToArray());
                    foundMatches = true;                    
                }
            }            

            return foundMatches;
        }

        private void RemoveFiguresAt(params int[] indices)
        {
            Array.Sort(indices);
            Array.Reverse(indices);

            foreach (int index in indices)
            {
                if (index < _figures.Count && _figures[index] != null)
                {
                    FigureGUIView figureGUIView = _figures[index];
                    figureGUIView.transform.DOScale(Vector3.zero, 0.25f).SetLink(figureGUIView.gameObject).OnComplete(() => Destroy(figureGUIView.gameObject));
                    _figures[index] = null;
                }
            }
        }

        private void CompactFigures()
        {
            List<FigureGUIView> compactedFigures = new List<FigureGUIView>();

            for (int i = 0; i < _figures.Count; i++)
            {
                if (_figures[i] != null)
                    compactedFigures.Add(_figures[i]);
            }

            while (compactedFigures.Count < _slots.Length)
                compactedFigures.Add(null);

            _figures = compactedFigures;

            for (int i = 0; i < _figures.Count; i++)
            {
                if (_figures[i] != null)
                {
                    _figures[i].transform.SetParent(_slots[i]);
                    _figures[i].transform.DOLocalMove(Vector3.zero, 0.25f).SetLink(_figures[i].gameObject);
                }
            }
        }

        private bool IsContainerEmpty()
        {
            foreach (var figure in _figures)
            {
                if (figure != null)
                    return false;
            }
            return true;
        }

        public void Clear()
        {
            _figures.Clear();

            for (int i = 0; i < _slots.Length; i++)
            {
                Utils.ClearTransformChilds(_slots[i]);
                _figures.Add(null);
            }
        }
    }
}