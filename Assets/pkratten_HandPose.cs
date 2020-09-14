using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;

[Serializable]
public struct HandPose
{
    public HandPoseName name;
    [HideInInspector]
    public List<Vector3> localPositions;
    [HideInInspector]
    public List<Quaternion> localRotations;
    [HideInInspector]
    public List<Vector3> Positions;
    [HideInInspector]
    public List<Quaternion> Rotations;
    [HideInInspector]
    public List<Vector3> relativePositions;
    public Handedness handedness;
}

public enum HandPoseName
{
    None,
    New,
    One,
    Two,
    Three,
    Four,
    Five,
    Stop,
    Open,
    ThumbsUp,
    Pinch,
    PinchGrab
}

public class pkratten_HandPose : MonoBehaviour
{
    public HandPose currentPose;
    public float Tolerance;
    public pkratten_MRTKHands Hands;
    public Record record = Record.None;
    public List<HandPose> Poses;

    public enum Record
    {
        None,
        Right,
        Left
    }

    Handedness ComparePose(HandPose pose)
    {
        Handedness handedness = Handedness.None;

        int countR = 2;
        int countL = 2;

        for (int i = 2; i < pose.Positions.Count; i++)
        {
            Vector3 relativePosition = Hands.HandRight[1].transform.InverseTransformPoint(Hands.HandRight[i].position);
            if (Vector3.Distance(relativePosition, pose.relativePositions[i]) < Tolerance) countR++;
            relativePosition = Vector3.Reflect(relativePosition, Vector3.right);
            if (Vector3.Distance(relativePosition, pose.relativePositions[i]) < Tolerance) countL++;
        }

        if (countR == pose.Positions.Count) handedness = Handedness.Right;
        if (countL == pose.Positions.Count) handedness = Handedness.Left;

        return handedness;
    }

    void SavePose()
    {
        if (record == Record.None) return;

        HandPose pose = new HandPose();
        pose.name = HandPoseName.New;
        pose.localPositions = new List<Vector3>();
        pose.localRotations = new List<Quaternion>();
        pose.Positions = new List<Vector3>();
        pose.Rotations = new List<Quaternion>();
        pose.relativePositions = new List<Vector3>();
        pose.handedness = Handedness.None;

        for (int i = 0; i < Hands.HandRight.Count; i++)
        {
            Transform current = Hands.HandRight[i];
            pose.Positions.Add(current.position);
            pose.Rotations.Add(current.rotation);
            pose.localPositions.Add(current.localPosition);
            pose.localRotations.Add(current.localRotation);
            pose.relativePositions.Add(Hands.HandRight[1].transform.InverseTransformPoint(current.position));
        }
        if(record == Record.Left)
        {
            for (int i = 0; i < pose.relativePositions.Count; i++)
            {
                pose.relativePositions[i] = Vector3.Reflect(pose.relativePositions[i], Vector3.right);
            }
        }

        Poses.Add(pose);
    }

    HandPose None = new HandPose();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SavePose();
        }

        foreach (var pose in Poses)
        {
            Handedness handedness = ComparePose(pose);
            if(handedness!= Handedness.None)
            {
                currentPose = pose;
                currentPose.handedness = handedness;
                return;
            }
        }

        currentPose = None;
    }
}
