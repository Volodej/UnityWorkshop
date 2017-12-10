using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameMainUI : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Text _startText;
        [SerializeField] private Text _enemiesLeftText;
        [SerializeField] private Text _winText;
        [SerializeField] private Text _loseText;

        [SerializeField, Space] private float _fadeDelay = 3;
        [SerializeField] private float _fadeTime = 1;

        private void Start()
        {
        }

        private void ShowTextWithFadeOut(Text text, float fadeDelay, float fadeTime)
        {
        }

        private IEnumerator FadeOutCoroutine(Text text, float fadeDelay, float fadeTime)
        {
            throw new NotImplementedException();
        }
    }
}