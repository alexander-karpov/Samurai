class TextroomClient {
    constructor(HEAP32, SendMessage) {
        this.HEAP32 = HEAP32;
        this.SendMessage = SendMessage;
        this.inRoom = 0;
        this.receivedText = undefined;
        this.lastSentVersion = 0;

        this.textroom = new window.Textroom({
            onJoin: (user) => {
                console.log('Joined ', user.username);
            },
            onLeave: (user) => {
                console.log('Left ', user.username);
            },
            onMessage: (text) => {
                this.receivedText = text;
            },
        });

        this.connectedThen = this.textroom.connect([
            'ws://51.250.64.16:8188/',
            'http://51.250.64.16:8088/janus'
        ]);
    }

    JoinRoom(roomId) {
        return this.connectedThen.then(() => {
            return this.textroom.join(roomId);
        }).then(() => {
            this.inRoom = roomId;
            this.SendMessage('Textroom', 'OnEvent', 'JoinRoom');
        });
    };

    TransmitState(offset, numberOfFields) {
        if (!this.inRoom) {
            return;
        }

        const player = new Int32Array(this.HEAP32.buffer, offset, numberOfFields);
        const enemy = new Int32Array(this.HEAP32.buffer, offset + (numberOfFields << 2), numberOfFields);

        /**
         * Send
         */
        if (player[0] > this.lastSentVersion) {
            this.lastSentVersion = player[0];

            this.textroom.message(player.toString());
        }

        /**
         * Receive
         */
        if (this.receivedText) {
            enemy.set(this.receivedText.split(','));
            this.receivedText = undefined;
        }
    };
};
