using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MotionData
{
    [SerializeField] private Animator BoneAnimator;
    [SerializeField] private Transform CharacterNose;

    [SerializeField] private GameObject[] RigPoints;
    [SerializeField]private  GameObject[] Lines3D;
    [SerializeField]private  GameObject[] Lines2D;

    protected override void Start()
    {
        Debug.Log("r " + Vector3.Distance(Vector3.zero, Vector3.right));
        Debug.Log("ru " + Vector3.Distance(Vector3.zero, Vector3.right + Vector3.up));
        Debug.Log("one " + Vector3.Distance(Vector3.zero, Vector3.one));
        base.Start();
        SetupBone(BoneAnimator, CharacterNose);
    }


    void Update()
    {
        Texture2D texture = GetTexture2D();
        if (texture == null)
        {
            Debug.LogError("<color=red>Texture is null</color>");
            return;
        }

        Mat imgmat = GetTextureMat(texture);
        JointPoint[] rigs = GetRigData(imgmat);
        UpdatePoseData();

    

        Rig rig = new Rig(rigs, 30);
        Drawing(rig, true ,true );

        UpdateUIInfo(rig);
    

    }

    private void Drawing(Rig rig, bool Draw3D, bool Draw3D_Line, bool Draw2D_Line = false)
    {
        //Get Body Data
        SubRig R_hand = rig.GetSubRig("R_Hand");
        SubRig L_hand = rig.GetSubRig("L_Hand");
        SubRig L_leg = rig.GetSubRig("L_Leg");
        SubRig R_leg = rig.GetSubRig("R_Leg");
        SubRig[] body = rig.GetBodyRig();

        //Draw Line in 3D Space
        if (Draw3D_Line)
        { 
            rig.DrawLine(R_hand, Lines3D[0]);
            rig.DrawLine(L_hand, Lines3D[1]);
            rig.DrawLine(R_leg, Lines3D[2]);
            rig.DrawLine(L_leg, Lines3D[3]);
            rig.DrawLine(body, Lines3D[4]);
        }

        //Draw Line in 2D Space
        if (Draw2D_Line)
        { 
            rig.DrawLine(R_hand, Lines2D[0], false);
            rig.DrawLine(L_hand, Lines2D[1], false);
            rig.DrawLine(R_leg, Lines2D[2], false);
            rig.DrawLine(L_leg, Lines2D[3], false);
            rig.DrawLine(body, Lines2D[4], false);
        }


        if (Draw3D)
        { 
            rig.DrawSphere(R_hand, RigPoints[0], RigPoints[1], RigPoints[2]);
            rig.DrawSphere(L_hand, RigPoints[3], RigPoints[4], RigPoints[5]);
            rig.DrawSphere(R_leg, RigPoints[6], RigPoints[7], RigPoints[8]);
            rig.DrawSphere(L_leg, RigPoints[9], RigPoints[10], RigPoints[11]);
        }
    }



    private void UpdateUIInfo(Rig rig)
    {
        this.HandLength = rig.Hand_Length;
        this.LegLength = rig.Leg_Length;
        this.BodyLength = rig.Body_Length;
        this.ShoulderWidth = rig.Shoulder_Width;
        this.AcrossHip = rig.Across_Hip;
        this.TotalHeight = rig.Total_Height;
        IDisplay.ShowData(HandLength, LegLength, BodyLength, ShoulderWidth, AcrossHip, TotalHeight);

    }

    [Header("Total Body Information")]
    public float TotalHeight;
    public float HandLength;
    public float LegLength;
    public float BodyLength;
    public float ShoulderWidth;
    public float AcrossHip;

    public UIDisplay IDisplay;
}
