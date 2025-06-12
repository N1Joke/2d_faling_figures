using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;
using Assets._Project.Scripts.Presets;
using Presets;
using Assets._Project.Scripts.Gameplay;
using GUI;
using Assets._Project.Scripts.Events;

namespace MonoInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Transform _parent;

        private List<IDisposable> _disposables = new List<IDisposable>();
        [Inject]
        private ItemConfig _itemConfig;
        [Inject]
        private GameSettings _gameSettings;
        [Inject]
        private GUIView _guiView;
        [Inject]
        private Events _events;

        public override void InstallBindings() { }

        private void Awake()
        {
            _disposables.Add(new FigureSpawner(new FigureSpawner.Ctx
            {
                config = _itemConfig,
                gameSettings = _gameSettings,
                parent = _parent,
                spawnPoints = _spawnPoints,
                onRestart = _events.onRestart,
                onBarClear = _events.onBarClear,
                onWinLevel = _events.onWinLevel,
            }));
            _disposables.Add(new FigureDetector(_guiView.figureContainer));            
        }

        private void OnDestroy()
        {
            for (int i = _disposables.Count - 1; i >= 0; i--)
                _disposables[i]?.Dispose();
            _disposables.Clear();
        }
    }
}