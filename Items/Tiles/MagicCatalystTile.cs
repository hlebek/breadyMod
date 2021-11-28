using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace breadyMod.Items.Tiles
{
    public class MagicCatalystTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(200, 200, 0));
            drop = ModContent.ItemType<Items.InvItems.MagicCatalyst>();
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 32, ModContent.ItemType<Items.InvItems.MagicCatalyst>());
        }

        public override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (player.statMana > 100)
            {
                player.AddBuff(BuffID.ManaSickness, 60 * 10);
                // TODO: Nie dodaje czasu, tylko ustawia ciągle na 30 sekund! Poprawić, żeby czas się dodawał do max 300 sekund
                if (player.HasBuff(ModContent.BuffType<Items.Buffs.MagicExhaustion>()) && player.buffTime[player.FindBuffIndex(ModContent.BuffType<Items.Buffs.MagicExhaustion>())] <= 270)
                    player.buffTime[player.FindBuffIndex(ModContent.BuffType<Items.Buffs.MagicExhaustion>())] += 60 * 30;
                else
                    player.AddBuff(ModContent.BuffType<Items.Buffs.MagicExhaustion>(), 60 * 30);

                bool bloodItemTaken = false;
                for (int k = 0; k < player.inventory.Length; k++)
                {
                    if (player.inventory[k].type == ModContent.ItemType<Items.InvItems.Blood>())
                    {
                        player.inventory[k].stack--;
                        bloodItemTaken = true;
                    }
                }

                if (!bloodItemTaken)
                    player.statLife -= 30;
                player.statMana -= 100;
                Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Items.InvItems.ManaEssence>());

                if (player.statLife <= 0)
                {
                    PlayerDeathReason death = new PlayerDeathReason();
                    death.SourceCustomReason = player.name + " decided to be a blood donor.";
                    player.KillMe(death, 5000, 1);
                }
            }

            return true;
        }
    }
}