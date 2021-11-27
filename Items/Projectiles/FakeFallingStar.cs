using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;


namespace breadyMod.Items.Projectiles
{
    class FakeFallingStar : ModProjectile
    {
        int[] arr = {-1, 1};

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 24;
            projectile.damage = 999;
            projectile.friendly = true;
            projectile.light = 0.8f;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 3600;
            projectile.ignoreWater = true;
            projectile.velocity = new Vector2(Main.rand.Next(arr), 10);
            projectile.aiStyle = 5;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 20;
                Main.PlaySound(SoundID.Item9, projectile.position);
            }
        }

        public override void Kill(int timeLeft)
        {
            Item.NewItem(projectile.position, 16, 16, ItemID.FallenStar);
            Main.PlaySound(SoundID.Item14, projectile.position);
        }
    }
}
