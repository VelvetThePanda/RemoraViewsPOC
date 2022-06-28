using System.Collections.Concurrent;
using System.Reflection;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;
using Remora.Results;
using RemoraViewsPOC.Attributes;
using RemoraViewsPOC.Results;
using RemoraViewsPOC.Types;

namespace RemoraViewsPOC.Extensions;

internal static class ViewHelper
{
    /// <summary>
    /// Renders a given <see cref="IView"/>.
    /// </summary>
    /// <param name="view">The view to render.</param>
    /// <returns>The rendered view.</returns>
    public static Result<RenderedView> Render(this IView view)
    {
        var properties = view.GetType()
                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(prop => prop.GetCustomAttribute<DoNotCaptureAttribute>(true) is null);

        var contentProperty = properties.FirstOrDefault(prop => prop.PropertyType == typeof(string) && string.Equals(prop.Name, "Content", StringComparison.OrdinalIgnoreCase));

        if (contentProperty is null)
            contentProperty = properties.FirstOrDefault(prop => prop.PropertyType == typeof(string) && prop.GetCustomAttribute<ContentAttribute>() is not null);

        var embeds     = properties.Where(prop => prop.PropertyType.IsAssignableTo(typeof(IEmbed)));
        var components = properties.Where(prop => prop.PropertyType.IsAssignableTo(typeof(IMessageComponent)));

        embeds = OrderProperties(embeds);

        var orderedComponents = OrderChunkedProperties(embeds.Select((p, i) => (p, i)));
        
        var validationResult = ValidateComponentGroupings(orderedComponents);
        
        if (!validationResult.IsSuccess)
            return Result<RenderedView>.FromError(validationResult.Error);
        
        var contentValue = contentProperty?.GetValue(view) as string ?? string.Empty;

        var embedsValue = embeds.Select(prop => (IEmbed?)prop.GetValue(view))
                                .Where(prop => prop is not null)
                                .ToArray();
        
        var componentsValue = orderedComponents.Select
                                               (
                                                   props => props.Where(component => component is not null)
                                                                 .Select(prop => (IMessageComponent)prop.GetValue(view))
                                                                 .ToArray()
                                               )
                                               .Select(components => new ActionRowComponent(components))
                                               .ToArray();
        
        var returnContent    = string.IsNullOrEmpty(contentValue) ? default(Optional<string>) : contentValue;
        var returnEmbeds     = embedsValue.Length == 0 ? default(Optional<IReadOnlyList<IEmbed>>) : embedsValue;
        var returnComponents = componentsValue.Length == 0 ? default(Optional<IReadOnlyList<IMessageComponent>>) : componentsValue;

        
        
        return Result<RenderedView>.FromSuccess(new RenderedView(returnContent, returnEmbeds, returnComponents));
    }
    
    private static Result ValidateComponentGroupings(IEnumerable<IEnumerable<PropertyInfo>> orderedComponents)
    {
        var row = 0;
        var column = 0;
        
        if (orderedComponents.Count() > 5)
            return Result.FromError(new TooManyComponentsError());

        foreach (var componentList in orderedComponents)
        {
            row++;
            column = 0;
            
            foreach (var component in componentList)
            {
                if (component.PropertyType.IsAssignableTo(typeof(ISelectMenuComponent)) || component.PropertyType.IsAssignableTo(typeof(ITextInputComponent)))
                {
                    if (column > 0)
                    {
                        return Result.FromError(new TooManyComponentsError($"The component (defined as {component.Name}) is an exclusive component and requires it's own row. " +
                                                                           $"Consider specifying the row with {nameof(RowAttribute)} on the property.."));
                    }
                }

                column++;
                
                if (column > 5)
                    return Result.FromError(new TooManyComponentsError($"The component (defined as {component.Name}) exceeds the maximum number of columns (5)"));
            }
        }
            
        return Result.FromSuccess();
    }

    private static IEnumerable<PropertyInfo> OrderProperties(IEnumerable<PropertyInfo> properties)
    {
        var ordered = new List<PropertyInfo>(properties.Count());
        
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<OrderAttribute>();
            
            if (attribute is null)
            {
                ordered.Add(property);
                continue;
            }
            
            ordered.Insert((int)attribute.Order, property);            
        }

        return ordered;
    }
    
    private static IEnumerable<IEnumerable<PropertyInfo>> OrderChunkedProperties(IEnumerable<(PropertyInfo Property, int Index)> properties)
    {
        var ordered = new ConcurrentDictionary<int, List<PropertyInfo>>();
        
        foreach (var prop in properties)
        {
            // This requires that buttons in the same row be explicitly stated to be in the same row,
            // but perhaps that's not a bad thing, since you can mix and match any valid components here.
            var rowIndex = prop.Property.GetCustomAttribute<RowAttribute>()?.Row ?? prop.Index;
            
            var row = ordered.GetOrAdd((int)rowIndex, new List<PropertyInfo>());

            if (prop.Property.GetCustomAttribute<OrderAttribute>()?.Order is { } order)
            {
                row.Insert((int)order, prop.Property);
            }
            else
            {
                row.Add(prop.Property);
            }
        }

        return ordered.Values;
    }
}