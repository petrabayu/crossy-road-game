using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerreainRepeat = 3;

    Dictionary<int, BlockTerrain> map = new Dictionary<int, BlockTerrain>(50);
    TMP_Text gameOverText;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();


        for (int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(grass, z);

        }


        for (int z = 1; z <= frontDistance; z++)
        {
            var prefab = GetNextRandomTerrainPrefab(z);

            CreateTerrain(prefab, z);
        }

        player.SetUp(backDistance, extent);
    }
    private int playerLastMaxTravel;

    private void Update()
    {
        if (player.IsDie && gameOverPanel.activeInHierarchy == false)
            StartCoroutine(ShowGameOverPanel());

        if (player.MaxTravel == playerLastMaxTravel)
            return;

        playerLastMaxTravel = player.MaxTravel;


        var randTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel + frontDistance);

        CreateTerrain(randTbPrefab, player.MaxTravel + frontDistance);

        var lastTB = map[player.MaxTravel - 1 + backDistance];

        map.Remove(player.MaxTravel - 1 + backDistance);
        Destroy(lastTB.gameObject);

        player.SetUp(player.MaxTravel + backDistance, extent);

    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1);

        gameOverText.text = "GAME OVER YOUR SCORE : " + player.MaxTravel;
        gameOverPanel.SetActive(true);
    }

    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab, new Vector3(0, 0, zPos), Quaternion.identity);
        var tb = go.GetComponent<BlockTerrain>();
        tb.Build(extent);

        map.Add(zPos, tb);
        // Debug.Log(map[zPos] is Road);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos - 1];

        for (int distance = 2; distance <= maxSameTerreainRepeat; distance++)
        {
            if (map[nextPos - distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }

        if (isUniform)
        {
            if (tbRef is Grass)
                return road;
            else
                return grass;
        }
        return Random.value > 0.5f ? road : grass;
    }


}
