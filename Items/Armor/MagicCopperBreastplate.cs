using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class MagicCopperBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shiny!" +
                                "\n+5% magic crit.");

            DisplayName.AddTranslation(GameCulture.Polish, "Napierśnik z Magicznej Miedzi");
            Tooltip.AddTranslation(GameCulture.Polish, "Błyszczący!" +
                                                        "+5% sznansy na magiczne obrażenia krytyczne.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicCrit += 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.MagicCopperBar>(), 30);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}