using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace breadyMod.Items.Dusts
{
    public class DezintegratorDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            //dust.color = new Color(230, 0, 0);
            dust.noGravity = true;
            //dust.frame = new Rectangle(0, 0, 30, 30);
            dust.noLight = false;
            dust.alpha = 0;
            dust.scale = 1f;
            dust.firstFrame = true;
            //If our texture had 2 different dust on top of each other (a 30x60 pixel image), we might do this:
            //dust.frame = new Rectangle(0, Main.rand.Next(2) * 30, 30, 30);
        }

        public override bool MidUpdate(Dust dust)
        {
            dust.rotation = dust.velocity.ToRotation();
            dust.alpha += 1;
            if (dust.alpha >= 240)
            {
                dust.active = false;
            }
            return false;
        }
    }
}