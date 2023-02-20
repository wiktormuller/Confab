namespace Confab.Shared.Infrastructure.Modules;

internal interface IModuleSerializer
{
    byte[] Serialize<T>(T value);
    object Deserialize(byte[] value, Type type);
    T Deserialize<T>(byte[] value);
}