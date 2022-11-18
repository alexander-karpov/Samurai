mergeInto(LibraryManager.library, {
    Init: function () {
        return window.textrooom = new window.TextroomClient(HEAP32, SendMessage);
    },
    Join: function () {
        return window.textrooom.JoinRoom();
    },
    Send: function (offset, length) {
        return window.textrooom.Send(offset, length);
    },
    Receive: function (offset, length) {
        return window.textrooom.Receive(offset, length);
    },
});