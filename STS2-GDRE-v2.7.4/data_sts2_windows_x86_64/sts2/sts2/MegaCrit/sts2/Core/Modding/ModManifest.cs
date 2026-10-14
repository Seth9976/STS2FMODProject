using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Modding
{
	/// <summary>
	/// Represents the JSON file which defines mod manifests.
	/// </summary>
	// Token: 0x02000B9A RID: 2970
	[NullableContext(2)]
	[Nullable(0)]
	public class ModManifest : IEquatable<ModManifest>
	{
		// Token: 0x1700225C RID: 8796
		// (get) Token: 0x06007BF6 RID: 31734 RVA: 0x002A4190 File Offset: 0x002A2390
		[Nullable(1)]
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[NullableContext(1)]
			[CompilerGenerated]
			get
			{
				return typeof(ModManifest);
			}
		}

		// Token: 0x06007BF7 RID: 31735 RVA: 0x002A419C File Offset: 0x002A239C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static ModManifest ReadFromStream(Stream stream, [Nullable(new byte[] { 2, 1 })] out List<LocString> errors)
		{
			errors = null;
			JsonNode jsonNode = JsonNode.Parse(stream, null, default(JsonDocumentOptions));
			if (jsonNode == null)
			{
				return null;
			}
			JsonNode jsonNode2 = jsonNode["dependencies"];
			if (jsonNode2 != null && jsonNode2.GetValueKind() == JsonValueKind.Array)
			{
				JsonNode jsonNode3 = jsonNode2.AsArray().FirstOrDefault<JsonNode>();
				if (jsonNode3 != null && jsonNode3.GetValueKind() == JsonValueKind.String)
				{
					Log.Error("Detected old-style dependencies without min version specified! It works for now but this will be removed in a future release. Let the mod author know.", 2);
					LocString locString = new LocString("main_menu_ui", "MOD_ERROR.MIGRATION_REQUIRED");
					LocString locString2 = locString;
					string text = "id";
					JsonNode jsonNode4 = jsonNode["id"];
					locString2.Add(text, ((jsonNode4 != null) ? jsonNode4.GetValue<string>() : null) ?? "<null>");
					if (errors == null)
					{
						errors = new List<LocString>();
					}
					errors.Add(locString);
					JsonArray jsonArray = new JsonArray(null);
					foreach (JsonNode jsonNode5 in jsonNode2.AsArray())
					{
						if (jsonNode5 != null)
						{
							jsonArray.Add<JsonObject>(new JsonObject(new <>z__ReadOnlyArray<KeyValuePair<string, JsonNode>>(new KeyValuePair<string, JsonNode>[]
							{
								new KeyValuePair<string, JsonNode>("id", jsonNode5.GetValue<string>()),
								new KeyValuePair<string, JsonNode>("min_version", null)
							}), null));
						}
					}
					jsonNode["dependencies"] = jsonArray;
				}
			}
			return jsonNode.Deserialize<ModManifest>(JsonSerializationUtility.GetTypeInfo<ModManifest>());
		}

		// Token: 0x06007BF8 RID: 31736 RVA: 0x002A4320 File Offset: 0x002A2520
		[NullableContext(1)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModManifest");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06007BF9 RID: 31737 RVA: 0x002A436C File Offset: 0x002A256C
		[NullableContext(1)]
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("id = ");
			builder.Append(this.id);
			builder.Append(", name = ");
			builder.Append(this.name);
			builder.Append(", author = ");
			builder.Append(this.author);
			builder.Append(", description = ");
			builder.Append(this.description);
			builder.Append(", version = ");
			builder.Append(this.version);
			builder.Append(", hasPck = ");
			builder.Append(this.hasPck.ToString());
			builder.Append(", hasDll = ");
			builder.Append(this.hasDll.ToString());
			builder.Append(", dependencies = ");
			builder.Append(this.dependencies);
			builder.Append(", affectsGameplay = ");
			builder.Append(this.affectsGameplay.ToString());
			builder.Append(", minGameVersion = ");
			builder.Append(this.minGameVersion);
			return true;
		}

		// Token: 0x06007BFA RID: 31738 RVA: 0x002A449A File Offset: 0x002A269A
		[CompilerGenerated]
		public static bool operator !=(ModManifest left, ModManifest right)
		{
			return !(left == right);
		}

		// Token: 0x06007BFB RID: 31739 RVA: 0x002A44A6 File Offset: 0x002A26A6
		[CompilerGenerated]
		public static bool operator ==(ModManifest left, ModManifest right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06007BFC RID: 31740 RVA: 0x002A44BC File Offset: 0x002A26BC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.id)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.name)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.author)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.description)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.version)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.hasPck)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.hasDll)) * -1521134295 + EqualityComparer<List<ModDependency>>.Default.GetHashCode(this.dependencies)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.affectsGameplay)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.minGameVersion);
		}

		// Token: 0x06007BFD RID: 31741 RVA: 0x002A45BF File Offset: 0x002A27BF
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModManifest);
		}

		// Token: 0x06007BFE RID: 31742 RVA: 0x002A45D0 File Offset: 0x002A27D0
		[CompilerGenerated]
		public virtual bool Equals(ModManifest other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.id, other.id) && EqualityComparer<string>.Default.Equals(this.name, other.name) && EqualityComparer<string>.Default.Equals(this.author, other.author) && EqualityComparer<string>.Default.Equals(this.description, other.description) && EqualityComparer<string>.Default.Equals(this.version, other.version) && EqualityComparer<bool>.Default.Equals(this.hasPck, other.hasPck) && EqualityComparer<bool>.Default.Equals(this.hasDll, other.hasDll) && EqualityComparer<List<ModDependency>>.Default.Equals(this.dependencies, other.dependencies) && EqualityComparer<bool>.Default.Equals(this.affectsGameplay, other.affectsGameplay) && EqualityComparer<string>.Default.Equals(this.minGameVersion, other.minGameVersion));
		}

		// Token: 0x06007C00 RID: 31744 RVA: 0x002A4708 File Offset: 0x002A2908
		[CompilerGenerated]
		protected ModManifest([Nullable(1)] ModManifest original)
		{
			this.id = original.id;
			this.name = original.name;
			this.author = original.author;
			this.description = original.description;
			this.version = original.version;
			this.hasPck = original.hasPck;
			this.hasDll = original.hasDll;
			this.dependencies = original.dependencies;
			this.affectsGameplay = original.affectsGameplay;
			this.minGameVersion = original.minGameVersion;
		}

		// Token: 0x06007C01 RID: 31745 RVA: 0x002A4793 File Offset: 0x002A2993
		public ModManifest()
		{
			this.affectsGameplay = true;
			base..ctor();
		}

		// Token: 0x0400293C RID: 10556
		[JsonPropertyName("id")]
		public string id;

		// Token: 0x0400293D RID: 10557
		[JsonPropertyName("name")]
		public string name;

		// Token: 0x0400293E RID: 10558
		[JsonPropertyName("author")]
		public string author;

		// Token: 0x0400293F RID: 10559
		[JsonPropertyName("description")]
		public string description;

		// Token: 0x04002940 RID: 10560
		[JsonPropertyName("version")]
		public string version;

		// Token: 0x04002941 RID: 10561
		[JsonPropertyName("has_pck")]
		public bool hasPck;

		// Token: 0x04002942 RID: 10562
		[JsonPropertyName("has_dll")]
		public bool hasDll;

		// Token: 0x04002943 RID: 10563
		[Nullable(new byte[] { 2, 1 })]
		[JsonPropertyName("dependencies")]
		public List<ModDependency> dependencies;

		// Token: 0x04002944 RID: 10564
		[JsonPropertyName("affects_gameplay")]
		public bool affectsGameplay;

		// Token: 0x04002945 RID: 10565
		[JsonPropertyName("min_game_version")]
		public string minGameVersion;
	}
}
