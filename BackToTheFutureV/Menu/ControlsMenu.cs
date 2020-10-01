﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BackToTheFutureV.GUI;
using BackToTheFutureV.Settings;
using BackToTheFutureV.Utility;
using GTA;
using GTA.UI;
using LemonUI.Elements;
using LemonUI.Menus;
using Control = GTA.Control;

namespace BackToTheFutureV.Menu
{
    public class ControlsMenu : CustomNativeMenu
    {
        private NativeCheckboxItem UseControlForMainMenu;
        private NativeListItem<Keys> MainMenu;

        private NativeCheckboxItem CombinationsForInteractionMenu;
        private NativeListItem<CustomControl> InteractionMenu1;
        private NativeListItem<CustomControl> InteractionMenu2;

        private NativeCheckboxItem LongPressForHover;
        private NativeListItem<CustomControl> Hover;
        private NativeListItem<CustomControl> HoverBoost;
        private NativeListItem<CustomControl> HoverVTOL;
        private NativeListItem<Keys> HoverAltitudeHold;

        private NativeListItem<Keys> TCToggle;
        private NativeListItem<Keys> CutsceneToggle;
        private NativeListItem<Keys> InputToggle;

        private bool _doNotUpdate;

        public ControlsMenu() : base("", Game.GetLocalizedString("BTTFV_Menu_ControlsMenu"))
        {
            Banner = null;

            Offset = new PointF(0, 60);

            Width = 600;

            Shown += ControlsMenu_Shown;
            Closing += ControlsMenu_Closing;
            OnItemCheckboxChanged += ControlsMenu_OnItemCheckboxChanged;

            Add(MainMenu = new NativeListItem<Keys>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_MainMenu"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_MainMenu_Description")));
            MainMenu.Items = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            MainMenu.ItemChanged += MainMenu_ItemChanged;

