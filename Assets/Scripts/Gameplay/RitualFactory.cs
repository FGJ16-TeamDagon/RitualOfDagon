using UnityEngine;
using System.Collections;

public class RitualFactory : MonoBehaviour 
{
    public Ritual GetRandomRitual()
    {
        Ritual.Point[] pattern;
        string textureId;

        switch(Random.Range(0, 8))
        {
            case 0:
                pattern = new Ritual.Point[4];
                pattern[0] = new Ritual.Point(0, 1);
                pattern[1] = new Ritual.Point(1, 0);
                pattern[2] = new Ritual.Point(1, 2);
                pattern[3] = new Ritual.Point(2, 1);
                textureId = "shape2";
                break;
            case 1:
                pattern = new Ritual.Point[4];
                pattern[0] = new Ritual.Point(0, 0);
                pattern[1] = new Ritual.Point(1, 0);
                pattern[2] = new Ritual.Point(1, 1);
                pattern[3] = new Ritual.Point(1, 2);
                textureId = "shape3";
                break;
            case 2:
                pattern = new Ritual.Point[3];
                pattern[0] = new Ritual.Point(0, 1);
                pattern[1] = new Ritual.Point(1, 0);
                pattern[2] = new Ritual.Point(2, 1);
                textureId = "shape4";
                break;
            case 3:
                pattern = new Ritual.Point[4];
                pattern[0] = new Ritual.Point(0, 3);
                pattern[1] = new Ritual.Point(1, 2);
                pattern[2] = new Ritual.Point(2, 1);
                pattern[2] = new Ritual.Point(3, 0);
                textureId = "shape5";
                break;
            case 4:
                pattern = new Ritual.Point[5];
                pattern[0] = new Ritual.Point(0, 0);
                pattern[1] = new Ritual.Point(0, 1);
                pattern[2] = new Ritual.Point(0, 2);
                pattern[2] = new Ritual.Point(1, 0);
                pattern[2] = new Ritual.Point(2, 0);
                textureId = "shape6";
                break;
            case 5:
                pattern = new Ritual.Point[6];
                pattern[0] = new Ritual.Point(0, 1);
                pattern[1] = new Ritual.Point(1, 1);
                pattern[2] = new Ritual.Point(1, 2);
                pattern[2] = new Ritual.Point(2, 0);
                pattern[2] = new Ritual.Point(2, 2);
                pattern[2] = new Ritual.Point(3, 1);
                textureId = "shape7";
                break;
            case 6:
                pattern = new Ritual.Point[6];
                pattern[0] = new Ritual.Point(0, 0);
                pattern[1] = new Ritual.Point(1, 0);
                pattern[2] = new Ritual.Point(1, 1);
                pattern[2] = new Ritual.Point(1, 2);
                pattern[2] = new Ritual.Point(1, 3);
                pattern[2] = new Ritual.Point(2, 0);
                textureId = "shape8";
                break;
            default:
                pattern = new Ritual.Point[4];
                pattern[0] = new Ritual.Point(0, 0);
                pattern[1] = new Ritual.Point(1, 0);
                pattern[2] = new Ritual.Point(2, 0);
                pattern[3] = new Ritual.Point(1, 1);
                textureId = "shape1";
                break;
        }

        return MakeRitual(pattern, textureId);
    }

    private Ritual MakeRitual(Ritual.Point[] pattern, string textureId)
    {
        Ritual ritual = new Ritual();

        ritual.image = Resources.Load<Texture2D>("RitualPatterns/" + textureId);

        ritual.pattern = pattern;

        return ritual;
    }
}
