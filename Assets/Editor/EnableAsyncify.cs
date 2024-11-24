using UnityEditor;

public class EnableAsyncify
{
    [InitializeOnLoadMethod]
    static void AddAsyncifyFlags()
    {
        // Enable Asyncify
        PlayerSettings.WebGL.emscriptenArgs = "-s ASYNCIFY";

        // Correctly specify Asyncify imports (as an array, no quotes around the list)
        PlayerSettings.WebGL.emscriptenArgs += " -s ASYNCIFY_IMPORTS=[JS_SaveGameToIndexedDB,JS_LoadSaveSlotFromIndexedDB,JS_DeleteSaveFromIndexedDB,JS_SaveStateExistsInIndexedDB]";

        // Add optional memory adjustment if needed
        PlayerSettings.WebGL.emscriptenArgs += " -s TOTAL_STACK=5242880";

        UnityEngine.Debug.Log("Asyncify flags with proper imports added to WebGL build.");
    }
}