using System.Windows.Controls;

namespace InfiniteFusionHairStudio.Core
{
    public class SlotEditor
    {
        public int SlotNumber { get; }

        public RadioButton Selector { get; }

        public ComboBox HairCombo { get; }

        public ComboBox PaletteCombo { get; }

        public SlotEditor(
            int slotNumber,
            RadioButton selector,
            ComboBox hairCombo,
            ComboBox paletteCombo)
        {
            SlotNumber = slotNumber;
            Selector = selector;
            HairCombo = hairCombo;
            PaletteCombo = paletteCombo;
        }
    }
}