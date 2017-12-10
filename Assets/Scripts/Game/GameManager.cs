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
        }

        private void ReloadLevel()
        {
        }
    }
}