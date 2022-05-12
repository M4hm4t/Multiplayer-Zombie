using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
namespace Code
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _enemyKillsText;

        public static GameManager Instance = null;
        public int EnemyAmount { get; set; }

        public int EnemyDeadCounter
        {
            get => _enemyDeadCounter;
            set
            {
                _enemyDeadCounter = value;
                MatchManager.Instance.SetDisplay(SuccessValue);
            }
        }

        private int _enemyDeadCounter;

        private float SuccessValue => EnemyDeadCounter / (float)EnemyAmount;

        [SerializeField] private BarController barController;


        void Awake()
        {

            if (Instance == null)
            {
                Instance = this;
            }

            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            // Dont destroy on reloading the scene
            DontDestroyOnLoad(gameObject);


        }
        public void EnemyKilled(int enemyKillCount)
        {

            _enemyKillsText.text = "kills: " + enemyKillCount;
        }

        public PlayerController Player;
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.2f);
            MatchManager.Instance.SetDisplay(SuccessValue);
        }

        public void GameOver()
        {
            Debug.Log("Game Over");
            //Time.timeScale = 0;
        }


        public void Win()
        {
            Debug.Log("Win");
            Time.timeScale = 1;
            var current = FindObjectsOfType<EnemyController>().Length;
            var result = current / (float)EnemyAmount;
            var success = Mathf.Lerp(100, 0, result);
        }

        public void SetEnemyKillText(TextMeshProUGUI text)
        {

            _enemyKillsText = text;
        }
    }
}
