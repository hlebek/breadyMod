using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace breadyMod.Items.Projectiles
{
    class StarPowderProjectile : ModProjectile // TODO: Zamiana bloków, które uderzy w inne bloki
    {
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.friendly = true;
            drawOffsetX = -9;
            drawOriginOffsetY = -3;
            projectile.penetrate = -1;
            projectile.timeLeft = 150;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Vector2 dustPosition = projectile.Center + Main.rand.NextVector2Circular(-30, 31);
            Dust dust = Dust.NewDustPerfect(dustPosition, 44, null, 100, Color.Gold, 1f);
            dust.velocity *= 0.2f;
            dust.noGravity = true;

            // Set the rotation so the projectile points towards where it's going.
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }

            projectile.ai[0] += 1f;
            if(projectile.ai[0] >=15)
            {
                projectile.velocity *= 0.99f;
            }

            int xOffsetLeft = (int)(projectile.position.X / 16f - ((float)projectile.width / 16f));
            int xOffsetRight = (int)(projectile.position.X / 16f + ((float)projectile.width / 16f));
            int yOffsetUp = (int)(projectile.position.Y / 16f - ((float)projectile.height / 16f));
            int yOffsetDown = (int)(projectile.position.Y / 16f + ((float)projectile.height / 16f));

            for (int i = xOffsetLeft; i < xOffsetRight; i++)
            {
                for (int j = yOffsetUp; j < yOffsetDown; j++)
                {
                    if (Main.tile[i, j].type == ItemID.DirtBlock) //coś za bardzo w lewo skanuje chyba
                    {
                        projectile.velocity *= 0;
                        //WorldGen.KillTile(i, j, false, false, true);
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Oblicz odległość w jakiej znajduje się środek projectile'a od uderzonego klocka.
            //Na podstawie odległości możesz stwierdzić ile koordynatów dalej od środka projectile'a
            //znajduje się blok.
            //Usuwasz ten blok i kładziesz w jego miejsce inny

            //Albo inny sposób, bo ciężko wykminić jaki konkretnie blok uderzył projectile
            //Gdy zostanie wykryte uderzenie, to usuń wszystkie bloki, które mają odpowienie ID
            //W ich miejsce wstaw inne bloki
            //voila

            return false; // return false because we are handling collision
        }
    }
}
