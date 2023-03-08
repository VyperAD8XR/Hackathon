using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class Hands : MonoBehaviour
{
    [HideInInspector]
    public Hands Instance;

    public GameObject handPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;

    private InputDevice _device;
    private Animator _handAnimator;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeHand();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (_device.isValid)
        {
            UpdateHand();
        }
        else
        {
            InitializeHand();
        }
    }
        void InitializeHand()
        {
            List<InputDevice> _devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, _devices);

            if (_devices.Count > 0)
            {
                _device = _devices[0];
                GameObject _newHand = Instantiate(handPrefab, this.transform);
                _handAnimator = _newHand.GetComponent<Animator>();
            }
        }
    

    void UpdateHand()
    {
        if (_device.TryGetFeatureValue(CommonUsages.trigger, out float _triggerValue))
        {
            _handAnimator.SetFloat("Trigger", _triggerValue);
        }
        if (_device.TryGetFeatureValue(CommonUsages.grip, out float _gripValue))
        {
            _handAnimator.SetFloat("Grip", _gripValue);

        }
    }
}
