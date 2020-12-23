using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    private ArmorType type;
    private ItemGrade grade;

    public ItemGrade Grade { get => grade;}
    public ArmorType Type { get => type;}

    public void Init(ArmorType type,ItemGrade grade)
    {
        this.type = type;
        this.grade = grade;
    }
}
