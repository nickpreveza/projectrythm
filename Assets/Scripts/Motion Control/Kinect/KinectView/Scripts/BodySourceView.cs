using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour 
{
    float time = 0.0f;
    public float PlayTrailerAfterSeconds = 2f;

    public Material BoneMaterial;
    public GameObject BodySourceManager, left_hand, right_hand, videoPlayer, videoPanel;

    public bool playTrailer = false, stopPlaying =false, allowedToPlayTrailer = true, bodyHasBeenInView = false;
    
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    private List<Kinect.JointType> _joints = new List<Kinect.JointType>
    {
        Kinect.JointType.HandLeft,
        Kinect.JointType.HandRight,
    };

    //private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    //{
    //    { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
    //    { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
    //    { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
    //    { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

    //    { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
    //    { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
    //    { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
    //    { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

    //    { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
    //    { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
    //    { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
    //    { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
    //    { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
    //    { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

    //    { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
    //    { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
    //    { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
    //    { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
    //    { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
    //    { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

    //    { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
    //    { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
    //    { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
    //    { Kinect.JointType.Neck, Kinect.JointType.Head },
    //};

    private void Start()
    {
        videoPanel = UIManager.Instance.videoPanel;
        videoPlayer = UIManager.Instance.videoPlayer;
    }

    void Update () 
    {
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }


        if (videoPlayer == null || videoPlayer == null)
        {

            videoPanel = UIManager.Instance.videoPanel;
            videoPlayer = UIManager.Instance.videoPlayer;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            if (bodyHasBeenInView)
            {
                allowedToPlayTrailer = true;
            }
            if (allowedToPlayTrailer && GameManager.Instance.State != GameState.GAME)
            {
                //PLAY TRAILER AFTER A SET AMMOUNT OF TIME
                time += Time.deltaTime;
                if (time > PlayTrailerAfterSeconds && !videoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying)
                {
                    playTrailer = true;
                    time -= PlayTrailerAfterSeconds;
                }
                if (playTrailer)
                {
                    videoPanel.SetActive(true);
                    AudioManager.Instance.Stop("mainMenuTrack");
                    UnityEngine.Video.VideoPlayer vp = videoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>();
                    vp.frame = 0;
                    vp.Play();
                    playTrailer = false;
                }
                
                    if (videoPlayer == null || videoPlayer == null){

                        videoPanel = UIManager.Instance.videoPanel;
                        videoPlayer = UIManager.Instance.videoPlayer;
                    }
                if (videoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying && stopPlaying)
                {
                    AudioManager.Instance.Play("mainMenuTrack");
                    videoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
                    videoPanel.SetActive(false);
                    stopPlaying = false;
                }
            }
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
                
            if(body.IsTracked)
            {
                allowedToPlayTrailer = false;
                bodyHasBeenInView = true;
                if (videoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying)
                {
                    stopPlaying = true;
                }
                trackedIds.Add (body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
            
            if(body.IsTracked)
            {
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        body.AddComponent<DontDestroyPls>();
        foreach (Kinect.JointType joint in _joints)
        {
            GameObject jointObj;

            if (joint == Kinect.JointType.HandLeft)
            {
                jointObj = Instantiate(left_hand);
            }
            else
                jointObj = Instantiate(right_hand);

            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = joint.ToString();
            jointObj.transform.parent = body.transform;
        }

        //for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        //{
        //    GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //    LineRenderer lr = jointObj.AddComponent<LineRenderer>();
        //    lr.SetVertexCount(2);
        //    lr.material = BoneMaterial;
        //    lr.SetWidth(0.05f, 0.05f);



        //    jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        //    jointObj.name = jt.ToString();
        //    jointObj.transform.parent = body.transform;
        //}
        
        return body;
    }
    
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        foreach (Kinect.JointType joint in _joints)
        {
            Kinect.Joint sourceJoint = body.Joints[joint];
            //Kinect.Joint? targetJoint = null;

            //if (_BoneMap.ContainsKey(joint))
            //{
            //    targetJoint = body.Joints[_BoneMap[joint]];
            //}

            Transform jointObj = bodyObject.transform.Find(joint.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            
            
        }

        //for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        //{
        //    Kinect.Joint sourceJoint = body.Joints[jt];
        //    Kinect.Joint? targetJoint = null;
            
        //    if(_BoneMap.ContainsKey(jt))
        //    {
        //        targetJoint = body.Joints[_BoneMap[jt]];
        //    }
            
        //    Transform jointObj = bodyObject.transform.Find(jt.ToString());
        //    jointObj.localPosition = GetVector3FromJoint(sourceJoint);
            
        //    LineRenderer lr = jointObj.GetComponent<LineRenderer>();
        //    if(targetJoint.HasValue)
        //    {
        //        lr.SetPosition(0, jointObj.localPosition);
        //        lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
        //        lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
        //    }
        //    else
        //    {
        //        lr.enabled = false;
        //    }
        //}
    }
    
    //private static Color GetColorForState(Kinect.TrackingState state)
    //{
    //    switch (state)
    //    {
    //    case Kinect.TrackingState.Tracked:
    //        return Color.green;

    //    case Kinect.TrackingState.Inferred:
    //        return Color.red;

    //    default:
    //        return Color.black;
    //    }
    //}
    
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 35, joint.Position.Y * 35, joint.Position.Z * 35);
    }
}
