using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.InvItems
{
    class StarPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Smells funny. Tastes slimy.");

            DisplayName.AddTranslation(GameCulture.Polish, "Gwiezdny Pył");
            Tooltip.AddTranslation(GameCulture.Polish, "Dziwnie pachnie. Smakuje żelowato.");
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
            item.shootSpeed = 8f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}
