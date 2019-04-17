/*===============================================================================
Copyright (c) 2015-2019 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;

public class BlackMaskBehaviour : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES
    float fadeFactor;
    Camera cam;
    Renderer meshRenderer;
    #endregion //PRIVATE_MEMBER_VARIABLES

    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        this.meshRenderer = GetComponent<Renderer>();

        SetFadeFactor(0);

        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    void Update()
    {
        if (this.cam != null)
        {
            float fovX = 2.0f * Mathf.Atan(1.0f / this.cam.projectionMatrix[0, 0]);
            float fovY = 2.0f * Mathf.Atan(1.0f / this.cam.projectionMatrix[1, 1]);

            // Set black mask position at near clip plane
            float near = this.cam.nearClipPlane;
            transform.localPosition = 1.05f * Vector3.forward * near;
            transform.localScale = new Vector3(
                16.0f * near * Mathf.Tan(fovX / 2),
                16.0f * near * Mathf.Tan(fovY / 2),
                1);
        }

        this.meshRenderer.material.SetFloat("_Alpha", this.fadeFactor);
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    public void SetFadeFactor(float ff)
    {
        this.fadeFactor = Mathf.Clamp01(ff);
    }
    #endregion // PUBLIC_METHODS


    #region VUFORIA_CALLBACKS
    void OnVuforiaStarted()
    {
        this.cam = DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;
    }
    #endregion // VUFORIA_CALLBACKS
}
