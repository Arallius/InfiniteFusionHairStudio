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

        private readonly ExportSlot[] _exportSlots =
{
    new ExportSlot(),
    new ExportSlot(),
    new ExportSlot(),
    new ExportSlot()
};

        private bool _loadingSlot;

        


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

            HairPart baldBangs = _composerLibrary.Bangs.First(p => p.Name == "Bald");
            HairPart baldBase = _composerLibrary.Base.First(p => p.Name == "Bald");
            HairPart baldBack = _composerLibrary.Back.First(p => p.Name == "Bald");

            foreach (ExportSlot slot in _exportSlots)
            {
                slot.Bangs = baldBangs;
                slot.Base = baldBase;
                slot.Back = baldBack;
            }

            ReplaceHairComboBox.ItemsSource = _hairLibrary.Hairstyles;
            ReplaceHairComboBox.DisplayMemberPath = "Name";
            ReplaceHairComboBox.SelectedIndex = 0;



            SetupComposer();

            _slots.Add(new SlotEditor(1, Slot1Selector));
            _slots.Add(new SlotEditor(2, Slot2Selector));
            _slots.Add(new SlotEditor(3, Slot3Selector));
            _slots.Add(new SlotEditor(4, Slot4Selector));

            foreach (var slot in _slots)
            {
                slot.Selector.Checked += ActiveSlotChanged;
            }

            LoadPreviewBody();
            UpdatePreviewHair();
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

        private ExportSlot GetActiveExportSlot()
        {
            return _exportSlots[GetActiveSlot().SlotNumber - 1];
        }


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

        private void ComposerSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (_loadingSlot)
                return;
            ExportSlot slot = GetActiveExportSlot();

            slot.Bangs = (HairPart?)ComposerBangs.SelectedItem;
            slot.Base = (HairPart?)ComposerBase.SelectedItem;
            slot.Back = (HairPart?)ComposerBack.SelectedItem;

            slot.BangsColor = (HairColor)ComposerBangsColor.SelectedItem;
            slot.BaseColor = (HairColor)ComposerBaseColor.SelectedItem;
            slot.BackColor = (HairColor)ComposerBackColor.SelectedItem;

            slot.BangsShade = (HairShade)ComposerBangsShade.SelectedItem;
            slot.BaseShade = (HairShade)ComposerBaseShade.SelectedItem;
            slot.BackShade = (HairShade)ComposerBackShade.SelectedItem;

            UpdatePreviewHair();
        }
        private void ActiveSlotChanged(object? sender, RoutedEventArgs e)
        {
            _loadingSlot = true;

            try
            {
                ExportSlot slot = GetActiveExportSlot();

                if (_composerLibrary != null)
                {
                    ComposerBangs.SelectedItem = slot.Bangs;
                    ComposerBase.SelectedItem = slot.Base;
                    ComposerBack.SelectedItem = slot.Back;
                }

                ComposerBangsColor.SelectedItem = slot.BangsColor;
                ComposerBaseColor.SelectedItem = slot.BaseColor;
                ComposerBackColor.SelectedItem = slot.BackColor;

                ComposerBangsShade.SelectedItem = slot.BangsShade;
                ComposerBaseShade.SelectedItem = slot.BaseShade;
                ComposerBackShade.SelectedItem = slot.BackShade;
            }
            finally
            {
                _loadingSlot = false;
            }

            UpdatePreviewHair();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            

            var replaceHair = (HairAsset)ReplaceHairComboBox.SelectedItem;

            string exportsFolder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Exports");

            Exporter.Export(
    exportsFolder,
    replaceHair,
    _exportSlots);

            MessageBox.Show(
                $"Export complete!\n\nSaved to:\n{exportsFolder}");
        }
    }
}
