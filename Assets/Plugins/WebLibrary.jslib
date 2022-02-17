mergeInto(LibraryManager.library, {
    Request_AccoutClear : function()
    {
        window.accounts = undefined;
    },

    Request_IsAliveWeb3 : function()
    {
        return window.web3 ? true : false;
    },

    Request_IsExistAccountID : function()
    {
        return window.accounts ? true : false;
    },

    Request_OnAlert: function (str) {
        window.alert(Pointer_stringify(str));
    },

    Request_GetAccountID : function()
    {
        var returnStr = window.accounts;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },

    PrintFloatArray: function (array, size) {
        for(var i = 0; i < size; i++)
        console.log(HEAPF32[(array >> 2) + i]);
    },

    AddNumbers: function (x, y) {
        return x + y;
    },

});