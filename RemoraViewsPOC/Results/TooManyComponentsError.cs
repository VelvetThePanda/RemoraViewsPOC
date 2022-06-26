using Remora.Results;

namespace RemoraViewsPOC.Results;

public record TooManyComponentsError(string Message = "There were too manay components in the given view.") : ResultError(Message);