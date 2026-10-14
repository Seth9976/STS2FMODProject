using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Container for Modal Windows (Confirmation screens, FTUEs, etc). This is rendered above most UI!
	/// There can never be two modals active at once. If so, Casey has failed in the UI department.
	/// </summary>
	// Token: 0x020003E6 RID: 998
	[NullableContext(2)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NModalContainer.cs")]
	public class NModalContainer : Control
	{
		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x001D11D1 File Offset: 0x001CF3D1
		// (set) Token: 0x06003CC8 RID: 15560 RVA: 0x001D11D8 File Offset: 0x001CF3D8
		public static NModalContainer Instance { get; private set; }

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06003CC9 RID: 15561 RVA: 0x001D11E0 File Offset: 0x001CF3E0
		// (set) Token: 0x06003CCA RID: 15562 RVA: 0x001D11E8 File Offset: 0x001CF3E8
		public IScreenContext OpenModal { get; private set; }

		// Token: 0x06003CCB RID: 15563 RVA: 0x001D11F1 File Offset: 0x001CF3F1
		public override void _Ready()
		{
			if (NModalContainer.Instance != null)
			{
				Log.Error("NModalContainer already exists.", 2);
				this.QueueFreeSafely();
				return;
			}
			NModalContainer.Instance = this;
			this._backstop = base.GetNode<ColorRect>("Backstop");
		}

		/// <summary>
		/// Used when we create a FTUE or modal popup. We add it to our ModalContainer!
		/// </summary>
		// Token: 0x06003CCC RID: 15564 RVA: 0x001D1228 File Offset: 0x001CF428
		[NullableContext(1)]
		public void Add(Node modalToCreate, bool showBackstop = true)
		{
			if (this.OpenModal != null)
			{
				Log.Warn("There's another modal already open.", 2);
				return;
			}
			this.OpenModal = (IScreenContext)modalToCreate;
			this.AddChildSafely(modalToCreate);
			ActiveScreenContext.Instance.Update();
			if (showBackstop)
			{
				this.ShowBackstop();
			}
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x001D1264 File Offset: 0x001CF464
		public void Clear()
		{
			foreach (Node node in base.GetChildren(false))
			{
				if (node != this._backstop)
				{
					node.QueueFreeSafely();
				}
			}
			this.OpenModal = null;
			ActiveScreenContext.Instance.Update();
			this.HideBackstop();
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x001D12D4 File Offset: 0x001CF4D4
		public void ShowBackstop()
		{
			base.MouseFilter = 0L;
			this._backstop.Visible = true;
			Tween backstopTween = this._backstopTween;
			if (backstopTween != null)
			{
				backstopTween.Kill();
			}
			this._backstopTween = base.CreateTween();
			this._backstopTween.TweenProperty(this._backstop, "color:a", 0.85f, 0.3);
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x001D1344 File Offset: 0x001CF544
		public void HideBackstop()
		{
			base.MouseFilter = 2L;
			Tween backstopTween = this._backstopTween;
			if (backstopTween != null)
			{
				backstopTween.Kill();
			}
			this._backstopTween = base.CreateTween();
			this._backstopTween.TweenProperty(this._backstop, "color:a", 0f, 0.3);
			this._backstopTween.TweenCallback(Callable.From<bool>(() => this._backstop.Visible = false));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CD0 RID: 15568 RVA: 0x001D13C4 File Offset: 0x001CF5C4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NModalContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModalContainer.MethodName.Add, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "modalToCreate", 0L, "", 6L, new StringName("Node"), false),
					new PropertyInfo(1L, "showBackstop", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NModalContainer.MethodName.Clear, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModalContainer.MethodName.ShowBackstop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModalContainer.MethodName.HideBackstop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CD1 RID: 15569 RVA: 0x001D1518 File Offset: 0x001CF718
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NModalContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModalContainer.MethodName.Add && args.Count == 2)
			{
				this.Add(VariantUtils.ConvertTo<Node>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModalContainer.MethodName.Clear && args.Count == 0)
			{
				this.Clear();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModalContainer.MethodName.ShowBackstop && args.Count == 0)
			{
				this.ShowBackstop();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModalContainer.MethodName.HideBackstop && args.Count == 0)
			{
				this.HideBackstop();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003CD2 RID: 15570 RVA: 0x001D1604 File Offset: 0x001CF804
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NModalContainer.MethodName._Ready || (ref method) == NModalContainer.MethodName.Add || (ref method) == NModalContainer.MethodName.Clear || (ref method) == NModalContainer.MethodName.ShowBackstop || (ref method) == NModalContainer.MethodName.HideBackstop || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003CD3 RID: 15571 RVA: 0x001D1663 File Offset: 0x001CF863
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NModalContainer.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NModalContainer.PropertyName._backstopTween)
			{
				this._backstopTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003CD4 RID: 15572 RVA: 0x001D16A4 File Offset: 0x001CF8A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NModalContainer.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
				return true;
			}
			if ((ref name) == NModalContainer.PropertyName._backstopTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._backstopTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CD5 RID: 15573 RVA: 0x001D16FC File Offset: 0x001CF8FC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NModalContainer.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModalContainer.PropertyName._backstopTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CD6 RID: 15574 RVA: 0x001D1752 File Offset: 0x001CF952
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NModalContainer.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
			info.AddProperty(NModalContainer.PropertyName._backstopTween, Variant.From<Tween>(ref this._backstopTween));
		}

		/// <inheritdoc />
		// Token: 0x06003CD7 RID: 15575 RVA: 0x001D1788 File Offset: 0x001CF988
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NModalContainer.PropertyName._backstop, ref variant))
			{
				this._backstop = variant.As<ColorRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NModalContainer.PropertyName._backstopTween, ref variant2))
			{
				this._backstopTween = variant2.As<Tween>();
			}
		}

		// Token: 0x040017B5 RID: 6069
		[Nullable(1)]
		private ColorRect _backstop;

		// Token: 0x040017B6 RID: 6070
		private Tween _backstopTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200186A RID: 6250
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F81 RID: 24449
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Add' method.
			/// </summary>
			// Token: 0x04005F82 RID: 24450
			public static readonly StringName Add = "Add";

			/// <summary>
			/// Cached name for the 'Clear' method.
			/// </summary>
			// Token: 0x04005F83 RID: 24451
			public static readonly StringName Clear = "Clear";

			/// <summary>
			/// Cached name for the 'ShowBackstop' method.
			/// </summary>
			// Token: 0x04005F84 RID: 24452
			public static readonly StringName ShowBackstop = "ShowBackstop";

			/// <summary>
			/// Cached name for the 'HideBackstop' method.
			/// </summary>
			// Token: 0x04005F85 RID: 24453
			public static readonly StringName HideBackstop = "HideBackstop";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200186B RID: 6251
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x04005F86 RID: 24454
			public static readonly StringName _backstop = "_backstop";

			/// <summary>
			/// Cached name for the '_backstopTween' field.
			/// </summary>
			// Token: 0x04005F87 RID: 24455
			public static readonly StringName _backstopTween = "_backstopTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200186C RID: 6252
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
