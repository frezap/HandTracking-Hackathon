﻿using JetBrains.Annotations;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HandPoseV1
{
    public string Name;
    public List<Quaternion> LocalRotations;
    public Handedness Handedness;
}

public class pkratten_HandPoseV1 : MonoBehaviour
{
    public pkratten_MRTKHands Hands;
    public float Tolerance;
    public HandPoseV1 currentPose;
    public List<HandPoseV1> Poses;
    public Record record = Record.Off;

    public enum Record
    {
        Off,
        Right,
        Left
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool ComparePose(List<Transform> hand, List<Quaternion> pose)
    {
        bool equals = true;
        for (int i = 2; i < hand.Count; i++)
        {
            double angle = Quaternion.Angle(hand[i].localRotation, pose[i]);
            if(angle > Tolerance)
            {
                equals = false;
                break;
            }
        }
        return equals;
    }

    HandPoseV1 empty = new HandPoseV1();

    // Update is called once per frame
    void Update()
    {
        bool foundPose = false;
        foreach (var pose in Poses)
        {
            Handedness handedness = Handedness.None;
            if(ComparePose(Hands.HandRight, pose.LocalRotations)) handedness = Handedness.Right;
            if(ComparePose(Hands.HandLeft, pose.LocalRotations)) handedness |= Handedness.Left;

            if(handedness != Handedness.None)
            {
                currentPose = pose;
                currentPose.Handedness = handedness;
                foundPose = true;
                break;
            }
        }
        if (!foundPose) currentPose = empty;

        if(Input.GetKeyDown(KeyCode.H))
        {
            SavePose(record);
        }
    }

    void SavePose(Record record)
    {
        if (record == Record.Off) return;

        HandPoseV1 pose = new HandPoseV1();
        pose.Name = DateTime.Now.ToString();
        pose.LocalRotations = new List<Quaternion>();
        for (int i = 0; i < Hands.HandRight.Count; i++)
        {
            if(record == Record.Right) pose.LocalRotations.Add(Hands.HandRight[i].localRotation);
            else pose.LocalRotations.Add(Hands.HandLeft[i].localRotation);
        }
        Poses.Add(pose);
    }
}
