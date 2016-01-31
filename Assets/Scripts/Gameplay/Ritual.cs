using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ritual 
{
    public struct Point
    {
        public int X;
        public int Z;
        public Point(int x, int z)
        {
            this.X = x;
            this.Z = z;
        }
    }

    public Point[] pattern;

    public Texture2D image;

    public int BestFit(List<GameCharacter> characters)
    {
        int bestFit = 0;

        for (int i = 0; i < characters.Count; i++)
        {
            bestFit = Mathf.Max(bestFit, GetFitCount(characters, i));
        }

        return bestFit;
    }

    public bool Fit(List<GameCharacter> characters)
    {
        return BestFit(characters) == pattern.Length;
    }

    private int GetFitCount(List<GameCharacter> characters, int startCharacter)
    {
        int fit = 0;
 
        int startX = characters[startCharacter].Position.X;
        int startZ = characters[startCharacter].Position.Z;

        string str = "Ritual positions: ";
        for (int i = 0; i < pattern.Length; i++)
        {
            str += " " + (startX + pattern[i].X) + ":" + (startZ + pattern[i].Z);
        }
        //Debug.Log(str);

        Point offset = new Point(startX, startZ);

        for (int i = 0; i < pattern.Length; i++)
        {
            for (int j = 0; j < characters.Count; j ++)
            {
                if (characters[j] == null)
                {
                    continue;
                }

                if (characters[j].Position.X == startX + pattern[i].X &&
                    characters[j].Position.Z == startZ + pattern[i].Z)
                {
                    fit++;
                    
                    break;
                }
            }
        }

        return fit;
    }
}
