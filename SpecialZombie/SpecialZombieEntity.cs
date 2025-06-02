using dc;
using dc.en.mob;
using dc.pr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialZombie
{
    internal class SpecialZombieEntity : Entity
    {
        public Zombie zombie;
        public int maxLife;
        public int state = 0;
        public SpecialZombieEntity(Zombie target, Level lvl, int x, int y) : base(lvl, x, y)
        {
            init();
            zombie = target;
        }

        public override void preUpdate()
        {
            var life = zombie.life;
            if(state > 3 || 
                life == 0)
            {
                return;
            }
            if(state == 0 && zombie.elite)
            {
                return;
            }
            if (life > maxLife)
            {
                maxLife = life;
            }
            if(life < maxLife / 2)
            {
                //Angry
                zombie.initLife(maxLife * 1.5f, maxLife * 2);
            }
            else
            {
                return;
            }
            if (state == 0)
            {
                zombie.sprScaleX *= 1.25f;
                zombie.sprScaleY *= 1.25f;
                zombie.setOutlineColor(Utils.GetColor(80, 20, 20));
                _level.fx.dirtExplosion(zombie.cx, zombie.cy, 4, 0, Utils.GetColor(80, 20, 20), default);
            }
            else if(state == 1)
            {
                var fz = new Defender(_level, zombie.cx, zombie.cy, 1, 1);
                fz.init();
                fz.initLife(maxLife / 2, maxLife);
            }
            else if (state == 2)
            {
                zombie.setElite(true);
                _level.fx.boneSmoke(zombie.cx, zombie.cy, Utils.GetColor(10, 10, 100));
            }
            else if (state == 3)
            {
                zombie.sprScaleX *= 1.25f;
                zombie.sprScaleY *= 1.25f;
                zombie.elite = false;
                zombie.setElite(false);
                _level.fx.boneSmoke(zombie.cx, zombie.cy, Utils.GetColor(10, 10, 100));

                var fz = new Defender(_level, zombie.cx, zombie.cy, 1, 1);
                fz.init();
                fz.initLife(maxLife / 3, maxLife / 2);
                fz.setElite(false);
            }
            state++;
        }
    }
}
