using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace breadyMod.Items.Projectiles
{
    class DezintegratorProjectileReal : ModProjectile
    {
        bool didPierceWall = false;
        bool anotherWall = false;
        int oldPositionX = 0;
        int oldPositionY = 0;

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.light = 0.6f;
            projectile.magic = true;
            //drawOffsetX = -15;
            //drawOriginOffsetY = -3;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.damage = 62;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}
