using InfiniteFusionHairStudio.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace InfiniteFusionHairStudio
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string libraryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets",
                "Library");

            var hairLibrary = new HairLibrary(libraryPath);

            SetupHairComboBox(ReplaceHairComboBox, hairLibrary);

            SetupHairComboBox(Slot1Hair, hairLibrary);
            SetupHairComboBox(Slot2Hair, hairLibrary);
            SetupHairComboBox(Slot3Hair, hairLibrary);
            SetupHairComboBox(Slot4Hair, hairLibrary);

            string[] palettes =
            {
                "Brown",
                "Blonde",
                "Black",
                "Purple"
            };

            Slot1Color.ItemsSource = palettes;
            Slot2Color.ItemsSource = palettes;
            Slot3Color.ItemsSource = palettes;
            Slot4Color.ItemsSource = palettes;

            Slot1Color.SelectedIndex = 0;
            Slot2Color.SelectedIndex = 1;
            Slot3Color.SelectedIndex = 2;
            Slot4Color.SelectedIndex = 3;
        }

        private void SetupHairComboBox(ComboBox comboBox, HairLibrary library)
        {
            comboBox.ItemsSource = library.Hairstyles;
            comboBox.DisplayMemberPath = "Name";
            comboBox.SelectedIndex = 0;
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var slot1 = new ExportSlot
            {
                Hair = (HairAsset)Slot1Hair.SelectedItem,
                PaletteIndex = Slot1Color.SelectedIndex
            };

            var slot2 = new ExportSlot
            {
                Hair = (HairAsset)Slot2Hair.SelectedItem,
                PaletteIndex = Slot2Color.SelectedIndex
            };

            var slot3 = new ExportSlot
            {
                Hair = (HairAsset)Slot3Hair.SelectedItem,
                PaletteIndex = Slot3Color.SelectedIndex
            };

            var slot4 = new ExportSlot
            {
                Hair = (HairAsset)Slot4Hair.SelectedItem,
                PaletteIndex = Slot4Color.SelectedIndex
            };

            var replaceHair = (HairAsset)ReplaceHairComboBox.SelectedItem;

            string exportsFolder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Exports");

            Exporter.Export(
                exportsFolder,
                replaceHair,
                slot1,
                slot2,
                slot3,
                slot4);

            MessageBox.Show(
                $"Export complete!\n\nSaved to:\n{exportsFolder}");
        }
    }
}