using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MotionData
{
    [SerializeField] private Animator BoneAnimator;
    [SerializeField] private Transform CharacterNose;
    public GameObject[] RigPoints;
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
        UpdatePose();

        // Update_3DCloneRig(rigs);
        // Update_2DCloneRig(rigs);


        Rig rig = new Rig(rigs, 32);

        SubRig R_hand = rig.GetSubRig("R_Hand");
        SubRig L_hand = rig.GetSubRig("L_Hand");
        SubRig L_leg = rig.GetSubRig("L_Leg");
        SubRig R_leg = rig.GetSubRig("R_Leg");
        SubRig[] body = rig.GetBodyRig();
        
        rig.DrawLine(R_hand, Lines3D[0]);
        rig.DrawLine(L_hand, Lines3D[1]);
        rig.DrawLine(R_leg, Lines3D[2]);
        rig.DrawLine(L_leg, Lines3D[3]);
        rig.DrawLine(body, Lines3D[4]);

        rig.DrawLine(R_hand, Lines2D[0], false );
        rig.DrawLine(L_hand, Lines2D[1], false);
        rig.DrawLine(R_leg, Lines2D[2], false);
        rig.DrawLine(L_leg, Lines2D[3], false);
        rig.DrawLine(body, Lines2D[4], false);


        this.HandLength = rig.Hand_Length;
        this.LegLength = rig.Leg_Length;
        this.BodyLength = rig.Body_Length;
        this.ShoulderWidth = rig.Shoulder_Width;
        this.AcrossHip = rig.Across_Hip;
        this.TotalHeight = rig.Total_Height;
        IDisplay.ShowData(HandLength, LegLength, BodyLength, ShoulderWidth, AcrossHip, TotalHeight);
    }


    private void Update_3DCloneRig(JointPoint[]Rigs)
    {
       // RigPoints[PositionIndex.hip.Int()].transform.parent.position = Rigs[PositionIndex.hip.Int()].Transform.position;
        for (int i = 0; i < Rigs.Length; i++)
        {
            RigPoints[i].transform.localPosition = Rigs[i].Pos3D / 45;
        }

        //R hand
        Vector3 A = RigPoints[0].transform.position;
        Vector3 B = RigPoints[1].transform.position;
        Vector3 C = RigPoints[2].transform.position;
        DrawRig(A, B, Lines3D[0]);
        DrawRig(B, C, Lines3D[1]);

        

        //L hand
        A = RigPoints[5].transform.position;
        B = RigPoints[6].transform.position;
        C = RigPoints[7].transform.position;
        DrawRig(A, B, Lines3D[2]);
        DrawRig(B, C, Lines3D[3]);


        //R Leg
        A = RigPoints[15].transform.position;
        B = RigPoints[16].transform.position;
        C = RigPoints[17].transform.position;
        DrawRig(A, B, Lines3D[4]);
        DrawRig(B, C, Lines3D[5]);


        //L Leg
        A = RigPoints[19].transform.position;
        B = RigPoints[20].transform.position;
        C = RigPoints[21].transform.position;
        DrawRig(A, B, Lines3D[6]);
        DrawRig(B, C, Lines3D[7]);


        //Body
        A = RigPoints[0].transform.position;//R hand Root
        B = RigPoints[5].transform.position;//L hand Root
        C = RigPoints[15].transform.position;//R leg Root
        Vector3 D = RigPoints[19].transform.position;// L leg Root
        DrawRig(A, B, Lines3D[8]);
        DrawRig(C, D, Lines3D[9]);
        DrawRig(A, C, Lines3D[10]);
        DrawRig(B, D, Lines3D[11]);

    }


    private void Update_2DCloneRig(JointPoint[] Rigs)
    {
        //R hand
        Vector3 A = Rigs[0].Pos2D;
        Vector3 B = Rigs[1].Pos2D;
        Vector3 C = Rigs[2].Pos2D;

        DrawRig(A, B, Lines2D[0]);
        DrawRig(B, C, Lines2D[1]);


        //L hand
        A = Rigs[5].Pos2D;
        B = Rigs[6].Pos2D;
        C = Rigs[7].Pos2D;

        DrawRig(A, B, Lines2D[2]);
        DrawRig(B, C, Lines2D[3]);

        //R Leg
        A = Rigs[15].Pos2D;
        B = Rigs[16].Pos2D;
        C = Rigs[17].Pos2D;

        DrawRig(A, B, Lines2D[4]);
        DrawRig(B, C, Lines2D[5]);

        //L Leg
        A = Rigs[19].Pos2D;
        B = Rigs[20].Pos2D;
        C = Rigs[21].Pos2D;
        DrawRig(A, B, Lines2D[6]);
        DrawRig(B, C, Lines2D[7]);

        //Body
        A = Rigs[0].Pos2D;//R hand Root
        B = Rigs[5].Pos2D;//L hand Root
        C = Rigs[15].Pos2D;//R leg Root
        Vector2 D = Rigs[19].Pos2D;// L leg Root
        DrawRig(A, B, Lines2D[8]);
        DrawRig(C, D, Lines2D[9]);
        DrawRig(A, C, Lines2D[10]);
        DrawRig(B, D, Lines2D[11]);
    }

    [SerializeField] GameObject[] Lines3D;
    [SerializeField] GameObject[] Lines2D;
    private void DrawRig(Vector3 A, Vector3 B, GameObject lineObj)
    {

        LineRenderer rend = lineObj.GetComponent<LineRenderer>() == null ? lineObj.AddComponent<LineRenderer>() : lineObj.GetComponent<LineRenderer>();
        rend.startWidth = 0.1f;
        rend.endWidth = 0.1f;
        rend.SetPosition(0, A);
        rend.SetPosition(1, B);
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
