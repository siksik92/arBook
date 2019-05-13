using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.Video;


public class myVBtnScript : MonoBehaviour, IVirtualButtonEventHandler
{
	public GameObject vbBtnObj;
    public TextMesh text_left;

    public VideoPlayer vp;
    public VideoClip[] videoClips;

    private int cnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Register Virtual button EventHandler
        vbBtnObj = GameObject.Find("MyVBtn1");
        vbBtnObj.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);

        // Get text_left
        text_left = GameObject.Find("text_left").GetComponent<TextMesh>();

        // Get VideoPlane
        vp = GameObject.Find("VideoPlane").GetComponent<VideoPlayer> ();
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("Button pressed");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("Button released");

        // Set new string
        cnt++;
        text_left.text = cnt.ToString();

        if (cnt >= 4)
            cnt = 0;

        // Set new VideoCLip
        vp.clip = videoClips[cnt];

        if (vp.isPlaying)
        {
            {
                vp.Pause();
            }
            else
            {
                vp.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
