* Сохранять все полученные от сервера сообщения и позволить клиенту самому решить как их обработать. 
    - прыжок может быть нажат в одном из пакетов, который будет отброшен
    - за один кадр может прийти несколько пакетов из-за задержки
* Использовать экстраполяцию врага, но ! дорисовывать промежуточные кадры если он слишком быстро перемещается, как в кинематографической съёмке, может быть текущий кадр тоже полупрозрачным
* Добавить комнаты. Соединять в signal только с одинаковыми комнатами. Передавать через get параметр
* Добавить в список stun и turn серверов гугловские и прочие на всякий случай
* Закодировать состояния типа bool через один бит. Так большую часть данных можно передать одним битом. Возможно перейти на передачу данных на int и float передавать в int
---
* Установить плагины аналитики Unity проекта
* Перейти на Burst компилятор
* Перейти на ECM или как его
* При передаче сообщений использовать ArrayBufferView чтобы избежать копирования
* Сменить тип компрессии с gzip на бротли
---
Build speed
-rw-------  1 kukuruku  LD\Domain Users   2.7M Nov  5 02:56 Build.data.br
-rw-------  1 kukuruku  LD\Domain Users    67K Nov  5 02:58 Build.framework.js.br
-rw-r--r--  1 kukuruku  LD\Domain Users    20K Nov  5 02:54 Build.loader.js
-rw-------  1 kukuruku  LD\Domain Users   4.8M Nov  5 02:58 Build.wasm.br

Build size
drwxr-xr-x  6 kukuruku  LD\Domain Users   192B Nov  5 03:08 .
drwxr-xr-x  5 kukuruku  LD\Domain Users   160B Nov  5 02:54 ..
-rw-------  1 kukuruku  LD\Domain Users   2.7M Nov  5 02:56 Build.data.br
-rw-------  1 kukuruku  LD\Domain Users    67K Nov  5 03:08 Build.framework.js.br
-rw-r--r--  1 kukuruku  LD\Domain Users    20K Nov  5 02:54 Build.loader.js
-rw-------  1 kukuruku  LD\Domain Users   4.5M Nov  5 03:09 Build.wasm.br
