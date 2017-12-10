using System;
using System.Linq;
using Smooth.Slinq;
using UniRx;
using UnitScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tuple = Smooth.Algebraics.Tuple;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float _timeToRestart = 5f;
        [SerializeField] private string _sceneToReload;
        
        private const string PlayerTag = "Player";

        public UniRx.IObservable<Smooth.Algebraics.Tuple<int, int>> EnemiesLeftAndTotalStream => _enemiesLeftAndTotal;
        public UniRx.IObservable<GameResult> GameResultStream => _gameResult;
        
        private readonly Subject<Smooth.Algebraics.Tuple<int, int>> _enemiesLeftAndTotal = new Subject<Smooth.Algebraics.Tuple<int, int>>();
        private readonly Subject<GameResult> _gameResult = new Subject<GameResult>();
        
        private void Start()
        {
            var allUnits = FindObjectsOfType<UnitHealth>();
            var player = allUnits.First(health => health.gameObject.CompareTag(PlayerTag));
            
            var enemies = allUnits.Slinq()
                .Where(health => !health.gameObject.CompareTag(PlayerTag))
                .ToList();
            var enemiesCount = enemies.Count;

            enemies.Slinq()
                .Select(health => health.HealthPercentageStream)
                .ForEach(observable => observable.Subscribe(_ => { }, () =>
                {
                    var leftEnemies = enemies.Slinq().Where(health => health.IsAlive).Count();
                    _enemiesLeftAndTotal.OnNext(Tuple.Create(leftEnemies, enemiesCount));
                }));

            _enemiesLeftAndTotal.Where(leftAndTotal => leftAndTotal.Item1 == 0)
                .Subscribe(_ => _gameResult.OnNext(GameResult.Win));
            player.HealthPercentageStream.Subscribe(_ => { }, () => _gameResult.OnNext(GameResult.Lose));

            _gameResult.Delay(TimeSpan.FromSeconds(_timeToRestart)).Subscribe(_ => ReloadLevel());
        }

        private void ReloadLevel()
        {
            SceneManager.LoadScene(_sceneToReload);
        }
    }
}