            Add(UseControlForMainMenu = new NativeCheckboxItem(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_UseControlForMainMenu"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_UseControlForMainMenu_Description")));

            
            Add(InteractionMenu1 = new NativeListItem<CustomControl>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_InteractionMenu1"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_InteractionMenu1_Description")));
            InteractionMenu1.Items = CustomControl.CustomControls;
            InteractionMenu1.ItemChanged += InteractionMenu1_ItemChanged;

            Add(InteractionMenu2 = new NativeListItem<CustomControl>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_InteractionMenu2"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_InteractionMenu2_Description")));
            InteractionMenu2.Items = CustomControl.CustomControls;
            InteractionMenu2.ItemChanged += InteractionMenu2_ItemChanged;

            Add(CombinationsForInteractionMenu = new NativeCheckboxItem(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_CombinationsForInteractionMenu"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_CombinationsForInteractionMenu_Description")));

            
            Add(Hover = new NativeListItem<CustomControl>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_Hover"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_Hover_Description")));
            Hover.Items = CustomControl.CustomControls;
            Hover.ItemChanged += Hover_ItemChanged;

            Add(LongPressForHover = new NativeCheckboxItem(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_LongPressForHover"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_LongPressForHover_Description")));

            Add(HoverBoost = new NativeListItem<CustomControl>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_HoverBoost"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_HoverBoost_Description")));
            HoverBoost.Items = CustomControl.CustomControls;
            HoverBoost.ItemChanged += HoverBoost_ItemChanged;

            Add(HoverVTOL = new NativeListItem<CustomControl>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_HoverVTOL"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_HoverVTOL_Description")));
            HoverVTOL.Items = CustomControl.CustomControls;
            HoverVTOL.ItemChanged += HoverVTOL_ItemChanged;

            Add(HoverAltitudeHold = new NativeListItem<Keys>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_HoverAltitudeHold"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_HoverAltitudeHold_Description")));
            HoverAltitudeHold.Items = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            HoverAltitudeHold.ItemChanged += HoverAltitudeHold_ItemChanged;

            
            Add(TCToggle = new NativeListItem<Keys>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_TCToggle"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_TCToggle_Description")));
            TCToggle.Items = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            TCToggle.ItemChanged += TCToggle_ItemChanged;

            Add(CutsceneToggle = new NativeListItem<Keys>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_CutsceneToggle"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_CutsceneToggle_Description")));
            CutsceneToggle.Items = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            CutsceneToggle.ItemChanged += CutsceneToggle_ItemChanged;

            Add(InputToggle = new NativeListItem<Keys>(Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_InputToggle"), Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_InputToggle_Description")));
            InputToggle.Items = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            InputToggle.ItemChanged += InputToggle_ItemChanged;

            Main.ObjectPool.Add(this);
        }

        private void InputToggle_ItemChanged(object sender, ItemChangedEventArgs<Keys> e)
        {
            ModControls.InputToggle = e.Object;
        }

        private void CutsceneToggle_ItemChanged(object sender, ItemChangedEventArgs<Keys> e)
        {
            ModControls.CutsceneToggle = e.Object;
        }

        private void TCToggle_ItemChanged(object sender, ItemChangedEventArgs<Keys> e)
        {
            ModControls.TCToggle = e.Object;
        }

        private void ControlsMenu_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ModSettings.SaveSettings();
        }

        private void HoverAltitudeHold_ItemChanged(object sender, ItemChangedEventArgs<Keys> e)
        {
            ModControls.HoverAltitudeHold = e.Object;
        }

        private void HoverVTOL_ItemChanged(object sender, ItemChangedEventArgs<CustomControl> e)
        {
            if (_doNotUpdate)
                return;

            ModControls.HoverVTOL = e.Object.Control;
            GTA.UI.Screen.ShowHelpTextThisFrame($"{Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_Button")} {e.Object.Button}", false);
        }

        private void HoverBoost_ItemChanged(object sender, ItemChangedEventArgs<CustomControl> e)
        {
            if (_doNotUpdate)
                return;

            ModControls.HoverBoost = e.Object.Control;
            GTA.UI.Screen.ShowHelpTextThisFrame($"{Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_Button")} {e.Object.Button}", false);
        }

        private void Hover_ItemChanged(object sender, ItemChangedEventArgs<CustomControl> e)
        {
            if (_doNotUpdate)
                return;

            ModControls.Hover = e.Object.Control;
            GTA.UI.Screen.ShowHelpTextThisFrame($"{Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_Button")} {e.Object.Button}", false);
        }

        private void InteractionMenu2_ItemChanged(object sender, ItemChangedEventArgs<CustomControl> e)
        {
            if (_doNotUpdate)
                return;

            ModControls.InteractionMenu2 = e.Object.Control;
            GTA.UI.Screen.ShowHelpTextThisFrame($"{Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_Button")} {e.Object.Button}", false);
        }

        private void InteractionMenu1_ItemChanged(object sender, ItemChangedEventArgs<CustomControl> e)
        {
            if (_doNotUpdate)
                return;

            ModControls.InteractionMenu1 = e.Object.Control;
            GTA.UI.Screen.ShowHelpTextThisFrame($"{Game.GetLocalizedString("BTTFV_Menu_ControlsMenu_Button")} {e.Object.Button}", false);
        }

        private void MainMenu_ItemChanged(object sender, ItemChangedEventArgs<Keys> e)
        {
            ModControls.MainMenu = e.Object;
        }

        private void ControlsMenu_OnItemCheckboxChanged(NativeCheckboxItem sender, EventArgs e, bool Checked)
        {
            if (sender == UseControlForMainMenu)
                ModControls.UseControlForMainMenu = Checked;
            else if (sender == CombinationsForInteractionMenu)
                ModControls.CombinationsForInteractionMenu = Checked;
            else if (sender == LongPressForHover)
                ModControls.LongPressForHover = Checked;
        }

        private void ControlsMenu_Shown(object sender, EventArgs e)
        {
            _doNotUpdate = true;

            UseControlForMainMenu.Checked = ModControls.UseControlForMainMenu;
            MainMenu.SelectedIndex = MainMenu.Items.IndexOf(ModControls.MainMenu);

            CombinationsForInteractionMenu.Checked = ModControls.CombinationsForInteractionMenu;
            InteractionMenu1.SelectedIndex = CustomControl.IndexOf(ModControls.InteractionMenu1);
            InteractionMenu2.SelectedIndex = CustomControl.IndexOf(ModControls.InteractionMenu2);

            LongPressForHover.Checked = ModControls.LongPressForHover;
            Hover.SelectedIndex = CustomControl.IndexOf(ModControls.Hover);
            HoverBoost.SelectedIndex = CustomControl.IndexOf(ModControls.HoverBoost);
            HoverVTOL.SelectedIndex = CustomControl.IndexOf(ModControls.HoverVTOL);
            HoverAltitudeHold.SelectedIndex = HoverAltitudeHold.Items.IndexOf(ModControls.HoverAltitudeHold);

            TCToggle.SelectedIndex = TCToggle.Items.IndexOf(ModControls.TCToggle);
            CutsceneToggle.SelectedIndex = CutsceneToggle.Items.IndexOf(ModControls.CutsceneToggle);
            InputToggle.SelectedIndex = InputToggle.Items.IndexOf(ModControls.InputToggle);

            Recalculate();

            _doNotUpdate = false;
        }

        public override void Tick()
        {
            
        }
    }
}