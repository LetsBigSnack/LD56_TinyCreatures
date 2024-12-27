mergeInto(LibraryManager.library, {
    JS_SaveGameToIndexedDB: function (jsonStringPtr, slotIndex) {
        return Asyncify.handleAsync(() => {
            const jsonString = UTF8ToString(jsonStringPtr);
            return new Promise((resolve, reject) => {
                const request = indexedDB.open("MonkepokSaveData", 1);

                request.onupgradeneeded = function () {
                    console.log("Upgrading IndexedDB...");
                    const db = request.result;
                    if (!db.objectStoreNames.contains("saves")) {
                        db.createObjectStore("saves");
                        console.log("'saves' object store created.");
                    }
                };

                request.onsuccess = function () {
                    console.log("IndexedDB opened successfully.");
                    const db = request.result;
                    const transaction = db.transaction(["saves"], "readwrite");
                    const store = transaction.objectStore("saves");
                    const key = `SaveSlot${slotIndex}`;
                    const putRequest = store.put(jsonString, key);

                    putRequest.onsuccess = function () {
                        console.log(`Saved data to key: ${key}`);
                        resolve();
                    };

                    putRequest.onerror = function (event) {
                        console.error(`Put request error: ${event.target.error}`);
                        reject(event.target.error);
                    };
                };

                request.onerror = function (event) {
                    console.error("Error opening IndexedDB:", event.target.error);
                    reject(event.target.error);
                };
            });
        });
    },

    JS_LoadSaveSlotFromIndexedDB: function (slotIndex) {
        return Asyncify.handleAsync(() => {
            return new Promise((resolve, reject) => {
                const request = indexedDB.open("MonkepokSaveData", 1);

                request.onsuccess = function () {
                    const db = request.result;

                    if (!db.objectStoreNames.contains("saves")) {
                        console.log("'saves' object store does not exist.");
                        resolve(null);
                        return;
                    }

                    const transaction = db.transaction(["saves"], "readonly");
                    const store = transaction.objectStore("saves");
                    const key = `SaveSlot${slotIndex}`;
                    const getRequest = store.get(key);

                    getRequest.onsuccess = function () {
                        if (getRequest.result) {
                            console.log(`Loaded data for key: ${key}`);
                            resolve(allocateUTF8(getRequest.result));
                        } else {
                            console.log(`No data found for key: ${key}`);
                            resolve(0); // Null or no data
                        }
                    };

                    getRequest.onerror = function (event) {
                        console.error(`Get request error: ${event.target.error}`);
                        reject(event.target.error);
                    };
                };

                request.onerror = function (event) {
                    console.error("Error opening IndexedDB:", event.target.error);
                    reject(event.target.error);
                };
            });
        });
    },

    JS_DeleteSaveFromIndexedDB: function (slotIndex) {
        return Asyncify.handleAsync(() => {
            return new Promise((resolve, reject) => {
                const request = indexedDB.open("MonkepokSaveData", 1);

                request.onsuccess = function () {
                    const db = request.result;

                    if (!db.objectStoreNames.contains("saves")) {
                        console.log("'saves' object store does not exist.");
                        resolve(false);
                        return;
                    }

                    const transaction = db.transaction(["saves"], "readwrite");
                    const store = transaction.objectStore("saves");
                    const key = `SaveSlot${slotIndex}`;
                    const deleteRequest = store.delete(key);

                    deleteRequest.onsuccess = function () {
                        console.log(`Deleted data for key: ${key}`);
                        resolve(true);
                    };

                    deleteRequest.onerror = function (event) {
                        console.error(`Delete request error: ${event.target.error}`);
                        reject(event.target.error);
                    };
                };

                request.onerror = function (event) {
                    console.error("Error opening IndexedDB:", event.target.error);
                    reject(event.target.error);
                };
            });
        });
    },

    JS_SaveStateExistsInIndexedDB: function (slotIndex) {
        return Asyncify.handleAsync(() => {
            return new Promise((resolve) => {
                const request = indexedDB.open("MonkepokSaveData", 1);

                request.onupgradeneeded = function () {
                    console.log("Upgrading IndexedDB...");
                    const db = request.result;
                    if (!db.objectStoreNames.contains("saves")) {
                        db.createObjectStore("saves");
                        console.log("'saves' object store created.");
                    }
                };

                request.onsuccess = function () {
                    const db = request.result;

                    if (!db.objectStoreNames.contains("saves")) {
                        console.log("'saves' object store does not exist.");
                        resolve(0); // Save state does not exist
                        return;
                    }

                    const transaction = db.transaction(["saves"], "readonly");
                    const store = transaction.objectStore("saves");
                    const key = `SaveSlot${slotIndex}`;
                    const getRequest = store.get(key);

                    getRequest.onsuccess = function () {
                        if (getRequest.result) {
                            console.log(`Save state exists for key: ${key}`);
                            resolve(1); // Exists
                        } else {
                            console.log(`Save state does not exist for key: ${key}`);
                            resolve(0); // Does not exist
                        }
                    };

                    getRequest.onerror = function () {
                        console.error("Error retrieving save data.");
                        resolve(0); // Default to "does not exist" on error
                    };
                };

                request.onerror = function () {
                    console.error("Error opening IndexedDB.");
                    resolve(0); // Default to "does not exist" on error
                };
            });
        });
    }
});
