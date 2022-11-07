using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class RealtimeController : MonoBehaviour
{
    float[] data = new float[14];

    MoveController player;
    MoveController enemy;
    float lastReceivedVersion;

    [DllImport("__Internal")]
    private static extern void TransmitRealtimeData(float[] array, int size);

    bool isOnline = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<MoveController>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<MoveController>();

        // Длинна массива для обмена данными должна быть чётной
        Debug.Assert(data.Length % 2 == 0);

        try
        {
            TransmitRealtimeData(data, data.Length);
            isOnline = true;
        }
        catch
        {
            Debug.LogWarning("RealtimeController disabled");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerPosition = player.transform.position;

        var direction = Input.GetAxisRaw("Horizontal");
        var jumpDown = Input.GetButtonDown("Jump") ? 1 : 0;
        var jumpUp = Input.GetButtonUp("Jump") ? 1 : 0; ;
        var fire = Input.GetButtonDown("Fire1") ? 1 : 0;

        if (!isOnline)
        {
            player.Move(direction, fire > 0, jumpDown > 0, jumpUp > 0);
            enemy.Move(0, true, false, false);
            return;
        }

        if (jumpDown != data[5] ||
            jumpUp != data[6] ||
            direction != data[3] ||
            fire != data[4] ||
            playerPosition.x != data[1] ||
            playerPosition.y != data[2])
        {
            data[0] = Time.frameCount;
            data[1] = playerPosition.x;
            data[2] = playerPosition.y;
            data[3] = direction;
            data[4] = fire;
            data[5] = jumpDown;
            data[6] = jumpUp;

        }

        var receiveOffset = data.Length / 2;

        TransmitRealtimeData(data, receiveOffset);

        var receivedVersion = data[receiveOffset];

        if (receivedVersion > lastReceivedVersion)
        {
            lastReceivedVersion = receivedVersion;
            enemy.FixPosition(data[receiveOffset + 1], data[receiveOffset + 2]);
        }

        player.Move(direction, fire > 0, jumpDown > 0, jumpUp > 0);
        enemy.Move(data[receiveOffset + 3], data[receiveOffset + 4] > 0, data[receiveOffset + 5] > 0, data[receiveOffset + 6] > 0);
    }
}
