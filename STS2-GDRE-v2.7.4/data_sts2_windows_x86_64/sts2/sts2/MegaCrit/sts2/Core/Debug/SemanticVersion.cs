using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Debug
{
	/// <summary>
	/// Implementation of semver. Mostly used for comparing versions.
	/// https://semver.org
	/// </summary>
	// Token: 0x02000D16 RID: 3350
	[NullableContext(2)]
	[Nullable(0)]
	public class SemanticVersion : IComparable<SemanticVersion>, IEquatable<SemanticVersion>
	{
		// Token: 0x17002539 RID: 9529
		// (get) Token: 0x06008729 RID: 34601 RVA: 0x002D70F0 File Offset: 0x002D52F0
		public int Major { get; }

		// Token: 0x1700253A RID: 9530
		// (get) Token: 0x0600872A RID: 34602 RVA: 0x002D70F8 File Offset: 0x002D52F8
		public int Minor { get; }

		// Token: 0x1700253B RID: 9531
		// (get) Token: 0x0600872B RID: 34603 RVA: 0x002D7100 File Offset: 0x002D5300
		public int Patch { get; }

		// Token: 0x1700253C RID: 9532
		// (get) Token: 0x0600872C RID: 34604 RVA: 0x002D7108 File Offset: 0x002D5308
		[Nullable(new byte[] { 2, 1 })]
		public List<string> Prerelease
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
		}

		// Token: 0x1700253D RID: 9533
		// (get) Token: 0x0600872D RID: 34605 RVA: 0x002D7110 File Offset: 0x002D5310
		public string Metadata { get; }

		// Token: 0x0600872E RID: 34606 RVA: 0x002D7118 File Offset: 0x002D5318
		public SemanticVersion(int major, int minor, int patch, string metadata = null, [Nullable(new byte[] { 2, 1 })] List<string> prerelease = null)
		{
			this.Major = major;
			this.Minor = minor;
			this.Patch = patch;
			this.Metadata = metadata;
			this.Prerelease = prerelease;
		}

		// Token: 0x0600872F RID: 34607 RVA: 0x002D7148 File Offset: 0x002D5348
		[NullableContext(1)]
		public static bool TryFromString(string str, [Nullable(2)] out SemanticVersion version)
		{
			bool flag;
			try
			{
				version = SemanticVersion.FromString(str);
				flag = true;
			}
			catch (Exception ex) when (ex is InvalidOperationException || ex is FormatException)
			{
				version = null;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06008730 RID: 34608 RVA: 0x002D71A4 File Offset: 0x002D53A4
		[NullableContext(1)]
		public static SemanticVersion FromString(string version)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			List<string> list = null;
			string text = null;
			int num4 = 0;
			SemanticVersion.ParseState parseState = SemanticVersion.ParseState.None;
			for (int i = 0; i < version.Length; i++)
			{
				if (i == 0 && version[i] == 'v')
				{
					num4++;
				}
				else if (version[i] == '.')
				{
					if (parseState == SemanticVersion.ParseState.Major)
					{
						int num5 = num4;
						num = int.Parse(version.Substring(num5, i - num5));
						parseState = SemanticVersion.ParseState.Minor;
					}
					else if (parseState == SemanticVersion.ParseState.Minor)
					{
						int num5 = num4;
						num2 = int.Parse(version.Substring(num5, i - num5));
						parseState = SemanticVersion.ParseState.Patch;
					}
					else
					{
						if (parseState == SemanticVersion.ParseState.Patch)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Version ");
							defaultInterpolatedStringHandler.AppendFormatted(version);
							defaultInterpolatedStringHandler.AppendLiteral(" has a . in an invalid place! Parse state is ");
							defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion.ParseState>(parseState);
							defaultInterpolatedStringHandler.AppendLiteral(", index is ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(i);
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						if (parseState == SemanticVersion.ParseState.Prerelease)
						{
							List<string> list2 = list;
							int num5 = num4;
							list2.Add(version.Substring(num5, i - num5));
						}
					}
					num4 = i + 1;
				}
				else if (version[i] == '-')
				{
					if (parseState != SemanticVersion.ParseState.Patch)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Version ");
						defaultInterpolatedStringHandler.AppendFormatted(version);
						defaultInterpolatedStringHandler.AppendLiteral(" has a - in an invalid place! Parse state is ");
						defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion.ParseState>(parseState);
						defaultInterpolatedStringHandler.AppendLiteral(", index is ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(i);
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int num5 = num4;
					num3 = int.Parse(version.Substring(num5, i - num5));
					parseState = SemanticVersion.ParseState.Prerelease;
					list = new List<string>();
					num4 = i + 1;
				}
				else
				{
					if (version[i] == '+')
					{
						int num5;
						if (parseState == SemanticVersion.ParseState.Patch)
						{
							num5 = num4;
							num3 = int.Parse(version.Substring(num5, i - num5));
						}
						else
						{
							if (parseState != SemanticVersion.ParseState.Prerelease)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Version ");
								defaultInterpolatedStringHandler.AppendFormatted(version);
								defaultInterpolatedStringHandler.AppendLiteral(" has a + in an invalid place! Parse state is ");
								defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion.ParseState>(parseState);
								defaultInterpolatedStringHandler.AppendLiteral(", index is ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(i);
								throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							List<string> list3 = list;
							num5 = num4;
							list3.Add(version.Substring(num5, i - num5));
						}
						num5 = i + 1;
						text = version.Substring(num5, version.Length - num5);
						parseState = SemanticVersion.ParseState.Metadata;
						break;
					}
					if (parseState == SemanticVersion.ParseState.None)
					{
						parseState = SemanticVersion.ParseState.Major;
					}
				}
			}
			if (parseState == SemanticVersion.ParseState.Patch)
			{
				int num5 = num4;
				num3 = int.Parse(version.Substring(num5, version.Length - num5));
			}
			else if (parseState == SemanticVersion.ParseState.Prerelease)
			{
				List<string> list4 = list;
				int num5 = num4;
				list4.Add(version.Substring(num5, version.Length - num5));
			}
			else if (parseState != SemanticVersion.ParseState.Metadata)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Version terminated in an invalid place! Parse state is ");
				defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion.ParseState>(parseState);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new SemanticVersion(num, num2, num3, text, list);
		}

		// Token: 0x06008731 RID: 34609 RVA: 0x002D74B8 File Offset: 0x002D56B8
		public int CompareTo(SemanticVersion other)
		{
			if (this == other)
			{
				return 0;
			}
			if (other == null)
			{
				return 1;
			}
			int num = this.Major.CompareTo(other.Major);
			if (num != 0)
			{
				return num;
			}
			int num2 = this.Minor.CompareTo(other.Minor);
			if (num2 != 0)
			{
				return num2;
			}
			int num3 = this.Patch.CompareTo(other.Patch);
			if (num3 != 0)
			{
				return num3;
			}
			List<string> prerelease = this.Prerelease;
			int num4 = ((prerelease != null) ? prerelease.Count : 0);
			List<string> prerelease2 = other.Prerelease;
			int num5 = ((prerelease2 != null) ? prerelease2.Count : 0);
			if (num4 == 0 && num5 == 0)
			{
				return 0;
			}
			if (num4 > 0 && num5 == 0)
			{
				return -1;
			}
			if (num4 == 0 && num5 > 0)
			{
				return 1;
			}
			for (int i = 0; i < Math.Min(num4, num5); i++)
			{
				if (!(this.Prerelease[i] == other.Prerelease[i]))
				{
					int num6;
					bool flag = int.TryParse(this.Prerelease[i], out num6);
					int num7;
					bool flag2 = int.TryParse(other.Prerelease[i], out num7);
					if (!flag && !flag2)
					{
						return string.Compare(this.Prerelease[i], other.Prerelease[i], StringComparison.Ordinal);
					}
					if (flag && !flag2)
					{
						return -1;
					}
					if (!flag && flag2)
					{
						return 1;
					}
					if (flag && flag2)
					{
						return num6.CompareTo(num7);
					}
				}
			}
			return num4.CompareTo(num5);
		}

		// Token: 0x06008732 RID: 34610 RVA: 0x002D7628 File Offset: 0x002D5828
		public bool Equals(SemanticVersion other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06008733 RID: 34611 RVA: 0x002D7634 File Offset: 0x002D5834
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
			defaultInterpolatedStringHandler.AppendLiteral("v");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Major);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Minor);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Patch);
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			if (this.Prerelease != null)
			{
				text = text + "-" + string.Join(".", this.Prerelease);
			}
			if (this.Metadata != null)
			{
				text = text + "+" + this.Metadata;
			}
			return text;
		}

		// Token: 0x02002426 RID: 9254
		[NullableContext(0)]
		private enum ParseState
		{
			// Token: 0x040098B5 RID: 39093
			None,
			// Token: 0x040098B6 RID: 39094
			Major,
			// Token: 0x040098B7 RID: 39095
			Minor,
			// Token: 0x040098B8 RID: 39096
			Patch,
			// Token: 0x040098B9 RID: 39097
			Prerelease,
			// Token: 0x040098BA RID: 39098
			Metadata
		}
	}
}
