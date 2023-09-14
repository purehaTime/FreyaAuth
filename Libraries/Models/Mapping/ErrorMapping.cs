using GrpcHelper.Proto.Common;
using Error = Models.Common.Error;

namespace Models.Mapping;

public static class ErrorMapping
{
    public static GrpcHelper.Proto.Common.Error ToGrpcData(this Error error)
    {
        return new GrpcHelper.Proto.Common.Error()
        {
            Code = error.Code,
            Level = (ErrorLevel)error.Level,
            Message = error.Message,
            StackTrace = error.StackTrace
        };
    }
}