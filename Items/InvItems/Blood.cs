using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.InvItems
{
    class Blood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Acquired in a horrible way");

            DisplayName.AddTranslation(GameCulture.Polish, "Krew");
            Tooltip.AddTranslation(GameCulture.Polish, "Pozyskana w straszliwy sposób");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
        }
    }
}