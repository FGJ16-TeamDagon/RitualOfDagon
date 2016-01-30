using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player
{
    public string Name { get; protected set; }

    public List<GameCharacter> characters = new List<GameCharacter>();

    public Player(string name)
    {
        this.Name = name;
    }
}
