using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004E5 RID: 1253
	[NullableContext(1)]
	[Nullable(0)]
	public class ModelId : IComparable<ModelId>, IEquatable<ModelId>
	{
		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004D25 RID: 19749 RVA: 0x0022DBE5 File Offset: 0x0022BDE5
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModelId);
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06004D26 RID: 19750 RVA: 0x0022DBF1 File Offset: 0x0022BDF1
		public string Category { get; }

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06004D27 RID: 19751 RVA: 0x0022DBF9 File Offset: 0x0022BDF9
		public string Entry { get; }

		// Token: 0x06004D28 RID: 19752 RVA: 0x0022DC01 File Offset: 0x0022BE01
		public ModelId(string category, string entry)
		{
			if (category.EndsWith("_MODEL"))
			{
				throw new ArgumentException("Category cannot end with '_MODEL'.", "category");
			}
			this.Category = category;
			this.Entry = entry;
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x0022DC34 File Offset: 0x0022BE34
		public static ModelId Deserialize(string json)
		{
			string[] array = json.Split('.', StringSplitOptions.None);
			if (array.Length != 2)
			{
				throw new JsonException("'" + json + "' does not match the expected ModelId form.");
			}
			return new ModelId(array[0], array[1]);
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x0022DC72 File Offset: 0x0022BE72
		public override string ToString()
		{
			return this.Category + "." + this.Entry;
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x0022DC8C File Offset: 0x0022BE8C
		[NullableContext(2)]
		public int CompareTo(ModelId other)
		{
			int num = string.Compare(this.Category, (other != null) ? other.Category : null, StringComparison.Ordinal);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(this.Entry, (other != null) ? other.Entry : null, StringComparison.Ordinal);
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x0022DCCF File Offset: 0x0022BECF
		public static string SlugifyCategory<[Nullable(2)] T>()
		{
			return ModelId.SlugifyCategory(typeof(T).Name);
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x0022DCE8 File Offset: 0x0022BEE8
		public static string SlugifyCategory(string category)
		{
			string text = StringHelper.Slugify(category);
			if (text.EndsWith("_MODEL"))
			{
				string text2 = text;
				int length = "_MODEL".Length;
				text = text2.Substring(0, text2.Length - length);
			}
			return text;
		}

		// Token: 0x06004D2E RID: 19758 RVA: 0x0022DD27 File Offset: 0x0022BF27
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Category = ");
			builder.Append(this.Category);
			builder.Append(", Entry = ");
			builder.Append(this.Entry);
			return true;
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x0022DD61 File Offset: 0x0022BF61
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModelId left, ModelId right)
		{
			return !(left == right);
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x0022DD6D File Offset: 0x0022BF6D
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModelId left, ModelId right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x0022DD81 File Offset: 0x0022BF81
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Category>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Entry>k__BackingField);
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x0022DDC1 File Offset: 0x0022BFC1
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModelId);
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x0022DDD0 File Offset: 0x0022BFD0
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(ModelId other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Category>k__BackingField, other.<Category>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Entry>k__BackingField, other.<Entry>k__BackingField));
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x0022DE31 File Offset: 0x0022C031
		[CompilerGenerated]
		protected ModelId(ModelId original)
		{
			this.Category = original.<Category>k__BackingField;
			this.Entry = original.<Entry>k__BackingField;
		}

		// Token: 0x04001D58 RID: 7512
		public static readonly ModelId none = new ModelId("NONE", "NONE");

		// Token: 0x04001D59 RID: 7513
		private const string _bannedSuffix = "_MODEL";
	}
}
