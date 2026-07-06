using System.Windows.Controls;

namespace InfiniteFusionHairStudio.Core
{
    public class SlotEditor
    {
        public int SlotNumber { get; }

        public RadioButton Selector { get; }

        public SlotEditor(
            int slotNumber,
            RadioButton selector)
        {
            SlotNumber = slotNumber;
            Selector = selector;
        }
    }
}