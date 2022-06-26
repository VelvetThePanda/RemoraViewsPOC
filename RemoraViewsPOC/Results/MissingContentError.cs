using Remora.Results;

namespace RemoraViewsPOC.Results;

public record MissingContentError(string Message = "Content is required, but was not provided.") : ResultError(Message); 