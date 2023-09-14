using ConfigHelper.Configs;

namespace ConfigHelper.Interfaces;

public interface IConfigHelper
{
    public DatabaseSettings DatabaseSettings { get; }
}