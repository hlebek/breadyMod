using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.InvItems
{
    class CompressedMana : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hard");

            DisplayName.AddTranslation(GameCulture.Polish, "Skompresowana Mana");
            Tooltip.AddTranslation(GameCulture.Polish, "Twarda");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.LiquidMana>(), 1);
            recipe.AddIngredient(ItemID.Bomb, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}