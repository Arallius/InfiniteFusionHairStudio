using InfiniteFusionHairStudio.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InfiniteFusionHairStudio
{
    public partial class MainWindow : Window
    {
        private PreviewRenderer? _preview;
        private HairLibrary? _hairLibrary;

        private readonly List<SlotEditor> _slots = new();

        public MainWindow()
        {
            InitializeComponent();

            _preview = new PreviewRenderer(PreviewBody, PreviewHair);

            string libraryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets",
                "Library");

            _hairLibrary = new HairLibrary(libraryPath);

            SetupHairComboBox(ReplaceHairComboBox, _hairLibrary);

            SetupHairComboBox(Slot1Hair, _hairLibrary);
            SetupHairComboBox(Slot2Hair, _hairLibrary);
            SetupHairComboBox(Slot3Hair, _hairLibrary);
            SetupHairComboBox(Slot4Hair, _hairLibrary);

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

            _slots.Add(new SlotEditor(1, Slot1Selector, Slot1Hair, Slot1Color));
            _slots.Add(new SlotEditor(2, Slot2Selector, Slot2Hair, Slot2Color));
            _slots.Add(new SlotEditor(3, Slot3Selector, Slot3Hair, Slot3Color));
            _slots.Add(new SlotEditor(4, Slot4Selector, Slot4Hair, Slot4Color));

            foreach (var slot in _slots)
            {
                slot.Selector.Checked += ActiveSlotChanged;
                slot.HairCombo.SelectionChanged += ActiveHairChanged;
            }

            LoadPreviewBody();
            UpdatePreviewHair();
        }

        private void SetupHairComboBox(ComboBox comboBox, HairLibrary library)
        {
            comboBox.ItemsSource = library.Hairstyles;
            comboBox.DisplayMemberPath = "Name";
            comboBox.SelectedIndex = 0;
        }

        private SlotEditor GetActiveSlot()
        {
            return _slots.First(s => s.Selector.IsChecked == true);
        }

        private void LoadPreviewBody()
        {
            string bodyPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets",
                "Preview",
                "trainer_5.png");

            _preview?.SetBody(bodyPath);
        }

        private void UpdatePreviewHair()
        {
            var active = GetActiveSlot();

            // Update the title automatically
            PreviewGroup.Header = $"Live Preview — Slot {active.SlotNumber}";

            if (active.HairCombo.SelectedItem is not HairAsset hair)
                return;

            string hairPath = Path.Combine(
                hair.FolderPath,
                $"hair_trainer_{active.SlotNumber}_{hair.FilePrefix}.png");

            _preview?.SetHair(hairPath);
        }

        private void ActiveSlotChanged(object? sender, RoutedEventArgs e)
        {
            UpdatePreviewHair();
        }

        private void ActiveHairChanged(object? sender, SelectionChangedEventArgs e)
        {
            UpdatePreviewHair();
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