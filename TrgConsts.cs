using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapUpgrader
{
    class TrgConsts
    {
        public const string trigger1Id = "%ID1%";
        public const string trigger2Id = "%ID2%";
        public const string triggerLabId = "%ID_LAB%";
        public const string triggerResGrpId = "%ID_RES_GRP%";
        public const string triggersPath = "%TRG_PATH%";

        public const string triggerFolder = "resource_control";
        public const string triggersFoldersTable = @"-- tool:%TRG_PATH% --

-- list of all trigger folders
folders = {
   ""default"",
   """ + triggerFolder + @""",
}
default = {
}";
        public const string resourcesTriggers = triggerFolder + @" = {
   {
      id = %ID1%,
      name = ""resources_on"",
      pset = 16,
      active = 1,
      looping = 0,
      conditions = {
         {
            type = ""Player is not restricted to"",
            args = {
               {type = 13, val = %ID_LAB%},
            }
         },
      },
      actions = {
         {
            type = ""GroupVisible in FOW"",
            args = {
               {type = 12, val = %ID_RES_GRP%},
               {type = 3, val = 4},
               {type = 10, val = 1},
            }
         },
      },
   },
   {
      id = %ID2%,
      name = ""resources_off"",
      pset = 16,
      active = 1,
      looping = 0,
      conditions = {
         {
            type = ""GameTime"",
            args = {
               {type = 0, val = 1},
            }
         },
         {
            type = ""Player is not restricted to"",
            args = {
               {type = 13, val = %ID_LAB%},
            }
         },
      },
      actions = {
         {
            type = ""GroupVisible in FOW"",
            args = {
               {type = 12, val = %ID_RES_GRP%},
               {type = 3, val = 4},
               {type = 10, val = 0},
            }
         },
      },
   },
}";
    }
}
