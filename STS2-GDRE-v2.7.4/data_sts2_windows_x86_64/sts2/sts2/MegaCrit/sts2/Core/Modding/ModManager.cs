using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using MegaCrit.Sts2.Core.TestSupport;
using Steamworks;

namespace MegaCrit.Sts2.Core.Modding
{
	// Token: 0x02000B97 RID: 2967
	[NullableContext(1)]
	[Nullable(0)]
	public static class ModManager
	{
		// Token: 0x17002257 RID: 8791
		// (get) Token: 0x06007BCA RID: 31690 RVA: 0x002A162C File Offset: 0x0029F82C
		// (set) Token: 0x06007BCB RID: 31691 RVA: 0x002A1633 File Offset: 0x0029F833
		public static ModManagerState State { get; private set; }

		// Token: 0x17002258 RID: 8792
		// (get) Token: 0x06007BCC RID: 31692 RVA: 0x002A163B File Offset: 0x0029F83B
		public static IReadOnlyList<Mod> Mods
		{
			get
			{
				return ModManager._mods;
			}
		}

		/// <summary>
		/// Called when a new mod is detected. Use this to display a warning at runtime if the user adds new mods.
		/// </summary>
		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06007BCD RID: 31693 RVA: 0x002A1644 File Offset: 0x0029F844
		// (remove) Token: 0x06007BCE RID: 31694 RVA: 0x002A1678 File Offset: 0x0029F878
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public static event Action<Mod> OnModDetected;

		/// <summary>
		/// Used by mods to hook into the metrics upload process.
		/// This is unused by STS2 code. It is only used by mods, and is called when we would ordinarily upload metrics.
		/// </summary>
		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06007BCF RID: 31695 RVA: 0x002A16AC File Offset: 0x0029F8AC
		// (remove) Token: 0x06007BD0 RID: 31696 RVA: 0x002A16E0 File Offset: 0x0029F8E0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public static event ModManager.MetricsUploadHook OnMetricsUpload;

		// Token: 0x17002259 RID: 8793
		// (get) Token: 0x06007BD1 RID: 31697 RVA: 0x002A1713 File Offset: 0x0029F913
		public static bool PlayerAgreedToModLoading
		{
			get
			{
				ModSettings settings = ModManager._settings;
				return settings != null && settings.PlayerAgreedToModLoading;
			}
		}

		// Token: 0x1700225A RID: 8794
		// (get) Token: 0x06007BD2 RID: 31698 RVA: 0x002A1725 File Offset: 0x0029F925
		// (set) Token: 0x06007BD3 RID: 31699 RVA: 0x002A172C File Offset: 0x0029F92C
		public static bool UnmoddedSavesWereCopied { get; private set; }

		/// <summary>
		/// Used only in test mode.
		/// </summary>
		// Token: 0x1700225B RID: 8795
		// (get) Token: 0x06007BD4 RID: 31700 RVA: 0x002A1734 File Offset: 0x0029F934
		public static Dictionary<string, Action> TestInitializers { get; } = new Dictionary<string, Action>();

		/// <summary>
		/// Loads all mods from the appropriate directories. This includes the "mods" directory next to the executable, as
		/// well as Steam workshop files.
		/// This should be called as early as possible in the game's initialization process so that downstream classes
		/// (ModelDb, LocManager) pick up the changes that the mods apply.
		/// </summary>
		// Token: 0x06007BD5 RID: 31701 RVA: 0x002A173C File Offset: 0x0029F93C
		public static async Task Initialize(IModManagerFileIo fileIo, [Nullable(2)] ModSettings settings, [Nullable(2)] SemanticVersion gameVersion)
		{
			ModManager._settings = settings;
			ModManager._fileIo = fileIo;
			ModManager._gameVersion = gameVersion;
			if (ModManager._gameVersion == null)
			{
				Log.Warn("Game doesn't have ReleaseInfo. We can't check version compatibility, so assuming all mods are supported.", 2);
			}
			if (CommandLineHelper.HasArg("nomods"))
			{
				Log.Info("'nomods' passed as executable argument, skipping mod initialization", 2);
				ModManager.State = ModManagerState.Skipped;
			}
			else if (TestMode.IsOn && !ModManager._allowInitForTests)
			{
				ModManager.State = ModManagerState.Skipped;
			}
			else
			{
				ModManager._allowInitForTests = false;
				AppDomain currentDomain = AppDomain.CurrentDomain;
				ResolveEventHandler resolveEventHandler;
				if ((resolveEventHandler = ModManager.<>O.<0>__HandleAssemblyResolveFailure) == null)
				{
					resolveEventHandler = (ModManager.<>O.<0>__HandleAssemblyResolveFailure = new ResolveEventHandler(ModManager.HandleAssemblyResolveFailure));
				}
				currentDomain.AssemblyResolve += resolveEventHandler;
				string executablePath = OS.GetExecutablePath();
				string directoryName = Path.GetDirectoryName(executablePath);
				string text = Path.Combine(directoryName, "mods");
				string text2 = Path.Combine(directoryName, "mods_STEAMTEST");
				if (fileIo.DirectoryExists(text))
				{
					ModManager.ReadModsInDirRecursive(text, ModSource.ModsDirectory, null);
				}
				if (fileIo.DirectoryExists(text2))
				{
					ModManager.ReadModsInDirRecursive(text2, ModSource.SteamWorkshop, null);
				}
				if (SteamInitializer.Initialized)
				{
					ModManager.ReadSteamMods();
				}
				if (OS.IsDebugBuild())
				{
					await Task.Yield();
				}
				if (ModManager._mods.Count == 0)
				{
					ModManager.State = ModManagerState.Initialized;
				}
				else
				{
					await ModManager.CheckSteamBranchSupport();
					ModManager.RemoveDisabledMods();
					ModSettings settings2 = ModManager._settings;
					ModManager.SortModList(((settings2 != null) ? settings2.ModList : null) ?? new List<SettingsSaveMod>());
					foreach (Mod mod2 in ModManager._mods)
					{
						ModManager.TryLoadMod(mod2);
					}
					if (ModManager.IsRunningModded())
					{
						int num = ModManager._mods.Count<Mod>((Mod m) => m.state == ModLoadState.Loaded);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 2);
						defaultInterpolatedStringHandler.AppendLiteral(" --- RUNNING MODDED! --- Loaded ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num);
						defaultInterpolatedStringHandler.AppendLiteral(" mods (");
						defaultInterpolatedStringHandler.AppendFormatted<int>(ModManager._mods.Count);
						defaultInterpolatedStringHandler.AppendLiteral(" total)");
						Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
					ModManager.State = ModManagerState.Initialized;
					bool flag = false;
					if (ModManager._settings != null)
					{
						List<SettingsSaveMod> list = new List<SettingsSaveMod>();
						using (List<Mod>.Enumerator enumerator2 = ModManager._mods.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Mod mod = enumerator2.Current;
								SettingsSaveMod settingsSaveMod = new SettingsSaveMod(mod);
								SettingsSaveMod settingsSaveMod2 = ModManager._settings.ModList.FirstOrDefault<SettingsSaveMod>(delegate(SettingsSaveMod m)
								{
									string id = m.Id;
									ModManifest manifest = mod.manifest;
									return id == ((manifest != null) ? manifest.id : null);
								});
								settingsSaveMod.IsEnabled = settingsSaveMod2 == null || settingsSaveMod2.IsEnabled;
								list.Add(settingsSaveMod);
							}
						}
						flag = ModManager._settings.ModList.Count == 0;
						ModManager._settings.ModList = list;
					}
					if (flag)
					{
						Log.Info("Player is playing modded for the first time. Checking if we need to copy unmodded save files", 2);
						ModManager.CopyUnmoddedSaveFilesIfNeeded();
					}
				}
			}
		}

		/// <summary>
		/// This should only be called by tests.
		/// Typically, ModManager is initialized once at the start of the game and never cleared.
		/// </summary>
		// Token: 0x06007BD6 RID: 31702 RVA: 0x002A1790 File Offset: 0x0029F990
		public static void ResetForTests()
		{
			if (TestMode.IsOff)
			{
				throw new NotImplementedException("Tried to reset ModManager outside of tests! This is not allowed, as we cannot unload DLLs or PCKs");
			}
			ModManager._mods.Clear();
			ModManager.State = ModManagerState.None;
			ModManager._settings = null;
			ModManager._fileIo = null;
			ModManager._allowInitForTests = true;
			ModManager._circularDependencies.Clear();
			ModManager.TestInitializers.Clear();
		}

