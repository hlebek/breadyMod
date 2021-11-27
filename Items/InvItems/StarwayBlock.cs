using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.InvItems
{
    class StarwayBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You feel blessed with stars when you run on it!");

            DisplayName.AddTranslation(GameCulture.Polish, "Blok gwiazdostrady");
            Tooltip.AddTranslation(GameCulture.Polish, "Czujesz błogosławieństwo gwiazd, gdy mkniesz po gwiazdostradzie!");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = ModContent.TileType<Items.Tiles.StarwayBlockTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddIngredient(ItemID.AsphaltBlock, 1);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.StarPowder>(), 1);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}