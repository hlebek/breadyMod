using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.InvItems
{
    class ManaEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This is magic");

            DisplayName.AddTranslation(GameCulture.Polish, "Esencja many");
            Tooltip.AddTranslation(GameCulture.Polish, "To jest magia");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
        }
    }
}