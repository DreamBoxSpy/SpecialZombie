using dc;
using dc.en.mob;
using dc.pr;
using ModCore.Serialization;
using ModCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialZombie
{
    internal class SpecialZombieEntity : Entity, 
        IHxbitSerializable<SpecialZombieEntity.Data>,
        IHxbitSerializeCallback
    {
        private class Data
        {
            public Defender defender = null!;
            public Zombie zombie = null!;
            public int maxLife;
            public int state = 0;
        }

        private Data data = new();

        public SpecialZombieEntity(Zombie target, Level lvl, int x, int y) : base(lvl, x, y)
        {
            init();
            data.zombie = target;
        }

        public override void preUpdate()
        {
            var life = data.zombie.life;
            if(data.state > 3 || 
                life == 0)
            {
                return;
            }
            if(data.state == 0 && data.zombie.elite)
            {
                return;
            }
            if (life > data.maxLife)
            {
                data.maxLife = life;
            }
            if(life < data.maxLife / 2)
            {
                //Angry
                data.zombie.initLife(data.maxLife * 1.5f, data.maxLife * 2);
            }
            else
            {
                return;
            }
            if (data.state == 0)
            {
                data.zombie.sprScaleX *= 1.25f;
                data.zombie.sprScaleY *= 1.25f;
                data.zombie.setOutlineColor(Utils.GetColor(80, 20, 20));
                _level.fx.dirtExplosion(data.zombie.cx, data.zombie.cy, 4, 0, Utils.GetColor(80, 20, 20), default);
            }
            else if(data.state == 1)
            {
                var fz = new Defender(_level, data.zombie.cx, data.zombie.cy, 1, 1);
                fz.init();
                fz.initLife(data.maxLife / 2, data.maxLife);
            }
            else if (data.state == 2)
            {
                data.zombie.setElite(true);
                _level.fx.boneSmoke(data.zombie.cx, data.zombie.cy, Utils.GetColor(10, 10, 100));
            }
            else if (data.state == 3)
            {
                data.zombie.sprScaleX *= 1.25f;
                data.zombie.elite = false;
                data.zombie.sprScaleY *= 1.25f;
                data.zombie.setElite(false);
                _level.fx.boneSmoke(data.zombie.cx, data.zombie.cy, Utils.GetColor(10, 10, 100));

                var fz = new Defender(_level, data.zombie.cx, data.zombie.cy, 1, 1);
                fz.init();
                fz.initLife(data.maxLife / 3, data.maxLife / 2);
                fz.setElite(false);
                data.defender = fz;
            }
            data.state++;
        }

        Data IHxbitSerializable<Data>.GetData()
        {
            return data;
        }

        void IHxbitSerializable<Data>.SetData(Data data)
        {
            this.data = data;
        }

        void IHxbitSerializeCallback.OnBeforeSerializing()
        {
            
        }

        void IHxbitSerializeCallback.OnAfterDeserializing()
        {
            Debug.Assert(data != null);
        }
    }
}
