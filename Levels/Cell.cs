using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Levels
{
    public struct Cell
    {
        const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
        const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
        const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;
        const float ROTATION_RADIANS = (float)(90 * Math.PI / 180);

        public Cell(uint id)
        {
            ID = id & ~(FLIPPED_HORIZONTALLY_FLAG | FLIPPED_VERTICALLY_FLAG | FLIPPED_DIAGONALLY_FLAG);
            FlippedHorizontally = (id & FLIPPED_HORIZONTALLY_FLAG) != 0;
            FlippedVertically = (id & FLIPPED_VERTICALLY_FLAG) != 0;
            FlippedDiagonally = (id & FLIPPED_DIAGONALLY_FLAG) != 0;
        }

        public uint ID { get; set; }
        public float Rotation => FlippedDiagonally ? ROTATION_RADIANS : 0f;
        public CellProperties Properties => CellProperties.GetCellProperties(ID);
        public bool FlippedDiagonally { get; set; }
        public bool FlippedHorizontally { get; set; }
        public bool FlippedVertically { get; set; }
    }
}
