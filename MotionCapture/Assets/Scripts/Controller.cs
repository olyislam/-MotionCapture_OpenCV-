using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MotionData
{
    [SerializeField] private Animator BoneAnimator;
    [SerializeField] private Transform CharacterNose;

    [SerializeField]private  GameObject[] Lines3D;

    protected override void Start()
    {
        base.Start();
        SetupBone(BoneAnimator, CharacterNose);
    }


    void Update()
    {
        Texture2D texture = GetTexture2D();
        if (texture == null)
        {
            return;
        }

        Mat imgmat = GetTextureMat(texture);
        JointPoint[] rigs = GetRigData(imgmat);
        UpdatePoseData();

        Rig rig = new Rig(rigs, 30);
        Drawing(rig);

        UpdateUIInfo(rig);
    
    }

    private void Drawing(Rig rig)
    {
        //Get Body Data
        SubRig R_hand = rig.GetSubRig("R_Hand");
        SubRig L_hand = rig.GetSubRig("L_Hand");
        SubRig L_leg = rig.GetSubRig("L_Leg");
        SubRig R_leg = rig.GetSubRig("R_Leg");
        SubRig[] body = rig.GetBodyRig();

        //Draw Line in 3D Space
        rig.DrawLine(R_hand, Lines3D[0]);
        rig.DrawLine(L_hand, Lines3D[1]);
        rig.DrawLine(R_leg, Lines3D[2]);
        rig.DrawLine(L_leg, Lines3D[3]);
        rig.DrawLine(body, Lines3D[4]);

    }


    public float scalereatio;
    private void UpdateUIInfo(Rig rig)
    {
        float HandLength = rig.Hand_Length * rig.Convert_To_Inch;
        float LegLength = rig.Leg_Length * rig.Convert_To_Inch;
        float BodyLength = rig.Body_Length * rig.Convert_To_Inch;
        float ShoulderWidth = rig.Shoulder_Width * rig.Convert_To_Inch;
        float AcrossHip = rig.Across_Hip * rig.Convert_To_Inch;
        float TotalHeight = rig.Total_Height * rig.Convert_To_Inch;

        //pass to ui screen
        IDisplay.ShowData(HandLength, LegLength, BodyLength, ShoulderWidth, AcrossHip, TotalHeight);

    }


    public UIDisplay IDisplay;
}
