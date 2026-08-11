--[[
    This file is part of the MapleQuestAdvisor planning tool
    Copyleft (L) 2026 RonanLana

    GNU General Public License v3.0

    Permissions of this strong copyleft license are conditioned on making available complete
    source code of licensed works and modifications, which include larger works using a licensed
    work, under the same license. Copyright and license notices must be preserved. Contributors
    provide an express grant of patent rights.
]]--

WzBmp = require("WzBmp")

local msBaseDir = "C:/Nexon/MapleStory"
local imgFolderPath = "C:/Nexon/MQA-LibInstaller/images"
local xmlFolderPath = "C:/Nexon/MQA-LibInstaller/xml"

-- BITMAP section
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Map.wz/MapHelper.img/worldMap/*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Map.wz/WorldMap/*.img/*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/Basic.img/Notice3.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/ITC.img/MyPage.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/FloatNotice.0.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/QuestIcon.*.0")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/Quest.backgrnd5")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/Quest.reward")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/Basic.img/ComboBox.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/Basic.img/HScr4.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/Basic.img/BtClose.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/ExceptionItemSearch/BtSearch.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/ExceptionItemSearch/BtSave.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/GuildBBS.img/GuildBBS/BtRetouch.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/ExceptionItemSearch/BtDelete.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/Basic.img/BtUP.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/EntrustedShop/BtArrange2.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/StatusBar.img/EquipKey.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/StatusBar.img/SkillKey.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/StatusBar.img/StatKey.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/itemSearch/BtGo.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/itemSearch/BtBack.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/Basic.img/Cursor.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/EnergyBar.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/Title.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/Stat.backgrnd")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/Item.shadow")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/Basic.img/Tab2.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/Item.backgrnd")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/Item.Tab.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/UIWindow.img/UtilDlgEx.bar")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/Basic.img/ItemNo.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "UI.wz/Basic.img/ItemNo.*")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Item.wz/Consume/*.img/\\w+.info.iconRaw")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Item.wz/Install/*.img/\\w+.info.iconRaw")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Item.wz/Etc/*.img/\\w+.info.iconRaw")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Item.wz/Cash/*.img/\\w+.info.iconRaw")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Character.wz/*/*.img/info.iconRaw")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Mob.wz/*.img/stand.0")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Mob.wz/*.img/fly.0")
WzBmp.c_extract_bitmap(msBaseDir, imgFolderPath, "Npc.wz/*.img/stand.0")

-- XML section
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "Item.wz/*/*")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "Map.wz/WorldMap/*")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "Map.wz/MapHelper.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "Mob.wz/*")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "Npc.wz/*")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "Quest.wz/Act.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "Quest.wz/Check.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "Quest.wz/QuestInfo.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "String.wz/Cash.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "String.wz/Consume.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "String.wz/Eqp.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "String.wz/Etc.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "String.wz/Ins.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "String.wz/Map.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "String.wz/Mob.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "String.wz/Npc.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "UI.wz/Basic.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "UI.wz/GuildBBS.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "UI.wz/ITC.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "UI.wz/StatusBar.img")
WzBmp.c_extract_xml(msBaseDir, xmlFolderPath, "UI.wz/UIWindow.img")
