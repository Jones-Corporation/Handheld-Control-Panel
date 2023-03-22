﻿using ControlzEx.Theming;
using Handheld_Control_Panel.Classes;
using Handheld_Control_Panel.Classes.Controller_Management;
using Handheld_Control_Panel.Classes.Global_Variables;
using Handheld_Control_Panel.Classes.TaskSchedulerWin32;
using Handheld_Control_Panel.Classes.UserControl_Management;
using Handheld_Control_Panel.Classes.Volume_Management;
using Handheld_Control_Panel.Styles;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Devices.Radios;

namespace Handheld_Control_Panel.UserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class AppType_Textbox : UserControl
    {
        private string windowpage = "";
        private string usercontrol = "";
        public AppType_Textbox()
        {
            InitializeComponent();
            setControlValue();
          
        }

        private void setControlValue()
        {
            switch (Global_Variables.profiles.editingProfile.AppType)
            {
                case "Steam":
                    controlToggle.Visibility = Visibility.Collapsed;
                    controlTextbox.Visibility = Visibility.Collapsed;
                    updateSteamImage();
                    
                    break;
                case "EpicGames":
                    controlToggle.Visibility = Visibility.Collapsed;
                    controlTextbox.IsReadOnly = true;
                    controlTextbox.Text = Global_Variables.profiles.editingProfile.Path;
                    updateIconImage();
                    break;

                default:
                    if (Global_Variables.profiles.editingProfile.Path != "")
                    {
                        updateIconImage();
                        controlTextbox.Text = Global_Variables.profiles.editingProfile.Path;
                        controlToggle.IsOn = true;

                    }
                    else
                    {
                        controlToggle.IsOn = false;
                        controlTextbox.Visibility = Visibility.Collapsed;
                        controlButton.Visibility = Visibility.Collapsed;
                    }

                    break;
            }

           

        }
        private void updateIconImage()
        {
            controlImage.Width = 34;
            controlImage.Height = 34;
            string file = Global_Variables.profiles.editingProfile.Path;
            if (File.Exists(file))
            {
                using (Icon ico = Icon.ExtractAssociatedIcon(file))
                {
                    controlImage.Source = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
            }

        }

        private void updateSteamImage()
        {
            string imageDirectory = Properties.Settings.Default.directorySteam + "\\appcache\\librarycache\\" + Global_Variables.profiles.editingProfile.GameID + "_header";
            if (File.Exists(imageDirectory + ".jpg"))
            {
                controlImage.Source = new BitmapImage(new Uri(imageDirectory + ".jpg", UriKind.RelativeOrAbsolute));
            }
            else
            {
                if (File.Exists(imageDirectory + ".png"))
                {
                    controlImage.Source = new BitmapImage(new Uri(imageDirectory + ".png", UriKind.RelativeOrAbsolute));
                }

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Controller_Window_Page_UserControl_Events.userControlControllerInput += handleControllerInputs;
            windowpage = WindowPageUserControl_Management.getWindowPageFromWindowToString(this);
            usercontrol = this.ToString().Replace("Handheld_Control_Panel.Pages.UserControls.","");
         
        }

        
        private void handleControllerInputs(object sender, EventArgs e)
        {
            controllerUserControlInputEventArgs args= (controllerUserControlInputEventArgs)e;
            if (args.WindowPage == windowpage && args.UserControl==usercontrol)
            {
                //Classes.UserControl_Management.UserControl_Management.handleUserControl(border, control, args.Action);
                switch(args.Action)
                {
                    case "A":
                        if (controlToggle.Visibility == Visibility.Visible)
                        {
                            controlToggle.IsOn = !controlToggle.IsOn;
                        }
                        break;
                }
            }
        }

        private void toggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (controlToggle.IsLoaded)
            {
                if (controlToggle.IsOn)
                {
                    controlTextbox.Visibility = Visibility.Visible;
                    controlButton.Visibility = Visibility.Visible;
                    controlImage.Visibility = Visibility.Visible;
                    if (Global_Variables.profiles.editingProfile.Path != "")
                    {
                        updateIconImage();
                       

                    }
                  

                }
                else
                {
                    controlTextbox.Text = "";
                    controlTextbox.Visibility = Visibility.Collapsed;
                    controlButton.Visibility = Visibility.Collapsed;
                    controlImage.Visibility = Visibility.Collapsed;

               
                }
            }

        }


        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Controller_Window_Page_UserControl_Events.userControlControllerInput -= handleControllerInputs;
           
        }

        private void controlButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void control_TextChanged(object sender, TextChangedEventArgs e)
        {
            Global_Variables.profiles.editingProfile.Path = controlTextbox.Text;
        }
    }
}
