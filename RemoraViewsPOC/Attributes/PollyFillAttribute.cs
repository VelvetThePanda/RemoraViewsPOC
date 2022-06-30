namespace RemoraViewsPOC.Attributes;

/// <summary>
/// Marks a view as being poly-filled. That is, if a property is not defined on the view, or is null, it will not be rendered.
/// This is only effective when using a view to update a message, in which the rendered view's properties will be an empty optional
/// where not set.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PollyFillAttribute : Attribute { }