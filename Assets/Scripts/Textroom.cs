using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Textroom : MonoBehaviour
{
    public int updatesRate = 30;

    int[] data;
    float lastUpdateTime = 0;
    Vector2 lastPlayerPosition;
    Vector2 lastPlayerInput;

    TransmitFrameView playerDataView;
    TransmitFrameView enemyDataView;

    Player player;
    Enemy enemy;

    [DllImport("__Internal")]
    private static extern void JoinRoom(int roomId);

    [DllImport("__Internal")]
    private static extern void TransmitState(int[] data, int numberOfFields);

    void Start()
    {
        data = new int[TransmitFrameView.NumberOfFields * 2];

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();

        playerDataView = new TransmitFrameView(data, 0);
        enemyDataView = new TransmitFrameView(data, TransmitFrameView.NumberOfFields);

        try
        {
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
        if (Time.realtimeSinceStartup < lastUpdateTime + 1f / updatesRate)
        {
            return;
        }

        UpdatePlayerState();
        TransmitState(data, TransmitFrameView.NumberOfFields);
        UpdateEnemyFromState();

        lastUpdateTime = Time.realtimeSinceStartup;
    }

    void UpdatePlayerState()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input = new Vector2(h, v);
        var position = (Vector2)player.transform.position;

        if (position != lastPlayerPosition || input != lastPlayerInput)
        {
            playerDataView.Updated = true;
            playerDataView.Position = position;
            playerDataView.Input = input;

            lastPlayerPosition = position;
            lastPlayerInput = input;
        }
        else
        {
            playerDataView.Updated = false;
        }
    }

    void UpdateEnemyFromState()
    {
        if (enemyDataView.Updated)
        {
            enemy.Sync(enemyDataView.Position, enemyDataView.Input);
            Debug.Log("Sync");
        }
    }
}
