using System;
using System.Reflection;
using HarmonyLib;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Plugins;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    public class Plugin : IPlugin, IDisposable
    {
        public const string Name = "StoreFix";
        public static Plugin Instance { get; private set; }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Instance = this;

            Harmony harmony = new Harmony(Name);

            var StoreGUIBuy = AccessTools.Method(typeof(MyStoreBlockViewModel), "UpdateTotalPriceToBuy");
            harmony.Patch(StoreGUIBuy, null, new HarmonyMethod(typeof(Plugin), "UpdateTotalBuyPricePatch"));

            var StoreGUISell = AccessTools.Method(typeof(MyStoreBlockViewModel), "UpdateTotalPriceToSell");
            harmony.Patch(StoreGUISell, null, new HarmonyMethod(typeof(Plugin), "UpdateTotalSellPricePatch"));
        }

        public void Dispose()
        {
            Instance = null;
        }

        public void Update()
        {

        }

        private static void UpdateTotalBuyPricePatch(float amount, MyStoreBlockViewModel __instance)
        {
            long num = 0L;
            if (!float.IsNaN(amount))
            {
                long longAmount = (long)amount;
                num = (longAmount * __instance.SelectedOfferItem.PricePerUnit);
            }
            __instance.TotalPriceToBuy = MyBankingSystem.GetFormatedValue(num, false);
            return;
        }

        private static void UpdateTotalSellPricePatch(float amount, MyStoreBlockViewModel __instance)
        {
            long num = 0L;
            if (!float.IsNaN(amount))
            {
                long longAmount = (long)amount;
                num = (longAmount * __instance.SelectedOrderItem.PricePerUnit);
            }
            __instance.TotalPriceToSell = MyBankingSystem.GetFormatedValue(num, false);
            return;
        }
    }
}