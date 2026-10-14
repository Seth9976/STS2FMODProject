using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using Godot.Collections;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Nodes;

namespace MegaCrit.Sts2.Core.AutoSlay
{
	/// <summary>
	/// Captures memory and resource snapshots during AutoSlay runs, logging deltas
	/// from a baseline to detect memory/resource leaks.
	/// </summary>
	// Token: 0x02000D6D RID: 3437
	[NullableContext(1)]
	[Nullable(0)]
	public static class MemoryProfiler
	{
		// Token: 0x06008AF1 RID: 35569 RVA: 0x002E60D8 File Offset: 0x002E42D8
		public static void SetBaseline()
		{
			MemoryProfiler.MemorySnapshot memorySnapshot = MemoryProfiler.Capture();
			MemoryProfiler._baseline = new MemoryProfiler.MemorySnapshot?(memorySnapshot);
			MemoryProfiler._previous = new MemoryProfiler.MemorySnapshot?(memorySnapshot);
			MemoryProfiler.LogLine("baseline", memorySnapshot, memorySnapshot);
		}

		// Token: 0x06008AF2 RID: 35570 RVA: 0x002E6110 File Offset: 0x002E4310
		public static void LogSnapshot(string context)
		{
			MemoryProfiler.MemorySnapshot memorySnapshot = MemoryProfiler.Capture();
			MemoryProfiler.MemorySnapshot valueOrDefault = MemoryProfiler._baseline.GetValueOrDefault(memorySnapshot);
			MemoryProfiler.LogLine(context, memorySnapshot, valueOrDefault);
			MemoryProfiler._previous = new MemoryProfiler.MemorySnapshot?(memorySnapshot);
		}

		// Token: 0x06008AF3 RID: 35571 RVA: 0x002E6142 File Offset: 0x002E4342
		public static void Reset()
		{
			MemoryProfiler._baseline = null;
			MemoryProfiler._previous = null;
			MemoryProfiler._snapshotCount = 0;
		}

		// Token: 0x06008AF4 RID: 35572 RVA: 0x002E6160 File Offset: 0x002E4360
		private static MemoryProfiler.MemorySnapshot Capture()
		{
			NGame instance = NGame.Instance;
			Window window;
			if (instance == null)
			{
				window = null;
			}
			else
			{
				SceneTree tree = instance.GetTree();
				window = ((tree != null) ? tree.Root : null);
			}
			Window window2 = window;
			int num = 0;
			if (window2 != null)
			{
				Array<Dictionary> signalConnectionList = window2.GetSignalConnectionList(Viewport.SignalName.SizeChanged);
				num = signalConnectionList.Count;
			}
			return new MemoryProfiler.MemorySnapshot(OS.GetStaticMemoryUsage(), RenderingServer.GetRenderingInfo(5L), GC.GetTotalMemory(false), (int)Performance.GetMonitor(7L), (int)Performance.GetMonitor(8L), (int)Performance.GetMonitor(9L), (int)Performance.GetMonitor(10L), PreloadManager.Cache.GetCacheKeys().Count<string>(), PreloadManager.Cache.MissedCacheAssetCount, num, GC.CollectionCount(0), GC.CollectionCount(1), GC.CollectionCount(2));
		}

