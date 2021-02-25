﻿using BackToTheFutureV.GUI;
using BackToTheFutureV.Settings;
using FusionLibrary;
using GTA;
using LemonUI.Elements;
using LemonUI.Menus;
using System;
using System.Drawing;
using static BackToTheFutureV.Utility.InternalEnums;

namespace BackToTheFutureV.Menu
{
    internal class TCDMenu : CustomNativeMenu
    {
        private NativeListItem<TCDBackground> tcdBackground;
        private NativeItem changeTCD;
        private NativeItem resetToDefaultTCD;

        private NativeItem changeRCGUI;

        private NativeCheckboxItem hideSID;
        private NativeCheckboxItem useExternalTCD;
        private NativeCheckboxItem useNetworkTCD;
        private NativeCheckboxItem hideIngameTCD;

        public TCDMenu() : base("", Game.GetLocalizedString("BTTFV_Menu_TCDMenu"))
        {
            Banner = new ScaledTexture(new PointF(0, 0), new SizeF(200, 100), "bttf_textures", "bttf_menu_banner");

            Shown += SettingsMenu_Shown;
            OnItemActivated += TCDMenu_OnItemActivated;
            OnItemCheckboxChanged += TCDMenu_OnItemCheckboxChanged;

            Add(tcdBackground = new NativeListItem<TCDBackground>(Game.GetLocalizedString("BTTFV_Menu_TCDBackground"), Game.GetLocalizedString("BTTFV_Menu_TCDBackground_Description"), TCDBackground.Metal, TCDBackground.Transparent));
            tcdBackground.ItemChanged += TcdBackground_ItemChanged;

            Add(changeTCD = new NativeItem(Game.GetLocalizedString("BTTFV_Menu_TCDEditMode"), Game.GetLocalizedString("BTTFV_Menu_TCDEditMode_Description")));
            Add(changeRCGUI = new NativeItem("Edit RC HUD", ""));
            Add(resetToDefaultTCD = new NativeItem(Game.GetLocalizedString("BTTFV_Menu_TCDReset"), Game.GetLocalizedString("BTTFV_Menu_TCDReset_Description")));

            Add(hideSID = new NativeCheckboxItem(Game.GetLocalizedString("BTTFV_TCD_Edit_HideSID"), Game.GetLocalizedString("BTTFV_TCD_Edit_HideSID_Description"), ModSettings.HideSID));
            Add(useExternalTCD = new NativeCheckboxItem(Game.GetLocalizedString("BTTFV_TCD_Edit_External"), Game.GetLocalizedString("BTTFV_TCD_Edit_External_Description"), ModSettings.ExternalTCDToggle));
            Add(useNetworkTCD = new NativeCheckboxItem(Game.GetLocalizedString("BTTFV_TCD_Edit_Remote"), Game.GetLocalizedString("BTTFV_TCD_Edit_Remote_Description"), ModSettings.RemoteTCDToggle));
            Add(hideIngameTCD = new NativeCheckboxItem(Game.GetLocalizedString("BTTFV_TCD_Edit_HideHUD"), Game.GetLocalizedString("BTTFV_TCD_Edit_HideHUD_Description"), ModSettings.HideIngameTCDToggle));
        }

        private void TCDMenu_OnItemCheckboxChanged(NativeCheckboxItem sender, EventArgs e, bool Checked)
        {
            if (sender == useExternalTCD)
                ModSettings.ExternalTCDToggle = Checked;

            if (sender == useNetworkTCD)
                ModSettings.RemoteTCDToggle = Checked;

            if (sender == hideIngameTCD)
                ModSettings.HideIngameTCDToggle = Checked;

            if (sender == hideSID)
                ModSettings.HideSID = Checked;

            ModSettings.SaveSettings();
        }

        private void TCDMenu_OnItemActivated(NativeItem sender, EventArgs e)
        {
            if (sender == changeTCD)
            {
                TcdEditer.SetEditMode(true);

                Close();
            }

            if (sender == resetToDefaultTCD)
            {
                RCGUIEditer.ResetToDefault();
                TcdEditer.ResetToDefault();

                ModSettings.HideIngameTCDToggle = false;
                ModSettings.ExternalTCDToggle = false;
                ModSettings.RemoteTCDToggle = false;

                ModSettings.SaveSettings();
            }

            if (sender == changeRCGUI)
            {
                RCGUIEditer.SetEditMode(true);

                Close();
            }
        }

        private void TcdBackground_ItemChanged(object sender, ItemChangedEventArgs<TCDBackground> e)
        {
            ModSettings.TCDBackground = e.Object;
            ModSettings.SaveSettings();
        }

        private void SettingsMenu_Shown(object sender, EventArgs e)
        {
            useExternalTCD.Checked = ModSettings.ExternalTCDToggle;
        }

        public override void Tick()
        {

        }
    }
}
