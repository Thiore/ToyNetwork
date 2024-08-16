using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Audio_Set : MonoBehaviour
{
    public Camera PlayerCamera_BSJ;
    private bool isLocalPlayer_BSj;
    private void Start()
    {
        if (isLocalPlayer_BSj)
        {
            //로컬플레이어일 때만 오디오 리스너 활성화
            PlayerCamera_BSJ.enabled = true;
            PlayerCamera_BSJ.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            //로컬플레이어일 때만 오디오 리스너 활성화
            PlayerCamera_BSJ.enabled = false;
            PlayerCamera_BSJ.GetComponent<AudioListener>().enabled = false;
        }
    }
}
