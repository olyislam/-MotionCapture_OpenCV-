
using UnityEngine;

#region Mechanism
public enum PositionIndex : int
{
    rShldrBend = 0,
    rForearmBend,
    rHand,
    rThumb2,
    rMid1,

    lShldrBend,
    lForearmBend,
    lHand,
    lThumb2,
    lMid1,

    lEar,
    lEye,
    rEar,
    rEye,
    Nose,

    rThighBend,
    rShin,
    rFoot,
    rToe,

    lThighBend,
    lShin,
    lFoot,
    lToe,

    abdomenUpper,

    //Calculated coordinates
    hip,
    head,
    neck,
    spine,

    Count,
    None,
}

public enum TextureMode
{
    Camera,
    VideoClip,
    Image,
}

public static partial class EnumExtend
{
    public static int Int(this PositionIndex i)
    {
        return (int)i;
    }
}


public class JointPoint
{
    public Vector2 Pos2D = new Vector2();
    public float score2D;

    public Vector3 Pos3D = new Vector3();
    public Vector3 Now3D = new Vector3();
    public Vector3 PrevPos3D = new Vector3();
    public float score3D;

    // Bones
    public Transform Transform = null;
    public Quaternion InitRotation;
    public Quaternion Inverse;

    public JointPoint Child = null;
}


#endregion Mechanism


public class SubRig 
{
    //3D Data
    public Vector3 Root_3D;
    public Vector3 Center_3D;
    public Vector3 Destintion_3D;
   
    //2D Data
    public Vector3 Root_2D;
    public Vector3 Center_2D;
    public Vector3 Destintion_2D;

    public SubRig(Vector3 root3D, Vector3 center3D, Vector3 destination3D)
    {
        this.Root_3D = root3D;
        this.Center_3D = center3D;
        this.Destintion_3D = destination3D;

        this.Root_2D = Camera.main.WorldToScreenPoint(root3D);
        this.Center_2D = Camera.main.WorldToScreenPoint(center3D);
        this.Destintion_2D = Camera.main.WorldToScreenPoint(destination3D);


    }
}

public class Rig
{

    //World Anchore Data

    //Right Hand
    private Vector3 R_Hand_Shoulder_3D;
    private Vector3 R_Hand_Elbows_3D;
    private Vector3 R_Hand_Wrists_3D;

    //Left Hand
    private Vector3 L_Hand_Shoulder_3D;
    private Vector3 L_Hand_Elbows_3D;
    private Vector3 L_Hand_Wrists_3D;


    //Right Leg
    private Vector3 R_Leg_Hip_3D;
    private Vector3 R_Leg_Knees_3D;
    private Vector3 R_Leg_Ankles_3D;

    //Left Leg
    private Vector3 L_Leg_Hip_3D;
    private Vector3 L_Leg_Knees_3D;
    private Vector3 L_Leg_Ankles_3D;

    public int Scale_Factor;

    public Rig(JointPoint[] points, int ScaleFactor = 1)
    {
        Scale_Factor = ScaleFactor;
        //Right Hand
        R_Hand_Shoulder_3D = points[0].Pos3D / ScaleFactor;
        R_Hand_Elbows_3D = points[1].Pos3D / ScaleFactor;
        R_Hand_Wrists_3D = points[2].Pos3D / ScaleFactor;

        //Left Hand
        L_Hand_Shoulder_3D = points[5].Pos3D / ScaleFactor;
        L_Hand_Elbows_3D = points[6].Pos3D / ScaleFactor;
        L_Hand_Wrists_3D = points[7].Pos3D / ScaleFactor;


        //Right Leg
        R_Leg_Hip_3D = points[15].Pos3D / ScaleFactor;
        R_Leg_Knees_3D = points[16].Pos3D / ScaleFactor;
        R_Leg_Ankles_3D = points[17].Pos3D / ScaleFactor;

        //Left Leg
        L_Leg_Hip_3D = points[19].Pos3D / ScaleFactor;
        L_Leg_Knees_3D = points[20].Pos3D / ScaleFactor;
        L_Leg_Ankles_3D = points[21].Pos3D / ScaleFactor;
    }


    /// <summary>
    /// get 2D or 3D Rig data of a human body using name of body part. Example Name :"R_Hand", "L_Hand", "R_Leg":, "L_Leg"
    /// </summary>
    /// <param name="RigName">Body Part Name Like : "R_Hand", "L_Hand", "R_Leg":, "L_Leg":</param>
    /// <returns></returns>
    public SubRig GetSubRig(string RigName)
    {
        switch (RigName)
        {
            case "R_Hand":
                return new SubRig(R_Hand_Shoulder_3D, R_Hand_Elbows_3D, R_Hand_Wrists_3D);
            case "L_Hand":
                return new SubRig(L_Hand_Shoulder_3D, L_Hand_Elbows_3D, L_Hand_Wrists_3D);
            case "R_Leg":
                return new SubRig(R_Leg_Hip_3D, R_Leg_Knees_3D, R_Leg_Ankles_3D);
            case "L_Leg":
                return new SubRig(L_Leg_Hip_3D, L_Leg_Knees_3D, L_Leg_Ankles_3D);
            default:
                return null;

        }

    }

