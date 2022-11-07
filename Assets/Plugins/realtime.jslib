/**
 * В первом поле массива передаём версию пакета.
 * При передаче учитываем это и не отправляем пакет
 * если не было обновления версии (для экономии)
 * Версия полученного пакета обрабатывается в C# коде
 */
mergeInto(LibraryManager.library, {
    realtime: undefined,
    TransmitRealtimeData: function (array, size) {
        if (window.debugTransmitRealtimeData) {
            if (window.debugTransmitRealtimeData(HEAPF32, array, size)) {
                return;
            }
        }

        var start = array >> 2;

        if (!this.realtime) {
            this.realtime = new window.Realtime();
            this.realtime.connect();

            this.sendData = new Float32Array(size);
        }

        if (HEAPF32[start] > this.sendData[0]) {
            for (var i = 0; i < size; i++) {
                this.sendData[i] = HEAPF32[start + i];
            }

            this.realtime.send(this.sendData);
        }

        var received = this.realtime.read();

        if (received) {
            const receivedFloat = new Float32Array(received.buffer);

            for (var i = 0; i < size; i++)
                HEAPF32[start + size + i] = receivedFloat[i];
        }
    }
});