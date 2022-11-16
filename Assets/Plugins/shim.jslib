mergeInto(LibraryManager.library, {
    Init: function () {
        window.textrooom = new window.TextroomClient(HEAP32, SendMessage);
    },
    JoinRoom: function (roomId) {
        window.textrooom.JoinRoom(roomId);
    },
    TransmitState: function (offset, numberOfFields) {
        window.textrooom.TransmitState(offset, numberOfFields);
    },
});