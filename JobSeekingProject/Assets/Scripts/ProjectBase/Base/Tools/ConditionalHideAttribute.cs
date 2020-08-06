using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    //The name of the bool field that will be in control
    public string ConditionalSourceField = "";
    //TRUE = Negate the bool field that will be in control
    public bool Negate;
    //Record the value if the field is enum type, the value of enum element must be power of 2
    public int EnumCondition;
    //TRUE = Hide in inspector / FALSE = Disable in inspector 
    public bool HideInInspector = false;

    /*public ConditionalHideAttribute(string conditionalSourceField)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
    }*/

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector = false, bool negate = false)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.Negate = negate;
    }

    public ConditionalHideAttribute(string conditionalSourceField, int enumCondition, bool hideInInspector = false)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.EnumCondition = enumCondition;
    }

}