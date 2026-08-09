/*
    This file is part of the MapleQuestAdvisor planning tool
    Copyleft (L) 2026 RonanLana

    GNU General Public License v3.0

    Permissions of this strong copyleft license are conditioned on making available complete
    source code of licensed works and modifications, which include larger works using a licensed
    work, under the same license. Copyright and license notices must be preserved. Contributors
    provide an express grant of patent rights.
*/

#include <lua.hpp>

#if LUA_VERSION_NUM == 501
#define luaL_newlib(L,l) (lua_newtable(L), luaL_register(L, NULL, l))
#endif

// Declare the signature of the exported C# function
extern "C" __declspec(dllimport) int cs_extract_bitmap(char *maplePath, char *filePath, char *wzPath);

static int c_extract_bitmap(lua_State *L) {
    char *maplePath = (char *)lua_tostring(L, 1);
    char *filePath = (char *)lua_tostring(L, 2);
    char *wzPath = (char *)lua_tostring(L, 3);

    cs_extract_bitmap(maplePath, filePath, wzPath);
    return 0;
}

// Define array of functions mapping Lua names to C functions
static const struct luaL_Reg WzBmp[] = {
    {"c_extract_bitmap", c_extract_bitmap},
    {NULL, NULL}  /* sentinel */
};

// Main entry point when requiring the module.
extern "C" __declspec(dllexport) int luaopen_WzBmp(lua_State *L) {
    luaL_newlib(L, WzBmp);
    return 1;
}
