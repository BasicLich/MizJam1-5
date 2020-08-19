using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace MizJam1.Levels
{
    public struct CellProperties
    {
        private static Dictionary<uint, CellProperties> cellProperties = new Dictionary<uint, CellProperties>();
        static CellProperties()
        {
            CellProperties regularTile = new CellProperties(false, 20);
            CellProperties solidTile = new CellProperties(true, 20);
            CellProperties waterTile = new CellProperties(true, 20);
            CellProperties roadTile = new CellProperties(false, 10);
            CellProperties dirtRoadTile = new CellProperties(false, 15);
            CellProperties roughTerrain = new CellProperties(false, 25);
            CellProperties rougherTerrain = new CellProperties(false, 30);
            CellProperties lightForest = new CellProperties(false, 40);
            CellProperties forestTile = new CellProperties(false, 50);
            CellProperties boulderTile = new CellProperties(false, 60);
            CellProperties tightTile = new CellProperties(false, 60);

            for(uint i = 9; i <= 13; i++)
            {
                cellProperties[i] = roadTile;
            }

            cellProperties[49] = lightForest;
            cellProperties[50] = lightForest;
            cellProperties[51] = lightForest;
            cellProperties[52] = forestTile;
            cellProperties[53] = lightForest;
            cellProperties[54] = lightForest;
            cellProperties[55] = lightForest;
            cellProperties[56] = forestTile;
            cellProperties[57] = dirtRoadTile;
            cellProperties[58] = dirtRoadTile;
            cellProperties[59] = dirtRoadTile;
            cellProperties[60] = dirtRoadTile;
            cellProperties[61] = dirtRoadTile;

            cellProperties[97] = regularTile;
            cellProperties[100] = forestTile;
            cellProperties[101] = forestTile;
            cellProperties[102] = boulderTile;
            cellProperties[103] = lightForest;
            cellProperties[104] = lightForest;
            cellProperties[105] = roadTile;
            cellProperties[106] = roadTile;
            cellProperties[107] = roadTile;
            cellProperties[108] = roadTile;
            cellProperties[109] = roadTile;

            cellProperties[145] = solidTile;
            cellProperties[146] = solidTile;
            cellProperties[147] = solidTile;
            cellProperties[148] = solidTile;
            cellProperties[150] = solidTile;
            cellProperties[151] = tightTile;

            cellProperties[193] = solidTile;
            cellProperties[194] = solidTile;
            cellProperties[195] = solidTile;
            cellProperties[196] = solidTile;
            cellProperties[198] = solidTile;
            cellProperties[201] = waterTile;
            cellProperties[202] = waterTile;
            cellProperties[203] = waterTile;
            cellProperties[204] = waterTile;
            cellProperties[205] = waterTile;

            cellProperties[246] = solidTile;
            cellProperties[249] = waterTile;
            cellProperties[250] = waterTile;
            cellProperties[251] = waterTile;
            cellProperties[252] = waterTile;
            cellProperties[253] = waterTile;
            cellProperties[254] = waterTile;
            cellProperties[255] = waterTile;
            cellProperties[260] = lightForest;
            cellProperties[261] = lightForest;

            cellProperties[294] = solidTile;
            cellProperties[297] = solidTile;
            cellProperties[298] = solidTile;
            cellProperties[299] = solidTile;
            cellProperties[300] = solidTile;
            cellProperties[302] = roughTerrain;
            cellProperties[303] = roughTerrain;
            cellProperties[304] = roughTerrain;
            cellProperties[305] = roughTerrain;
            cellProperties[306] = roughTerrain;
            cellProperties[307] = solidTile;
            cellProperties[308] = roughTerrain;
            cellProperties[309] = roughTerrain;

            for (uint i = 339 ; i <= 350; i++)
            {
                cellProperties[i] = solidTile;
            }

            for (uint i = 385; i <= 398; i++)
            {
                cellProperties[i] = solidTile;
            }
            cellProperties[401] = solidTile;

            cellProperties[433] = solidTile;
            cellProperties[434] = solidTile;
            cellProperties[436] = solidTile;
            cellProperties[437] = solidTile;
            cellProperties[438] = solidTile;
            cellProperties[440] = solidTile;
            cellProperties[441] = solidTile;
            for (uint i = 443; i <= 449; i++)
            {
                cellProperties[i] = solidTile;
            }

            cellProperties[486] = solidTile;
            cellProperties[487] = solidTile;
            cellProperties[488] = solidTile;
            cellProperties[490] = solidTile;
            cellProperties[491] = solidTile;
            cellProperties[493] = solidTile;
            cellProperties[494] = solidTile;
            cellProperties[495] = solidTile;
            cellProperties[496] = solidTile;
            cellProperties[497] = solidTile;
            cellProperties[498] = solidTile;
            cellProperties[499] = solidTile;
            cellProperties[500] = solidTile;
            cellProperties[504] = solidTile;

            cellProperties[530] = solidTile;
            cellProperties[531] = solidTile;
            for (uint i = 533; i <= 540; i++)
            {
                cellProperties[i] = solidTile;
            }
            cellProperties[542] = solidTile;
            cellProperties[543] = solidTile;
            cellProperties[552] = solidTile;

            for (uint i = 577; i <= 588; i++)
            {
                cellProperties[i] = solidTile;
            }
            cellProperties[590] = solidTile;
            cellProperties[597] = solidTile;
            cellProperties[599] = solidTile;
            cellProperties[600] = solidTile;

            for (uint i = 625; i <= 636; i++)
            {
                cellProperties[i] = solidTile;
            }
            for (uint i = 638; i <= 648; i++)
            {
                cellProperties[i] = solidTile;
            }

            cellProperties[673] = rougherTerrain;
            cellProperties[674] = rougherTerrain;
            cellProperties[675] = rougherTerrain;
            cellProperties[676] = solidTile;
            cellProperties[677] = solidTile;
            cellProperties[678] = solidTile;
            cellProperties[679] = solidTile;
            cellProperties[680] = solidTile;
            cellProperties[681] = solidTile;
            cellProperties[682] = solidTile;
            cellProperties[683] = solidTile;
            for (uint i = 685; i <= 691; i++)
            {
                cellProperties[i] = solidTile;
            }

            cellProperties[723] = rougherTerrain;
            cellProperties[727] = solidTile;
            cellProperties[728] = solidTile;
            cellProperties[729] = solidTile;
            for (uint i = 731; i <= 739; i++)
            {
                cellProperties[i] = solidTile;
            }

            for (uint i = 769; i <= 787; i++)
            {
                cellProperties[i] = solidTile;
            }

            for (uint i = 817; i <= 835; i++)
            {
                cellProperties[i] = solidTile;
            }

            for (uint i = 865; i <= 883; i++)
            {
                cellProperties[i] = solidTile;
            }

            for (uint i = 913; i <= 931; i++)
            {
                cellProperties[i] = solidTile;
            }

            for (uint i = 961; i <= 979; i++)
            {
                cellProperties[i] = solidTile;
            }

            for (uint i = 1009; i <= 1027; i++)
            {
                cellProperties[i] = solidTile;
            }
        }

        public static CellProperties GetCellProperties(uint id)
        {
            if (cellProperties.ContainsKey((uint)(id % Global.SpriteSheetCount)))
            {
                return cellProperties[(uint)(id % Global.SpriteSheetCount)];
            }


            return new CellProperties(false, 20);
        }


        public CellProperties(bool isSolid, int difficulty)
        {
            IsSolid = isSolid;
            Difficulty = difficulty;
        }

        public bool IsSolid { get; }
        public int Difficulty { get; }
    }
}
