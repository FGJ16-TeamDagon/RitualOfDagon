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

        Point offset = new Point(startX, startZ);
        
        for (int i = 0; i < characters.Count; i++)
        {
            for (int j = 0; j < pattern.Length; j ++)
            {
               if (characters[i].Position.X == startX + pattern[j].X &&
                    characters[i].Position.Z == startZ + pattern[j].Z)
                {
                    fit++;
                    break;
                }
            }
        }

        return fit;
    }
}
