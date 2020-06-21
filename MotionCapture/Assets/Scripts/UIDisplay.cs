using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    public Text TotalHeight;
    public Text HandLength;
    public Text LegLength;
    public Text BodyHeight;
    public Text ShoulderWidth;
    public Text AcrossHip;

    public void ShowData(float HandLength, float LegLength, float BodyHeight, float ShoulderWidth, float AcrossHip, float TotalHeight)
    {
        this.HandLength.text = "Hand Length " + HandLength;
        this.LegLength.text = "Leg Length " + LegLength;
        this.BodyHeight.text = " Body Length " + BodyHeight;
        this.ShoulderWidth.text = "Shoulder Width " + ShoulderWidth;
        this.AcrossHip.text = "AcrossHip " + AcrossHip;
        this.TotalHeight.text = "Total Height " + TotalHeight;

    }


}
