using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

//Jak narazie jest ciekawie. Jedyne czego brakuje to wyliczania punkta końcowego (albo dwóch punktów, bo wiązka
//ma szerokość, także wyznaczałbym 2 punkty końcowe dla obu krawędzi najlepiej). Po dodaniu wyliczenia punkta
//końcowego lasera będzie trzeba zmienić rysowanie spriteBatchy - cały na raz ma się rysować, a nie dorysowywać
//w raz z pokonaną odległością projectile'a (dodanie punkta końcowego rozwiąże ten problem). Końcówka ma się
// rysować (a jakże by inaczej) na końcu.
//Możliwe, że spriteBatche będą o wiele za szybko znikać gdy już broń będzie skończona. Aby uniknąć efektów
//stroboskopowych czy wiązki lasera pojawiającej się na 2 klatki, można dodać niewidzialnego projectile'a, który
//będzie miał podpiętego pod siebie spriteBatcha i będzie żył np. przez 3 sekundy, a następnie umierał.

namespace breadyMod.Items.Projectiles
{
    class DezintegratorProjectile : ModProjectile
    {
        private Vector2 laserEndPoint;
        private Vector2 laserCurrPoint;
        private float laserDrawStep = 500f;

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.light = 0.6f;
            projectile.magic = true;
            //drawOffsetX = -15;
            //drawOriginOffsetY = -3;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 90;
            projectile.ignoreWater = true;
            //projectile.tileCollide = false;
            projectile.extraUpdates = 10;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 toEndPointVector = laserEndPoint - Main.player[projectile.owner].Center;
            toEndPointVector.Normalize();
            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center, toEndPointVector, 10, projectile.damage, -1.57f, 1f, 1000f, Color.White, 75);// this is the projectile sprite draw, 45 = the distance of where the projectile starts from the player

            return false;
        }

        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 toEndPointVector, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int projStartDistFromThePlayer = 50)
        {
            Vector2 origin = start;
            Vector2 tailOffset = toEndPointVector * 70; ;
            float r = toEndPointVector.ToRotation() + rotation;
            int distance = (int)Vector2.Distance(laserCurrPoint ,Main.player[projectile.owner].Center);


            //#region Draw laser body
            //for (float i = projStartDistFromThePlayer; i <= projStartDistFromThePlayer+distance-step; i += step)
            //{
            //    Color c = Color.White;
            //    origin = start + tailOffset + i * toEndPointVector;
            //    spriteBatch.Draw(texture, origin - Main.screenPosition,
            //    new Rectangle(0, 0, 30, 30), c, r,
            //    new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            //}
            //#endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + tailOffset - Main.screenPosition,
            new Rectangle(0, 63, 30, 30), Color.White, r+(float)Math.PI,
            new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (laserDrawStep + step) * toEndPointVector - Main.screenPosition,
            new Rectangle(0, 32, 30, 30), Color.White, r, new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            #endregion
        }

        public override void AI()
        {
            laserCurrPoint = projectile.position;
            Vector2 step = Main.player[projectile.owner].Center - Main.MouseWorld;
            step.Normalize();
            step *= -1;
            laserEndPoint = Main.player[projectile.owner].Center + step * laserDrawStep;


        }
    }
}
