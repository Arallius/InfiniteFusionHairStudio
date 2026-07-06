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
        private ComposerLibrary? _composerLibrary;

        private readonly List<SlotEditor> _slots = new();

        // Existing export slots still use this.
        private readonly string[] _palettes =
        {
            "Brown",
            "Blonde",
            "Black",
            "Purple"
        };



        // New Composer color list.
        private readonly HairColor[] _hairColors =
        {
            HairColor.Brown,
            HairColor.Blonde,
            HairColor.Black,
            HairColor.Purple,
            HairColor.Red,
            HairColor.Pink,
            HairColor.Blue,
            HairColor.Green,
            HairColor.White,
            HairColor.Silver,
            HairColor.Orange
        };

        // New Composer shade list.
        private readonly HairShade[] _hairShades =
        {
            HairShade.Light,
            HairShade.Normal,
            HairShade.Dark
        };

        public MainWindow()
        {
            InitializeComponent();

            _preview = new PreviewRenderer(
                PreviewBack,
                PreviewBody,
                PreviewBase,
                PreviewBangs);

            string libraryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets",
                "Library");

            _hairLibrary = new HairLibrary(libraryPath);

            string composerPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets",
                "Composer");

            _composerLibrary = new ComposerLibrary(composerPath);

            SetupHairComboBox(ReplaceHairComboBox, _hairLibrary);

            SetupHairComboBox(Slot1Hair, _hairLibrary);
            SetupHairComboBox(Slot2Hair, _hairLibrary);
            SetupHairComboBox(Slot3Hair, _hairLibrary);
            SetupHairComboBox(Slot4Hair, _hairLibrary);

            Slot1Color.ItemsSource = _palettes;
            Slot2Color.ItemsSource = _palettes;
            Slot3Color.ItemsSource = _palettes;
            Slot4Color.ItemsSource = _palettes;

            Slot1Color.SelectedIndex = 0;
            Slot2Color.SelectedIndex = 1;
            Slot3Color.SelectedIndex = 2;
            Slot4Color.SelectedIndex = 3;

            SetupComposer();

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

        private void SetupComposer()
        {
            if (_composerLibrary == null)
                return;

            ComposerBangs.ItemsSource = _composerLibrary.Bangs;
            ComposerBase.ItemsSource = _composerLibrary.Base;
            ComposerBack.ItemsSource = _composerLibrary.Back;

            ComposerBangs.DisplayMemberPath = "Name";
            ComposerBase.DisplayMemberPath = "Name";
            ComposerBack.DisplayMemberPath = "Name";

            ComposerBangsColor.ItemsSource = _hairColors;
            ComposerBaseColor.ItemsSource = _hairColors;
            ComposerBackColor.ItemsSource = _hairColors;

            ComposerBangsShade.ItemsSource = _hairShades;
            ComposerBaseShade.ItemsSource = _hairShades;
            ComposerBackShade.ItemsSource = _hairShades;

            SelectDefaultPart(ComposerBangs, "Bald");
            SelectDefaultPart(ComposerBase, "Bald");
            SelectDefaultPart(ComposerBack, "Bald");

            ComposerBangsColor.SelectedItem = HairColor.Brown;
            ComposerBaseColor.SelectedItem = HairColor.Brown;
            ComposerBackColor.SelectedItem = HairColor.Brown;

            ComposerBangsShade.SelectedItem = HairShade.Normal;
            ComposerBaseShade.SelectedItem = HairShade.Normal;
            ComposerBackShade.SelectedItem = HairShade.Normal;

            // Live preview updates
            ComposerBangs.SelectionChanged += ComposerSelectionChanged;
            ComposerBase.SelectionChanged += ComposerSelectionChanged;
            ComposerBack.SelectionChanged += ComposerSelectionChanged;

            ComposerBangsColor.SelectionChanged += ComposerSelectionChanged;
            ComposerBaseColor.SelectionChanged += ComposerSelectionChanged;
            ComposerBackColor.SelectionChanged += ComposerSelectionChanged;

            ComposerBangsShade.SelectionChanged += ComposerSelectionChanged;
            ComposerBaseShade.SelectionChanged += ComposerSelectionChanged;
            ComposerBackShade.SelectionChanged += ComposerSelectionChanged;
        }

        private static void SelectDefaultPart(ComboBox comboBox, string name)
        {
            if (comboBox.ItemsSource is not IEnumerable<HairPart> parts)
                return;

            HairPart? bald = parts.FirstOrDefault(p =>
                p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            comboBox.SelectedItem = bald ?? parts.FirstOrDefault();
        }

        private SlotEditor GetActiveSlot() => _slots.First(s => s.Selector.IsChecked == true);

        private void LoadPreviewBody()
        {
            string bodyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Assets","Preview","trainer_5.png");
            _preview?.SetBody(bodyPath);
        }

        private void UpdatePreviewHair()
        {
            var active = GetActiveSlot();
            PreviewGroup.Header = $"Live Preview — Slot {active.SlotNumber}";

            HairColor backColor = (HairColor)ComposerBackColor.SelectedItem;
            HairShade backShade = (HairShade)ComposerBackShade.SelectedItem;

            HairColor baseColor = (HairColor)ComposerBaseColor.SelectedItem;
            HairShade baseShade = (HairShade)ComposerBaseShade.SelectedItem;

            HairColor bangsColor = (HairColor)ComposerBangsColor.SelectedItem;
            HairShade bangsShade = (HairShade)ComposerBangsShade.SelectedItem;

            if (ComposerBack.SelectedItem is HairPart back)
            {
                _preview?.SetBack(
                    back.TrainerImagePath,
                    backColor,
                    backShade);
            }

            if (ComposerBase.SelectedItem is HairPart hairBase)
            {
                _preview?.SetBase(
                    hairBase.TrainerImagePath,
                    baseColor,
                    baseShade);
            }

            if (ComposerBangs.SelectedItem is HairPart bangs)
            {
                _preview?.SetBangs(
                    bangs.TrainerImagePath,
                    bangsColor,
                    bangsShade);
            }
        }

        private void ComposerSelectionChanged(object? sender, SelectionChangedEventArgs e) => UpdatePreviewHair();
        private void ActiveSlotChanged(object? sender, RoutedEventArgs e) => UpdatePreviewHair();
        private void ActiveHairChanged(object? sender, SelectionChangedEventArgs e) => UpdatePreviewHair();

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
