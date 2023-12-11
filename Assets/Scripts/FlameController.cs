using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    public Transform neck, jaw;

    public SkinnedMeshRenderer flameMesh;

    private Quaternion neckRotationBase;
    private float roll = 0, pitch = 0, yaw = 0;
    private float mouth = 0;

    public float mouthMin, mouthMax;

    private TextAsset beta;
    private List<float> betaList;
    public int shapeMultiplier = 1;

    public SkiManController skiManController;

    private void Start()
    {
        // neck = transform.Find("neck");
        // jaw = transform.Find("jaw");
        neckRotationBase = Quaternion.Euler(90, 0, 0);

        betaList = LoadBeta();
        for (int i = 0; i < 400; i++)
        {
            flameMesh.SetBlendShapeWeight(i, betaList[i] * shapeMultiplier);
        }
    }
    
    private void Update()
    {
        // for (int i = 0; i < 300; i++)
        // {
        //     flameMesh.SetBlendShapeWeight(i, betaList[i] * shapeMultiplier);
        // }
        if (!GameManager.IsPlaying) return;
        
        HeadRotation();
        MouthMoving();
    }

    private void FixedUpdate()
    {
        skiManController.MovePosition(roll, 30);
        GameManager.Instance.ControlSpeed(pitch, 30);
    }

    private List<float> LoadBeta()
    {
        beta = Resources.Load<TextAsset>("beta");
        char[] delimiterChars = {' ', ',', '\t', '\n', '\r'};
        var lines = beta.text.Split(delimiterChars);

        var betaList = new List<float>(400);
        int index = 0;
        foreach (var line in lines)
        {
            if (!float.TryParse(line, out var b))
            {
                continue;
                // throw new FormatException($"Incorrect format at line {index}: {line}");
            }

            index++;
            betaList.Add(b);
        }

        return betaList;
    }



    public void ParseMessage(String message)
    {
        string[] res = message.Split(' ');
        
        roll = float.Parse(res[0]);
        pitch = float.Parse(res[1]);
        yaw = float.Parse(res[2]);
        // ear_left = float.Parse(res[3]);
        // ear_right = float.Parse(res[4]);
        mouth = float.Parse(res[9]);
    }

    private void HeadRotation()
    {
        //TODO: angle clamp

        neck.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void MouthMoving()
    {
        float ratio = (mouth - mouthMin) / (mouthMax - mouthMin);
        jaw.localRotation = Quaternion.Euler(ratio * 15, 0, 0);
    }
}
