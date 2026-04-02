using System.Text.Json;

namespace KameliaRecycleSystem.Utilities.Helpers;

public class SerializationHelper
{
    public string Serialize<T>(T value) => JsonSerializer.Serialize(value);
}
