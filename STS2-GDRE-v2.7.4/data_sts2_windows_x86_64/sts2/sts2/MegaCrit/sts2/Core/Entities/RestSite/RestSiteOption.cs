using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	/// <summary>
	/// Base config class for rest site button options.
	/// </summary>
	// Token: 0x02000C6D RID: 3181
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class RestSiteOption
	{
		// Token: 0x170023A2 RID: 9122
		// (get) Token: 0x06008283 RID: 33411
		public abstract string OptionId { get; }

		// Token: 0x170023A3 RID: 9123
		// (get) Token: 0x06008284 RID: 33412 RVA: 0x002C1968 File Offset: 0x002BFB68
		protected Player Owner { get; }

		// Token: 0x170023A4 RID: 9124
		// (get) Token: 0x06008285 RID: 33413 RVA: 0x002C1970 File Offset: 0x002BFB70
		public LocString Title
		{
			get
			{
				return new LocString("rest_site_ui", "OPTION_" + this.OptionId + ".name");
			}
		}

		// Token: 0x170023A5 RID: 9125
		// (get) Token: 0x06008286 RID: 33414 RVA: 0x002C1991 File Offset: 0x002BFB91
		public virtual LocString Description
		{
			get
			{
				return new LocString("rest_site_ui", "OPTION_" + this.OptionId + ".description");
			}
		}

		// Token: 0x170023A6 RID: 9126
		// (get) Token: 0x06008287 RID: 33415 RVA: 0x002C19B2 File Offset: 0x002BFBB2
		private string IconPath
		{
			get
			{
				return ImageHelper.GetImagePath("ui/rest_site/option_" + this.OptionId.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x170023A7 RID: 9127
		// (get) Token: 0x06008288 RID: 33416 RVA: 0x002C19D3 File Offset: 0x002BFBD3
		public Texture2D Icon
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.IconPath);
			}
		}

		// Token: 0x170023A8 RID: 9128
		// (get) Token: 0x06008289 RID: 33417 RVA: 0x002C19E5 File Offset: 0x002BFBE5
		public virtual IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(this.IconPath);
			}
		}

		/// <summary>
		/// Whether this option is usable or not. You can still hover it for info.
		/// </summary>
		// Token: 0x170023A9 RID: 9129
		// (get) Token: 0x0600828A RID: 33418 RVA: 0x002C19F2 File Offset: 0x002BFBF2
		public virtual bool IsEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600828B RID: 33419 RVA: 0x002C19F5 File Offset: 0x002BFBF5
		protected RestSiteOption(Player owner)
		{
			this.Owner = owner;
		}

		/// <summary>
		/// Generates a list of rest site options to use for the next rest site.
		/// This list will include extra options added by models like the Shovel relic if the player has them.
		/// Calling this may increment RNG counters and make other run state changes.
		/// </summary>
		/// <returns>List of rest site options.</returns>
		// Token: 0x0600828C RID: 33420 RVA: 0x002C1A04 File Offset: 0x002BFC04
		public unsafe static List<RestSiteOption> Generate(Player player)
		{
			if (TestMode.IsOn && RestSiteOption.generateForTests != null)
			{
				return RestSiteOption.generateForTests(player);
			}
			int num = 2;
			List<RestSiteOption> list = new List<RestSiteOption>(num);
			CollectionsMarshal.SetCount<RestSiteOption>(list, num);
			Span<RestSiteOption> span = CollectionsMarshal.AsSpan<RestSiteOption>(list);
			int num2 = 0;
			*span[num2] = new HealRestSiteOption(player);
			num2++;
			*span[num2] = new SmithRestSiteOption(player);
			List<RestSiteOption> list2 = list;
			if (player.RunState.Players.Count > 1)
			{
				list2.Add(new MendRestSiteOption(player));
			}
			Hook.ModifyRestSiteOptions(player.RunState, player, list2);
			return list2;
		}

		/// <summary>
		/// Logic to run when this option is selected.
		/// </summary>
		/// <returns>
		/// Whether or not the option was "successful". Usually true, but false in certain cases (like if Smith is chosen
		/// and no upgradable cards are available).
		/// </returns>
		// Token: 0x0600828D RID: 33421
		public abstract Task<bool> OnSelect();

		/// <summary>
		/// Runs only for the owning player after the option is selected.
		/// </summary>
		// Token: 0x0600828E RID: 33422 RVA: 0x002C1A94 File Offset: 0x002BFC94
		public virtual Task DoLocalPostSelectVfx(CancellationToken ct = default(CancellationToken))
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Runs only for non-owning players after the option is selected.
		/// </summary>
		// Token: 0x0600828F RID: 33423 RVA: 0x002C1A9B File Offset: 0x002BFC9B
		public virtual Task DoRemotePostSelectVfx()
		{
			return Task.CompletedTask;
		}

		// Token: 0x06008290 RID: 33424 RVA: 0x002C1AA4 File Offset: 0x002BFCA4
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			RestSiteOption restSiteOption = obj as RestSiteOption;
			return restSiteOption != null && this.OptionId == restSiteOption.OptionId && this.Owner == restSiteOption.Owner;
		}

		// Token: 0x06008291 RID: 33425 RVA: 0x002C1AE0 File Offset: 0x002BFCE0
		public override int GetHashCode()
		{
			return new ValueTuple<string, Player>(this.OptionId, this.Owner).GetHashCode();
		}

		// Token: 0x06008292 RID: 33426 RVA: 0x002C1B0C File Offset: 0x002BFD0C
		[NullableContext(2)]
		public static bool operator ==(RestSiteOption left, RestSiteOption right)
		{
			if (left == right)
			{
				return true;
			}
			if (left == null)
			{
				return left == null && right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x06008293 RID: 33427 RVA: 0x002C1B28 File Offset: 0x002BFD28
		[NullableContext(2)]
		public static bool operator !=(RestSiteOption left, RestSiteOption right)
		{
			return !(left == right);
		}

		// Token: 0x04002BE9 RID: 11241
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		public static Func<Player, List<RestSiteOption>> generateForTests;
	}
}
