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

        public const string triggerFolderRes = "resource_control";
        public const string foldersTableWithRes = @"-- tool:%TRG_PATH% --

-- list of all trigger folders
folders = {
   ""default"",
   """ + triggerFolderRes + @""",
}
default = {
}";
        public const string triggersTableRes = triggerFolderRes + @" = {
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

        public const string triggerAmbientId = "%ID_AMBNT%";
        public const string triggerBattleId = "%ID_BTLL%";

        public const string triggerFolderMusic = "music_control";
        public const string foldersTableWithMusic = @"-- tool:%TRG_PATH% --

-- list of all trigger folders
folders = {
   ""default"",
   """ + triggerFolderMusic + @""",
}
default = {
}";
        public const string triggersTableMusic = triggerFolderMusic + @" = {
   {
      id = 0,
      name = ""ambient_music"",
      pset = 512,
      active = 1,
      looping = 0,
      conditions = {
         {
            type = ""Always"",
            args = {}
         },
      },
      actions = {
         {
            type = ""Play Music"",
            args = {
               {type = 9, val = %ID_AMBNT%},
            }
         },
      },
   },
   {
      id = 1,
      name = ""battle_track"",
      pset = 16,
      active = 1,
      looping = 0,
      conditions = {
         {
            type = ""Always"",
            args = {}
         },
      },
      actions = {
         {
            type = ""Set battle track"",
            args = {
               {type = 9, val = %ID_BTLL%},
            }
         },
      },
   },
}";
    }
}
