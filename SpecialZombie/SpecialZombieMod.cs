using dc.en;
using dc.en.mob;
using dc.pr;
using Hashlink.Proxy.Clousre;
using HaxeProxy.Runtime;
using ModCore.Events.Interfaces.Game;
using ModCore.Mods;
using ModCore.Modules;

namespace SpecialZombie
{
    public class SpecialZombieMod(ModInfo info) : ModBase(info)
    {
        public override void Initialize()
        {
            Logger.Information("Hello, World!");

            Hook_Mob.init += Hook_Mob_init;
        }

        private void Hook_Mob_init(Hook_Mob.orig_init orig, Mob self)
        {
            orig(self);
            if(self is Zombie zombie)
            {
                Logger.Information("Create zombie: {x}", self);
                var sze = new SpecialZombieEntity(zombie, self._level, self.cx, self.cy);
            }
            
        }

    }
}
