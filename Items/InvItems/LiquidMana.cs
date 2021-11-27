using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.InvItems
{
    class LiquidMana : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Quite dense");

            DisplayName.AddTranslation(GameCulture.Polish, "Ciekła Mana");
            Tooltip.AddTranslation(GameCulture.Polish, "Dość gęsta");
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
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.ManaEssence>(), 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(ModContent.TileType<Items.Tiles.MagicCatalystTile>());
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}