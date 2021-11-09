using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrashFight : MonoBehaviour
{
    public Transform player;
    public TilemapRenderer tilemapRenderer;
    public TilemapCollider2D tilemapCollider2D;

    private bool started;
    private float timeEntered;

    public Transform[] spawnPoints;
    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); }
        if (tilemapRenderer == null) { tilemapRenderer = this.GetComponent<TilemapRenderer>(); }
        if (tilemapCollider2D == null) { tilemapCollider2D = this.GetComponent<TilemapCollider2D>(); }

        // Debug.Log(spawnPoints.Length);
        // Debug.Log(enemies.Length);

        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (started && Time.time >= timeEntered + 0.2f)
        {
            GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (remainingEnemies.Length == 0)
            {
                End();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player.position.x > 45f && player.position.x < 50f)
        {
            timeEntered = Time.time;
            started = true;
            tilemapCollider2D.isTrigger = false;
            tilemapRenderer.enabled = true;

            // foreach (Transform spawnPoint in spawnPoints)
            // {
            //     int r = Random.Range(0, enemies.Length);
            //     Instantiate(enemies[r], spawnPoint.position, Quaternion.identity);
            //     Debug.Log(1);
            // }
        }
        else if (player.position.x > 82f)
        {
            PlayerLeft();
        }
    }

    private void End()
    {
        started = false;
        tilemapCollider2D.isTrigger = true;
        tilemapRenderer.enabled = false;
    }

    private void PlayerLeft()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Hijack();
    }
}