    /// <summary>
    /// Get bosy Area in world space rect Draw Clockwise
    /// </summary>
    /// <returns></returns>
    public SubRig[] GetBodyRig()
    {
        SubRig[] body = new SubRig[4];

        body[0] = new SubRig(R_Hand_Shoulder_3D, R_Hand_Elbows_3D, R_Hand_Wrists_3D);
        body[1] = new SubRig(L_Hand_Shoulder_3D, L_Hand_Elbows_3D, L_Hand_Wrists_3D);
        body[3] = new SubRig(L_Leg_Hip_3D, L_Leg_Knees_3D, L_Leg_Ankles_3D);
        body[2] = new SubRig(R_Leg_Hip_3D, R_Leg_Knees_3D, R_Leg_Ankles_3D);

        return body;
    }


    public void DrawLine(SubRig sub_rig, GameObject L_rendereer, bool is3D = true)
    {
        LineRenderer rend = L_rendereer.GetComponent<LineRenderer>() == null ? L_rendereer.AddComponent<LineRenderer>() : L_rendereer.GetComponent<LineRenderer>();
        rend.startWidth = 0.1f;
        rend.endWidth = 0.1f;
        rend.positionCount = 3;
        if (is3D)
        {
            rend.SetPosition(0, sub_rig.Root_3D);
            rend.SetPosition(1, sub_rig.Center_3D);
            rend.SetPosition(2, sub_rig.Destintion_3D);
        }
        else
        {
            rend.SetPosition(0, sub_rig.Root_2D);
            rend.SetPosition(1, sub_rig.Center_2D);
            rend.SetPosition(2, sub_rig.Destintion_2D);
        }
    }
    public void DrawLine(SubRig[] sub_rig, GameObject L_rendereer, bool is3D = true)
    {
        LineRenderer rend = L_rendereer.GetComponent<LineRenderer>() == null ? L_rendereer.AddComponent<LineRenderer>() : L_rendereer.GetComponent<LineRenderer>();
        rend.startWidth = 0.1f;
        rend.endWidth = 0.1f;
        rend.positionCount = 5;
        if (is3D)
        {
            rend.SetPosition(0, sub_rig[0].Root_3D);
            rend.SetPosition(1, sub_rig[1].Root_3D);
            rend.SetPosition(2, sub_rig[2].Root_3D);
            rend.SetPosition(3, sub_rig[3].Root_3D);
            rend.SetPosition(4, sub_rig[0].Root_3D);
        }
        else
        {
            rend.SetPosition(0, sub_rig[0].Root_2D);
            rend.SetPosition(1, sub_rig[1].Root_2D);
            rend.SetPosition(2, sub_rig[2].Root_2D);
            rend.SetPosition(3, sub_rig[3].Root_2D);
            rend.SetPosition(4, sub_rig[0].Root_2D);
        }
    }


    public void DrawSphere(SubRig sub_rig, GameObject A, GameObject B, GameObject C)
    {
        A.transform.localPosition = sub_rig.Root_3D;
        B.transform.localPosition = sub_rig.Center_3D;
        C.transform.localPosition = sub_rig.Destintion_3D;
    }


    public float Hand_UpperQuater
    {
        get
        {
            float r_side = Vector3.Distance(R_Hand_Shoulder_3D, R_Hand_Elbows_3D);
            float l_side = Vector3.Distance(L_Hand_Shoulder_3D, L_Hand_Elbows_3D);
            return r_side > l_side ? r_side : l_side;
        }
    }

    public float Hand_LowerQuater
    {
        get 
        {
            float r_side = Vector3.Distance(R_Hand_Elbows_3D, R_Hand_Wrists_3D);
            float l_side = Vector3.Distance(L_Hand_Elbows_3D, L_Hand_Wrists_3D);
            return r_side > l_side ? r_side : l_side;
        }
    }

    /// <summary>
    /// total hand length between shoulder to finger
    /// </summary>
    public float Hand_Length
    {
        get 
        {
            return Hand_UpperQuater + Hand_LowerQuater;
        }
    }


    public float Leg_UpperQuater
    { 
        get 
        { 
            float r_side = Vector3.Distance(R_Leg_Hip_3D, R_Leg_Knees_3D);
            float l_side = Vector3.Distance(L_Leg_Hip_3D, L_Leg_Knees_3D);
            return r_side > l_side ? r_side : l_side;
        }
    }

    public float Leg_LowerQuater
    {
        get
        {
            float r_side = Vector3.Distance(R_Leg_Knees_3D, R_Leg_Ankles_3D);
            float l_side = Vector3.Distance(L_Leg_Knees_3D, L_Leg_Ankles_3D);
            return r_side > l_side ? r_side : l_side;
        }
    }

    /// <summary>
    /// Total height of leg between hip ato foot
    /// </summary>
    public float Leg_Length
    {
        get
        {
            return Leg_UpperQuater + Leg_LowerQuater;
        }
    }

    /// <summary>
    /// height of body
    /// </summary>
    public float Body_Length
    {
        get
        {
           float r_side = Vector3.Distance(R_Hand_Shoulder_3D, R_Leg_Hip_3D);
           float l_side = Vector3.Distance(L_Hand_Shoulder_3D, L_Leg_Hip_3D);
           return r_side > l_side ? r_side : l_side;
        }
    }


    /// <summary>
    /// Upper side of body
    /// </summary>
    public float Shoulder_Width {get { return Vector3.Distance(R_Hand_Shoulder_3D, L_Hand_Shoulder_3D);}}

    /// <summary>
    /// Lower side of body
    /// </summary>
    public float Across_Hip { get{ return Vector3.Distance(R_Leg_Hip_3D, L_Leg_Hip_3D);} }


    public float Total_Height
    {
        get 
        {
            return Leg_Length + Body_Length;
        }
    }

}