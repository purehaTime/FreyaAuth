using Models.Enums;

namespace Models.Common;

public class Error
{
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public string MethodName { get; set; }
    public int Code { get; set; }
    public ErrorLevel Level { get; set; }
}