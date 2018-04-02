using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;


public class Opponent {

    [XmlAttribute("tag")]
    public string tag;

    public string name;
    public float colorRed;
    public float colorGreen;
    public float colorBlue;
    public float speed;
    public float reach;
    public float highHitChance;
    public float midHitChance;
    public float lowHitChance;
    public float xUnaccuracy;
    public float zUnaccuracy;
    public float forceUnaccuracy;


    public bool beaten = false;

    public bool TagComparison(string comparedTag)
    {
        if (comparedTag == tag) return true;
        else return false;
    }

    public void SetBeaten(bool beatenState)
    {
        beaten = beatenState;
    }

}