		// Token: 0x06008AF5 RID: 35573 RVA: 0x002E6208 File Offset: 0x002E4408
		private static void LogLine(string context, MemoryProfiler.MemorySnapshot current, MemoryProfiler.MemorySnapshot baseline)
		{
			MemoryProfiler.MemorySnapshot valueOrDefault = MemoryProfiler._previous.GetValueOrDefault(current);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(168, 27);
			defaultInterpolatedStringHandler.AppendLiteral("[MemProfile] context=");
			defaultInterpolatedStringHandler.AppendFormatted(context);
			defaultInterpolatedStringHandler.AppendLiteral(" | ");
			defaultInterpolatedStringHandler.AppendLiteral("StaticMem=");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Fmt(current.StaticMemBytes));
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.StaticMemBytes, baseline.StaticMemBytes));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("VRAM=");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Fmt(current.VramBytes));
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.VramBytes, baseline.VramBytes));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("GcMem=");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Fmt((ulong)current.GcTotalMemory));
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff((ulong)current.GcTotalMemory, (ulong)baseline.GcTotalMemory));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("Objects=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.ObjectCount);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.ObjectCount, baseline.ObjectCount));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("Resources=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.ResourceCount);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.ResourceCount, baseline.ResourceCount));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("Nodes=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.NodeCount);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.NodeCount, baseline.NodeCount));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("Orphans=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.OrphanNodeCount);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.OrphanNodeCount, baseline.OrphanNodeCount));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("CachedAssets=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.CachedAssets);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.CachedAssets, baseline.CachedAssets));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("MissedCache=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.MissedCacheAssets);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.MissedCacheAssets, baseline.MissedCacheAssets));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("RootSizeSignals=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.RootViewportSignals);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.RootViewportSignals, baseline.RootViewportSignals));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("GC0=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.GcGen0);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.GcGen0, baseline.GcGen0));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("GC1=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.GcGen1);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.GcGen1, baseline.GcGen1));
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("GC2=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(current.GcGen2);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.GcGen2, baseline.GcGen2));
			defaultInterpolatedStringHandler.AppendLiteral(")");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			if (MemoryProfiler._snapshotCount >= 2)
			{
				string text2 = text;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 6);
				defaultInterpolatedStringHandler.AppendLiteral(" | room-delta: StaticMem=");
				defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.StaticMemBytes, valueOrDefault.StaticMemBytes));
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendLiteral("VRAM=");
				defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.VramBytes, valueOrDefault.VramBytes));
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendLiteral("GcMem=");
				defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff((ulong)current.GcTotalMemory, (ulong)valueOrDefault.GcTotalMemory));
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendLiteral("Objects=");
				defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.ObjectCount, valueOrDefault.ObjectCount));
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendLiteral("Nodes=");
				defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.NodeCount, valueOrDefault.NodeCount));
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendLiteral("Orphans=");
				defaultInterpolatedStringHandler.AppendFormatted(MemoryProfiler.Diff(current.OrphanNodeCount, valueOrDefault.OrphanNodeCount));
				text = text2 + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			MemoryProfiler._snapshotCount++;
			AutoSlayLog.Info(text);
		}

		// Token: 0x06008AF6 RID: 35574 RVA: 0x002E6797 File Offset: 0x002E4997
		private static string Fmt(ulong bytes)
		{
			return NGame.FormatBytes(bytes);
		}

		// Token: 0x06008AF7 RID: 35575 RVA: 0x002E67A0 File Offset: 0x002E49A0
		private static string Diff(ulong current, ulong baseline)
		{
			long num = (long)(current - baseline);
			if (num < 0L)
			{
				return "-" + NGame.FormatBytes((ulong)(-(ulong)num));
			}
			return "+" + NGame.FormatBytes((ulong)num);
		}

		// Token: 0x06008AF8 RID: 35576 RVA: 0x002E67D8 File Offset: 0x002E49D8
		private static string Diff(int current, int baseline)
		{
			int num = current - baseline;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num < 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler.AppendLiteral("+");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400303A RID: 12346
		private static MemoryProfiler.MemorySnapshot? _baseline;

		// Token: 0x0400303B RID: 12347
		private static MemoryProfiler.MemorySnapshot? _previous;

		// Token: 0x0400303C RID: 12348
		private static int _snapshotCount;

		// Token: 0x02002516 RID: 9494
		[NullableContext(0)]
		private struct MemorySnapshot : IEquatable<MemoryProfiler.MemorySnapshot>
		{
			// Token: 0x0600C530 RID: 50480 RVA: 0x00444524 File Offset: 0x00442724
			public MemorySnapshot(ulong StaticMemBytes, ulong VramBytes, long GcTotalMemory, int ObjectCount, int ResourceCount, int NodeCount, int OrphanNodeCount, int CachedAssets, int MissedCacheAssets, int RootViewportSignals, int GcGen0, int GcGen1, int GcGen2)
			{
				this.StaticMemBytes = StaticMemBytes;
				this.VramBytes = VramBytes;
				this.GcTotalMemory = GcTotalMemory;
				this.ObjectCount = ObjectCount;
				this.ResourceCount = ResourceCount;
				this.NodeCount = NodeCount;
				this.OrphanNodeCount = OrphanNodeCount;
				this.CachedAssets = CachedAssets;
				this.MissedCacheAssets = MissedCacheAssets;
				this.RootViewportSignals = RootViewportSignals;
				this.GcGen0 = GcGen0;
				this.GcGen1 = GcGen1;
				this.GcGen2 = GcGen2;
			}

			// Token: 0x170026E1 RID: 9953
			// (get) Token: 0x0600C531 RID: 50481 RVA: 0x00444596 File Offset: 0x00442796
			// (set) Token: 0x0600C532 RID: 50482 RVA: 0x0044459E File Offset: 0x0044279E
			public ulong StaticMemBytes { readonly get; set; }

			// Token: 0x170026E2 RID: 9954
			// (get) Token: 0x0600C533 RID: 50483 RVA: 0x004445A7 File Offset: 0x004427A7
			// (set) Token: 0x0600C534 RID: 50484 RVA: 0x004445AF File Offset: 0x004427AF
			public ulong VramBytes { readonly get; set; }

			// Token: 0x170026E3 RID: 9955
			// (get) Token: 0x0600C535 RID: 50485 RVA: 0x004445B8 File Offset: 0x004427B8
			// (set) Token: 0x0600C536 RID: 50486 RVA: 0x004445C0 File Offset: 0x004427C0
			public long GcTotalMemory { readonly get; set; }

			// Token: 0x170026E4 RID: 9956
			// (get) Token: 0x0600C537 RID: 50487 RVA: 0x004445C9 File Offset: 0x004427C9
			// (set) Token: 0x0600C538 RID: 50488 RVA: 0x004445D1 File Offset: 0x004427D1
			public int ObjectCount { readonly get; set; }

			// Token: 0x170026E5 RID: 9957
			// (get) Token: 0x0600C539 RID: 50489 RVA: 0x004445DA File Offset: 0x004427DA
			// (set) Token: 0x0600C53A RID: 50490 RVA: 0x004445E2 File Offset: 0x004427E2
			public int ResourceCount { readonly get; set; }

			// Token: 0x170026E6 RID: 9958
			// (get) Token: 0x0600C53B RID: 50491 RVA: 0x004445EB File Offset: 0x004427EB
			// (set) Token: 0x0600C53C RID: 50492 RVA: 0x004445F3 File Offset: 0x004427F3
			public int NodeCount { readonly get; set; }

			// Token: 0x170026E7 RID: 9959
			// (get) Token: 0x0600C53D RID: 50493 RVA: 0x004445FC File Offset: 0x004427FC
			// (set) Token: 0x0600C53E RID: 50494 RVA: 0x00444604 File Offset: 0x00442804
			public int OrphanNodeCount { readonly get; set; }

			// Token: 0x170026E8 RID: 9960
			// (get) Token: 0x0600C53F RID: 50495 RVA: 0x0044460D File Offset: 0x0044280D
			// (set) Token: 0x0600C540 RID: 50496 RVA: 0x00444615 File Offset: 0x00442815
			public int CachedAssets { readonly get; set; }

			// Token: 0x170026E9 RID: 9961
			// (get) Token: 0x0600C541 RID: 50497 RVA: 0x0044461E File Offset: 0x0044281E
			// (set) Token: 0x0600C542 RID: 50498 RVA: 0x00444626 File Offset: 0x00442826
			public int MissedCacheAssets { readonly get; set; }

			// Token: 0x170026EA RID: 9962
			// (get) Token: 0x0600C543 RID: 50499 RVA: 0x0044462F File Offset: 0x0044282F
			// (set) Token: 0x0600C544 RID: 50500 RVA: 0x00444637 File Offset: 0x00442837
			public int RootViewportSignals { readonly get; set; }

			// Token: 0x170026EB RID: 9963
			// (get) Token: 0x0600C545 RID: 50501 RVA: 0x00444640 File Offset: 0x00442840
			// (set) Token: 0x0600C546 RID: 50502 RVA: 0x00444648 File Offset: 0x00442848
			public int GcGen0 { readonly get; set; }

			// Token: 0x170026EC RID: 9964
			// (get) Token: 0x0600C547 RID: 50503 RVA: 0x00444651 File Offset: 0x00442851
			// (set) Token: 0x0600C548 RID: 50504 RVA: 0x00444659 File Offset: 0x00442859
			public int GcGen1 { readonly get; set; }

			// Token: 0x170026ED RID: 9965
			// (get) Token: 0x0600C549 RID: 50505 RVA: 0x00444662 File Offset: 0x00442862
			// (set) Token: 0x0600C54A RID: 50506 RVA: 0x0044466A File Offset: 0x0044286A
			public int GcGen2 { readonly get; set; }

			// Token: 0x0600C54B RID: 50507 RVA: 0x00444674 File Offset: 0x00442874
			[CompilerGenerated]
			public override readonly string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("MemorySnapshot");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600C54C RID: 50508 RVA: 0x004446C0 File Offset: 0x004428C0
			[CompilerGenerated]
			private readonly bool PrintMembers(StringBuilder builder)
			{
				builder.Append("StaticMemBytes = ");
				builder.Append(this.StaticMemBytes.ToString());
				builder.Append(", VramBytes = ");
				builder.Append(this.VramBytes.ToString());
				builder.Append(", GcTotalMemory = ");
				builder.Append(this.GcTotalMemory.ToString());
				builder.Append(", ObjectCount = ");
				builder.Append(this.ObjectCount.ToString());
				builder.Append(", ResourceCount = ");
				builder.Append(this.ResourceCount.ToString());
				builder.Append(", NodeCount = ");
				builder.Append(this.NodeCount.ToString());
				builder.Append(", OrphanNodeCount = ");
				builder.Append(this.OrphanNodeCount.ToString());
				builder.Append(", CachedAssets = ");
				builder.Append(this.CachedAssets.ToString());
				builder.Append(", MissedCacheAssets = ");
				builder.Append(this.MissedCacheAssets.ToString());
				builder.Append(", RootViewportSignals = ");
				builder.Append(this.RootViewportSignals.ToString());
				builder.Append(", GcGen0 = ");
				builder.Append(this.GcGen0.ToString());
				builder.Append(", GcGen1 = ");
				builder.Append(this.GcGen1.ToString());
				builder.Append(", GcGen2 = ");
				builder.Append(this.GcGen2.ToString());
				return true;
			}

			// Token: 0x0600C54D RID: 50509 RVA: 0x004448C9 File Offset: 0x00442AC9
			[CompilerGenerated]
			public static bool operator !=(MemoryProfiler.MemorySnapshot left, MemoryProfiler.MemorySnapshot right)
			{
				return !(left == right);
			}

			// Token: 0x0600C54E RID: 50510 RVA: 0x004448D5 File Offset: 0x00442AD5
			[CompilerGenerated]
			public static bool operator ==(MemoryProfiler.MemorySnapshot left, MemoryProfiler.MemorySnapshot right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600C54F RID: 50511 RVA: 0x004448E0 File Offset: 0x00442AE0
			[CompilerGenerated]
			public override readonly int GetHashCode()
			{
				return (((((((((((EqualityComparer<ulong>.Default.GetHashCode(this.<StaticMemBytes>k__BackingField) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.<VramBytes>k__BackingField)) * -1521134295 + EqualityComparer<long>.Default.GetHashCode(this.<GcTotalMemory>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<ObjectCount>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<ResourceCount>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<NodeCount>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<OrphanNodeCount>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<CachedAssets>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<MissedCacheAssets>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<RootViewportSignals>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<GcGen0>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<GcGen1>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<GcGen2>k__BackingField);
			}

			// Token: 0x0600C550 RID: 50512 RVA: 0x00444A11 File Offset: 0x00442C11
			[CompilerGenerated]
			public override readonly bool Equals(object obj)
			{
				return obj is MemoryProfiler.MemorySnapshot && this.Equals((MemoryProfiler.MemorySnapshot)obj);
			}

			// Token: 0x0600C551 RID: 50513 RVA: 0x00444A2C File Offset: 0x00442C2C
			[CompilerGenerated]
			public readonly bool Equals(MemoryProfiler.MemorySnapshot other)
			{
				return EqualityComparer<ulong>.Default.Equals(this.<StaticMemBytes>k__BackingField, other.<StaticMemBytes>k__BackingField) && EqualityComparer<ulong>.Default.Equals(this.<VramBytes>k__BackingField, other.<VramBytes>k__BackingField) && EqualityComparer<long>.Default.Equals(this.<GcTotalMemory>k__BackingField, other.<GcTotalMemory>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<ObjectCount>k__BackingField, other.<ObjectCount>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<ResourceCount>k__BackingField, other.<ResourceCount>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<NodeCount>k__BackingField, other.<NodeCount>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<OrphanNodeCount>k__BackingField, other.<OrphanNodeCount>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<CachedAssets>k__BackingField, other.<CachedAssets>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<MissedCacheAssets>k__BackingField, other.<MissedCacheAssets>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<RootViewportSignals>k__BackingField, other.<RootViewportSignals>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<GcGen0>k__BackingField, other.<GcGen0>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<GcGen1>k__BackingField, other.<GcGen1>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<GcGen2>k__BackingField, other.<GcGen2>k__BackingField);
			}

			// Token: 0x0600C552 RID: 50514 RVA: 0x00444B88 File Offset: 0x00442D88
			[CompilerGenerated]
			public readonly void Deconstruct(out ulong StaticMemBytes, out ulong VramBytes, out long GcTotalMemory, out int ObjectCount, out int ResourceCount, out int NodeCount, out int OrphanNodeCount, out int CachedAssets, out int MissedCacheAssets, out int RootViewportSignals, out int GcGen0, out int GcGen1, out int GcGen2)
			{
				StaticMemBytes = this.StaticMemBytes;
				VramBytes = this.VramBytes;
				GcTotalMemory = this.GcTotalMemory;
				ObjectCount = this.ObjectCount;
				ResourceCount = this.ResourceCount;
				NodeCount = this.NodeCount;
				OrphanNodeCount = this.OrphanNodeCount;
				CachedAssets = this.CachedAssets;
				MissedCacheAssets = this.MissedCacheAssets;
				RootViewportSignals = this.RootViewportSignals;
				GcGen0 = this.GcGen0;
				GcGen1 = this.GcGen1;
				GcGen2 = this.GcGen2;
			}
		}
	}
}
