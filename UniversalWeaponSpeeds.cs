namespace UniversalWeaponSpeeds
{
    using System.Collections.Generic;
    using UnityEngine;
    using BepInEx;
    using System;
    using System.Linq;
    using HarmonyLib;
    using TinyHelper;

    [BepInPlugin(GUID, NAME, VERSION)]
    public class UniversalWeaponSpeeds : BaseUnityPlugin
    {
        public const string GUID = "com.ehaugw.universalweaponspeeds";
        public const string VERSION = "2.0.0";
        public const string NAME = "Universal Weapon Speeds";

        internal void Awake()
        {
            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(ItemDetailsDisplay), "RefreshDetail")]
        public class ItemDetailsDisplay_RefreshDetail
        {
            [HarmonyPrefix]
            public static Boolean Prefix(ItemDetailsDisplay __instance, ref int _rowIndex, ref ItemDetailsDisplay.DisplayedInfos _infoType, ref bool __result, Weapon ___cachedWeapon)
            {
                __result = true;

                if (_infoType == ItemDetailsDisplay.DisplayedInfos.AttackSpeed)
                {
                    if (___cachedWeapon.Type != Weapon.WeaponType.Shield && ___cachedWeapon.Type != Weapon.WeaponType.Bow)
                    {
                        if (At.Call(__instance, "GetRow", new object[] { _rowIndex }) is ItemDetailRowDisplay speedRow)
                        {
                            speedRow.SetInfo(LocalizationManager.Instance.GetLoc("ItemStat_AttackSpeed"), Convert.ToSingle(Math.Round(___cachedWeapon.BaseAttackSpeed * 2 * WeaponManager.Speeds[Weapon.WeaponType.Sword_1H]/WeaponManager.Speeds[___cachedWeapon.Type],1)));
                            __result = true;
                            return false; //don't call orig
                        }
                    }
                }
                return true;
            }
        }
    }
}