using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace breadyMod.Items.InvItems
{
    class StarPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Smells funny. Tastes slimy.");
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.shootSpeed = 4f;
            item.width = 16;
            item.height = 24;
            item.maxStack = 999;
            item.consumable = true;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 15;
            item.useTime = 15;
            item.noMelee = true;
            item.value = 75;
            item.shoot = ModContent.ProjectileType<Projectiles.StarPowderProjectile>();
            item.shootSpeed = 1f;
        }
    }
}
