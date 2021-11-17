using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

//TODO: Projectile traktuje każdego tile'a jak blok stały i dlatego jak przeleci przez np. drzewo albo platformę
//to potem przy następnej platformie się zabija... zmienić, żeby sprawdzić czy blok jest solidny czy nie.
//Do tego trzeba sprawić, aby wyświetlała się grafika lasera, ale jak?

namespace breadyMod.Items.Projectiles
{
    class DezintegratorProjectile : ModProjectile
    {
        bool didPierceWall = false;
        bool anotherWall = false;
        int oldPositionX = 0;
        int oldPositionY = 0;

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 11;
            projectile.friendly = true;
            projectile.light = 0.6f;
            projectile.magic = true;
            drawOffsetX = -15;
            drawOriginOffsetY = -3;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 10;
            projectile.tileCollide = false;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(230, 0, 0, 0);

        public override void AI()
        {
            //Dust dust = Dust.NewDustPerfect(projectile.Center, ModContent.DustType<Items.Dusts.DezintegratorDust>(), projectile.velocity*0.01f, 0, Color.Red, 1f);
            int positionX = (int)(projectile.Center.X / 16f);
            int positionY = (int)(projectile.Center.Y / 16f);

            if (Main.tile[positionX, positionY].active() && (oldPositionX != positionX && oldPositionY != positionY))   //if projectile hits the tile first time (is in it)
            {
                didPierceWall = true;
                oldPositionX = positionX;
                oldPositionY = positionY;
            }
            if (didPierceWall)                              //if projectile hit the wall
            {
                if (!Main.tile[positionX, positionY].active() && (oldPositionX != positionX && oldPositionY != positionY))   //if projectile did exit the wall
                    anotherWall = true;
            }
            if (anotherWall && Main.tile[positionX, positionY].active())    //if projectile is hitting a wall 2nd time
                projectile.Kill();
        }
    }
}
