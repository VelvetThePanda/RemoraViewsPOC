﻿using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;
using Remora.Results;
using RemoraViewsPOC.Attributes;
using RemoraViewsPOC.Types;

namespace RemoraViewsPOC.Extensions;

public static class IDiscordRestAPIExtensions
{
    public static Task<Result<IMessage>> CreateMessageAsync(this IDiscordRestChannelAPI channels, Snowflake channelID, IView view, CancellationToken ct = default)
    {
        var viewResult = ViewHelper.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result<IMessage>.FromError(viewResult.Error));
        }
        
        var renderedView = viewResult.Entity;
        
        return channels.CreateMessageAsync(channelID, renderedView.Content, embeds: renderedView.Embeds, components: renderedView.Components, ct: ct);
    }
    
    public static Task<Result> CreateInteractionResponseAsync(this IDiscordRestInteractionAPI interactions, Snowflake interactionID, string interactionToken, IView view, bool ephemeral = false, CancellationToken ct = default)
    {
        var viewResult = ViewHelper.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result.FromError(viewResult.Error));
        }
        
        var renderedView = viewResult.Entity;

        var response = new InteractionResponse
        (
            InteractionCallbackType.DeferredChannelMessageWithSource, 
            new(new InteractionMessageCallbackData(Content: renderedView.Content, Embeds: renderedView.Embeds, Components: renderedView.Components))
        );
        
        return interactions.CreateInteractionResponseAsync(interactionID, interactionToken, response, ct: ct);
    }
    
    public static Task<Result<IMessage>> CreateFollowupMessageAsync(this IDiscordRestInteractionAPI interactions, Snowflake interactionID, string interactionToken, IView view, CancellationToken ct = default)
    {
        var viewResult = ViewHelper.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result<IMessage>.FromError(viewResult.Error));
        }
        
        var renderedView = viewResult.Entity;
        
        return interactions.CreateFollowupMessageAsync(interactionID, interactionToken, renderedView.Content, embeds: renderedView.Embeds, components: renderedView.Components, ct: ct);
    }
}