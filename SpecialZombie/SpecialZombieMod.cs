using dc.en;
using dc.en.mob;
using dc.pr;
using Hashlink.Proxy.Clousre;
using HaxeProxy.Runtime;
using ModCore.Events.Interfaces.Game;
using ModCore.Mods;
using ModCore.Modules;
using ModCore.Storage;

namespace SpecialZombie
{
    public class SpecialZombieMod(ModInfo info) : ModBase(info)
    {
        public readonly SaveData<SData> save = new("SpecialZombieMod");
        public class SData
        {
            public int couting;
        }
        public override void Initialize()
        {
            Logger.Information("Hello, World!");

            Hook__Mob.create += Hook__Mob_create;
            Hook_Mob.onDie += Hook_Mob_onDie;
        }

        private Mob Hook__Mob_create(Hook__Mob.orig_create orig, dc.String k, Level level, int cx, int cy, int dmgTier, Ref<int> lifeTier)
        {
            if(k.ToString().Equals("rat", StringComparison.CurrentCultureIgnoreCase))
            {
                var result = new SpecialZombieEntity(level, cx, cy, dmgTier, lifeTier.value);
                result.init();
                return result;
            }
            return orig(k, level, cx, cy, dmgTier, lifeTier);
        }

        private void Hook_Mob_onDie(Hook_Mob.orig_onDie orig, Mob self)
        {
            Logger.Information("Mob Die: {x}", self);
            orig(self);
        }


    }
}
