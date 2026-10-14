using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x02000217 RID: 535
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/LocalizedTexture.cs")]
	public class LocalizedTexture : Resource
	{
		// Token: 0x06001B68 RID: 7016 RVA: 0x000B2550 File Offset: 0x000B0750
		[NullableContext(2)]
		public bool TryGetTexture(out Texture2D texture)
		{
			texture = null;
			if (SaveManager.Instance.SettingsSave == null)
			{
				return false;
			}
			string language = SaveManager.Instance.SettingsSave.Language;
			if (string.IsNullOrEmpty(language))
			{
				return false;
			}
			Texture2D texture2D;
			if (!this._textures.TryGetValue(language, ref texture2D))
			{
				return false;
			}
			texture = texture2D;
			return true;
		}

		/// <inheritdoc />
		// Token: 0x06001B69 RID: 7017 RVA: 0x000B259E File Offset: 0x000B079E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == LocalizedTexture.PropertyName._textures)
			{
				this._textures = VariantUtils.ConvertToDictionary<string, Texture2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001B6A RID: 7018 RVA: 0x000B25C3 File Offset: 0x000B07C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == LocalizedTexture.PropertyName._textures)
			{
				value = VariantUtils.CreateFromDictionary<string, Texture2D>(this._textures);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B6B RID: 7019 RVA: 0x000B25F0 File Offset: 0x000B07F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(27L, LocalizedTexture.PropertyName._textures, 23L, "4/0:;24/17:Texture2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B6C RID: 7020 RVA: 0x000B2626 File Offset: 0x000B0826
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(LocalizedTexture.PropertyName._textures, Variant.CreateFrom<string, Texture2D>(this._textures));
		}

		/// <inheritdoc />
		// Token: 0x06001B6D RID: 7021 RVA: 0x000B2648 File Offset: 0x000B0848
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(LocalizedTexture.PropertyName._textures, ref variant))
			{
				this._textures = variant.AsGodotDictionary<string, Texture2D>();
			}
		}

		// Token: 0x04000A61 RID: 2657
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011AC RID: 4524
		public class MethodName : Resource.MethodName
		{
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011AD RID: 4525
		public class PropertyName : Resource.PropertyName
		{
			/// <summary>
			/// Cached name for the '_textures' field.
			/// </summary>
			// Token: 0x040044EB RID: 17643
			public static readonly StringName _textures = "_textures";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011AE RID: 4526
		public class SignalName : Resource.SignalName
		{
		}
	}
}
