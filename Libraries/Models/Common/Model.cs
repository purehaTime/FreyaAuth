using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace Models.Common;

public class Model<T>
{
    public T Data { get; set; }
    public Error Error { get; set; }
}