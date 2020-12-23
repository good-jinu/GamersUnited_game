using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor
{
    private ArmorType type;
    private ItemGrade grade;

    public ItemGrade Grade { get => grade;}
    public ArmorType Type { get => type;}

    public Armor(ArmorType type,ItemGrade grade)
    {
        this.type = type;
        this.grade = grade;
    }
}
