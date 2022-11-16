class TextroomClient {
    constructor(HEAP32, SendMessage) {
        this.HEAP32 = HEAP32;
        this.SendMessage = SendMessage;
        this.receivedText = undefined;
        this.lastSentVersion = 0;
        this.opponentUsername = undefined;

        this.textroom = new window.Textroom({
            onJoin: (user) => {
                this.OnOpponentJoined(user.username);
            },
            onLeave: (user) => {
                this.OnOpponentLeave(user.username);
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

    Emit(eventName) {
        this.SendMessage('Textroom', 'OnEvent', eventName);
    }

    JoinRoom() {
        this.connectedThen.then(() => {
            return this.textroom.roomsList();
        }).then((rooms) => {
            const onePlayerRooms = rooms.filter((r) => r.num_participants === 1);
            const zeroPlayersRooms = rooms.filter((r) => r.num_participants === 0);
            const rooms_ = onePlayerRooms.length ? onePlayerRooms : zeroPlayersRooms;

            if (rooms_.length === 0) {
                throw new Error('No suitable rooms');
            }

            const randomRoom = rooms_[Math.floor(Math.random() * rooms_.length)];

            return this.textroom.join(randomRoom.room).then((participants) => {
                return {
                    room: randomRoom.room,
                    participants: participants,
                }
            });
        }).then(({ room, participants }) => {
            console.log(`I joined the room ${room}. Participants: ${participants.length}`);

            if (participants.length === 1) {
                this.opponentUsername = participants[0].username;
                this.Emit('Start');
            }

            if (participants.length > 1) {
                console.log(`Too many opponents. Leave…`);
                this.textroom.leave().then(() => {
                    return this.JoinRoom();
                });
            }
        }).catch((error) => {
            setTimeout(() => this.JoinRoom(), 3000);

            console.error(error, 'Retry…');
        });
    };

    OnOpponentJoined(username) {
        console.log(`Opponent joined. ${username}`);

        if (this.opponentUsername) {
            console.log(`Unnecessary`);

            return;
        }

        this.opponentUsername = username;

        this.Emit('Start');
    }

    OnOpponentLeave(username) {
        console.log(`Opponent left. ${username}`);

        if (this.opponentUsername === username) {
            console.log(`No opponents. Leave…`);

            this.opponentUsername = undefined;
            this.Emit('Wait');

            this.textroom.leave().then(() => {
                return this.JoinRoom();
            });
        }
    }

    TransmitState(offset, numberOfFields) {
        const player = new Int32Array(this.HEAP32.buffer, offset, numberOfFields);
        const enemy = new Int32Array(this.HEAP32.buffer, offset + (numberOfFields << 2), numberOfFields);

        /**
         * Send
         */
        if (player[0] > this.lastSentVersion) {
            this.lastSentVersion = player[0];
            const message = player.toString();

            this.textroom.message(message);

            console.log('Send input', message);
        }

        /**
         * Receive
         */
        if (this.receivedText) {
            enemy.set(this.receivedText.split(','));

            console.log('Receive input', this.receivedText);

            this.receivedText = undefined;
        }
    };
};
