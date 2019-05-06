using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.Video;


public class myVBtnScript : MonoBehaviour, IVirtualButtonEventHandler
{
	public GameObject vbBtnObj;
    public TextMesh text_left;
    private int cnt = 0;

    public VideoPlayer vp;
    public VideoClip[] videoClips;

    // Start is called before the first frame update
    void Start()
    {
        vbBtnObj = GameObject.Find("MyVBtn1");
        vbBtnObj.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);

        text_left = GameObject.Find("text_left").GetComponent<TextMesh>();

        vp = GameObject.Find("VideoPlane").GetComponent<VideoPlayer> ();

    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("Button pressed");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        cnt++;
        Debug.Log("Button released");
        text_left.text = cnt.ToString();

        if (cnt >= 4)
            cnt = 0;

        vp.clip = videoClips[cnt];

        if (vp.isPlaying)
            {
                vp.Pause();
            }
            else
            {
                vp.Play();
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
