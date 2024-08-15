using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class VirtualCameraBehaviour : MonoBehaviour
{
    private CinemachineTransposer _vCamTransposer;

    [SerializeField] private Vector3 firstPersonCameraOffset;
    [SerializeField] private Vector3 thirdPersonCameraOffset;
    void Start()
    {
        _vCamTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
    }


    void Update()
    {
        Vector3 newCameraOffset;
        if (PlayerPrefs.GetInt("Car View", 1) == 1)
        {
            newCameraOffset = firstPersonCameraOffset;
        }
        else
        {
            newCameraOffset = thirdPersonCameraOffset;
        }

        _vCamTransposer.m_FollowOffset = newCameraOffset;
    }
}
