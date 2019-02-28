using System.Collections.Generic;

public class LinkedCellList
{
    public CellTrigger Cell { get; set; }
    public List<LinkedCellList> NextCells { get; set; }
    public LinkedCellList LastCell { get; set; }

    public LinkedCellList()
    {
        this.NextCells = new List<LinkedCellList>();
    }
}
