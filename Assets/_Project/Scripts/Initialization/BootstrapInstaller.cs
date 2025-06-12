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

            #region GUI
            _guiInstance = Instantiate(_guiViewPrefab, transform);
            Container.Bind<GUIView>().FromInstance(_guiInstance).AsSingle();
            _events = new Events()
            {
                canClick = new(),
                onRestart = new(),
                onBarClear = new(),
                onWinLevel = new(),
                onLoseLevel = new(),
            };
            Container.Bind<Events>().FromInstance(_events).AsSingle();
            //if more logic need, transfer region to some GUIContorller
            _guiInstance.figureContainer.Construct(_events);
            _guiInstance.restartBtn.onClick.AddListener(() => _events.onRestart?.Notify());
            _disposables.Add(_events.onWinLevel.SubscribeWithSkip(() => { _guiInstance.winPanel.gameObject.SetActive(true); _events.canClick.Value = false; _guiInstance.restartBtn.gameObject.SetActive(false); }));
            _disposables.Add(_events.onLoseLevel.SubscribeWithSkip(() => { _guiInstance.losePanel.gameObject.SetActive(true); _events.canClick.Value = false; _guiInstance.restartBtn.gameObject.SetActive(false); }));
            _guiInstance.winPanel.closeBtn.onClick.AddListener(() => { _events.onRestart?.Notify(); _guiInstance.winPanel.gameObject.SetActive(false); _events.canClick.Value = true; _guiInstance.restartBtn.gameObject.SetActive(true); });
            _guiInstance.losePanel.closeBtn.onClick.AddListener(() => { _events.onRestart?.Notify(); _guiInstance.losePanel.gameObject.SetActive(false); _events.canClick.Value = true; _guiInstance.restartBtn.gameObject.SetActive(true); });
            #endregion
        }

        private void OnDestroy()
        {
            _guiInstance.restartBtn.onClick.RemoveAllListeners();
            GameObject.Destroy(_guiInstance.gameObject);

            for (int i = _disposables.Count - 1; i >= 0; i--)
                _disposables[i]?.Dispose();
            _disposables.Clear();
        }
    }
}