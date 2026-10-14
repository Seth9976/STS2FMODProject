using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x0200040E RID: 1038
	[ScriptPath("res://src/Core/Nodes/Combat/NSelectionReticle.cs")]
	public class NSelectionReticle : Control
	{
		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06004149 RID: 16713 RVA: 0x001F85F8 File Offset: 0x001F67F8
		// (set) Token: 0x0600414A RID: 16714 RVA: 0x001F8600 File Offset: 0x001F6800
		public bool IsSelected { get; private set; }

		// Token: 0x0600414B RID: 16715 RVA: 0x001F8609 File Offset: 0x001F6809
		public override void _Ready()
		{
			base.Modulate = Colors.Transparent;
			base.PivotOffset = base.Size * 0.5f;
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x001F862C File Offset: 0x001F682C
		public override void _ExitTree()
		{
			base._ExitTree();
			this._cancelToken.Cancel();
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x001F8640 File Offset: 0x001F6840
		public void OnSelect()
		{
			if (NCombatUi.IsDebugHideTargetingUi)
			{
				return;
			}
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween().SetParallel(true);
			this._currentTween.TweenProperty(this, "modulate:a", 1f, 0.20000000298023224);
			this._currentTween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L)
				.From(Vector2.One * 0.9f);
			base.Modulate = Colors.White;
			base.Scale = Vector2.One;
			this.IsSelected = true;
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x001F8710 File Offset: 0x001F6910
		public void OnDeselect()
		{
			if (this._cancelToken.IsCancellationRequested)
			{
				return;
			}
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			if (!this.IsValid() || !base.IsInsideTree())
			{
				return;
			}
			Tween tween = base.CreateTween();
			this._currentTween = ((tween != null) ? tween.SetParallel(true) : null);
			Tween currentTween2 = this._currentTween;
			if (currentTween2 != null)
			{
				currentTween2.TweenProperty(this, "modulate:a", 0f, 0.20000000298023224).SetEase(1L).SetTrans(1L);
			}
			Tween currentTween3 = this._currentTween;
			if (currentTween3 != null)
			{
				currentTween3.TweenProperty(this, "scale", Vector2.One * 1.05f, 0.20000000298023224).SetEase(1L).SetTrans(1L);
			}
			this.IsSelected = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600414F RID: 16719 RVA: 0x001F87F4 File Offset: 0x001F69F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NSelectionReticle.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSelectionReticle.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSelectionReticle.MethodName.OnSelect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSelectionReticle.MethodName.OnDeselect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004150 RID: 16720 RVA: 0x001F88C8 File Offset: 0x001F6AC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSelectionReticle.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSelectionReticle.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSelectionReticle.MethodName.OnSelect && args.Count == 0)
			{
				this.OnSelect();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSelectionReticle.MethodName.OnDeselect && args.Count == 0)
			{
				this.OnDeselect();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06004151 RID: 16721 RVA: 0x001F8974 File Offset: 0x001F6B74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSelectionReticle.MethodName._Ready || (ref method) == NSelectionReticle.MethodName._ExitTree || (ref method) == NSelectionReticle.MethodName.OnSelect || (ref method) == NSelectionReticle.MethodName.OnDeselect || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004152 RID: 16722 RVA: 0x001F89C4 File Offset: 0x001F6BC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSelectionReticle.PropertyName.IsSelected)
			{
				this.IsSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSelectionReticle.PropertyName._currentTween)
			{
				this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06004153 RID: 16723 RVA: 0x001F8A04 File Offset: 0x001F6C04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSelectionReticle.PropertyName.IsSelected)
			{
				bool isSelected = this.IsSelected;
				value = VariantUtils.CreateFrom<bool>(ref isSelected);
				return true;
			}
			if ((ref name) == NSelectionReticle.PropertyName._currentTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004154 RID: 16724 RVA: 0x001F8A5C File Offset: 0x001F6C5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSelectionReticle.PropertyName._currentTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSelectionReticle.PropertyName.IsSelected, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004155 RID: 16725 RVA: 0x001F8AB4 File Offset: 0x001F6CB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isSelected = NSelectionReticle.PropertyName.IsSelected;
			bool isSelected2 = this.IsSelected;
			info.AddProperty(isSelected, Variant.From<bool>(ref isSelected2));
			info.AddProperty(NSelectionReticle.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
		}

		/// <inheritdoc />
		// Token: 0x06004156 RID: 16726 RVA: 0x001F8AF8 File Offset: 0x001F6CF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSelectionReticle.PropertyName.IsSelected, ref variant))
			{
				this.IsSelected = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSelectionReticle.PropertyName._currentTween, ref variant2))
			{
				this._currentTween = variant2.As<Tween>();
			}
		}

		// Token: 0x0400195B RID: 6491
		[Nullable(2)]
		private Tween _currentTween;

		// Token: 0x0400195C RID: 6492
		[Nullable(1)]
		private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001924 RID: 6436
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040062E9 RID: 25321
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040062EA RID: 25322
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnSelect' method.
			/// </summary>
			// Token: 0x040062EB RID: 25323
			public static readonly StringName OnSelect = "OnSelect";

			/// <summary>
			/// Cached name for the 'OnDeselect' method.
			/// </summary>
			// Token: 0x040062EC RID: 25324
			public static readonly StringName OnDeselect = "OnDeselect";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001925 RID: 6437
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsSelected' property.
			/// </summary>
			// Token: 0x040062ED RID: 25325
			public static readonly StringName IsSelected = "IsSelected";

			/// <summary>
			/// Cached name for the '_currentTween' field.
			/// </summary>
			// Token: 0x040062EE RID: 25326
			public static readonly StringName _currentTween = "_currentTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001926 RID: 6438
		public class SignalName : Control.SignalName
		{
		}
	}
}
