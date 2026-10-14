using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Debug
{
	// Token: 0x020003C9 RID: 969
	[ScriptPath("res://src/Core/Nodes/Debug/NParticleCounter.cs")]
	public class NParticleCounter : Control
	{
		// Token: 0x06003AB8 RID: 15032 RVA: 0x001C0824 File Offset: 0x001BEA24
		public override void _Ready()
		{
			if (!OS.HasFeature("editor"))
			{
				this.QueueFreeSafely();
				return;
			}
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._label = base.GetNode<Label>("%Label");
			base.Visible = false;
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x001C0877 File Offset: 0x001BEA77
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			this.CheckForHotkey(inputEvent);
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x001C0880 File Offset: 0x001BEA80
		[NullableContext(1)]
		private void CheckForHotkey(InputEvent inputEvent)
		{
			if (!inputEvent.IsActionReleased(NParticleCounter._toggleParticleCounter, false))
			{
				return;
			}
			if (NDevConsole.IsConsoleVisible)
			{
				return;
			}
			base.Visible = !base.Visible;
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x001C08A8 File Offset: 0x001BEAA8
		public override void _Process(double delta)
		{
			if (!base.Visible)
			{
				return;
			}
			if (this._label == null)
			{
				return;
			}
			this._secondsSinceLastUpdate += delta;
			bool flag = this._totalParticles > 1 || this._updateCount > 500;
			if (flag && this._secondsSinceLastUpdate < 5.0)
			{
				return;
			}
			this._updateCount++;
			this._secondsSinceLastUpdate = 0.0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (Node node in NParticleCounter.GetChildrenRecursive(base.GetTree().Root))
			{
				CpuParticles2D cpuParticles2D = node as CpuParticles2D;
				if (cpuParticles2D == null)
				{
					GpuParticles2D gpuParticles2D = node as GpuParticles2D;
					if (gpuParticles2D != null)
					{
						num3 += gpuParticles2D.Amount;
						num4++;
					}
				}
				else
				{
					num += cpuParticles2D.Amount;
					num2++;
				}
			}
			this._totalParticles = num + num3;
			Label label = this._label;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 7);
			defaultInterpolatedStringHandler.AppendLiteral("All particles: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._totalParticles);
			defaultInterpolatedStringHandler.AppendLiteral("\nCPU particles: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" in ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
			defaultInterpolatedStringHandler.AppendLiteral(" node");
			defaultInterpolatedStringHandler.AppendFormatted((num2 == 1) ? "" : "s");
			defaultInterpolatedStringHandler.AppendLiteral("\nGPU particles: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
			defaultInterpolatedStringHandler.AppendLiteral(" in ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num4);
			defaultInterpolatedStringHandler.AppendLiteral(" node");
			defaultInterpolatedStringHandler.AppendFormatted((num4 == 1) ? "" : "s");
			label.Text = defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x001C0A88 File Offset: 0x001BEC88
		[NullableContext(1)]
		private unsafe static List<Node> GetChildrenRecursive(Node root)
		{
			int num = 1;
			List<Node> list = new List<Node>(num);
			CollectionsMarshal.SetCount<Node>(list, num);
			Span<Node> span = CollectionsMarshal.AsSpan<Node>(list);
			int num2 = 0;
			*span[num2] = root;
			List<Node> list2 = list;
			foreach (Node node in root.GetChildren(false))
			{
				list2.AddRange(NParticleCounter.GetChildrenRecursive(node));
			}
			return list2;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003ABD RID: 15037 RVA: 0x001C0B08 File Offset: 0x001BED08
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NParticleCounter.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NParticleCounter.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NParticleCounter.MethodName.CheckForHotkey, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NParticleCounter.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003ABE RID: 15038 RVA: 0x001C0C60 File Offset: 0x001BEE60
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NParticleCounter.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NParticleCounter.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NParticleCounter.MethodName.CheckForHotkey && args.Count == 1)
			{
				this.CheckForHotkey(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NParticleCounter.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003ABF RID: 15039 RVA: 0x001C0D34 File Offset: 0x001BEF34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NParticleCounter.MethodName._Ready || (ref method) == NParticleCounter.MethodName._Input || (ref method) == NParticleCounter.MethodName.CheckForHotkey || (ref method) == NParticleCounter.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003AC0 RID: 15040 RVA: 0x001C0D84 File Offset: 0x001BEF84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NParticleCounter.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NParticleCounter.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<Label>(ref value);
				return true;
			}
			if ((ref name) == NParticleCounter.PropertyName._secondsSinceLastUpdate)
			{
				this._secondsSinceLastUpdate = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			if ((ref name) == NParticleCounter.PropertyName._totalParticles)
			{
				this._totalParticles = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NParticleCounter.PropertyName._updateCount)
			{
				this._updateCount = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003AC1 RID: 15041 RVA: 0x001C0E20 File Offset: 0x001BF020
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NParticleCounter.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NParticleCounter.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<Label>(ref this._label);
				return true;
			}
			if ((ref name) == NParticleCounter.PropertyName._secondsSinceLastUpdate)
			{
				value = VariantUtils.CreateFrom<double>(ref this._secondsSinceLastUpdate);
				return true;
			}
			if ((ref name) == NParticleCounter.PropertyName._totalParticles)
			{
				value = VariantUtils.CreateFrom<int>(ref this._totalParticles);
				return true;
			}
			if ((ref name) == NParticleCounter.PropertyName._updateCount)
			{
				value = VariantUtils.CreateFrom<int>(ref this._updateCount);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003AC2 RID: 15042 RVA: 0x001C0ED8 File Offset: 0x001BF0D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NParticleCounter.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NParticleCounter.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(3L, NParticleCounter.PropertyName._secondsSinceLastUpdate, 0L, "", 4096L, false),
				new PropertyInfo(2L, NParticleCounter.PropertyName._totalParticles, 0L, "", 4096L, false),
				new PropertyInfo(2L, NParticleCounter.PropertyName._updateCount, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003AC3 RID: 15043 RVA: 0x001C0F90 File Offset: 0x001BF190
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NParticleCounter.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NParticleCounter.PropertyName._label, Variant.From<Label>(ref this._label));
			info.AddProperty(NParticleCounter.PropertyName._secondsSinceLastUpdate, Variant.From<double>(ref this._secondsSinceLastUpdate));
			info.AddProperty(NParticleCounter.PropertyName._totalParticles, Variant.From<int>(ref this._totalParticles));
			info.AddProperty(NParticleCounter.PropertyName._updateCount, Variant.From<int>(ref this._updateCount));
		}

		/// <inheritdoc />
		// Token: 0x06003AC4 RID: 15044 RVA: 0x001C1014 File Offset: 0x001BF214
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NParticleCounter.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NParticleCounter.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<Label>();
			}
			Variant variant3;
			if (info.TryGetProperty(NParticleCounter.PropertyName._secondsSinceLastUpdate, ref variant3))
			{
				this._secondsSinceLastUpdate = variant3.As<double>();
			}
			Variant variant4;
			if (info.TryGetProperty(NParticleCounter.PropertyName._totalParticles, ref variant4))
			{
				this._totalParticles = variant4.As<int>();
			}
			Variant variant5;
			if (info.TryGetProperty(NParticleCounter.PropertyName._updateCount, ref variant5))
			{
				this._updateCount = variant5.As<int>();
			}
		}

		// Token: 0x040016EC RID: 5868
		[Nullable(1)]
		private static readonly StringName _toggleParticleCounter = new StringName("toggle_particle_counter");

		// Token: 0x040016ED RID: 5869
		private const float _secondsPerUpdate = 5f;

		// Token: 0x040016EE RID: 5870
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x040016EF RID: 5871
		[Nullable(2)]
		private Label _label;

		// Token: 0x040016F0 RID: 5872
		private double _secondsSinceLastUpdate;

		// Token: 0x040016F1 RID: 5873
		private int _totalParticles;

		// Token: 0x040016F2 RID: 5874
		private int _updateCount;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001803 RID: 6147
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005DD5 RID: 24021
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005DD6 RID: 24022
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'CheckForHotkey' method.
			/// </summary>
			// Token: 0x04005DD7 RID: 24023
			public static readonly StringName CheckForHotkey = "CheckForHotkey";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005DD8 RID: 24024
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001804 RID: 6148
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04005DD9 RID: 24025
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005DDA RID: 24026
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_secondsSinceLastUpdate' field.
			/// </summary>
			// Token: 0x04005DDB RID: 24027
			public static readonly StringName _secondsSinceLastUpdate = "_secondsSinceLastUpdate";

			/// <summary>
			/// Cached name for the '_totalParticles' field.
			/// </summary>
			// Token: 0x04005DDC RID: 24028
			public static readonly StringName _totalParticles = "_totalParticles";

			/// <summary>
			/// Cached name for the '_updateCount' field.
			/// </summary>
			// Token: 0x04005DDD RID: 24029
			public static readonly StringName _updateCount = "_updateCount";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001805 RID: 6149
		public class SignalName : Control.SignalName
		{
		}
	}
}
