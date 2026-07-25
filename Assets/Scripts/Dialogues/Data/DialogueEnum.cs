namespace Game.Dialogue
{
    /// <summary>
    /// Defines the kind of interaction a dialogue node expects.
    /// </summary>
    public enum DialogueType
    {
        Normal,
        YesNo
    }


    /// <summary>
    /// Variables that can influence the dialogue system.
    /// Add/remove values depending on your game needs.
    /// </summary>
    public enum DialogueVariable
    {
        Reputation,
        DaysLeft
    }


    /// <summary>
    /// Operations used when modifying dialogue variables.
    /// </summary>
    public enum VariableOperation
    {
        Set,
        Add,
        Subtract
    }


    /// <summary>
    /// Comparison types used for dialogue conditions.
    /// </summary>
    public enum ConditionOperator
    {
        Equal,
        Greater,
        Less,
        GreaterOrEqual,
        LessOrEqual,
        NotEqual
    }
}