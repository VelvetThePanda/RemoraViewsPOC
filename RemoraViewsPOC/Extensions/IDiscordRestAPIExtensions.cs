using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;
using Remora.Results;
using RemoraViewsPOC.Attributes;
using RemoraViewsPOC.Types;

namespace RemoraViewsPOC.Extensions;

public static class IDiscordRestAPIExtensions
{
    public static Task<Result<IMessage>> CreateMessageAsync
    (
        this IDiscordRestChannelAPI channels,
        Snowflake channelID,
        IView view,
        CancellationToken ct = default
    )
    {
        var viewResult = ViewRenderer.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result<IMessage>.FromError(viewResult.Error));
        }
        
        var renderedView = viewResult.Entity;
        
        return channels.CreateMessageAsync
        (
            channelID,
            renderedView.Content, 
            embeds: renderedView.Embeds,
            components: renderedView.Components,
            ct: ct
        );
    }
    
    public static Task<Result<IMessage>> EditMessageAsync
    (
        this IDiscordRestChannelAPI channels,
        Snowflake channelID,
        Snowflake messageID,
        IView view,
        CancellationToken ct = default
    )
    {
        var viewResult = ViewRenderer.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result<IMessage>.FromError(viewResult.Error));
        }
        
        var renderedView = viewResult.Entity;
        
        return channels.EditMessageAsync(channelID, messageID, renderedView.Content, embeds: renderedView.Embeds, components: renderedView.Components, ct: ct);
    }
    
    
    public static Task<Result> CreateInteractionResponseAsync
    (
        this IDiscordRestInteractionAPI interactions,
        Snowflake interactionID,
        string interactionToken,
        InteractionCallbackType type,
        IView view,
        bool ephemeral = false,
        CancellationToken ct = default
    )
    {
        var viewResult = ViewRenderer.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result.FromError(viewResult.Error));
        }
        
        var renderedView = viewResult.Entity;

        var response = new InteractionResponse
        (
            type, 
            new(new InteractionMessageCallbackData(Content: renderedView.Content, Embeds: renderedView.Embeds, Components: renderedView.Components))
        );
        
        return interactions.CreateInteractionResponseAsync(interactionID, interactionToken, response, ct: ct);
    }
    
    public static Task<Result<IMessage>> EditOriginalInteractionResponseAsync
    (
        this IDiscordRestInteractionAPI interactions,
        Snowflake applicationID,
        string interactionToken,
        IView view,
        CancellationToken ct = default
    )
    {
        var viewResult = ViewRenderer.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result<IMessage>.FromError(viewResult.Error));
        }
        
        var renderedView = viewResult.Entity;

        return interactions.EditOriginalInteractionResponseAsync
        (
            applicationID, 
            interactionToken,
            renderedView.Content,
            embeds: renderedView.Embeds, 
            components: renderedView.Components,
            ct: ct
        );
    }
    
    public static Task<Result<IMessage>> CreateFollowupMessageAsync(this IDiscordRestInteractionAPI interactions, Snowflake interactionID, string interactionToken, IView view, CancellationToken ct = default)
    {
        var viewResult = ViewRenderer.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result<IMessage>.FromError(viewResult.Error));
        }
        
        var renderedView = viewResult.Entity;
        
        return interactions.CreateFollowupMessageAsync
        (
            interactionID,
            interactionToken,
            renderedView.Content,
            embeds: renderedView.Embeds,
            components: renderedView.Components,
            ct: ct
        );
    }
    
    public static Task<Result<IMessage>> EditFollowupMessageAsync
    (
        this IDiscordRestInteractionAPI interactions,
        Snowflake interactionID,
        string interactionToken,
        Snowflake messageID,
        IView view,
        CancellationToken ct = default
    )
    {
        var viewResult = ViewRenderer.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result<IMessage>.FromError(viewResult.Error));
        }
        
        var renderedView = viewResult.Entity;
        
        return interactions.EditFollowupMessageAsync
        (
            interactionID, 
            interactionToken,
            messageID,
            renderedView.Content,
            embeds: renderedView.Embeds,
            components: renderedView.Components,
            ct: ct
        );
    }

    public static Task<Result<IMessage>> ExecuteWebhookAsync
    (
        this IDiscordRestWebhookAPI webhooks,
        Snowflake webhookID, 
        string webhookToken,
        IView view,
        Optional<string> username = default,
        Optional<string> avatarUrl = default, 
        CancellationToken ct = default
    )
    {
        var viewResult = ViewRenderer.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result<IMessage>.FromError(viewResult.Error));
        }

        var renderedView = viewResult.Entity;
        
        return webhooks.ExecuteWebhookAsync
        (
            webhookID,
            webhookToken,
            content: renderedView.Content,
            embeds: renderedView.Embeds,
            components: renderedView.Components, 
            username: username,
            avatarUrl: avatarUrl,
            ct: ct
        );
    }
    
    public static Task<Result<IMessage>> EditWebhookMessageAsync
    (
        this IDiscordRestWebhookAPI webhooks,
        Snowflake webhookId,
        string webhookToken,
        Snowflake messageID,
        IView view
    )
    {
        var viewResult = ViewRenderer.Render(view);

        if (!viewResult.IsSuccess)
        {
            return Task.FromResult(Result<IMessage>.FromError(viewResult.Error));
        }

        var renderedView = viewResult.Entity;
        
        return webhooks.EditWebhookMessageAsync
        (
            webhookId, 
            webhookToken,
            messageID,
            content: renderedView.Content,
            embeds: renderedView.Embeds,
            components: renderedView.Components
        );
    }
}