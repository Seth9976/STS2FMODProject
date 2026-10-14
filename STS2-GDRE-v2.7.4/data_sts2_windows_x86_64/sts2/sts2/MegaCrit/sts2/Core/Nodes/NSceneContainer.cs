using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes
{
	// Token: 0x0200017B RID: 379
	[ScriptPath("res://src/Core/Nodes/NSceneContainer.cs")]
	public class NSceneContainer : Control
	{
		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x0006B9BC File Offset: 0x00069BBC
		// (set) Token: 0x0600123E RID: 4670 RVA: 0x0006B9EC File Offset: 0x00069BEC
		[Nullable(2)]
		public Control CurrentScene
		{
			[NullableContext(2)]
			get
			{
				if (this._currentScene == null)
				{
					return null;
				}
				if (!GodotObject.IsInstanceValid(this._currentScene))
				{
					return null;
				}
				if (this._currentScene.IsQueuedForDeletion())
				{
					return null;
				}
				return this._currentScene;
			}
			[NullableContext(2)]
			private set
			{
				this._currentScene = value;
			}
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0006B9F8 File Offset: 0x00069BF8
		[NullableContext(1)]
		public void SetCurrentScene(Control node)
		{
			foreach (Node node2 in base.GetChildren(false))
			{
				this.RemoveChildSafely(node2);
				node2.QueueFreeSafely();
			}
			this.CurrentScene = node;
			if (node.GetParent() == null)
			{
				this.AddChildSafely(node);
				return;
			}
			node.Reparent(this, true);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001240 RID: 4672 RVA: 0x0006BA6C File Offset: 0x00069C6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NSceneContainer.MethodName.SetCurrentScene, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001241 RID: 4673 RVA: 0x0006BAE0 File Offset: 0x00069CE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSceneContainer.MethodName.SetCurrentScene && args.Count == 1)
			{
				this.SetCurrentScene(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001242 RID: 4674 RVA: 0x0006BB1E File Offset: 0x00069D1E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSceneContainer.MethodName.SetCurrentScene || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001243 RID: 4675 RVA: 0x0006BB36 File Offset: 0x00069D36
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSceneContainer.PropertyName.CurrentScene)
			{
				this.CurrentScene = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSceneContainer.PropertyName._currentScene)
			{
				this._currentScene = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001244 RID: 4676 RVA: 0x0006BB78 File Offset: 0x00069D78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSceneContainer.PropertyName.CurrentScene)
			{
				Control currentScene = this.CurrentScene;
				value = VariantUtils.CreateFrom<Control>(ref currentScene);
				return true;
			}
			if ((ref name) == NSceneContainer.PropertyName._currentScene)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._currentScene);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001245 RID: 4677 RVA: 0x0006BBD0 File Offset: 0x00069DD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSceneContainer.PropertyName._currentScene, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSceneContainer.PropertyName.CurrentScene, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001246 RID: 4678 RVA: 0x0006BC28 File Offset: 0x00069E28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName currentScene = NSceneContainer.PropertyName.CurrentScene;
			Control currentScene2 = this.CurrentScene;
			info.AddProperty(currentScene, Variant.From<Control>(ref currentScene2));
			info.AddProperty(NSceneContainer.PropertyName._currentScene, Variant.From<Control>(ref this._currentScene));
		}

		/// <inheritdoc />
		// Token: 0x06001247 RID: 4679 RVA: 0x0006BC6C File Offset: 0x00069E6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSceneContainer.PropertyName.CurrentScene, ref variant))
			{
				this.CurrentScene = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSceneContainer.PropertyName._currentScene, ref variant2))
			{
				this._currentScene = variant2.As<Control>();
			}
		}

		// Token: 0x0400066E RID: 1646
		[Nullable(2)]
		private Control _currentScene;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F7E RID: 3966
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'SetCurrentScene' method.
			/// </summary>
			// Token: 0x04003DD6 RID: 15830
			public static readonly StringName SetCurrentScene = "SetCurrentScene";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F7F RID: 3967
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CurrentScene' property.
			/// </summary>
			// Token: 0x04003DD7 RID: 15831
			public static readonly StringName CurrentScene = "CurrentScene";

			/// <summary>
			/// Cached name for the '_currentScene' field.
			/// </summary>
			// Token: 0x04003DD8 RID: 15832
			public static readonly StringName _currentScene = "_currentScene";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F80 RID: 3968
		public class SignalName : Control.SignalName
		{
		}
	}
}
