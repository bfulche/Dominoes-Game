using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Credit: https://www.youtube.com/watch?v=cqGq__JjhMM
/// </summary>
public class RotateSky : MonoBehaviour
{
    [Range(0f, 5f)]
    [SerializeField] float rotateSpeed = 1.2f;

    Skybox box;

    private void Start()
    {
        box = GetComponent<Skybox>();
    }

    private void Update()
    {
        box.material.SetFloat("_Rotation", Time.time * rotateSpeed);
      //  RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}

