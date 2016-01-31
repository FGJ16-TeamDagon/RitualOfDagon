using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player
{
    public static Color DeepOneColorDark = new Color((float)15 / 255, (float)167 / 255, (float)17 / 255);
    public static Color StrandedColorDark = new Color((float)185 / 255, (float)24 /255 ,1);
    public static Color DeepOneColorLight = new Color((float)125 / 255, 1, (float)127 / 255);
    public static Color StrandedColorLight = new Color(1, (float)169 / 255, (float)238 / 255);

    public string Name { get; protected set; }

    public List<GameCharacter> characters = new List<GameCharacter>();

    public Player(string name)
    {
        this.Name = name;
    }
}
