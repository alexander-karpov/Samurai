using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Textroom : MonoBehaviour
{
    public int updatesRate = 30;

    int[] transmissionBuffer;
    Vector2 lastPlayerInput;
    int lastSentVersion = 0;
    int lastReceivedVersion = 0;

    TransmitFrameView playerDataView;
    TransmitFrameView enemyDataView;

    Player player;
    Enemy enemy;

    [DllImport("__Internal")]
    private static extern void Init();

    [DllImport("__Internal")]
    private static extern void JoinRoom(int roomId);

    [DllImport("__Internal")]
    private static extern void TransmitState(int[] data, int numberOfFields);

    void Start()
    {
        transmissionBuffer = new int[TransmitFrameView.NumberOfFields * 2];
        playerDataView = new TransmitFrameView(transmissionBuffer, 0);
        enemyDataView = new TransmitFrameView(transmissionBuffer, TransmitFrameView.NumberOfFields);

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();

        try
        {
            Init();
            JoinRoom(1001);
        }
        catch
        {
            enabled = false;
            Debug.LogWarning("Textroom disabled");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WritePlayerStateToBuffer();
        TransmitState(transmissionBuffer, TransmitFrameView.NumberOfFields);
        ReadEnemyStateFromBuffer();
    }

    void OnEvent(string payload)
    {
        Debug.Log($"OnEvent payload {payload}");
    }

    void WritePlayerStateToBuffer()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input = new Vector2(h, v);

        if (input != lastPlayerInput)
        {
            lastPlayerInput = input;
            lastSentVersion += 1;

            playerDataView.Version = lastSentVersion;
            playerDataView.Position = player.transform.position;
            playerDataView.Input = input;
        }
    }

    void ReadEnemyStateFromBuffer()
    {
        if (enemyDataView.Version > lastReceivedVersion)
        {
            lastReceivedVersion = enemyDataView.Version;

            enemy.Sync(enemyDataView.Position, enemyDataView.Input);
        }
    }
}
