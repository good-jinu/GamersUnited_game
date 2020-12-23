using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor
{
    private ArmorType type;
    private ItemGrade grade;

    public ItemGrade Grade { get => grade; set => grade = value; }
    public ArmorType Type { get => type; set => type = value; }
}
