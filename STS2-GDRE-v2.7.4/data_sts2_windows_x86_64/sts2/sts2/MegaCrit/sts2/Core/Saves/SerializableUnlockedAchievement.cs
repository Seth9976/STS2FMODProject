using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Records an achievement unlock in the SerializableProgress file.
	/// Stores the achievement identifier as a raw string at the wire format boundary.
	/// Enum resolution happens in <see cref="M:MegaCrit.Sts2.Core.Saves.ProgressState.ParseAchievements(System.Collections.Generic.List{MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement},System.Collections.Generic.Dictionary{MegaCrit.Sts2.Core.Achievements.Achievement,System.Int64},System.Collections.Generic.List{MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement},MegaCrit.Sts2.Core.Saves.Validation.DeserializationContext)" />.
	/// </summary>
	// Token: 0x020000B4 RID: 180
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableUnlockedAchievement : IEquatable<SerializableUnlockedAchievement>
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x000413A5 File Offset: 0x0003F5A5
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(SerializableUnlockedAchievement);
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x000413B1 File Offset: 0x0003F5B1
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x000413B9 File Offset: 0x0003F5B9
		[JsonPropertyName("achievement")]
		public string Achievement { get; set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x000413C2 File Offset: 0x0003F5C2
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x000413CA File Offset: 0x0003F5CA
		[JsonPropertyName("unlock_time")]
		public long UnlockTime { get; set; }

		// Token: 0x060008A2 RID: 2210 RVA: 0x000413D4 File Offset: 0x0003F5D4
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SerializableUnlockedAchievement");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00041420 File Offset: 0x0003F620
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Achievement = ");
			builder.Append(this.Achievement);
			builder.Append(", UnlockTime = ");
			builder.Append(this.UnlockTime.ToString());
			return true;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00041473 File Offset: 0x0003F673
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(SerializableUnlockedAchievement left, SerializableUnlockedAchievement right)
		{
			return !(left == right);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0004147F File Offset: 0x0003F67F
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(SerializableUnlockedAchievement left, SerializableUnlockedAchievement right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00041493 File Offset: 0x0003F693
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Achievement>k__BackingField)) * -1521134295 + EqualityComparer<long>.Default.GetHashCode(this.<UnlockTime>k__BackingField);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x000414D3 File Offset: 0x0003F6D3
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SerializableUnlockedAchievement);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x000414E4 File Offset: 0x0003F6E4
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(SerializableUnlockedAchievement other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Achievement>k__BackingField, other.<Achievement>k__BackingField) && EqualityComparer<long>.Default.Equals(this.<UnlockTime>k__BackingField, other.<UnlockTime>k__BackingField));
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00041545 File Offset: 0x0003F745
		[CompilerGenerated]
		protected SerializableUnlockedAchievement(SerializableUnlockedAchievement original)
		{
			this.Achievement = original.<Achievement>k__BackingField;
			this.UnlockTime = original.<UnlockTime>k__BackingField;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00041565 File Offset: 0x0003F765
		public SerializableUnlockedAchievement()
		{
			this.Achievement = "";
			base..ctor();
		}
	}
}
