using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Textroom : MonoBehaviour
{
    int[] data = new int[14];
    TransmitFrameView playerDataView;
    TransmitFrameView enemyDataView;

    volatile Player player;
    volatile Enemy enemy;

    [DllImport("__Internal")]
    private static extern void JoinRoom(int roomId);

    [DllImport("__Internal")]
    private static extern void TransmitState(int[] data, int numberOfFields);

    void Start()
    {
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
        if (Input.GetButtonDown("Jump"))
        {
            enemyDataView.Enabled = true;
            enemyDataView.Position = new Vector2(2, 3);
            enemyDataView.Input = new Vector2(4, 5);

            UpdatePlayerState();
            Debug.Log($"C# player before {playerDataView.Enabled} {playerDataView.Position.x} {playerDataView.Position.y}");
            Debug.Log($"C# enemy before {enemyDataView.Enabled} {enemyDataView.Position.x} {enemyDataView.Position.y}");

            TransmitState(data, TransmitFrameView.NumberOfFields);

            Debug.Log($"C# player after {playerDataView.Enabled} {playerDataView.Position.x} {playerDataView.Position.y}");
            Debug.Log($"C# enemy after {enemyDataView.Enabled} {enemyDataView.Position.x} {enemyDataView.Position.y}");


            for (int i = 0; i < data.Length; i++)
            {
                Debug.Log(data[i]);
            }

            UpdateEnemyFromState();
        }
    }

    void UpdatePlayerState()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input = new Vector2(h, v);
        var position = (Vector2)player.transform.position;

        if (position != playerDataView.Position || input != playerDataView.Input)
        {
            playerDataView.Enabled = true;
            playerDataView.Position = position;
            playerDataView.Input = input;
        }
        else
        {
            playerDataView.Enabled = false;
        }
    }

    void UpdateEnemyFromState()
    {
        enemy.enabled = enemyDataView.Enabled;

        if (enemy.enabled)
        {
            enemy.Sync(enemyDataView.Position, enemyDataView.Input);
        }
    }
}
