using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
  
    public static Track Instance = null;
    public List<Frame> frames;


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    public void GenerateTrack()
    {


    }

    public Frame GetNextFrame(bool past)
    {
        Frame frame=null;
        if (frames.Count != 0)
        {
            frame = frames[0];
            if (!frame.isFirst || past)
                frames.RemoveAt(0);
        }

        return frame;
    }
}
