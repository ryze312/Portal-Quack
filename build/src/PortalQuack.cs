using HarmonyLib;
using System.Reflection;

// The title of your mod, as displayed in menus
[assembly: AssemblyTitle("Portal Quack")]

// The author of the mod
[assembly: AssemblyCompany("Duck Team")]

// The description of the mod
[assembly: AssemblyDescription("Cake is Quack")]

// The mod's version
[assembly: AssemblyVersion("1.0.0.0")]

namespace DuckGame.PortalQuack
{
	public class PortalQuack : Mod
    {

		Harmony harmony = new Harmony("team.DuckTeam.PortalQuack");

		// The mod's priority; this property controls the load order of the mod.
		public override Priority priority
		{
			get { return Priority.Highest; }
		}

		// This function is run before all mods are finished loading.


		protected override void OnPreInitialize()
		{
			harmony.PatchAll();
			base.OnPreInitialize();
		}

		// This function is run after all mods are loaded.
		protected override void OnPostInitialize()
		{
			base.OnPostInitialize();
		}
	}
}
