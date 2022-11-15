(function () {
    const textroom = new window.Textroom({
        onJoin: async (user) => {
            console.log('Joined ', user.username);
        },
        onLeave: async (user) => {
            console.log('Left ', user.username);
        },
        onMessage: async (text, user) => {
            console.log(user, text);
        }
    });

    const connectedThen = textroom.connect([
        'ws://51.250.64.16:8188/',
        'http://51.250.64.16:8088/janus'
    ]);

    let isInRoom = false;

    window.JoinRoom = function (roomId) {
        connectedThen.then(() => {
            return textroom.join(roomId);
        }).then(() => {
            isInRoom = true;
        });
    };

    window.TransmitState = function (HEAP32, offset, numberOfFields) {
        if (!isInRoom) {
            return;
        }

        player = offset >> 2;
        enemy = (offset >> 2) + numberOfFields;

        for (let i = 0; i < numberOfFields; i++) {
            HEAP32[enemy + i] = HEAP32[player + i];
        }
    };
})();


// mergeInto(LibraryManager.library, {
//     InitializeRealtime: function (size) {
//         window.RealtimeClient = {
//             /**
//              * Индексы в массивах данных
//              */
//             version: 0,
//             x: 1,
//             y: 2,
//             flagsFrom: 3,
//             /**
//              * @param size Длина массива для передачи и получения
//              */
//             InitializeRealtime: function (size) {
//                 this.size = size;
//                 this.sendData = new Float32Array(size);
//                 this.receivedData = new Float32Array(size);

//                 console.log('this', this);

//                 this.realtime = new window.Realtime((function (data) {
//                     this._receive(data);
//                 }).bind(this));

//                 this.realtime.connect();
//             },

//             TransmitRealtimeData: function (array) {
//                 if (window.debugTransmitRealtimeData) {
//                     if (window.debugTransmitRealtimeData.bind(this)(HEAPF32, array, size)) {
//                         return;
//                     }
//                 }

//                 const start = array >> 2;

//                 this._send(HEAPF32, start);
//                 this._flushReceived(HEAPF32, start);
//             },

//             /**
//              * Обновляет сохранённые полученные данные
//              * @param data Новые полученные данные
//              */
//             _receive(data) {
//                 const newData = new Float32Array(data.buffer);

//                 // Копируем координаты если версия больше текущей
//                 if (newData[this.version] > this.receivedData[this.version]) {
//                     this.receivedData[this.version] = newData[this.version];
//                     this.receivedData[this.x] = newData[this.x];
//                     this.receivedData[this.y] = newData[this.y];

//                     console.log('receive ', newData);
//                 }

//                 // Остальные поля складываем по принципу OR
//                 // (если в одном из пакетов был сигнал, его надо обработать)
//                 for (let i = this.flagsFrom; i < this.size; i++) {
//                     if (newData[i] == 0) {
//                         this.receivedData[i] = newData[i];
//                     }
//                 }
//             },

//             /**
//              * Копирует полученные данные в память wasm и очищает накопитель
//              * @param heap Общая память wasm
//              * @param start Начало наших данных
//              */
//             _flushReceived(heap, start) {
//                 heap.set(this.receivedData, start + this.size);
//                 this.receivedData.fill(0);
//             },

//             /**
//              * Сразу отправляет данные
//              * @param heap Общая память wasm
//              * @param start Начало наших данных
//              */
//             _send(heap, start) {
//                 if (heap[start + this.version] <= this.sendData[this.version]) {
//                     return;
//                 }

//                 for (let i = 0; i < size; i++) {
//                     this.sendData[i] = HEAPF32[start + i];
//                 }

//                 console.log('send ', this.sendData);

//                 this.realtime.send(this.sendData);
//             }
//         };

//         window.RealtimeClient.InitializeRealtime(size);
//     },

//     TransmitRealtimeData: function (array) {
//         window.RealtimeClient.TransmitRealtimeData(array);
//     },
// });

