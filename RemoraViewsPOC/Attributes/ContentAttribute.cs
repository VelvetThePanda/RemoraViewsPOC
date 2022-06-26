namespace RemoraViewsPOC.Attributes;

/// <summary>
/// Marks a property as being content for a view.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ContentAttribute : Attribute { }