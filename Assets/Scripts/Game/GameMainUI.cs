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
            ShowTextWithFadeOut(_startText, _fadeDelay, _fadeTime);

            _gameManager.EnemiesLeftAndTotalStream
                .Select(leftAndTotal => $"{leftAndTotal.Item1} enemies left from {leftAndTotal.Item2}!")
                .Subscribe(message =>
                {
                    _enemiesLeftText.text = message;
                    ShowTextWithFadeOut(_enemiesLeftText, _fadeDelay, _fadeTime);
                });

            _gameManager.GameResultStream
                .Select(result => result == GameResult.Win ? _winText : _loseText)
                .Subscribe(resultText => resultText.gameObject.SetActive(true));
        }

        private void ShowTextWithFadeOut(Text text, float fadeDelay, float fadeTime)
        {
            text.gameObject.SetActive(true);
            StartCoroutine(FadeOutCoroutine(text, fadeDelay, fadeTime));
        }

        private IEnumerator FadeOutCoroutine(Text text, float fadeDelay, float fadeTime)
        {
            var color = text.color;

            var startColor = new Color(color.r, color.g, color.b, 1);
            var endColor = new Color(color.r, color.g, color.b, 0);

            text.color = startColor;
            
            yield return new WaitForSeconds(fadeDelay);
            
            var startTime = Time.time;
            var endTime = startTime + fadeTime;

            while (Time.time<endTime)
            {
                text.color = Color.Lerp(startColor, endColor, (Time.time - startTime) / fadeTime);
                yield return null;
            }
            
            text.gameObject.SetActive(false);
        }
    }
}