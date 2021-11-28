using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.Buffs
{
    public class MagicExhaustion : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Magic Exhaustion");
            Description.SetDefault("Magic weapons require 2x mana to use.");

            DisplayName.AddTranslation(GameCulture.Polish, "Magiczne Wyczerpanie");
            Description.AddTranslation(GameCulture.Polish, "Używanie magicznych broni wymaga 2 razy więcej many.");

            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.manaCost *= 2;
        }
    }
}