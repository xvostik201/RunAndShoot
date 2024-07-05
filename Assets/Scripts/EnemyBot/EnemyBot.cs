using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBot : MonoBehaviour
{
    [SerializeField] private EnemyBotData _enemyBotData;

    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillImage;

    [SerializeField] private AudioClip _deathClip;

    private Rigidbody[] _allRigidbodies;
    private Animator _animator;

    private int _currentHealth;
    private Vector3 _lastHitDirection;
    private bool _isDead;
    private Level _level;

    private void Awake()
    {
        _level = FindObjectOfType<Level>();
        _animator = GetComponent<Animator>();
        _allRigidbodies = GetComponentsInChildren<Rigidbody>();

    }

    void Start()
    {
        _currentHealth = _enemyBotData.Health;
        _slider.maxValue = _currentHealth;
        _slider.value = _currentHealth;
    }

    void Update()
    {
        _slider.transform.LookAt(Camera.main.transform);
    }

    public void TakeDamage(int damage, Vector3 bulletVector, float deathForce)
    {
        _lastHitDirection = bulletVector.normalized;
        _currentHealth -= damage;
        CheckSliderValue(_currentHealth);

        if (_currentHealth <= 0 && !_isDead)
        {
            ActivateRagdoll(deathForce);
        }
    }

    private void ActivateRagdoll(float force)
    {
        _isDead = true;
        _level.SomeoneBotIsDead(this);
        _animator.enabled = false;
        _slider.gameObject?.SetActive(false);

        AudioManager.Instance.PlaySFX(_deathClip);

        foreach (Rigidbody rb in _allRigidbodies)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(_lastHitDirection * -force, ForceMode.Impulse);
        }
        Destroy(gameObject, _enemyBotData.TimeToDestroy);
    }


    private void CheckSliderValue(int currentHealth)
    {
        _slider.value = currentHealth;
        float orangeValue = _slider.maxValue / 1.5f;
        float redValue = _slider.maxValue / 3f;

        if (currentHealth < orangeValue && currentHealth > redValue)
        {
            _fillImage.color = Color.yellow;
        }
        else if (currentHealth < redValue)
        {
            _fillImage.color = Color.red;
        }
    }

    public Slider GetSlider() { return _slider; }

}
