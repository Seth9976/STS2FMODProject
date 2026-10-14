using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x02000237 RID: 567
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NFanOfKnivesVfx.cs")]
	public class NFanOfKnivesVfx : Node2D
	{
		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001CDA RID: 7386 RVA: 0x000BBA01 File Offset: 0x000B9C01
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NFanOfKnivesVfx._scenePath);
			}
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000BBA10 File Offset: 0x000B9C10
		[return: Nullable(2)]
		public static NFanOfKnivesVfx Create(Creature target)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NFanOfKnivesVfx nfanOfKnivesVfx = PreloadManager.Cache.GetScene(NFanOfKnivesVfx._scenePath).Instantiate<NFanOfKnivesVfx>(0L);
			nfanOfKnivesVfx._spawnPosition = NCombatRoom.Instance.GetCreatureNode(target).VfxSpawnPosition;
			return nfanOfKnivesVfx;
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000BBA54 File Offset: 0x000B9C54
		public override void _Ready()
		{
			this._shiv1 = base.GetNode<Node2D>("ShivFanParticle1");
			this._shiv2 = base.GetNode<Node2D>("ShivFanParticle2");
			this._shiv3 = base.GetNode<Node2D>("ShivFanParticle3");
			this._shiv4 = base.GetNode<Node2D>("ShivFanParticle4");
			this._shiv5 = base.GetNode<Node2D>("ShivFanParticle5");
			this._shiv6 = base.GetNode<Node2D>("ShivFanParticle6");
			this._shiv7 = base.GetNode<Node2D>("ShivFanParticle7");
			this._shiv8 = base.GetNode<Node2D>("ShivFanParticle8");
			this._shiv9 = base.GetNode<Node2D>("ShivFanParticle9");
			this._shivs.Add(this._shiv1);
			this._shivs.Add(this._shiv2);
			this._shivs.Add(this._shiv3);
			this._shivs.Add(this._shiv4);
			this._shivs.Add(this._shiv5);
			this._shivs.Add(this._shiv6);
			this._shivs.Add(this._shiv7);
			this._shivs.Add(this._shiv8);
			this._shivs.Add(this._shiv9);
			foreach (Node2D node2D in this._shivs)
			{
				node2D.Scale = Vector2.One * Rng.Chaotic.NextFloat(0.98f, 1.02f);
				node2D.GlobalPosition = this._spawnPosition;
			}
			TaskHelper.RunSafely(this.Animate());
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000BBC3C File Offset: 0x000B9E3C
		public override void _ExitTree()
		{
			Tween fanTween = this._fanTween;
			if (fanTween != null)
			{
				fanTween.Kill();
			}
			Tween spawnTween = this._spawnTween;
			if (spawnTween == null)
			{
				return;
			}
			spawnTween.Kill();
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000BBC60 File Offset: 0x000B9E60
		private async Task Animate()
		{
			SfxCmd.Play("event:/sfx/characters/silent/silent_fan_of_knives", 1f);
			this._spawnTween = base.CreateTween().SetParallel(true);
			foreach (Node2D node2D in this._shivs)
			{
				float num = Rng.Chaotic.NextFloat(0.4f, 0.8f);
				this._spawnTween.TweenProperty(node2D, "offset:y", -180f, (double)num).From(0f).SetEase(1L)
					.SetTrans(10L);
				this._spawnTween.TweenProperty(node2D, "modulate", Colors.White, (double)num).From(StsColors.transparentBlack);
				this._spawnTween.TweenProperty(node2D.GetNode<Node2D>("Shadow"), "offset:y", -180f, (double)num).From(0f).SetEase(1L)
					.SetTrans(10L);
			}
			this._spawnTween.Chain();
			foreach (Node2D node2D2 in this._shivs)
			{
				this._spawnTween.TweenProperty(node2D2, "modulate", StsColors.transparentWhite, 0.4).SetDelay(Rng.Chaotic.NextDouble(0.25, 0.5));
			}
			this._fanTween = base.CreateTween().SetParallel(true);
			this._fanTween.TweenInterval(0.4000000059604645);
			this._fanTween.Chain();
			this._fanTween.TweenProperty(this._shiv1, "rotation", -1.74533f, 0.8).SetEase(1L).SetTrans(10L);
			this._fanTween.TweenProperty(this._shiv2, "rotation", -1.3089975f, 0.8).SetEase(1L).SetTrans(10L);
			this._fanTween.TweenProperty(this._shiv3, "rotation", -0.872665f, 0.8).SetEase(1L).SetTrans(10L);
			this._fanTween.TweenProperty(this._shiv4, "rotation", -0.4363325f, 0.8).SetEase(1L).SetTrans(10L);
			this._fanTween.TweenProperty(this._shiv6, "rotation", 0.4363325f, 0.8).SetEase(1L).SetTrans(10L);
			this._fanTween.TweenProperty(this._shiv7, "rotation", 0.872665f, 0.8).SetEase(1L).SetTrans(10L);
			this._fanTween.TweenProperty(this._shiv8, "rotation", 1.3089975f, 0.8).SetEase(1L).SetTrans(10L);
			this._fanTween.TweenProperty(this._shiv9, "rotation", 1.74533f, 0.8).SetEase(1L).SetTrans(10L);
			TaskAwaiter<bool> taskAwaiter = this._spawnTween.AwaitFinished(this).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				TaskAwaiter<bool> taskAwaiter3 = this._fanTween.AwaitFinished(this).GetAwaiter();
				if (!taskAwaiter3.IsCompleted)
				{
					await taskAwaiter3;
					taskAwaiter3 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter3.GetResult())
				{
					this.QueueFreeSafely();
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001CDF RID: 7391 RVA: 0x000BBCA4 File Offset: 0x000B9EA4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NFanOfKnivesVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFanOfKnivesVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CE0 RID: 7392 RVA: 0x000BBD18 File Offset: 0x000B9F18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFanOfKnivesVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFanOfKnivesVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001CE1 RID: 7393 RVA: 0x000BBD78 File Offset: 0x000B9F78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFanOfKnivesVfx.MethodName._Ready || (ref method) == NFanOfKnivesVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001CE2 RID: 7394 RVA: 0x000BBDA0 File Offset: 0x000B9FA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv1)
			{
				this._shiv1 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv2)
			{
				this._shiv2 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv3)
			{
				this._shiv3 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv4)
			{
				this._shiv4 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv5)
			{
				this._shiv5 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv6)
			{
				this._shiv6 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv7)
			{
				this._shiv7 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv8)
			{
				this._shiv8 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv9)
			{
				this._shiv9 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._spawnPosition)
			{
				this._spawnPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._spawnTween)
			{
				this._spawnTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._fanTween)
			{
				this._fanTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001CE3 RID: 7395 RVA: 0x000BBEFC File Offset: 0x000BA0FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv1)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._shiv1);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv2)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._shiv2);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv3)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._shiv3);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv4)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._shiv4);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv5)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._shiv5);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv6)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._shiv6);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv7)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._shiv7);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv8)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._shiv8);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._shiv9)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._shiv9);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._spawnPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._spawnPosition);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._spawnTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._spawnTween);
				return true;
			}
			if ((ref name) == NFanOfKnivesVfx.PropertyName._fanTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._fanTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001CE4 RID: 7396 RVA: 0x000BC094 File Offset: 0x000BA294
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._shiv1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._shiv2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._shiv3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._shiv4, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._shiv5, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._shiv6, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._shiv7, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._shiv8, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._shiv9, 0L, "", 4096L, false),
				new PropertyInfo(5L, NFanOfKnivesVfx.PropertyName._spawnPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._spawnTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFanOfKnivesVfx.PropertyName._fanTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CE5 RID: 7397 RVA: 0x000BC234 File Offset: 0x000BA434
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv1, Variant.From<Node2D>(ref this._shiv1));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv2, Variant.From<Node2D>(ref this._shiv2));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv3, Variant.From<Node2D>(ref this._shiv3));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv4, Variant.From<Node2D>(ref this._shiv4));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv5, Variant.From<Node2D>(ref this._shiv5));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv6, Variant.From<Node2D>(ref this._shiv6));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv7, Variant.From<Node2D>(ref this._shiv7));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv8, Variant.From<Node2D>(ref this._shiv8));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv9, Variant.From<Node2D>(ref this._shiv9));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._spawnPosition, Variant.From<Vector2>(ref this._spawnPosition));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._spawnTween, Variant.From<Tween>(ref this._spawnTween));
			info.AddProperty(NFanOfKnivesVfx.PropertyName._fanTween, Variant.From<Tween>(ref this._fanTween));
		}

		/// <inheritdoc />
		// Token: 0x06001CE6 RID: 7398 RVA: 0x000BC350 File Offset: 0x000BA550
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv1, ref variant))
			{
				this._shiv1 = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv2, ref variant2))
			{
				this._shiv2 = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv3, ref variant3))
			{
				this._shiv3 = variant3.As<Node2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv4, ref variant4))
			{
				this._shiv4 = variant4.As<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv5, ref variant5))
			{
				this._shiv5 = variant5.As<Node2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv6, ref variant6))
			{
				this._shiv6 = variant6.As<Node2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv7, ref variant7))
			{
				this._shiv7 = variant7.As<Node2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv8, ref variant8))
			{
				this._shiv8 = variant8.As<Node2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv9, ref variant9))
			{
				this._shiv9 = variant9.As<Node2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._spawnPosition, ref variant10))
			{
				this._spawnPosition = variant10.As<Vector2>();
			}
			Variant variant11;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._spawnTween, ref variant11))
			{
				this._spawnTween = variant11.As<Tween>();
			}
			Variant variant12;
			if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._fanTween, ref variant12))
			{
				this._fanTween = variant12.As<Tween>();
			}
		}

		// Token: 0x04000B16 RID: 2838
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/fan_of_knives_vfx");

		// Token: 0x04000B17 RID: 2839
		private const string _fanOfKnivesSfx = "event:/sfx/characters/silent/silent_fan_of_knives";

		// Token: 0x04000B18 RID: 2840
		private readonly List<Node2D> _shivs = new List<Node2D>();

		// Token: 0x04000B19 RID: 2841
		private Node2D _shiv1;

		// Token: 0x04000B1A RID: 2842
		private Node2D _shiv2;

		// Token: 0x04000B1B RID: 2843
		private Node2D _shiv3;

		// Token: 0x04000B1C RID: 2844
		private Node2D _shiv4;

		// Token: 0x04000B1D RID: 2845
		private Node2D _shiv5;

		// Token: 0x04000B1E RID: 2846
		private Node2D _shiv6;

		// Token: 0x04000B1F RID: 2847
		private Node2D _shiv7;

		// Token: 0x04000B20 RID: 2848
		private Node2D _shiv8;

		// Token: 0x04000B21 RID: 2849
		private Node2D _shiv9;

		// Token: 0x04000B22 RID: 2850
		private Vector2 _spawnPosition;

		// Token: 0x04000B23 RID: 2851
		private const double _fanDuration = 0.8;

		// Token: 0x04000B24 RID: 2852
		[Nullable(2)]
		private Tween _spawnTween;

		// Token: 0x04000B25 RID: 2853
		[Nullable(2)]
		private Tween _fanTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200120E RID: 4622
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400460A RID: 17930
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400460B RID: 17931
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200120F RID: 4623
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_shiv1' field.
			/// </summary>
			// Token: 0x0400460C RID: 17932
			public static readonly StringName _shiv1 = "_shiv1";

			/// <summary>
			/// Cached name for the '_shiv2' field.
			/// </summary>
			// Token: 0x0400460D RID: 17933
			public static readonly StringName _shiv2 = "_shiv2";

			/// <summary>
			/// Cached name for the '_shiv3' field.
			/// </summary>
			// Token: 0x0400460E RID: 17934
			public static readonly StringName _shiv3 = "_shiv3";

			/// <summary>
			/// Cached name for the '_shiv4' field.
			/// </summary>
			// Token: 0x0400460F RID: 17935
			public static readonly StringName _shiv4 = "_shiv4";

			/// <summary>
			/// Cached name for the '_shiv5' field.
			/// </summary>
			// Token: 0x04004610 RID: 17936
			public static readonly StringName _shiv5 = "_shiv5";

			/// <summary>
			/// Cached name for the '_shiv6' field.
			/// </summary>
			// Token: 0x04004611 RID: 17937
			public static readonly StringName _shiv6 = "_shiv6";

			/// <summary>
			/// Cached name for the '_shiv7' field.
			/// </summary>
			// Token: 0x04004612 RID: 17938
			public static readonly StringName _shiv7 = "_shiv7";

			/// <summary>
			/// Cached name for the '_shiv8' field.
			/// </summary>
			// Token: 0x04004613 RID: 17939
			public static readonly StringName _shiv8 = "_shiv8";

			/// <summary>
			/// Cached name for the '_shiv9' field.
			/// </summary>
			// Token: 0x04004614 RID: 17940
			public static readonly StringName _shiv9 = "_shiv9";

			/// <summary>
			/// Cached name for the '_spawnPosition' field.
			/// </summary>
			// Token: 0x04004615 RID: 17941
			public static readonly StringName _spawnPosition = "_spawnPosition";

			/// <summary>
			/// Cached name for the '_spawnTween' field.
			/// </summary>
			// Token: 0x04004616 RID: 17942
			public static readonly StringName _spawnTween = "_spawnTween";

			/// <summary>
			/// Cached name for the '_fanTween' field.
			/// </summary>
			// Token: 0x04004617 RID: 17943
			public static readonly StringName _fanTween = "_fanTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001210 RID: 4624
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
