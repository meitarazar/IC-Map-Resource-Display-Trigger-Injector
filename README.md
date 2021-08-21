# IC Map Resource Unveiler
Upgrading IC maps to display resources right from the start of the match!


It works by creating a new entity group that contains all resources and adding triggers to the map's `TRG` file that expose this group for 1 second at the begginig of the match.

### :information_source: How to upgrade a map/folder of maps? :information_source:
1. Press 'Load Map' to load a single map or press 'Select Folder' to load all the maps in a specific folder.
1. Press 'Add Triggers!' and the magic will happen!

### :warning: Unupgradable maps! :warning:
> **Maps have to meet the following requirements in order to be upgradeable:**
- There is no entity group named 'resources'
- There is no trigger named 'resources_on'
- There is no trigger named 'resources_off'

### [Releases Page](https://github.com/meitarazar/IC-Map-Resource-Unveiler/releases)
Download latest version: [IC Map Resource Unveiler v0.1.2](https://github.com/meitarazar/IC-Map-Resource-Unveiler/releases/download/v0.1.2/Pre_Release_v0.1.2.rar)

### [Project License (MIT License)](https://github.com/meitarazar/IC-Map-Resource-Unveiler/blob/master/LICENSE)

### Dependencies
This project uses the [LuaInterface project](https://code.google.com/archive/p/luainterface/) under the [MIT License](https://opensource.org/licenses/MIT)
