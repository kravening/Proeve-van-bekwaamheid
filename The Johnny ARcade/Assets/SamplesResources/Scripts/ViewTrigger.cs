/*===============================================================================
Copyright (c) 2015-2019 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;

public class ViewTrigger : MonoBehaviour, IFaderNotify
{
    public enum TriggerType
    {
        VR_TRIGGER,
        AR_TRIGGER
    }

    #region PUBLIC_MEMBER_VARIABLES
    public TriggerType triggerType = TriggerType.VR_TRIGGER;
    public float activationTime = 1.5f;
    public Material focusedMaterial;
    public Material nonFocusedMaterial;
    public bool Focused { get; set; }
    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES
    float focusedTime;
    bool triggered;
    TransitionManager transitionManager;
    Transform cameraTransform;
    Renderer meshRenderer;
    Fader fader;
    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        this.transitionManager = FindObjectOfType<TransitionManager>();
        this.fader = FindObjectOfType<Fader>();
        this.meshRenderer = GetComponent<Renderer>();
        this.meshRenderer.material = this.nonFocusedMaterial;
        this.cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (this.triggered)
            return;

        RaycastHit hit;
        Ray cameraGaze = new Ray(cameraTransform.position, cameraTransform.forward);
        Physics.Raycast(cameraGaze, out hit, Mathf.Infinity);
        this.Focused = hit.collider && (hit.collider.gameObject == gameObject);
        SetFocusedMaterial(this.Focused);

        bool startAction = false || Input.GetMouseButtonUp(0);

        if (this.Focused)
        {
            // Update the "focused state" time
            this.focusedTime += Time.deltaTime;

            if ((this.focusedTime > this.activationTime) || startAction)
            {
                this.triggered = true;
                this.focusedTime = 0;

                // Activate transition from AR to VR or vice versa
                bool toAR = (this.triggerType == TriggerType.AR_TRIGGER);
                this.transitionManager.SwitchMode(toAR);
            }
        }
        else
        {
            // Reset the "focused state" time
            this.focusedTime = 0;
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS
    private void SetFocusedMaterial(bool focused)
    {
        this.meshRenderer.material = focused ? this.focusedMaterial : this.nonFocusedMaterial;
    }
    #endregion // PRIVATE_METHODS


    #region IFADERNOTIFY_CALLBACK_METHODS

    void IFaderNotify.OnFadeOutFinished()
    {
        VLog.Log("cyan", "IFaderNotify.OnFadeOutFinished() called: " + gameObject.name);

        // When fade-out is finished, we reset the focused state and update materials.
        this.Focused = false;
        SetFocusedMaterial(false);
    }

    void IFaderNotify.OnFadeInFinished()
    {
        VLog.Log("cyan", "IFaderNotify.OnFadeInFinished() called: " + gameObject.name);

        // When fade-in is finished, we then reset triggered state and focused time.
        this.triggered = false;
        this.focusedTime = 0;
    }

    #endregion // IFADERNOTIFY_CALLBACK_METHODS
}

