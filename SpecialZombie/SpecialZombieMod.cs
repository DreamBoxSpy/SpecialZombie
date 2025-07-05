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
        public readonly static SaveData<SData> save = new("SpecialZombieMod");
        public class SData
        {
            public int couting;
        }
        public override void Initialize()
        {
            Logger.Information("Hello, World!");

            Hook_Mob.onDie += Hook_Mob_onDie;
            Hook_Mob.init += Hook_Mob_init;
        }

        private void Hook_Mob_onDie(Hook_Mob.orig_onDie orig, Mob self)
        {
            Logger.Information("Mob Die: {x}", self);
            orig(self);
        }

        private void Hook_Mob_init(Hook_Mob.orig_init orig, Mob self)
        {
            orig(self);
            if(self is Zombie zombie)
            {
                Logger.Information("Create zombie: {x} {a}", self, save.Value.couting++);
                var sze = new SpecialZombieEntity(zombie, self._level, self.cx, self.cy);
            }
            
        }

    }
}
