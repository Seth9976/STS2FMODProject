using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders
{
	// Token: 0x02000420 RID: 1056
	[ScriptPath("res://src/Core/Nodes/Cards/Holders/NSelectedHandCardHolder.cs")]
	public class NSelectedHandCardHolder : NCardHolder
	{
		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06004377 RID: 17271 RVA: 0x0020C373 File Offset: 0x0020A573
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("/cards/holders/selected_hand_card_holder");
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06004378 RID: 17272 RVA: 0x0020C37F File Offset: 0x0020A57F
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NSelectedHandCardHolder.ScenePath);
			}
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x0020C38C File Offset: 0x0020A58C
		[NullableContext(1)]
		public static NSelectedHandCardHolder Create(NHandCardHolder originalHolder)
		{
			NCard cardNode = originalHolder.CardNode;
			NSelectedHandCardHolder nselectedHandCardHolder = PreloadManager.Cache.GetScene(NSelectedHandCardHolder.ScenePath).Instantiate<NSelectedHandCardHolder>(0L);
			originalHolder.Clear();
			Node node = nselectedHandCardHolder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
			defaultInterpolatedStringHandler.AppendLiteral("SelectedHandCardHolder-");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(cardNode.Model.Id);
			node.Name = defaultInterpolatedStringHandler.ToStringAndClear();
			nselectedHandCardHolder.SetCard(cardNode);
			nselectedHandCardHolder.Scale = nselectedHandCardHolder.SmallScale;
			return nselectedHandCardHolder;
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x0020C40C File Offset: 0x0020A60C
		public override void _Ready()
		{
			base.ConnectSignals();
			this._tween = base.CreateTween();
			this._tween.TweenProperty(base.CardNode, "position", base.Position, 0.15000000596046448).SetEase(1L).SetTrans(7L);
			this._tween.Play();
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0020C474 File Offset: 0x0020A674
		protected override void CreateHoverTips()
		{
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x0020C476 File Offset: 0x0020A676
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600437D RID: 17277 RVA: 0x0020C488 File Offset: 0x0020A688
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NSelectedHandCardHolder.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "originalHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSelectedHandCardHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSelectedHandCardHolder.MethodName.CreateHoverTips, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSelectedHandCardHolder.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600437E RID: 17278 RVA: 0x0020C598 File Offset: 0x0020A798
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSelectedHandCardHolder.MethodName.Create && args.Count == 1)
			{
				NSelectedHandCardHolder nselectedHandCardHolder = NSelectedHandCardHolder.Create(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = VariantUtils.CreateFrom<NSelectedHandCardHolder>(ref nselectedHandCardHolder);
				return true;
			}
			if ((ref method) == NSelectedHandCardHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSelectedHandCardHolder.MethodName.CreateHoverTips && args.Count == 0)
			{
				this.CreateHoverTips();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSelectedHandCardHolder.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x0020C658 File Offset: 0x0020A858
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSelectedHandCardHolder.MethodName.Create && args.Count == 1)
			{
				NSelectedHandCardHolder nselectedHandCardHolder = NSelectedHandCardHolder.Create(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = VariantUtils.CreateFrom<NSelectedHandCardHolder>(ref nselectedHandCardHolder);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06004380 RID: 17280 RVA: 0x0020C6A8 File Offset: 0x0020A8A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSelectedHandCardHolder.MethodName.Create || (ref method) == NSelectedHandCardHolder.MethodName._Ready || (ref method) == NSelectedHandCardHolder.MethodName.CreateHoverTips || (ref method) == NSelectedHandCardHolder.MethodName._ExitTree || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06004381 RID: 17281 RVA: 0x0020C6F8 File Offset: 0x0020A8F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSelectedHandCardHolder.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06004382 RID: 17282 RVA: 0x0020C71D File Offset: 0x0020A91D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSelectedHandCardHolder.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004383 RID: 17283 RVA: 0x0020C748 File Offset: 0x0020A948
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSelectedHandCardHolder.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004384 RID: 17284 RVA: 0x0020C77D File Offset: 0x0020A97D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSelectedHandCardHolder.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06004385 RID: 17285 RVA: 0x0020C79C File Offset: 0x0020A99C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSelectedHandCardHolder.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x04001A33 RID: 6707
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001972 RID: 6514
		public new class MethodName : NCardHolder.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04006496 RID: 25750
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006497 RID: 25751
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'CreateHoverTips' method.
			/// </summary>
			// Token: 0x04006498 RID: 25752
			public new static readonly StringName CreateHoverTips = "CreateHoverTips";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006499 RID: 25753
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001973 RID: 6515
		public new class PropertyName : NCardHolder.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400649A RID: 25754
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001974 RID: 6516
		public new class SignalName : NCardHolder.SignalName
		{
		}
	}
}
