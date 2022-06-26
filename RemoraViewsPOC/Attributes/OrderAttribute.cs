namespace RemoraViewsPOC.Attributes;

/// <summary>
/// When applied to a non-exclusive component (e.g. Select, Text Input), what intra-component ordering should be used.
/// That is, what order should be placed. If this attribute is ommitted, then the declared order of the components is used.
///
/// If it is applied to an embed, however, this will define the order of the embed, else the declared order is used.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class OrderAttribute : Attribute
{
    public OrderAttribute(ushort order) => Order = order;
    
    public ushort Order { get; set; }
}