using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPointsController : MonoBehaviour {
    [SerializeField]
    Color _activeHealthPointsColor;
    [SerializeField]
    Color _inactiveHealthPointsColor;

    [SerializeField]
    Image _healthPoint0;
    [SerializeField]
    Image _healthPoint1;
    [SerializeField]
    Image _healthPoint2;
    [SerializeField]
    Image _healthPoint3;
    [SerializeField]
    Image _healthPoint4;

    public void SetMaxHealthPoints(int maxHealthPoints) {
        _healthPoint0.enabled = maxHealthPoints >= 1;
        _healthPoint1.enabled = maxHealthPoints >= 2;
        _healthPoint2.enabled = maxHealthPoints >= 3;
        _healthPoint3.enabled = maxHealthPoints >= 4;
        _healthPoint4.enabled = maxHealthPoints >= 5;
    }

    public void SetHealthPoints(int healthPoints) {
        Debug.Log($"healthPoints: {healthPoints}");
        
        _healthPoint0.color = (healthPoints >= 1) ? _activeHealthPointsColor : _inactiveHealthPointsColor;
        _healthPoint1.color = (healthPoints >= 2) ? _activeHealthPointsColor : _inactiveHealthPointsColor;
        _healthPoint2.color = (healthPoints >= 3) ? _activeHealthPointsColor : _inactiveHealthPointsColor;
        _healthPoint3.color = (healthPoints >= 4) ? _activeHealthPointsColor : _inactiveHealthPointsColor;
        _healthPoint4.color = (healthPoints >= 5) ? _activeHealthPointsColor : _inactiveHealthPointsColor;
    }
}
