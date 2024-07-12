using System.Text.Json;
using System.Text.Json.Serialization;

namespace OllamaCommitGen.OllamaSharp;

public class StopOptionJsonConverter : JsonConverter<string>
{
	public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.StartArray)
		{
			var elements = new List<string>();
			
			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndArray)
					break;

				if (reader.TokenType == JsonTokenType.String)
				{
					elements.Add(reader.GetString()!);
				}
			}
			
			return string.Join("|", elements);
		}
        
		throw new JsonException("Expected start of array to convert to pipe-separated string");
	}

	public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
	{
		var values = value.Split('|');
		writer.WriteStartArray();
		
		foreach (var val in values)
		{
			writer.WriteStringValue(val);
		}
		
		writer.WriteEndArray();
	}
}