		/// <summary>
		/// Sorts mods so that dependencies are loaded before the mods that depend on them (topological sort).
		/// Uses Kahn's algorithm: https://en.wikipedia.org/wiki/Topological_sorting#Kahn's_algorithm
		///
		/// How it works:
		///   1. Build a graph: count each mod's dependencies (in-degree) and track reverse edges (dependentsMap).
		///   2. Seed a frontier queue with all mods that have zero dependencies (they're ready to load).
		///   3. Dequeue a mod, add it to the sorted output, and decrement the in-degree of each mod that depends
		///      on it. When a mod's in-degree reaches zero, all its dependencies are satisfied, so enqueue it.
		///   4. When the queue is empty, any mod NOT in the sorted output is part of a dependency cycle (its
		///      in-degree never reached zero because it's waiting on something that's waiting on it).
		///
		/// For mods with no dependency relationship between them, we use a PriorityQueue keyed by the user's
		/// manual ordering (from settings) to keep the sort stable.
		/// </summary>
		// Token: 0x06007BD7 RID: 31703 RVA: 0x002A17E8 File Offset: 0x0029F9E8
		private static void SortModList(List<SettingsSaveMod> manualOrdering)
		{
			List<Mod> list = new List<Mod>();
			List<Mod> list2 = new List<Mod>();
			foreach (Mod mod in ModManager._mods)
			{
				if (mod.state != ModLoadState.None)
				{
					list2.Add(mod);
				}
				else
				{
					list.Add(mod);
				}
			}
			List<int> list3 = new List<int>();
			Dictionary<Mod, List<Mod>> dictionary = new Dictionary<Mod, List<Mod>>();
			for (int i = 0; i < list.Count; i++)
			{
				Mod mod2 = list[i];
				int num = 0;
				ModManifest manifest = mod2.manifest;
				if (((manifest != null) ? manifest.dependencies : null) != null)
				{
					using (List<ModDependency>.Enumerator enumerator2 = mod2.manifest.dependencies.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ModDependency declaredDependency = enumerator2.Current;
							Mod mod3 = list.FirstOrDefault<Mod>(delegate(Mod m)
							{
								ModManifest manifest6 = m.manifest;
								return ((manifest6 != null) ? manifest6.id : null) == declaredDependency.id;
							});
							if (mod3 != null)
							{
								num++;
								List<Mod> list4;
								if (!dictionary.TryGetValue(mod3, out list4))
								{
									list4 = new List<Mod>();
									dictionary[mod3] = list4;
								}
								list4.Add(mod2);
							}
						}
					}
				}
				list3.Add(num);
			}
			PriorityQueue<Mod, int> priorityQueue = new PriorityQueue<Mod, int>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			for (int j = 0; j < manualOrdering.Count; j++)
			{
				dictionary2[manualOrdering[j].Id] = j;
			}
			for (int k = 0; k < list.Count; k++)
			{
				if (list3[k] == 0)
				{
					int num3;
					int num2 = (dictionary2.TryGetValue(list[k].manifest.id, out num3) ? num3 : 999999999);
					priorityQueue.Enqueue(list[k], num2);
				}
			}
			List<Mod> list5 = new List<Mod>();
			while (priorityQueue.Count > 0)
			{
				Mod mod4 = priorityQueue.Dequeue();
				list5.Add(mod4);
				List<Mod> list6;
				if (dictionary.TryGetValue(mod4, out list6))
				{
					foreach (Mod mod5 in list6)
					{
						int num4 = list.IndexOf(mod5);
						if (num4 < 0)
						{
							throw new InvalidOperationException("Bug in mod sorting logic!");
						}
						List<int> list7 = list3;
						int num5 = num4;
						int num6 = list7[num5];
						list7[num5] = num6 - 1;
						if (list3[num4] == 0)
						{
							int num8;
							int num7 = (dictionary2.TryGetValue(mod5.manifest.id, out num8) ? num8 : 999999999);
							priorityQueue.Enqueue(mod5, num7);
						}
					}
				}
			}
			HashSet<Mod> hashSet = new HashSet<Mod>();
			foreach (Mod mod6 in list5)
			{
				hashSet.Add(mod6);
			}
			HashSet<Mod> sortedSet = hashSet;
			string text = string.Join(", ", list.Where<Mod>((Mod m) => !sortedSet.Contains(m)).Select<Mod, string>(delegate(Mod m)
			{
				ModManifest manifest7 = m.manifest;
				if (manifest7 == null)
				{
					return null;
				}
				return manifest7.id;
			}));
			foreach (Mod mod7 in list)
			{
				if (!sortedSet.Contains(mod7))
				{
					ModManifest manifest2 = mod7.manifest;
					if (((manifest2 != null) ? manifest2.id : null) != null)
					{
						ModManager._circularDependencies[mod7.manifest.id] = text;
					}
				}
			}
			foreach (Mod mod8 in list)
			{
				if (!sortedSet.Contains(mod8))
				{
					list5.Add(mod8);
				}
			}
			bool flag = manualOrdering.Count != list5.Count;
			if (!flag)
			{
				for (int l = 0; l < manualOrdering.Count; l++)
				{
					string id = manualOrdering[l].Id;
					ModManifest manifest3 = list5[l].manifest;
					if (id != ((manifest3 != null) ? manifest3.id : null))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				Log.Info("Mods have been re-sorted because we detected a change or dependency order was broken. New sorting order:", 2);
				for (int n = 0; n < list5.Count; n++)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
					defaultInterpolatedStringHandler.AppendLiteral("  ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(n);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					ModManifest manifest4 = list5[n].manifest;
					defaultInterpolatedStringHandler.AppendFormatted((manifest4 != null) ? manifest4.name : null);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					ModManifest manifest5 = list5[n].manifest;
					defaultInterpolatedStringHandler.AppendFormatted((manifest5 != null) ? manifest5.id : null);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
			}
			list5.AddRange(list2);
			ModManager._mods = list5;
		}

		/// <summary>
		/// Sets mods in _mods to disabled for various reasons.
		///
		/// If it's disabled in settings.save, the mod is unconditionally disabled.
		///
		/// If the user has subscribed to a mod via Steam workshop and also has it in their local directory:
		///  - Prefer the newer mod if it is the same version
		///  - Otherwise, load the mod that has a greater version
		/// This is to help mod developers in development.
		/// </summary>
		// Token: 0x06007BD8 RID: 31704 RVA: 0x002A1D44 File Offset: 0x0029FF44
		private static void RemoveDisabledMods()
		{
			Dictionary<string, Mod> dictionary = new Dictionary<string, Mod>();
			foreach (Mod mod in ModManager._mods)
			{
				ModManifest manifest = mod.manifest;
				if (((manifest != null) ? manifest.id : null) != null)
				{
					ModSettings settings = ModManager._settings;
					bool flag = settings != null && settings.IsModDisabled(mod.manifest.id, mod.modSource);
					if (flag)
					{
						Log.Info("Skipping loading mod " + mod.manifest.id + ", it is set to disabled in settings", 2);
						mod.state = ModLoadState.Disabled;
					}
					else if (mod.modSource == ModSource.ModsDirectory)
					{
						dictionary.TryAdd(mod.manifest.id, mod);
					}
				}
			}
			foreach (Mod mod2 in ModManager._mods)
			{
				ModManifest manifest2 = mod2.manifest;
				Mod mod3;
				if (((manifest2 != null) ? manifest2.id : null) != null && mod2.modSource == ModSource.SteamWorkshop && mod2.state == ModLoadState.None && dictionary.TryGetValue(mod2.manifest.id, out mod3))
				{
					SemanticVersion semanticVersion = null;
					SemanticVersion semanticVersion2 = null;
					ModManifest manifest3 = mod3.manifest;
					if (((manifest3 != null) ? manifest3.version : null) != null)
					{
						SemanticVersion.TryFromString(mod3.manifest.version, out semanticVersion);
					}
					if (mod2.manifest.version != null)
					{
						SemanticVersion.TryFromString(mod2.manifest.version, out semanticVersion2);
					}
					if (semanticVersion2 == null || semanticVersion == null)
					{
						Log.Warn("Mod with ID " + mod2.manifest.id + " and unknown version is loaded both via Steam and local mods directory. Disabling the Steam workshop version.", 2);
						mod2.state = ModLoadState.DisabledDuplicate;
						List<LocString> errors = mod2.errors;
						if (errors != null)
						{
							errors.Clear();
						}
					}
					else
					{
						int num = semanticVersion2.CompareTo(semanticVersion);
						if (num == 0)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(114, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Mod with ID ");
							defaultInterpolatedStringHandler.AppendFormatted(mod2.manifest.id);
							defaultInterpolatedStringHandler.AppendLiteral(" and version ");
							defaultInterpolatedStringHandler.AppendFormatted(mod2.manifest.version);
							defaultInterpolatedStringHandler.AppendLiteral(" is loaded both via Steam and local mods directory. Disabling the Steam workshop version.");
							Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							mod2.state = ModLoadState.DisabledDuplicate;
							List<LocString> errors2 = mod2.errors;
							if (errors2 != null)
							{
								errors2.Clear();
							}
						}
						else if (num > 0)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(153, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Mod with ID ");
							defaultInterpolatedStringHandler.AppendFormatted(mod2.manifest.id);
							defaultInterpolatedStringHandler.AppendLiteral(" is loaded both via Steam and local mods directory. Steam version (");
							defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion>(semanticVersion2);
							defaultInterpolatedStringHandler.AppendLiteral(") is greater than local version (");
							defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion>(semanticVersion);
							defaultInterpolatedStringHandler.AppendLiteral("), so we are disabling the local version.");
							Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							mod3.state = ModLoadState.DisabledDuplicate;
							List<LocString> errors3 = mod3.errors;
							if (errors3 != null)
							{
								errors3.Clear();
							}
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(162, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Mod with ID ");
							defaultInterpolatedStringHandler.AppendFormatted(mod2.manifest.id);
							defaultInterpolatedStringHandler.AppendLiteral(" is loaded both via Steam and local mods directory. Local version (");
							defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion>(semanticVersion);
							defaultInterpolatedStringHandler.AppendLiteral(") is greater than steam version (");
							defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion>(semanticVersion2);
							defaultInterpolatedStringHandler.AppendLiteral("), so we are disabling the Steam workshop version.");
							Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							mod2.state = ModLoadState.DisabledDuplicate;
							List<LocString> errors4 = mod2.errors;
							if (errors4 != null)
							{
								errors4.Clear();
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Recurses through the directory and reads all the mod PCKs and DLLs in it, putting it in the _mods list.
		/// We allow mods in directories to give modders a little slack in how they organize their files.
		/// Pass the newMods list if you wish to get the mods that have been newly read.
		/// </summary>
		// Token: 0x06007BD9 RID: 31705 RVA: 0x002A2114 File Offset: 0x002A0314
		private static void ReadModsInDirRecursive(string path, ModSource source, [Nullable(new byte[] { 2, 1 })] List<Mod> newMods)
		{
			IModManagerFileIo fileIo = ModManager._fileIo;
			foreach (string text in ((fileIo != null) ? fileIo.GetFilesAt(path) : null) ?? Array.Empty<string>())
			{
				if (text.EndsWith(".json"))
				{
					string text2 = Path.Combine(path, text);
					Log.Info("Found mod manifest file " + text2, 2);
					Mod mod = ModManager.ReadModManifest(text2, source);
					if (mod != null)
					{
						ModManager._mods.Add(mod);
						if (newMods != null)
						{
							newMods.Add(mod);
						}
					}
				}
			}
			IModManagerFileIo fileIo2 = ModManager._fileIo;
			foreach (string text3 in ((fileIo2 != null) ? fileIo2.GetDirectoriesAt(path) : null) ?? Array.Empty<string>())
			{
				string text4 = Path.Combine(path, text3);
				if (ModManager._fileIo.DirectoryExists(text4))
				{
					ModManager.ReadModsInDirRecursive(text4, source, newMods);
				}
			}
		}

		// Token: 0x06007BDA RID: 31706 RVA: 0x002A21F4 File Offset: 0x002A03F4
		[return: Nullable(2)]
		private static Mod ReadModManifest(string filename, ModSource source)
		{
			if (ModManager._fileIo == null)
			{
				return null;
			}
			Mod mod;
			try
			{
				using (Stream stream = ModManager._fileIo.OpenStream(filename, 1L))
				{
					List<LocString> list;
					ModManifest modManifest = ModManifest.ReadFromStream(stream, out list);
					if (modManifest == null)
					{
						throw new InvalidOperationException("JSON deserialization returned null when trying to deserialize mod manifest!");
					}
					if (modManifest.id == null)
					{
						if (modManifest.name == null && modManifest.author == null && modManifest.description == null && modManifest.version == null)
						{
							Log.Info("JSON file " + filename + " does not look like a mod manifest; skipping it.", 2);
						}
						else
						{
							Log.Error("JSON file " + filename + " looks like a mod manifest but is missing the 'id' field! This is not allowed.", 2);
						}
						mod = null;
					}
					else
					{
						Mod mod2 = new Mod
						{
							path = StringExtensions.GetBaseDir(filename),
							modSource = source,
							manifest = modManifest,
							errors = list
						};
						mod = mod2;
					}
				}
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Caught ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(ex.GetType());
				defaultInterpolatedStringHandler.AppendLiteral(" trying to deserialize mod manifest json at path ");
				defaultInterpolatedStringHandler.AppendFormatted(filename);
				defaultInterpolatedStringHandler.AppendLiteral(":\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				mod = null;
			}
			return mod;
		}

		/// <summary>
		/// Decides whether <paramref name="currentBranch" /> is supported by a Steam Workshop item, given the supported
		/// game-version ranges Steam reports for it. Each range is a (min, max) pair of branch names ordered oldest to
		/// newest (see <see cref="T:MegaCrit.Sts2.Core.Platform.PlatformBranch" />); an empty string is a documented open-ended bound (empty min = no lower
		/// bound, empty max = no upper bound, both empty = every branch). Returns the newest supported branch seen so the
		/// caller can surface it in an error message. Pure and side-effect free so it can be unit tested without Steam.
		/// </summary>
		// Token: 0x06007BDB RID: 31707 RVA: 0x002A234C File Offset: 0x002A054C
		[NullableContext(0)]
		[return: TupleElementNames(new string[] { "IsSupported", "MaxSupportedBranch" })]
		public static ValueTuple<bool, PlatformBranch?> EvaluateBranchSupport(PlatformBranch currentBranch, [TupleElementNames(new string[] { "MinBranch", "MaxBranch" })] [Nullable(new byte[] { 1, 0, 1, 1 })] IReadOnlyList<ValueTuple<string, string>> supportedVersions)
		{
			if (supportedVersions.Count == 0)
			{
				return new ValueTuple<bool, PlatformBranch?>(true, null);
			}
			PlatformBranch? platformBranch = null;
			foreach (ValueTuple<string, string> valueTuple in supportedVersions)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				bool flag = string.IsNullOrEmpty(item);
				bool flag2 = string.IsNullOrEmpty(item2);
				PlatformBranch? platformBranch2 = (flag ? null : PlatformBranchExtensions.FromName(item));
				PlatformBranch? platformBranch3 = (flag2 ? null : PlatformBranchExtensions.FromName(item2));
				PlatformBranch? platformBranch4 = (flag2 ? new PlatformBranch?(PlatformBranch.DevTest) : platformBranch3);
				if (platformBranch4 != null)
				{
					if (platformBranch != null)
					{
						PlatformBranch? platformBranch5 = platformBranch4;
						PlatformBranch? platformBranch6 = platformBranch;
						if (!((platformBranch5.GetValueOrDefault() > platformBranch6.GetValueOrDefault()) & ((platformBranch5 != null) & (platformBranch6 != null))))
						{
							goto IL_00CD;
						}
					}
					platformBranch = platformBranch4;
				}
				IL_00CD:
				bool flag3;
				if (!flag)
				{
					if (platformBranch2 != null)
					{
						PlatformBranch? platformBranch6 = platformBranch2;
						flag3 = (currentBranch >= platformBranch6.GetValueOrDefault()) & (platformBranch6 != null);
					}
					else
					{
						flag3 = false;
					}
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = flag3;
				bool flag5;
				if (!flag2)
				{
					if (platformBranch3 != null)
					{
						PlatformBranch? platformBranch6 = platformBranch3;
						flag5 = (currentBranch <= platformBranch6.GetValueOrDefault()) & (platformBranch6 != null);
					}
					else
					{
						flag5 = false;
					}
				}
				else
				{
					flag5 = true;
				}
				bool flag6 = flag5;
				if (flag4 && flag6)
				{
					return new ValueTuple<bool, PlatformBranch?>(true, platformBranch);
				}
			}
			return new ValueTuple<bool, PlatformBranch?>(false, platformBranch);
		}

		// Token: 0x06007BDC RID: 31708 RVA: 0x002A24D4 File Offset: 0x002A06D4
		private static void ReadSteamMods()
		{
			uint num = SteamUGC.GetNumSubscribedItems();
			PublishedFileId_t[] array = new PublishedFileId_t[num];
			num = SteamUGC.GetSubscribedItems(array, num);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				PublishedFileId_t publishedFileId_t = array[num2];
				ModManager.TryReadModFromSteam(publishedFileId_t, null);
				num2++;
			}
			Callback<ItemInstalled_t>.DispatchDelegate dispatchDelegate;
			if ((dispatchDelegate = ModManager.<>O.<1>__OnSteamWorkshopItemInstalled) == null)
			{
				dispatchDelegate = (ModManager.<>O.<1>__OnSteamWorkshopItemInstalled = new Callback<ItemInstalled_t>.DispatchDelegate(ModManager.OnSteamWorkshopItemInstalled));
			}
			ModManager._steamItemInstalledCallback = Callback<ItemInstalled_t>.Create(dispatchDelegate);
		}

		// Token: 0x06007BDD RID: 31709 RVA: 0x002A2538 File Offset: 0x002A0738
		private static void TryReadModFromSteam(PublishedFileId_t workshopItemId, [Nullable(new byte[] { 2, 1 })] List<Mod> newMods)
		{
			ulong num;
			string text;
			uint num2;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!SteamUGC.GetItemInstallInfo(workshopItemId, ref num, ref text, 256U, ref num2))
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not get Steam Workshop item install info for item ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(workshopItemId.m_PublishedFileId);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Looking for mods to load from Steam Workshop mod ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(workshopItemId.m_PublishedFileId);
			defaultInterpolatedStringHandler.AppendLiteral(" in ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(" (size ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(num);
			defaultInterpolatedStringHandler.AppendLiteral(", last modified ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(num2);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (ModManager._fileIo != null && !ModManager._fileIo.DirectoryExists(text))
			{
				Log.Warn("Could not open Steam Workshop folder: " + text, 2);
				return;
			}
			List<Mod> list = new List<Mod>();
			ModManager.ReadModsInDirRecursive(text, ModSource.SteamWorkshop, list);
			foreach (Mod mod in list)
			{
				mod.workshopId = new ulong?(workshopItemId.m_PublishedFileId);
			}
			if (newMods != null)
			{
				newMods.AddRange(list);
			}
		}

		/// <summary>
		/// Checks all workshop-based mods on whether they support the current steam branch or not.
		/// If a given mod does not support the current steam branch, then it shows an error, but loading continues anyway.
		/// </summary>
		// Token: 0x06007BDE RID: 31710 RVA: 0x002A2694 File Offset: 0x002A0894
		private static async Task CheckSteamBranchSupport()
		{
			PublishedFileId_t[] workshopMods = (from m in ModManager._mods
				where m.workshopId != null
				select new PublishedFileId_t(m.workshopId.Value)).Distinct<PublishedFileId_t>().ToArray<PublishedFileId_t>();
			if (workshopMods.Length != 0)
			{
				UGCQueryHandle_t queryHandle = SteamUGC.CreateQueryUGCDetailsRequest(workshopMods, (uint)workshopMods.Length);
				try
				{
					using (SteamCallResult<SteamUGCQueryCompleted_t> callResult = new SteamCallResult<SteamUGCQueryCompleted_t>(SteamUGC.SendQueryUGCRequest(queryHandle), SteamInitializer.DisconnectToken))
					{
						SteamUGCQueryCompleted_t steamUGCQueryCompleted_t = await callResult.Task;
						SteamUGCQueryCompleted_t steamUGCQueryCompleted_t2 = steamUGCQueryCompleted_t;
						if (steamUGCQueryCompleted_t2.m_eResult != 1)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(83, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Steam UGC branch-support query failed with ");
							defaultInterpolatedStringHandler.AppendFormatted<EResult>(steamUGCQueryCompleted_t2.m_eResult);
							defaultInterpolatedStringHandler.AppendLiteral("; loading mods without the branch check.");
							Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							return;
						}
						uint num = 0U;
						while ((ulong)num < (ulong)((long)workshopMods.Length))
						{
							PublishedFileId_t publishedFileId_t = workshopMods[(int)num];
							uint numSupportedGameVersions = SteamUGC.GetNumSupportedGameVersions(steamUGCQueryCompleted_t2.m_handle, num);
							List<ValueTuple<string, string>> list = new List<ValueTuple<string, string>>();
							for (uint num2 = 0U; num2 < numSupportedGameVersions; num2 += 1U)
							{
								string text;
								string text2;
								if (SteamUGC.GetSupportedGameVersionData(steamUGCQueryCompleted_t2.m_handle, num, num2, ref text, ref text2, 999U))
								{
									list.Add(new ValueTuple<string, string>(text, text2));
								}
							}
							if (numSupportedGameVersions > 0U && list.Count == 0)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(117, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Steam reported ");
								defaultInterpolatedStringHandler.AppendFormatted<uint>(numSupportedGameVersions);
								defaultInterpolatedStringHandler.AppendLiteral(" supported game version range(s) for mod ");
								defaultInterpolatedStringHandler.AppendFormatted<ulong>(publishedFileId_t.m_PublishedFileId);
								defaultInterpolatedStringHandler.AppendLiteral(" but none could be read; loading it without the branch check.");
								Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							}
							PlatformBranch platformBranch = PlatformUtil.GetPlatformBranch();
							ValueTuple<bool, PlatformBranch?> valueTuple = ModManager.EvaluateBranchSupport(platformBranch, list);
							bool item = valueTuple.Item1;
							PlatformBranch? item2 = valueTuple.Item2;
							if (!item)
							{
								foreach (Mod mod in ModManager._mods)
								{
									ulong? workshopId = mod.workshopId;
									ulong publishedFileId = publishedFileId_t.m_PublishedFileId;
									if ((workshopId.GetValueOrDefault() == publishedFileId) & (workshopId != null))
									{
										LocString locString = new LocString("main_menu_ui", "MOD_ERROR.STEAM_BRANCH_UNSUPPORTED");
										string text3 = "id";
										ModManifest manifest = mod.manifest;
										locString.Add(text3, ((manifest != null) ? manifest.id : null) ?? "<null>");
										locString.Add("currentBranch", platformBranch.ToName());
										locString.Add("supportedBranch", ((item2 != null) ? item2.GetValueOrDefault().ToName() : null) ?? "<null>");
										if (mod.errors == null)
										{
											mod.errors = new List<LocString>();
										}
										mod.errors.Add(locString);
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(134, 3);
										defaultInterpolatedStringHandler.AppendLiteral("Tried to load mod with id ");
										ModManifest manifest2 = mod.manifest;
										defaultInterpolatedStringHandler.AppendFormatted((manifest2 != null) ? manifest2.id : null);
										defaultInterpolatedStringHandler.AppendLiteral(", but the current Steam branch ");
										defaultInterpolatedStringHandler.AppendFormatted(platformBranch.ToName());
										defaultInterpolatedStringHandler.AppendLiteral(" does not lie in the min/max steam branches the mod supports! Max supported: ");
										defaultInterpolatedStringHandler.AppendFormatted((item2 != null) ? item2.GetValueOrDefault().ToName() : null);
										Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
									}
								}
							}
							num += 1U;
						}
					}
					SteamCallResult<SteamUGCQueryCompleted_t> callResult = null;
				}
				catch (Exception ex)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(81, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Could not verify Steam branch support for mods. Loading them anyways. Exception: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				finally
				{
					SteamUGC.ReleaseQueryUGCRequest(queryHandle);
				}
			}
		}

		// Token: 0x06007BDF RID: 31711 RVA: 0x002A26D0 File Offset: 0x002A08D0
		private static void OnSteamWorkshopItemInstalled(ItemInstalled_t ev)
		{
			if ((ulong)ev.m_unAppID.m_AppId != 2868840UL)
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Detected new Steam Workshop item installation, id: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(ev.m_nPublishedFileId.m_PublishedFileId);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			List<Mod> list = new List<Mod>();
			ModManager.TryReadModFromSteam(ev.m_nPublishedFileId, list);
			foreach (Mod mod in list)
			{
				mod.state = ModLoadState.AddedAtRuntime;
				ModManager.InvokeOnModDetected(mod);
			}
		}

		// Token: 0x06007BE0 RID: 31712 RVA: 0x002A2780 File Offset: 0x002A0980
		private static void TryLoadMod(Mod mod)
		{
			if (mod.state != ModLoadState.None)
			{
				ModManager.InvokeOnModDetected(mod);
				return;
			}
			Assembly assembly = null;
			List<LocString> list = mod.errors ?? new List<LocString>();
			if (mod.manifest == null)
			{
				throw new InvalidOperationException("Tried to load mod before its manifest was loaded!");
			}
			SemanticVersion semanticVersion;
			if (mod.manifest.version == null)
			{
				Log.Warn("Mod " + mod.manifest.id + " does not declare a version", 2);
			}
			else if (!SemanticVersion.TryFromString(mod.manifest.version, out semanticVersion))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Mod ");
				defaultInterpolatedStringHandler.AppendFormatted(mod.manifest.id);
				defaultInterpolatedStringHandler.AppendLiteral(" declares version ");
				defaultInterpolatedStringHandler.AppendFormatted(mod.manifest.version);
				defaultInterpolatedStringHandler.AppendLiteral(" which is not a valid Semantic Version");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			else
			{
				mod.version = semanticVersion;
			}
			string modId = mod.manifest.id;
			bool flag = ModManager._mods.Any<Mod>(delegate(Mod m)
			{
				ModManifest manifest2 = m.manifest;
				return ((manifest2 != null) ? manifest2.id : null) == modId && m.state == ModLoadState.Loaded;
			});
			bool flag2 = false;
			bool flag3 = true;
			if (ModManager._gameVersion != null)
			{
				SemanticVersion semanticVersion2;
				if (mod.manifest.minGameVersion == null)
				{
					Log.Warn("Mod " + mod.manifest.id + " does not declare min game version. Assuming that it is supported.", 2);
				}
				else if (!SemanticVersion.TryFromString(mod.manifest.minGameVersion, out semanticVersion2))
				{
					flag2 = true;
				}
				else
				{
					flag3 = ModManager._gameVersion.CompareTo(semanticVersion2) >= 0;
				}
			}
			string text;
			if (ModManager.State != ModManagerState.None)
			{
				Log.Info("Skipping loading mod " + modId + ", can't load mods at runtime", 2);
				mod.state = ModLoadState.AddedAtRuntime;
			}
			else if (!ModManager.PlayerAgreedToModLoading)
			{
				Log.Info("Skipping loading mod " + modId + ", user has not yet seen the mods warning", 2);
				mod.state = ModLoadState.Disabled;
			}
			else if (flag)
			{
				LocString locString = new LocString("main_menu_ui", "MOD_ERROR.DUPLICATE_ID");
				locString.Add("id", modId);
				list.Add(locString);
				Log.Error("Tried to load mod with id " + modId + ", but a mod is already loaded with that name!", 2);
				mod.state = ModLoadState.Failed;
			}
			else if (ModManager._circularDependencies.TryGetValue(modId, out text))
			{
				LocString locString2 = new LocString("main_menu_ui", "MOD_ERROR.CIRCULAR_DEPENDENCY");
				locString2.Add("id", modId);
				locString2.Add("dependencyChain", text);
				list.Add(locString2);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to load mod with id ");
				defaultInterpolatedStringHandler.AppendFormatted(modId);
				defaultInterpolatedStringHandler.AppendLiteral(", but it is part of a circular dependency chain: ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				mod.state = ModLoadState.Failed;
			}
			else if (flag2)
			{
				LocString locString3 = new LocString("main_menu_ui", "MOD_ERROR.GAME_VERSION_INVALID");
				locString3.Add("id", modId);
				locString3.Add("minGameVersion", mod.manifest.minGameVersion ?? "<null>");
				list.Add(locString3);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(78, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Mod ");
				defaultInterpolatedStringHandler.AppendFormatted(mod.manifest.id);
				defaultInterpolatedStringHandler.AppendLiteral(" declares min game version ");
				defaultInterpolatedStringHandler.AppendFormatted(mod.manifest.minGameVersion);
				defaultInterpolatedStringHandler.AppendLiteral(" that can't be parsed! Assuming it is supported");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				mod.state = ModLoadState.Failed;
			}
			else if (!flag3)
			{
				LocString locString4 = new LocString("main_menu_ui", "MOD_ERROR.GAME_VERSION_UNSUPPORTED");
				locString4.Add("id", modId);
				locString4.Add("minGameVersion", mod.manifest.minGameVersion ?? "<null>");
				LocString locString5 = locString4;
				string text2 = "gameVersion";
				SemanticVersion gameVersion = ModManager._gameVersion;
				locString5.Add(text2, ((gameVersion != null) ? gameVersion.ToString() : null) ?? "<null>");
				list.Add(locString4);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(103, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to load mod with id ");
				defaultInterpolatedStringHandler.AppendFormatted(modId);
				defaultInterpolatedStringHandler.AppendLiteral(", but its declared min game version ");
				defaultInterpolatedStringHandler.AppendFormatted(mod.manifest.minGameVersion);
				defaultInterpolatedStringHandler.AppendLiteral(" is higher than the current game version ");
				defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion>(ModManager._gameVersion);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				mod.state = ModLoadState.Failed;
			}
			else
			{
				List<string> list2 = new List<string>();
				if (mod.manifest.dependencies != null)
				{
					using (List<ModDependency>.Enumerator enumerator = mod.manifest.dependencies.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ModDependency declaredDependency = enumerator.Current;
							Mod mod2 = ModManager._mods.FirstOrDefault<Mod>(delegate(Mod m)
							{
								ModManifest manifest3 = m.manifest;
								return ((manifest3 != null) ? manifest3.id : null) == declaredDependency.id;
							});
							if (mod2 == null || mod2.state != ModLoadState.Loaded)
							{
								list2.Add(declaredDependency.id);
							}
							else if (declaredDependency.minVersion != null)
							{
								SemanticVersion semanticVersion3;
								if (!SemanticVersion.TryFromString(declaredDependency.minVersion, out semanticVersion3))
								{
									LocString locString6 = new LocString("main_menu_ui", "MOD_ERROR.DEPENDENCY_MIN_VERSION_INVALID");
									locString6.Add("id", mod.manifest.id);
									locString6.Add("dependency", declaredDependency.id);
									locString6.Add("minVersion", declaredDependency.minVersion);
									list.Add(locString6);
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 3);
									defaultInterpolatedStringHandler.AppendLiteral("Mod ");
									defaultInterpolatedStringHandler.AppendFormatted(modId);
									defaultInterpolatedStringHandler.AppendLiteral(" which depends on ");
									defaultInterpolatedStringHandler.AppendFormatted(declaredDependency.id);
									defaultInterpolatedStringHandler.AppendLiteral(" with min version ");
									defaultInterpolatedStringHandler.AppendFormatted(declaredDependency.minVersion);
									defaultInterpolatedStringHandler.AppendLiteral(" which cannot be parsed");
									Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
									mod.state = ModLoadState.Failed;
								}
								else
								{
									ModManifest manifest = mod2.manifest;
									if (((manifest != null) ? manifest.version : null) == null)
									{
										LocString locString7 = new LocString("main_menu_ui", "MOD_ERROR.DEPENDENCY_VERSION_MISSING");
										locString7.Add("id", mod.manifest.id);
										locString7.Add("dependency", declaredDependency.id);
										locString7.Add("minVersion", declaredDependency.minVersion);
										list.Add(locString7);
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(88, 3);
										defaultInterpolatedStringHandler.AppendLiteral("Tried to load mod ");
										defaultInterpolatedStringHandler.AppendFormatted(modId);
										defaultInterpolatedStringHandler.AppendLiteral(" which depends on ");
										defaultInterpolatedStringHandler.AppendFormatted(declaredDependency.id);
										defaultInterpolatedStringHandler.AppendLiteral(" with min version ");
										defaultInterpolatedStringHandler.AppendFormatted(declaredDependency.minVersion);
										defaultInterpolatedStringHandler.AppendLiteral(", but the mod declares no version!");
										Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
										mod.state = ModLoadState.Failed;
									}
									else if (mod2.version == null)
									{
										LocString locString8 = new LocString("main_menu_ui", "MOD_ERROR.DEPENDENCY_VERSION_INVALID");
										locString8.Add("id", mod.manifest.id);
										locString8.Add("dependency", declaredDependency.id);
										locString8.Add("minVersion", declaredDependency.minVersion);
										locString8.Add("version", mod2.manifest.version);
										list.Add(locString8);
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(109, 4);
										defaultInterpolatedStringHandler.AppendLiteral("Tried to load mod ");
										defaultInterpolatedStringHandler.AppendFormatted(modId);
										defaultInterpolatedStringHandler.AppendLiteral(" which depends on ");
										defaultInterpolatedStringHandler.AppendFormatted(declaredDependency.id);
										defaultInterpolatedStringHandler.AppendLiteral(" with min version ");
										defaultInterpolatedStringHandler.AppendFormatted(declaredDependency.minVersion);
										defaultInterpolatedStringHandler.AppendLiteral(", but the mod declares version ");
										defaultInterpolatedStringHandler.AppendFormatted(mod2.manifest.version);
										defaultInterpolatedStringHandler.AppendLiteral(" which cannot be parsed!");
										Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
										mod.state = ModLoadState.Failed;
									}
									else if (mod2.version.CompareTo(semanticVersion3) < 0)
									{
										LocString locString9 = new LocString("main_menu_ui", "MOD_ERROR.DEPENDENCY_VERSION_UNSUPPORTED");
										locString9.Add("id", mod.manifest.id);
										locString9.Add("dependency", declaredDependency.id);
										locString9.Add("minVersion", declaredDependency.minVersion);
										locString9.Add("version", mod2.manifest.version);
										list.Add(locString9);
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 4);
										defaultInterpolatedStringHandler.AppendLiteral("Tried to load mod ");
										defaultInterpolatedStringHandler.AppendFormatted(modId);
										defaultInterpolatedStringHandler.AppendLiteral(" which depends on ");
										defaultInterpolatedStringHandler.AppendFormatted(declaredDependency.id);
										defaultInterpolatedStringHandler.AppendLiteral(" with min version ");
										defaultInterpolatedStringHandler.AppendFormatted(declaredDependency.minVersion);
										defaultInterpolatedStringHandler.AppendLiteral(" but you have ");
										defaultInterpolatedStringHandler.AppendFormatted<SemanticVersion>(mod2.version);
										defaultInterpolatedStringHandler.AppendLiteral("!");
										Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
										mod.state = ModLoadState.Failed;
									}
								}
							}
						}
					}
					if (list2.Count > 0)
					{
						string text3 = string.Join(",", list2);
						LocString locString10 = new LocString("main_menu_ui", "MOD_ERROR.MISSING_DEPENDENCY");
						locString10.Add("id", mod.manifest.id);
						locString10.Add("missingCount", list2.Count);
						locString10.Add("missingDependencies", text3);
						list.Add(locString10);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Tried to load mod ");
						defaultInterpolatedStringHandler.AppendFormatted(modId);
						defaultInterpolatedStringHandler.AppendLiteral(", but it depends on mods which have not been loaded: ");
						defaultInterpolatedStringHandler.AppendFormatted(text3);
						defaultInterpolatedStringHandler.AppendLiteral("!");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						mod.state = ModLoadState.Failed;
					}
				}
			}
			if (mod.state != ModLoadState.None)
			{
				mod.errors = ((list.Count == 0) ? null : list);
				ModManager.InvokeOnModDetected(mod);
				return;
			}
			try
			{
				bool flag4 = false;
				string text4 = Path.Combine(mod.path, modId + ".dll");
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (mod.manifest.hasDll && TestMode.IsOff)
				{
					if (ModManager._fileIo != null && ModManager._fileIo.FileExists(text4))
					{
						Log.Info("Loading assembly DLL " + text4, 2);
						AssemblyLoadContext loadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
						if (loadContext != null)
						{
							assembly = loadContext.LoadFromAssemblyPath(text4);
							flag4 = true;
						}
					}
					else
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(99, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Mod manifest for mod ");
						defaultInterpolatedStringHandler.AppendFormatted(mod.manifest.id);
						defaultInterpolatedStringHandler.AppendLiteral(" declares that it should load an assembly, but no assembly at path ");
						defaultInterpolatedStringHandler.AppendFormatted(text4);
						defaultInterpolatedStringHandler.AppendLiteral(" was found!");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
				else if (TestMode.IsOn && ModManager.TestInitializers.ContainsKey(modId))
				{
					flag4 = true;
				}
				string text5 = Path.Combine(mod.path, modId + ".pck");
				if (mod.manifest.hasPck && TestMode.IsOff)
				{
					if (ModManager._fileIo != null && ModManager._fileIo.FileExists(text5))
					{
						Log.Info("Loading Godot PCK " + text5, 2);
						if (!ProjectSettings.LoadResourcePack(text5, true, 0))
						{
							throw new InvalidOperationException("Godot errored while loading PCK file " + modId + "!");
						}
						flag4 = true;
					}
					else
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(88, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Mod manifest for mod ");
						defaultInterpolatedStringHandler.AppendFormatted(mod.manifest.id);
						defaultInterpolatedStringHandler.AppendLiteral(" declares that it should load a PCK, but no PCK at path ");
						defaultInterpolatedStringHandler.AppendFormatted(text5);
						defaultInterpolatedStringHandler.AppendLiteral(" was found!");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
				if (!flag4)
				{
					Log.Warn("Neither a DLL nor a PCK was loaded for mod " + mod.manifest.id + ", something seems wrong!", 2);
				}
				bool? flag5 = null;
				if (TestMode.IsOff && assembly != null)
				{
					flag5 = new bool?(true);
					List<Type> list3 = (from t in assembly.GetTypes()
						where t.GetCustomAttribute<ModInitializerAttribute>() != null
						select t).ToList<Type>();
					if (list3.Count > 0)
					{
						using (List<Type>.Enumerator enumerator2 = list3.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Type type = enumerator2.Current;
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Calling initializer method of type ");
								defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
								defaultInterpolatedStringHandler.AppendLiteral(" for ");
								defaultInterpolatedStringHandler.AppendFormatted<Assembly>(assembly);
								Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
								bool flag6 = ModManager.CallModInitializer(type);
								flag5 = new bool?(flag5.Value && flag6);
							}
							goto IL_0E46;
						}
					}
					try
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 1);
						defaultInterpolatedStringHandler.AppendLiteral("No ModInitializerAttribute detected. Calling Harmony.PatchAll for ");
						defaultInterpolatedStringHandler.AppendFormatted<Assembly>(assembly);
						Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						Harmony harmony = new Harmony((mod.manifest.author ?? "unknown") + "." + modId);
						harmony.PatchAll(assembly);
						goto IL_0E46;
					}
					catch (Exception ex)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Exception caught while trying to run PatchAll on assembly ");
						defaultInterpolatedStringHandler.AppendFormatted<Assembly>(assembly);
						defaultInterpolatedStringHandler.AppendLiteral(":\n");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						flag5 = new bool?(false);
						goto IL_0E46;
					}
				}
				if (TestMode.IsOn && mod.manifest.hasDll)
				{
					Action action;
					flag5 = new bool?(ModManager.TestInitializers.TryGetValue(mod.manifest.id, out action));
					if (flag5.Value)
					{
						action();
					}
				}
				IL_0E46:
				bool? flag7 = flag5;
				bool flag8 = false;
				if ((flag7.GetValueOrDefault() == flag8) & (flag7 != null))
				{
					LocString locString11 = new LocString("main_menu_ui", "MOD_ERROR.ASSEMBLY_LOAD");
					locString11.Add("id", mod.manifest.id);
					list.Add(locString11);
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Finished mod initialization for '");
				defaultInterpolatedStringHandler.AppendFormatted(mod.manifest.name);
				defaultInterpolatedStringHandler.AppendLiteral("' (");
				defaultInterpolatedStringHandler.AppendFormatted(modId);
				defaultInterpolatedStringHandler.AppendLiteral(").");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				if (assembly != null)
				{
					mod.assemblies.Add(assembly);
				}
				mod.state = ModLoadState.Loaded;
				mod.errors = ((list.Count == 0) ? null : list);
				ModManager.InvokeOnModDetected(mod);
			}
			catch (Exception ex2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception thrown while loading mod ");
				defaultInterpolatedStringHandler.AppendFormatted(modId);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex2);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				LocString locString12 = new LocString("main_menu_ui", "MOD_ERROR.EXCEPTION");
				locString12.Add("exceptionType", ex2.GetType().ToString());
				locString12.Add("id", mod.manifest.id);
				list.Add(locString12);
				if (assembly != null)
				{
					mod.assemblies.Add(assembly);
				}
				mod.state = ModLoadState.Failed;
				mod.errors = ((list.Count == 0) ? null : list);
				ModManager.InvokeOnModDetected(mod);
			}
		}

		// Token: 0x06007BE1 RID: 31713 RVA: 0x002A37DC File Offset: 0x002A19DC
		private static void InvokeOnModDetected(Mod mod)
		{
			Action<Mod> onModDetected = ModManager.OnModDetected;
			foreach (Delegate @delegate in ((onModDetected != null) ? onModDetected.GetInvocationList() : null) ?? Array.Empty<Delegate>())
			{
				try
				{
					@delegate.DynamicInvoke(new object[] { mod });
				}
				catch (Exception ex)
				{
					object target = @delegate.Target;
					if (((target != null) ? target.GetType().Assembly : null) == Assembly.GetExecutingAssembly())
					{
						throw;
					}
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Exception emitted from ");
					defaultInterpolatedStringHandler.AppendFormatted("OnModDetected");
					defaultInterpolatedStringHandler.AppendLiteral(" delegate ");
					defaultInterpolatedStringHandler.AppendFormatted<Delegate>(@delegate);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
			}
		}

		/// <summary>
		/// Calls the mod initializer on a type that has the ModInitializerAttribute on it.
		/// </summary>
		// Token: 0x06007BE2 RID: 31714 RVA: 0x002A38C0 File Offset: 0x002A1AC0
		private static bool CallModInitializer(Type initializerType)
		{
			ModInitializerAttribute customAttribute = initializerType.GetCustomAttribute<ModInitializerAttribute>();
			MethodInfo methodInfo = initializerType.GetMethod(customAttribute.initializerMethod, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (methodInfo == null)
			{
				methodInfo = initializerType.GetMethod(customAttribute.initializerMethod, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (methodInfo != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to call mod initializer ");
					defaultInterpolatedStringHandler.AppendFormatted(initializerType.Name);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					defaultInterpolatedStringHandler.AppendFormatted(customAttribute.initializerMethod);
					defaultInterpolatedStringHandler.AppendLiteral(" but it's not static! Declare it to be static");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(114, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Found mod initializer class of type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(initializerType);
					defaultInterpolatedStringHandler.AppendLiteral(", but it does not contain the method ");
					defaultInterpolatedStringHandler.AppendFormatted(customAttribute.initializerMethod);
					defaultInterpolatedStringHandler.AppendLiteral(" declared in the ModInitializerAttribute!");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				return false;
			}
			try
			{
				methodInfo.Invoke(null, null);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception thrown when calling mod initializer of type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(initializerType);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns the filenames of all the loc tables available in loaded mods for the given language and filename.
		/// For example, if "eng" and "cards.json" are provided, this returns all mods that supply a cards.json in english.
		/// </summary>
		// Token: 0x06007BE3 RID: 31715 RVA: 0x002A3A18 File Offset: 0x002A1C18
		public static IEnumerable<string> GetModdedLocTables(string language, string file)
		{
			ModManager.<GetModdedLocTables>d__43 <GetModdedLocTables>d__ = new ModManager.<GetModdedLocTables>d__43(-2);
			<GetModdedLocTables>d__.<>3__language = language;
			<GetModdedLocTables>d__.<>3__file = file;
			return <GetModdedLocTables>d__;
		}

		/// <summary>
		/// Returns a list of the names of all loaded mods that affect gameplay.
		/// For multiplayer - used to compare mods between different players.
		/// </summary>
		// Token: 0x06007BE4 RID: 31716 RVA: 0x002A3A30 File Offset: 0x002A1C30
		[return: Nullable(new byte[] { 2, 1 })]
		public static List<string> GetGameplayRelevantModNameList()
		{
			if (!ModManager.IsRunningModded())
			{
				return null;
			}
			return ModManager.GetLoadedMods().Where<Mod>(delegate(Mod m)
			{
				ModManifest manifest = m.manifest;
				return manifest == null || manifest.affectsGameplay;
			}).Select<Mod, string>(delegate(Mod m)
			{
				ModManifest manifest2 = m.manifest;
				string text = ((manifest2 != null) ? manifest2.id : null);
				string text2 = "-";
				ModManifest manifest3 = m.manifest;
				return text + text2 + ((manifest3 != null) ? manifest3.version : null);
			})
				.ToList<string>();
		}

		/// <summary>
		/// Returns a list of the names of all loaded mods that do not affect gameplay.
		/// </summary>
		// Token: 0x06007BE5 RID: 31717 RVA: 0x002A3A98 File Offset: 0x002A1C98
		[return: Nullable(new byte[] { 2, 1 })]
		public static List<string> GetNonGameplayRelevantModNameList()
		{
			if (!ModManager.IsRunningModded())
			{
				return null;
			}
			return ModManager.GetLoadedMods().Where<Mod>(delegate(Mod m)
			{
				ModManifest manifest = m.manifest;
				return manifest != null && !manifest.affectsGameplay;
			}).Select<Mod, string>(delegate(Mod m)
			{
				ModManifest manifest2 = m.manifest;
				string text = ((manifest2 != null) ? manifest2.id : null);
				string text2 = "-";
				ModManifest manifest3 = m.manifest;
				return text + text2 + ((manifest3 != null) ? manifest3.version : null);
			})
				.ToList<string>();
		}

		/// <summary>
		/// Resolves assemblies loaded by mods that may have a different version.
		/// When writing a mod DLL, the implementer may target a STS2 DLL that does not match the current version. When the
		/// dotnet runtime attempts to load the DLL, it tries to strictly match the version. Usually, we don't really need
		/// it to - a lot of our APIs don't change that often. Attaching this method to the AssemblyResolve event allows us
		/// to force dotnet to resolve the assembly to the correct one.
		/// </summary>
		// Token: 0x06007BE6 RID: 31718 RVA: 0x002A3B00 File Offset: 0x002A1D00
		private static Assembly HandleAssemblyResolveFailure([Nullable(2)] object source, ResolveEventArgs ev)
		{
			if (ev.Name.StartsWith("sts2,"))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(83, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to resolve assembly '");
				defaultInterpolatedStringHandler.AppendFormatted(ev.Name);
				defaultInterpolatedStringHandler.AppendLiteral("' but it looks like the STS2 assembly. Resolving using ");
				defaultInterpolatedStringHandler.AppendFormatted<Assembly>(Assembly.GetExecutingAssembly());
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return Assembly.GetExecutingAssembly();
			}
			if (ev.Name.StartsWith("0Harmony,"))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(86, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to resolve assembly '");
				defaultInterpolatedStringHandler.AppendFormatted(ev.Name);
				defaultInterpolatedStringHandler.AppendLiteral("' but it looks like the Harmony assembly. Resolving using ");
				defaultInterpolatedStringHandler.AppendFormatted<Assembly>(typeof(Harmony).Assembly);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return typeof(Harmony).Assembly;
			}
			return null;
		}

		/// <summary>
		/// Called by the metrics uploader when metrics would be uploaded, but are not because there are mods present.
		/// </summary>
		// Token: 0x06007BE7 RID: 31719 RVA: 0x002A3BE2 File Offset: 0x002A1DE2
		public static void CallMetricsHooks(SerializableRun run, bool isVictory, ulong localPlayerId)
		{
			ModManager.MetricsUploadHook onMetricsUpload = ModManager.OnMetricsUpload;
			if (onMetricsUpload == null)
			{
				return;
			}
			onMetricsUpload(run, isVictory, localPlayerId);
		}

		/// <summary>
		/// Associates an assembly with a mod.
		/// If your mod creates types within a dynamic assembly, it must associate the assembly to the mod so that the types
		/// within the mod are detected.
		/// Each assembly must be associated with exactly one mod.
		/// </summary>
		// Token: 0x06007BE8 RID: 31720 RVA: 0x002A3BF8 File Offset: 0x002A1DF8
		public static void AssociateAssemblyWithMod(string modId, Assembly assembly)
		{
			Mod mod = ModManager._mods.FirstOrDefault<Mod>(delegate(Mod m)
			{
				ModLoadState state = m.state;
				bool flag = state <= ModLoadState.Loaded;
				if (flag)
				{
					ModManifest manifest2 = m.manifest;
					return ((manifest2 != null) ? manifest2.id : null) == modId;
				}
				return false;
			});
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (mod != null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Associated assembly ");
				defaultInterpolatedStringHandler.AppendFormatted<Assembly>(assembly);
				defaultInterpolatedStringHandler.AppendLiteral(" with mod ");
				defaultInterpolatedStringHandler.AppendFormatted(modId);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				mod.assemblies.Add(assembly);
				if (AssemblyInfo.ModMap != null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(123, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Assembly ");
					defaultInterpolatedStringHandler.AppendFormatted<Assembly>(assembly);
					defaultInterpolatedStringHandler.AppendLiteral(" was associated with mod ");
					ModManifest manifest = mod.manifest;
					defaultInterpolatedStringHandler.AppendFormatted((manifest != null) ? manifest.id : null);
					defaultInterpolatedStringHandler.AppendLiteral(" after ");
					defaultInterpolatedStringHandler.AppendFormatted("AssemblyInfo");
					defaultInterpolatedStringHandler.AppendLiteral(" has already been initialized. The types will not be included in multiplayer maps.");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					AssemblyInfo.ModMap[assembly] = mod;
				}
				return;
			}
			mod = ModManager._mods.FirstOrDefault<Mod>(delegate(Mod m)
			{
				ModManifest manifest3 = m.manifest;
				return ((manifest3 != null) ? manifest3.id : null) == modId;
			});
			if (mod != null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to associate assembly ");
				defaultInterpolatedStringHandler.AppendFormatted<Assembly>(assembly);
				defaultInterpolatedStringHandler.AppendLiteral(" with mod ");
				defaultInterpolatedStringHandler.AppendFormatted(modId);
				defaultInterpolatedStringHandler.AppendLiteral(" but its state is ");
				defaultInterpolatedStringHandler.AppendFormatted<ModLoadState>(mod.state);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Tried to associate assembly ");
			defaultInterpolatedStringHandler.AppendFormatted<Assembly>(assembly);
			defaultInterpolatedStringHandler.AppendLiteral(" with mod ");
			defaultInterpolatedStringHandler.AppendFormatted(modId);
			defaultInterpolatedStringHandler.AppendLiteral(" but we couldn't find any such mod");
			Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x06007BE9 RID: 31721 RVA: 0x002A3DD2 File Offset: 0x002A1FD2
		public static bool IsRunningModded()
		{
			return ModManager._mods.Any<Mod>(delegate(Mod m)
			{
				ModLoadState state = m.state;
				return state - ModLoadState.Loaded <= 1;
			});
		}

		// Token: 0x06007BEA RID: 31722 RVA: 0x002A3E00 File Offset: 0x002A2000
		public static bool HasHarmonyPatches()
		{
			try
			{
				bool flag = ModManager._hasHarmonyPatches.GetValueOrDefault();
				if (ModManager._hasHarmonyPatches == null)
				{
					flag = Harmony.GetAllPatchedMethods().Any<MethodBase>();
					ModManager._hasHarmonyPatches = new bool?(flag);
				}
			}
			catch
			{
				ModManager._hasHarmonyPatches = new bool?(true);
			}
			return ModManager._hasHarmonyPatches.Value;
		}

		// Token: 0x06007BEB RID: 31723 RVA: 0x002A3E64 File Offset: 0x002A2064
		public static IEnumerable<Mod> GetLoadedMods()
		{
			return ModManager._mods.Where<Mod>((Mod m) => m.state == ModLoadState.Loaded);
		}

		// Token: 0x06007BEC RID: 31724 RVA: 0x002A3E8F File Offset: 0x002A208F
		public static void Dispose()
		{
			Callback<ItemInstalled_t> steamItemInstalledCallback = ModManager._steamItemInstalledCallback;
			if (steamItemInstalledCallback == null)
			{
				return;
			}
			steamItemInstalledCallback.Dispose();
		}

		// Token: 0x06007BED RID: 31725 RVA: 0x002A3EA0 File Offset: 0x002A20A0
		public static void CopyUnmoddedSaveFilesIfNeeded()
		{
			string accountScopedBasePath = UserDataPathProvider.GetAccountScopedBasePath(null, null, null);
			string accountScopedBasePath2 = UserDataPathProvider.GetAccountScopedBasePath("modded/", null, null);
			string profileSavePath = ProfileSaveManager.GetProfileSavePath(new bool?(false));
			string profileSavePath2 = ProfileSaveManager.GetProfileSavePath(new bool?(true));
			if (DirAccess.DirExistsAbsolute(accountScopedBasePath2))
			{
				if (!FileAccess.FileExists(StringExtensions.PathJoin(accountScopedBasePath, profileSavePath2)))
				{
					Log.Info("Modded saves exist, but profile.save wasn't present. Copying profile.save from unmodded to modded", 2);
					ModManager.Copy(accountScopedBasePath, profileSavePath, profileSavePath2);
				}
				Log.Info("Modded saves exist. Skipping first-time save copy", 2);
				return;
			}
			DirAccess.MakeDirRecursiveAbsolute(accountScopedBasePath2);
			if (!FileAccess.FileExists(StringExtensions.PathJoin(accountScopedBasePath, profileSavePath)))
			{
				Log.Info("Modded saves don't exist, but neither do unmodded saves. Skipping first-time copy", 2);
				return;
			}
			Log.Info("Copying all unmodded saves to the modded save location. Base path: " + accountScopedBasePath, 2);
			ModManager.Copy(accountScopedBasePath, profileSavePath, profileSavePath2);
			for (int i = 1; i <= 3; i++)
			{
				string progressPathForProfile = ProgressSaveManager.GetProgressPathForProfile(i, new bool?(false));
				string progressPathForProfile2 = ProgressSaveManager.GetProgressPathForProfile(i, new bool?(true));
				ModManager.Copy(accountScopedBasePath, progressPathForProfile, progressPathForProfile2);
				string runSavePath = RunSaveManager.GetRunSavePath(i, "current_run.save", new bool?(false));
				string runSavePath2 = RunSaveManager.GetRunSavePath(i, "current_run.save", new bool?(true));
				ModManager.Copy(accountScopedBasePath, runSavePath, runSavePath2);
				string runSavePath3 = RunSaveManager.GetRunSavePath(i, "current_run_mp.save", new bool?(false));
				string runSavePath4 = RunSaveManager.GetRunSavePath(i, "current_run_mp.save", new bool?(true));
				ModManager.Copy(accountScopedBasePath, runSavePath3, runSavePath4);
				string prefsPath = PrefsSaveManager.GetPrefsPath(i, new bool?(false));
				string prefsPath2 = PrefsSaveManager.GetPrefsPath(i, new bool?(true));
				ModManager.Copy(accountScopedBasePath, prefsPath, prefsPath2);
				string historyPath = RunHistorySaveManager.GetHistoryPath(i, new bool?(false));
				DirAccess dirAccess = DirAccess.Open(StringExtensions.PathJoin(accountScopedBasePath, historyPath));
				if (dirAccess != null)
				{
					string historyPath2 = RunHistorySaveManager.GetHistoryPath(i, new bool?(true));
					DirAccess.MakeDirRecursiveAbsolute(StringExtensions.PathJoin(accountScopedBasePath, historyPath2));
					foreach (string text in dirAccess.GetFiles())
					{
						string text2 = StringExtensions.PathJoin(historyPath, text);
						string text3 = StringExtensions.PathJoin(historyPath2, text);
						ModManager.Copy(accountScopedBasePath, text2, text3);
					}
				}
			}
			ModManager.UnmoddedSavesWereCopied = true;
		}

		// Token: 0x06007BEE RID: 31726 RVA: 0x002A40C0 File Offset: 0x002A22C0
		public static void Copy(string baseDir, string sourceFile, string targetFile)
		{
			string text = StringExtensions.PathJoin(baseDir, sourceFile);
			if (!FileAccess.FileExists(text))
			{
				return;
			}
			string text2 = StringExtensions.PathJoin(baseDir, targetFile);
			Log.Info("Copying " + sourceFile + " -> " + targetFile, 2);
			DirAccess.MakeDirRecursiveAbsolute(StringExtensions.GetBaseDir(text2));
			Error error = DirAccess.CopyAbsolute(text, text2, -1);
			if (error != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error: ");
				defaultInterpolatedStringHandler.AppendFormatted<Error>(error);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		// Token: 0x0400292B RID: 10539
		private static bool _allowInitForTests;

		// Token: 0x0400292C RID: 10540
		private static List<Mod> _mods = new List<Mod>();

		// Token: 0x0400292E RID: 10542
		[Nullable(2)]
		private static Callback<ItemInstalled_t> _steamItemInstalledCallback;

		// Token: 0x0400292F RID: 10543
		[Nullable(2)]
		private static ModSettings _settings;

		// Token: 0x04002930 RID: 10544
		[Nullable(2)]
		private static IModManagerFileIo _fileIo;

		// Token: 0x04002931 RID: 10545
		[Nullable(2)]
		private static SemanticVersion _gameVersion;

		// Token: 0x04002932 RID: 10546
		private static readonly Dictionary<string, string> _circularDependencies = new Dictionary<string, string>();

		// Token: 0x04002937 RID: 10551
		private static bool? _hasHarmonyPatches;

		// Token: 0x020022CA RID: 8906
		// (Invoke) Token: 0x0600BF4F RID: 48975
		[NullableContext(0)]
		public delegate void MetricsUploadHook(SerializableRun run, bool isVictory, ulong localPlayerId);

		// Token: 0x020022CB RID: 8907
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04009250 RID: 37456
			[Nullable(0)]
			public static ResolveEventHandler <0>__HandleAssemblyResolveFailure;

			// Token: 0x04009251 RID: 37457
			[Nullable(0)]
			public static Callback<ItemInstalled_t>.DispatchDelegate <1>__OnSteamWorkshopItemInstalled;
		}
	}
}
