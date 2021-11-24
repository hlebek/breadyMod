using Terraria;
using Terraria.ModLoader;

namespace breadyMod.Items.Dusts
{
    public class DezintegratorDustBody : ModDust
    {
        float random = Main.rand.NextFloat(1.01f, 1.02f);

        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.2f;
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale *= 1.5f;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.1f;
            dust.scale *= random;
            float light = 0.35f * dust.scale;
            Lighting.AddLight(dust.position, light, light, light);
            if (dust.scale > 1.7f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}