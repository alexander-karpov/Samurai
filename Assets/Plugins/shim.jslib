mergeInto(LibraryManager.library, {
    JoinRoom: function (roomId) {
        window.JoinRoom(roomId);
    },
    TransmitState: function (offset, numberOfFields) {
        window.TransmitState(HEAP32, offset, numberOfFields);
    },
});