(function () {
    let isInRoom = false;
    let received;

    const textroom = new window.Textroom({
        onJoin: (user) => {
            console.log('Joined ', user.username);
        },
        onLeave: (user) => {
            console.log('Left ', user.username);
        },
        onMessage: function (text) {
            window.ReceiveState(text);
        },
    });

    const connectedThen = textroom.connect([
        'ws://51.250.64.16:8188/',
        'http://51.250.64.16:8088/janus'
    ]);

    window.JoinRoom = function (roomId) {
        connectedThen.then(() => {
            return textroom.join(roomId);
        }).then(() => {
            isInRoom = true;
        });
    };

    window.ReceiveState = function (text, user) {
        received = text;
    };

    window.TransmitState = function (HEAP32, offset, numberOfFields) {
        if (!isInRoom) {
            return;
        }

        const player = new Int32Array(HEAP32.buffer, offset, numberOfFields);
        const enemy = new Int32Array(HEAP32.buffer, offset + (numberOfFields << 2), numberOfFields);

        /**
         * Send
         */
        if (player[0]) {
            textroom.message(player.toString());

            console.log('sent ', player.toString())
        }

        if (received) {
            enemy.set(received.split(','));
            received = undefined;

            console.log('received ', enemy.toString())
        } else {
            enemy[0] = 0;
        }
    };
})();
