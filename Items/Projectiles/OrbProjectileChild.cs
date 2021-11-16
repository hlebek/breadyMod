using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.Projectiles
{
    class OrbProjectileChild : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.light = 0.3f;
            projectile.magic = true;
            drawOffsetX = -8;
            drawOriginOffsetY = -3;
            projectile.penetrate = 2;
            projectile.timeLeft = 180;
            projectile.ignoreWater = true;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(126, 126, 255, 0);

        public override void AI()
        {
            // This part makes the projectile do a shime sound every 30 ticks as long as it is moving.
            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 30;
                Main.PlaySound(SoundID.Item43, projectile.position);
            }

            Vector2 dustPosition = projectile.Center + new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
            Dust dust = Dust.NewDustPerfect(dustPosition, 15, null, 100, Color.AliceBlue, 0.8f);
            dust.noGravity = true;

            // Set the rotation so the projectile points towards where it's going.
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.rotation = projectile.velocity.ToRotation();
            }

            // Seek and chase enemy
            float projectileRange = 200f; //default: 200
            bool lineOfSight = false;
            bool targetFound = false;
            Vector2 targetCenter = new Vector2();

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy())
                {
                    float between = Vector2.Distance(npc.Center, projectile.Center);
                    lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);

                    if (between < projectileRange && lineOfSight)
                    {
                        projectileRange = between;
                        targetCenter = npc.Center;
                        targetFound = true;
                    }
                }
            }

            if(targetFound && Vector2.Distance(projectile.Center, targetCenter) > 20)
            {
                Vector2 direction = targetCenter - projectile.position;
                direction.Normalize();
                direction *= 8f; // Speed of projectile after target has beed found. Just change the float value to change the speed. Default: 8
                projectile.velocity = direction;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.penetrate < 0)
                projectile.Kill();
            projectile.penetrate--;

            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }
            return false; // return false because we are handling collision
        }
    }
}
