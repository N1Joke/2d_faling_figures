using Assets._Project.Scripts.Presets;
using Assets.Scripts.Utils;
using Core;
using Presets;
using System.Collections.Generic;
using Tools.Extensions;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class FigureSpawner : BaseDisposable
    {
        public struct Ctx
        {
            public ItemConfig config;
            public GameSettings gameSettings;
            public Transform parent;
            public Transform[] spawnPoints;
            public ReactiveEvent onRestart;
            public ReactiveEvent onBarClear;
            public ReactiveEvent onWinLevel;
        }

        private readonly Ctx _ctx;
        private List<FigureConfig> _uniqueConfigs = new();

        public FigureSpawner(Ctx ctx)
        {
            _ctx = ctx;

            GetAllUniqueCombinations();
            SpawnFigures();

            AddDispose(_ctx.onRestart.SubscribeWithSkip(SpawnFigures));
            AddDispose(_ctx.onBarClear.SubscribeWithSkip(CheckWin));
        }

        private void CheckWin()
        {
            if (_ctx.parent.childCount == 0)
                _ctx.onWinLevel?.Notify();
        }

        private void SpawnFigures()
        {
            Utils.ClearTransformChilds(_ctx.parent);

            int count = _ctx.gameSettings.countFigures;
            int spawnPointCount = _ctx.spawnPoints.Length;
            int rows = Mathf.CeilToInt(count / spawnPointCount);
            int figuresSpawned = 0;
            var randomPosIndexes = GetRandomIndexes();
            int config = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int i = 0; i < _ctx.spawnPoints.Length; i++)
                {
                    if (config > _uniqueConfigs.Count)
                        config = 0;
                    Transform spawnPoint = _ctx.spawnPoints[randomPosIndexes[i]];
                    Vector3 spawnPosition = spawnPoint.position + new Vector3(Random.Range(-0.25f, 0.25f), row * 1.5f);

                    InitializeFigure(spawnPosition, _uniqueConfigs[config]);
                    figuresSpawned++;
                    if (figuresSpawned % 3 == 0)
                        config++;
                    if (figuresSpawned >= _ctx.gameSettings.countFigures)
                        break;
                }
                if (figuresSpawned >= _ctx.gameSettings.countFigures)
                    break;
            }
        }

        private List<int> GetRandomIndexes()
        {
            List<int> randomPosIndexes = new List<int>(_ctx.spawnPoints.Length + 1);
            for (int i = 0; i < _ctx.spawnPoints.Length; i++)
                randomPosIndexes.Add(i);

            randomPosIndexes.Shuffle();

            return randomPosIndexes;
        }

        private void GetAllUniqueCombinations()
        {
            _uniqueConfigs = new List<FigureConfig>();

            for (int colorId = 0; colorId < _ctx.config.colors.Length; colorId++)
            {
                for (int shapeId = 0; shapeId < _ctx.config.shapes.Length; shapeId++)
                {
                    for (int animalId = 0; animalId < _ctx.config.animalSprites.Length; animalId++)
                    {
                        _uniqueConfigs.Add(new FigureConfig
                        {
                            colorId = colorId,
                            shapeId = shapeId,
                            animalId = animalId
                        });
                        if (_uniqueConfigs.Count >= _ctx.gameSettings.maxVariants)
                            _uniqueConfigs.Shuffle();
                    }
                }
            }

            _uniqueConfigs.Shuffle();
        }

        private void AddGroup(List<FigureConfig> configs, int colorId, int shapeId, int animalId)
        {
            for (int i = 0; i < 3; i++)
            {
                configs.Add(new FigureConfig
                {
                    colorId = colorId,
                    shapeId = shapeId,
                    animalId = animalId
                });
            }
        }

        private void InitializeFigure(Vector3 spawnPosition, FigureConfig config)
        {
            FigureView figure = GameObject.Instantiate(_ctx.config.shapes[config.shapeId], spawnPosition, Quaternion.identity, _ctx.parent);
            figure.Setup(
                config,
                _ctx.config.animalSprites[config.animalId],
                _ctx.config.colors[config.colorId]
                );
        }
    }
}
