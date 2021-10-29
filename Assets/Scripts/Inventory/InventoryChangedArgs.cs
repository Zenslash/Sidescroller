public class InventoryChangedArgs
{
    public int SlotIndex;
    public SlotPanel panel;

    public InventoryChangedArgs(int slot, SlotPanel panel)
    {
        SlotIndex = slot;
        this.panel = panel;
    }
}