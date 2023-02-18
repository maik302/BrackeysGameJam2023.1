using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerPointsController : MonoBehaviour {
    [SerializeField]
    Color _activePowerPointsColor;
    [SerializeField]
    Color _inactivePowerPointsColor;

    [SerializeField]
    Image _powerPoint0;
    [SerializeField]
    Image _powerPoint1;
    [SerializeField]
    Image _powerPoint2;
    [SerializeField]
    Image _powerPoint3;
    [SerializeField]
    Image _powerPoint4;

    public void SetMaxPowerPointsAllowed(int maxPowerPoints) {
        _powerPoint0.enabled = maxPowerPoints >= 1;
        _powerPoint1.enabled = maxPowerPoints >= 2;
        _powerPoint2.enabled = maxPowerPoints >= 3;
        _powerPoint3.enabled = maxPowerPoints >= 4;
        _powerPoint4.enabled = maxPowerPoints >= 5;
    }

    public void SetPowerPoints(int healthPoints) {
        _powerPoint0.color = (healthPoints >= 1) ? _activePowerPointsColor : _inactivePowerPointsColor;
        _powerPoint1.color = (healthPoints >= 2) ? _activePowerPointsColor : _inactivePowerPointsColor;
        _powerPoint2.color = (healthPoints >= 3) ? _activePowerPointsColor : _inactivePowerPointsColor;
        _powerPoint3.color = (healthPoints >= 4) ? _activePowerPointsColor : _inactivePowerPointsColor;
        _powerPoint4.color = (healthPoints >= 5) ? _activePowerPointsColor : _inactivePowerPointsColor;
    }

}
