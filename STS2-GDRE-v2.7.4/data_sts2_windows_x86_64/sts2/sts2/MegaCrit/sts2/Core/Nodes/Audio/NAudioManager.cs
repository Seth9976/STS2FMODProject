using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Audio
{
	// Token: 0x02000422 RID: 1058
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Audio/NAudioManager.cs")]
	public class NAudioManager : Node
	{
		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06004389 RID: 17289 RVA: 0x0020C979 File Offset: 0x0020AB79
		[Nullable(2)]
		public static NAudioManager Instance
		{
			[NullableContext(2)]
			get
			{
				NGame instance = NGame.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.AudioManager;
			}
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x0020C98B File Offset: 0x0020AB8B
		public override void _EnterTree()
		{
			this._audioNode = base.GetNode<Node>("Proxy");
		}

		/// <summary>
		/// Starts an sfx that has a continuous FMOD event that is meant to loop
		/// ex: Waterfall Giant's ambient sfx. Thieving hopper's hovering sfx.
		/// </summary>
		/// <param name="path">path of the fmod sfx</param>
		/// <param name="usesLoopParam">Whether on not the event uses our standard "loop" parameter to end the loop.</param>
		// Token: 0x0600438B RID: 17291 RVA: 0x0020C9A3 File Offset: 0x0020ABA3
		public void PlayLoop(string path, bool usesLoopParam)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._playLoop, new Variant[] { path, usesLoopParam });
		}

		/// <summary>
		/// Stops the specified FMOD event that is looping.
		/// </summary>
		/// <param name="path"></param>
		// Token: 0x0600438C RID: 17292 RVA: 0x0020C9DE File Offset: 0x0020ABDE
		public void StopLoop(string path)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._stopLoop, new Variant[] { path });
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0020CA0C File Offset: 0x0020AC0C
		public void SetParam(string path, string param, float value)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._setParam, new Variant[] { path, param, value });
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x0020CA5F File Offset: 0x0020AC5F
		public void StopAllLoops()
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._stopAllLoops, Array.Empty<Variant>());
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0020CA80 File Offset: 0x0020AC80
		public void PlayOneShot(string path, Dictionary<string, float> parameters, float volume = 1f)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			Dictionary dictionary = new Dictionary();
			foreach (KeyValuePair<string, float> keyValuePair in parameters)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			this._audioNode.Call(NAudioManager._playOneShot, new Variant[] { path, dictionary, volume });
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0020CB34 File Offset: 0x0020AD34
		public void PlayOneShot(string path, float volume = 1f)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this.PlayOneShot(path, new Dictionary<string, float>(), volume);
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0020CB4B File Offset: 0x0020AD4B
		public void PlayMusic(string music)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._playMusic, new Variant[] { music });
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0020CB79 File Offset: 0x0020AD79
		public void UpdateMusicParameter(string parameter, string value)
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._updateMusicParameterCallback, new Variant[] { parameter, value });
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x0020CBB4 File Offset: 0x0020ADB4
		public void StopMusic()
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._stopMusic, Array.Empty<Variant>());
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0020CBD4 File Offset: 0x0020ADD4
		public void SetMasterVol(float volume)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._setMasterVolume, new Variant[] { Mathf.Pow(volume, 2f) });
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0020CC0C File Offset: 0x0020AE0C
		public void SetSfxVol(float volume)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._setSfxVolume, new Variant[] { Mathf.Pow(volume, 2f) });
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0020CC44 File Offset: 0x0020AE44
		public void SetAmbienceVol(float volume)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._setAmbienceVolume, new Variant[] { Mathf.Pow(volume, 2f) });
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0020CC7C File Offset: 0x0020AE7C
		public void SetBgmVol(float volume)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			this._audioNode.Call(NAudioManager._setBgmVolume, new Variant[] { Mathf.Pow(volume, 2f) });
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004398 RID: 17304 RVA: 0x0020CCB4 File Offset: 0x0020AEB4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NAudioManager.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAudioManager.MethodName.PlayLoop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false),
					new PropertyInfo(1L, "usesLoopParam", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAudioManager.MethodName.StopLoop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAudioManager.MethodName.SetParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false),
					new PropertyInfo(4L, "param", 0L, "", 6L, false),
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAudioManager.MethodName.StopAllLoops, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAudioManager.MethodName.PlayOneShot, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false),
					new PropertyInfo(3L, "volume", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAudioManager.MethodName.PlayMusic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "music", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAudioManager.MethodName.UpdateMusicParameter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "parameter", 0L, "", 6L, false),
					new PropertyInfo(4L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAudioManager.MethodName.StopMusic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAudioManager.MethodName.SetMasterVol, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "volume", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAudioManager.MethodName.SetSfxVol, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "volume", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAudioManager.MethodName.SetAmbienceVol, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "volume", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAudioManager.MethodName.SetBgmVol, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "volume", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004399 RID: 17305 RVA: 0x0020D144 File Offset: 0x0020B344
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAudioManager.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.PlayLoop && args.Count == 2)
			{
				this.PlayLoop(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.StopLoop && args.Count == 1)
			{
				this.StopLoop(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.SetParam && args.Count == 3)
			{
				this.SetParam(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<float>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.StopAllLoops && args.Count == 0)
			{
				this.StopAllLoops();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.PlayOneShot && args.Count == 2)
			{
				this.PlayOneShot(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.PlayMusic && args.Count == 1)
			{
				this.PlayMusic(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.UpdateMusicParameter && args.Count == 2)
			{
				this.UpdateMusicParameter(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.StopMusic && args.Count == 0)
			{
				this.StopMusic();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.SetMasterVol && args.Count == 1)
			{
				this.SetMasterVol(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.SetSfxVol && args.Count == 1)
			{
				this.SetSfxVol(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.SetAmbienceVol && args.Count == 1)
			{
				this.SetAmbienceVol(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAudioManager.MethodName.SetBgmVol && args.Count == 1)
			{
				this.SetBgmVol(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600439A RID: 17306 RVA: 0x0020D408 File Offset: 0x0020B608
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAudioManager.MethodName._EnterTree || (ref method) == NAudioManager.MethodName.PlayLoop || (ref method) == NAudioManager.MethodName.StopLoop || (ref method) == NAudioManager.MethodName.SetParam || (ref method) == NAudioManager.MethodName.StopAllLoops || (ref method) == NAudioManager.MethodName.PlayOneShot || (ref method) == NAudioManager.MethodName.PlayMusic || (ref method) == NAudioManager.MethodName.UpdateMusicParameter || (ref method) == NAudioManager.MethodName.StopMusic || (ref method) == NAudioManager.MethodName.SetMasterVol || (ref method) == NAudioManager.MethodName.SetSfxVol || (ref method) == NAudioManager.MethodName.SetAmbienceVol || (ref method) == NAudioManager.MethodName.SetBgmVol || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600439B RID: 17307 RVA: 0x0020D4DF File Offset: 0x0020B6DF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAudioManager.PropertyName._audioNode)
			{
				this._audioNode = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600439C RID: 17308 RVA: 0x0020D504 File Offset: 0x0020B704
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAudioManager.PropertyName._audioNode)
			{
				value = VariantUtils.CreateFrom<Node>(ref this._audioNode);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600439D RID: 17309 RVA: 0x0020D530 File Offset: 0x0020B730
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAudioManager.PropertyName._audioNode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600439E RID: 17310 RVA: 0x0020D565 File Offset: 0x0020B765
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAudioManager.PropertyName._audioNode, Variant.From<Node>(ref this._audioNode));
		}

		/// <inheritdoc />
		// Token: 0x0600439F RID: 17311 RVA: 0x0020D584 File Offset: 0x0020B784
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAudioManager.PropertyName._audioNode, ref variant))
			{
				this._audioNode = variant.As<Node>();
			}
		}

		// Token: 0x04001A36 RID: 6710
		private static readonly StringName _setBgmVolume = new StringName("set_bgm_volume");

		// Token: 0x04001A37 RID: 6711
		private static readonly StringName _setAmbienceVolume = new StringName("set_ambience_volume");

		// Token: 0x04001A38 RID: 6712
		private static readonly StringName _setSfxVolume = new StringName("set_sfx_volume");

		// Token: 0x04001A39 RID: 6713
		private static readonly StringName _setMasterVolume = new StringName("set_master_volume");

		// Token: 0x04001A3A RID: 6714
		private static readonly StringName _stopMusic = new StringName("stop_music");

		// Token: 0x04001A3B RID: 6715
		private static readonly StringName _playMusic = new StringName("play_music");

		// Token: 0x04001A3C RID: 6716
		private static readonly StringName _playOneShot = new StringName("play_one_shot");

		// Token: 0x04001A3D RID: 6717
		private static readonly StringName _stopAllLoops = new StringName("stop_all_loops");

		// Token: 0x04001A3E RID: 6718
		private static readonly StringName _setParam = new StringName("set_param");

		// Token: 0x04001A3F RID: 6719
		private static readonly StringName _stopLoop = new StringName("stop_loop");

		// Token: 0x04001A40 RID: 6720
		private static readonly StringName _playLoop = new StringName("play_loop");

		// Token: 0x04001A41 RID: 6721
		private static readonly StringName _updateMusicParameterCallback = new StringName("update_music_parameter");

		// Token: 0x04001A42 RID: 6722
		private Node _audioNode;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001978 RID: 6520
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040064A2 RID: 25762
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the 'PlayLoop' method.
			/// </summary>
			// Token: 0x040064A3 RID: 25763
			public static readonly StringName PlayLoop = "PlayLoop";

			/// <summary>
			/// Cached name for the 'StopLoop' method.
			/// </summary>
			// Token: 0x040064A4 RID: 25764
			public static readonly StringName StopLoop = "StopLoop";

			/// <summary>
			/// Cached name for the 'SetParam' method.
			/// </summary>
			// Token: 0x040064A5 RID: 25765
			public static readonly StringName SetParam = "SetParam";

			/// <summary>
			/// Cached name for the 'StopAllLoops' method.
			/// </summary>
			// Token: 0x040064A6 RID: 25766
			public static readonly StringName StopAllLoops = "StopAllLoops";

			/// <summary>
			/// Cached name for the 'PlayOneShot' method.
			/// </summary>
			// Token: 0x040064A7 RID: 25767
			public static readonly StringName PlayOneShot = "PlayOneShot";

			/// <summary>
			/// Cached name for the 'PlayMusic' method.
			/// </summary>
			// Token: 0x040064A8 RID: 25768
			public static readonly StringName PlayMusic = "PlayMusic";

			/// <summary>
			/// Cached name for the 'UpdateMusicParameter' method.
			/// </summary>
			// Token: 0x040064A9 RID: 25769
			public static readonly StringName UpdateMusicParameter = "UpdateMusicParameter";

			/// <summary>
			/// Cached name for the 'StopMusic' method.
			/// </summary>
			// Token: 0x040064AA RID: 25770
			public static readonly StringName StopMusic = "StopMusic";

			/// <summary>
			/// Cached name for the 'SetMasterVol' method.
			/// </summary>
			// Token: 0x040064AB RID: 25771
			public static readonly StringName SetMasterVol = "SetMasterVol";

			/// <summary>
			/// Cached name for the 'SetSfxVol' method.
			/// </summary>
			// Token: 0x040064AC RID: 25772
			public static readonly StringName SetSfxVol = "SetSfxVol";

			/// <summary>
			/// Cached name for the 'SetAmbienceVol' method.
			/// </summary>
			// Token: 0x040064AD RID: 25773
			public static readonly StringName SetAmbienceVol = "SetAmbienceVol";

			/// <summary>
			/// Cached name for the 'SetBgmVol' method.
			/// </summary>
			// Token: 0x040064AE RID: 25774
			public static readonly StringName SetBgmVol = "SetBgmVol";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001979 RID: 6521
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_audioNode' field.
			/// </summary>
			// Token: 0x040064AF RID: 25775
			public static readonly StringName _audioNode = "_audioNode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200197A RID: 6522
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
