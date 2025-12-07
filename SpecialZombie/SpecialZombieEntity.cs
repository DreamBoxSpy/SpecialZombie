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
    internal class SpecialZombieEntity : Zombie, 
        IHxbitSerializable<SpecialZombieEntity.Data>,
        IHxbitSerializeCallback
    {
        private class Data
        {
            public Defender defender = null!;
            public int maxLife;
            public int state = 0;

            public double sprSX = 0;
            public double sprSY = 0;
        }

        private Data data = new();

        public SpecialZombieEntity(Level lvl, int x, int y, int dmgTier, int lifeTier) : base(lvl, x, y, dmgTier, lifeTier)
        {

        }

        public override void preUpdate()
        {
            base.preUpdate();

            if(data.state > 3 || 
                life == 0)
            {
                return;
            }
            if(data.state == 0 && elite)
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
                initLife(data.maxLife * 1.5f, data.maxLife * 2);
            }
            else
            {
                return;
            }
            if (data.state == 0)
            {
                sprScaleX *= 1.25f;
                sprScaleY *= 1.25f;
                setOutlineColor(Utils.GetColor(80, 20, 20));
                _level.fx.dirtExplosion(cx, cy, 4, 0, Utils.GetColor(80, 20, 20), default);
            }
            else if(data.state == 1)
            {
                var fz = new Defender(_level, cx,cy, 1, 1);
                fz.init();
                fz.initLife(data.maxLife / 2, data.maxLife);
            }
            else if (data.state == 2)
            {
                setElite(true);
                _level.fx.boneSmoke(cx, cy, Utils.GetColor(10, 10, 100));
            }
            else if (data.state == 3)
            {
                sprScaleX *= 1.25f;
                elite = false;
                sprScaleY *= 1.25f;
                setElite(false);
                _level.fx.boneSmoke(cx, cy, Utils.GetColor(10, 10, 100));

                var fz = new Defender(_level, cx, cy, 1, 1);
                fz.init();
                fz.initLife(data.maxLife / 3, data.maxLife / 2);
                fz.setElite(false);
                data.defender = fz;
            }
            data.state++;
        }

        Data IHxbitSerializable<Data>.GetData()
        {
            data.sprSX = sprScaleX;
            data.sprSY = sprScaleY;
            return data;
        }

        void IHxbitSerializable<Data>.SetData(Data data)
        {
            sprScaleX = data.sprSX;
            sprScaleY = data.sprSY;
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
