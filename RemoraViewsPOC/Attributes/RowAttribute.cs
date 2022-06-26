namespace RemoraViewsPOC.Attributes;

/// <summary>
/// If a non-exclusive component (e.g. Selects, Text Input), defines the row the component should be placed in.
/// If this attribute is ommited, the declared order of the component will be used.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class RowAttribute : Attribute
{
    public RowAttribute(ushort row) => Row = row;
    
    /// <summary>
    /// The row the component should be placed in. Values > 5 will result in an error.
    /// </summary>
    public ushort Row { get; set; }
}