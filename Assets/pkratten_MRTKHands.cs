using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pkratten_MRTKHands : MonoBehaviour
{
    public List<Transform> HandRight;
    public List<Transform> HandLeft;

    List<Transform> mrtkTransformsRight = new List<Transform>(26);
    List<Transform> mrtkTransformsLeft = new List<Transform>(26);

    void GetRightJointTransforms()
    {
        var handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        if (handJointService != null)
        {
            for (int i = 1; i < 27; i++)
            {
                mrtkTransformsRight.Add(handJointService.RequestJointTransform((TrackedHandJoint)i, Handedness.Right));
            }
        }
    }
    void GetLeftJointTransforms()
    {
        var handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        if (handJointService != null)
        {
            for (int i = 1; i < 27; i++)
            {
                mrtkTransformsLeft.Add(handJointService.RequestJointTransform((TrackedHandJoint)i, Handedness.Left));
            }
        }
    }

    private void FixedUpdate()
    {
        if (mrtkTransformsRight.Count == 0) GetRightJointTransforms();
        else
        {
            for (int i = 0; i < 26; i++)
            {
                HandRight[i].position = mrtkTransformsRight[i].position;
                HandRight[i].rotation = mrtkTransformsRight[i].rotation;
            }
        }
        if (mrtkTransformsLeft.Count == 0) GetLeftJointTransforms();
        else
        {
            for (int i = 0; i < 26; i++)
            {
                HandLeft[i].position = mrtkTransformsLeft[i].position;
                HandLeft[i].rotation = mrtkTransformsLeft[i].rotation;
            }
        }
    }

    private void Update()
    {
        FixedUpdate();
    }
}
