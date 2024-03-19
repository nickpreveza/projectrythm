using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class SimpleBodySourceView : MonoBehaviour
{
    public BodySourceManager mBodySourceManager;
    public GameObject left_hand_object, right_hand_object, videoPlayer;

    public bool left_hand_closed = false;

    public bool playTrailer = false;

    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    private List<JointType> _joints = new List<JointType>
    {
        JointType.HandLeft,
        JointType.HandRight,
    };

    HandState last_state;

    public Sprite closed_left_hand_sprite, open_left_hand_sprite;

    void Update()
    {
       //Get Kinect data
        Body[] data = mBodySourceManager.GetData();

        if (data == null)
            return;

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
                continue;

            if (body.IsTracked)
                trackedIds.Add(body.TrackingId);
        }

        //Delete Kinect bodies
        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                // Destroy body object
                Destroy(mBodies[trackingId]);

                // Remove from list
                mBodies.Remove(trackingId);
            }
        }

        //Create Kinect bodies
        foreach (var body in data)
        {
            // If no body, skip
            if (body == null)
            {
                if (playTrailer)
                {
                    videoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                    playTrailer = false;
                }
                continue;
            }

            if (body.IsTracked)
            {
                // If body isn't tracked, create body
                if (!mBodies.ContainsKey(body.TrackingId) && mBodies.Count <= 0)
                    mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);

                // Update positions
                UpdateBodyObject(body, mBodies[body.TrackingId]);
            }
        }
    }

    private GameObject CreateBodyObject(ulong id)
    {
        // Create body parent
        GameObject body = new GameObject("Body:" + id);

        // Create joints
        foreach (JointType joint in _joints)
        {
            GameObject newJoint;
            // Create Object
            if (joint == JointType.HandLeft)
                newJoint = Instantiate(left_hand_object);
            else
                newJoint = Instantiate(right_hand_object);
            newJoint.name = joint.ToString();

            // Parent to body
            newJoint.transform.parent = body.transform;
        }

        DontDestroyOnLoad(body);

        return body;
    }

    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        // Update joints
        foreach (JointType _joint in _joints)
        {
            // Get new target position
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.z = 0;

            // Get joint, set new position
            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            jointObject.position = targetPosition;

            if (last_state == HandState.Closed && body.HandLeftState == HandState.Unknown)
            {
                left_hand_closed = true;
            }
            //GET HANDSTATE
            if (body.HandLeftState != HandState.Closed && body.HandLeftState != HandState.Unknown)
            {
                last_state = body.HandLeftState;
                left_hand_closed = false;

            }
            else if (body.HandLeftState == HandState.Unknown)
            {
                if (last_state == HandState.Closed)
                {
                    left_hand_closed = true;
                }
                else
                    left_hand_closed = false;
            }
            else
            {
                last_state = HandState.Closed;
                left_hand_closed = true;

            }

            if (_joint == JointType.HandLeft)
            {
                if(left_hand_closed)
                jointObject.GetComponent<SpriteRenderer>().sprite = closed_left_hand_sprite;
                else
                {
                    jointObject.GetComponent<SpriteRenderer>().sprite = open_left_hand_sprite;
                }
            }
            
            
        }
    }

    private Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    
}