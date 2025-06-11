using Assets._Project.Scripts.Events;
using Assets._Project.Scripts.Presets;
using GUI;
using Presets;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MonoInstallers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [Header("Prefabs")]
        [SerializeField] private GUIView _guiViewPrefab;
        [Header("Presets")]
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private ItemConfig _itemConfig;

        private List<IDisposable> _disposables = new List<IDisposable>();
        private GUIView _guiInstance;
        private Events _events;

        public override void InstallBindings()
        {
            DontDestroyOnLoad(this);

            Container.Bind<ItemConfig>().FromScriptableObject(_itemConfig).AsSingle();
            Container.Bind<GameSettings>().FromScriptableObject(_gameSettings).AsSingle();

            _guiInstance = Instantiate(_guiViewPrefab, transform);
            Container.Bind<GUIView>().FromInstance(_guiInstance).AsSingle();

            _events = new Events()
            {
                canClick = new(),
                onRestart = new(),
                onBarClear = new(),
                onWinLevel = new(),
            };
            Container.Bind<Events>().FromInstance(_events).AsSingle();
        }

        private void OnDestroy()
        {
            GameObject.Destroy(_guiInstance.gameObject);

            for (int i = _disposables.Count - 1; i >= 0; i--)
                _disposables[i]?.Dispose();
            _disposables.Clear();
        }
    }
}