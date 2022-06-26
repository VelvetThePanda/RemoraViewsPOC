using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest.Core;

namespace RemoraViewsPOC.Types;

/// <summary>
/// A record containing extracted content, embeds, and components from a view.
/// </summary>
/// <param name="Content"></param>
/// <param name="Embeds"></param>
/// <param name="Components"></param>
internal record RenderedView(Optional<string> Content, Optional<IReadOnlyList<IEmbed>> Embeds, Optional<IReadOnlyList<IMessageComponent>> Components);