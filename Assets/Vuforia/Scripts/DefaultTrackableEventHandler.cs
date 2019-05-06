/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using Vuforia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

// namespace UnityStandardAssets.Cameras;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    string[] left_page_content;
    string[] right_page_content;
    //video list

    VideoClip[] resourceVideoClips;
    TextAsset[] resourceTexts;
    int cur_left_page=1;
    int cur_right_page=2;
    
    VideoPlayer vp1,vp2;

    protected virtual void Start()
    {    
        
       resourceVideoClips= Resources.LoadAll<VideoClip>("video");
       resourceTexts=Resources.LoadAll<TextAsset>("text");

          TextMesh left_page=GameObject.Find("text_left").GetComponent<TextMesh>();
          TextMesh right_page=GameObject.Find("text_right").GetComponent<TextMesh>();
           left_page_content = resourceTexts[0].text.Split('\n');
           right_page_content =  resourceTexts[1].text.Split('\n');

           left_page.text="";
           right_page.text="";

           vp1=GameObject.Find("left_video").GetComponent<VideoPlayer>();
           vp2=GameObject.Find("right_video").GetComponent<VideoPlayer>();

         foreach (string line in left_page_content)
         {

             left_page.text+=(line+'\n');
         }       

         foreach (string line in right_page_content)
         {
             right_page.text+=(line+'\n');
         }     


          mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>

    void Update()
    {

        TextMesh left_page=GameObject.Find("text_left").GetComponent<TextMesh>();
        TextMesh right_page=GameObject.Find("text_right").GetComponent<TextMesh>();

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {

              if(cur_left_page==3)
                return;
            int next_left_page=cur_left_page%4+2;
            int next_right_page=cur_right_page%4+2;
            left_page_content=resourceTexts[next_left_page-1].text.Split('\n');
            right_page_content=resourceTexts[next_right_page-1].text.Split('\n');

            vp1.clip=resourceVideoClips[next_left_page-1];
            vp2.clip=resourceVideoClips[next_right_page-1];
            vp1.Play();
            vp2.Play();

            left_page.text="";
            right_page.text="";

             foreach (string line in left_page_content)
             {

                 left_page.text+=(line+'\n');
             }       

             foreach (string line in right_page_content)
             {
                 right_page.text+=(line+'\n');
             }     

             cur_left_page=next_left_page;
             cur_right_page=next_right_page;



        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(cur_left_page==1)
                return;
            int before_left_page=(cur_left_page+4-2)%4;
            int before_right_page=(cur_right_page+4-2)%4;
            left_page_content=resourceTexts[before_left_page-1].text.Split('\n');
            right_page_content=resourceTexts[before_right_page-1].text.Split('\n');

            
            vp1.clip=resourceVideoClips[before_left_page-1];
            vp2.clip=resourceVideoClips[before_right_page-1];

            left_page.text="";
            right_page.text="";
             foreach (string line in left_page_content)
             {

                 left_page.text+=(line+'\n');
             }       

             foreach (string line in right_page_content)
             {
                 right_page.text+=(line+'\n');
             }     



            cur_left_page=before_left_page;
            cur_right_page=before_right_page;


        }
    }


    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }


        //완식 수정!!
    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PROTECTED_METHODS
}
