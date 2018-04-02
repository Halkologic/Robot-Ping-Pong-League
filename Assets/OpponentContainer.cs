using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("OpponentCollection")]
public class OpponentContainer
{

    [XmlArray("Opponents")]
    [XmlArrayItem("Opponent")]

    public List<Opponent> opponents = new List<Opponent>();

    public static OpponentContainer Load(TextAsset ta)
    {
        //TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(OpponentContainer));
        StringReader reader = new StringReader(ta.text);
        OpponentContainer opponents = serializer.Deserialize(reader) as OpponentContainer;
        reader.Close();
        return opponents;
    }

    public Opponent GetOpponent(string opponentTag)
    {
        foreach (Opponent opponent in opponents)
        {
            if (opponent.TagComparison(opponentTag)) return opponent;
        }
        return null;
    }
    

}
