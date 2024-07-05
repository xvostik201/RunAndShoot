using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    [Serializable]
    struct WayPointsAndBots
    {
        public EnemyBot[] EnemyBotsOnPoint;
        public Transform WayPoint;
        public bool[] IsEnemyDestroyed;
    }

    [SerializeField] private WayPointsAndBots[] _wayPointsAndBots;

    private Player _player;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        for (int i = 0; i < _wayPointsAndBots.Length; i++)
        {
            _wayPointsAndBots[i].IsEnemyDestroyed = new bool[_wayPointsAndBots[i].EnemyBotsOnPoint.Length];
        }
    }

    public Transform[] WayPoints()
    {
        Transform[] allWayPoints = new Transform[_wayPointsAndBots.Length];
        for (int i = 0; i < _wayPointsAndBots.Length; i++)
        {
            allWayPoints[i] = _wayPointsAndBots[i].WayPoint;
        }
        return allWayPoints;
    }

    public void SomeoneBotIsDead(EnemyBot bot)
    {
        for (int i = 0; i < _wayPointsAndBots.Length; i++)
        {
            for (int k = 0; k < _wayPointsAndBots[i].EnemyBotsOnPoint.Length; k++)
            {
                if (_wayPointsAndBots[i].EnemyBotsOnPoint[k] == bot)
                {
                    _wayPointsAndBots[i].IsEnemyDestroyed[k] = true;
                }
            }
        }
        CheckAllBotsInThePoint(bot);
    }

    private void CheckAllBotsInThePoint(EnemyBot bot)
    {
        for (int i = 0; i < _wayPointsAndBots.Length; i++)
        {
            for (int k = 0; k < _wayPointsAndBots[i].EnemyBotsOnPoint.Length; k++)
            {
                if (_wayPointsAndBots[i].EnemyBotsOnPoint[k] == bot)
                {
                    Debug.Log(_wayPointsAndBots[i].WayPoint);
                    bool allEnemiesDestroyed = true;
                    foreach (bool isEnemyDestroyed in _wayPointsAndBots[i].IsEnemyDestroyed)
                    {
                        if (!isEnemyDestroyed)
                        {
                            allEnemiesDestroyed = false;
                            break;
                        }
                    }
                    if (allEnemiesDestroyed)
                    {
                        if (i == _wayPointsAndBots.Length - 1)
                        {
                           GameManager.Instance.RestartLevel(true);
                        }
                        else
                        {
                            _player.RunToNextPoint();
                        }
                    }
                    return;
                }
            }
        }
    }

    public void HideSliderByWayPoint(Transform wayPointTransform)
    {
        for (int i = 0; i < _wayPointsAndBots.Length; i++)
        {
            for (int k = 0; k < _wayPointsAndBots[i].EnemyBotsOnPoint.Length; k++)
            {
                if (_wayPointsAndBots[i].WayPoint == wayPointTransform)
                {
                    var slider = _wayPointsAndBots[i].EnemyBotsOnPoint[k].GetSlider();
                    if (slider != null)
                    {
                        slider.gameObject.SetActive(true);
                    }
                    _wayPointsAndBots[i].EnemyBotsOnPoint[k].enabled = true;

                }
                else
                {
                    var slider = _wayPointsAndBots[i].EnemyBotsOnPoint[k].GetSlider();
                    if (slider != null)
                    {
                        slider.gameObject.SetActive(false);
                    }
                    _wayPointsAndBots[i].EnemyBotsOnPoint[k].enabled = false;

                }
            }
        }
    }
}
