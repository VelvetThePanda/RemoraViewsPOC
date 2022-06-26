namespace RemoraViewsPOC.Attributes;

/// <summary>
/// When applied to a component, embed, or content, it will be excluded when rendering the view.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class DoNotCaptureAttribute : Attribute { }