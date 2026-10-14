using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.MapDrawing
{
	/// <summary>
	/// Converts a SerializableMapDrawings instance into a base64-encoded string that can be saved into the JSON save file.
	/// When we serialize the map drawings to a multiplayer client, we serialize it directly as a binary blob. JSON can't
	/// do this and so this class exists as an adapter.
	/// The gains from gzipping the output haven't really been measured, so if we find that it's causing performance problems
	/// we can remove it.
	/// </summary>
	// Token: 0x020000F6 RID: 246
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	public class SerializableMapDrawingsJsonConverter : JsonConverter<SerializableMapDrawings>
	{
		// Token: 0x06000AB8 RID: 2744 RVA: 0x000496E8 File Offset: 0x000478E8
		public override SerializableMapDrawings Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			SerializableMapDrawings serializableMapDrawings;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream(Convert.FromBase64String(reader.GetString())))
				{
					using (GZipStream gzipStream = new GZipStream(memoryStream2, CompressionMode.Decompress))
					{
						gzipStream.CopyTo(memoryStream);
					}
				}
				PacketReader packetReader = new PacketReader();
				packetReader.Reset(memoryStream.ToArray());
				serializableMapDrawings = packetReader.Read<SerializableMapDrawings>();
			}
			return serializableMapDrawings;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00049784 File Offset: 0x00047984
		public override void Write(Utf8JsonWriter writer, SerializableMapDrawings mapDrawings, JsonSerializerOptions options)
		{
			PacketWriter packetWriter = SerializableMapDrawingsJsonConverter._packetWriter;
			string text;
			lock (packetWriter)
			{
				SerializableMapDrawingsJsonConverter._packetWriter.Reset();
				SerializableMapDrawingsJsonConverter._packetWriter.Write<SerializableMapDrawings>(mapDrawings);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Fastest, true))
					{
						gzipStream.Write(SerializableMapDrawingsJsonConverter._packetWriter.Buffer, 0, SerializableMapDrawingsJsonConverter._packetWriter.BytePosition);
					}
					text = Convert.ToBase64String(memoryStream.ToArray());
				}
			}
			writer.WriteStringValue(text);
		}

		// Token: 0x040003B4 RID: 948
		private static readonly PacketWriter _packetWriter = new PacketWriter();
	}
}
