using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuildingObject : MonoBehaviour
{
    public PlanetHandler planetHandler;
    [SerializeField] SpriteRenderer topRenderer;
    [SerializeField] SpriteRenderer midRenderer;
    [SerializeField] SpriteRenderer bottomRenderer;
    bool spawnedNext;
    public bool isNarrow;
    private void Start()
    {
        RandomizeStructure();
    }

    public void RandomizeColor()
    {
        Color randomColor = GameManager.Instance.visuals.buildingColors[Random.Range(0, GameManager.Instance.visuals.buildingColors.Length)];
        topRenderer.color = randomColor;
        midRenderer.color = randomColor;
        bottomRenderer.color = randomColor;
    }

    public void SetSortingOrder(int index)
    {
        int realOrder = index * 10;
        topRenderer.sortingOrder = realOrder;
        midRenderer.sortingOrder = realOrder + 1;
        bottomRenderer.sortingOrder = realOrder + 2;
    }

    void RandomizeStructure()
    {
        int topIndex = 0;
        int midIndex = 0;
        int bottomIndex = 0;
        if (isNarrow)
        {
            topIndex = Random.Range(0, GameManager.Instance.visuals.narrowTopBlocks.Length);
            midIndex = Random.Range(0, GameManager.Instance.visuals.narrowMidBlocks.Length);
            bottomIndex = Random.Range(0, GameManager.Instance.visuals.narrowBottomBlocks.Length);
        }
        else
        {
            topIndex = Random.Range(0, GameManager.Instance.visuals.topBlocks.Length);
            midIndex = Random.Range(0, GameManager.Instance.visuals.midBlocks.Length);
            bottomIndex = Random.Range(0, GameManager.Instance.visuals.bottomBlocks.Length);
        }
        
        if (isNarrow)
        {
            topRenderer.sprite = GameManager.Instance.visuals.narrowTopBlocks[topIndex];
            midRenderer.sprite = GameManager.Instance.visuals.narrowMidBlocks[midIndex];
            bottomRenderer.sprite = GameManager.Instance.visuals.narrowBottomBlocks[bottomIndex];
        }
        else
        {
            topRenderer.sprite = GameManager.Instance.visuals.topBlocks[topIndex];
            midRenderer.sprite = GameManager.Instance.visuals.midBlocks[midIndex];
            bottomRenderer.sprite = GameManager.Instance.visuals.bottomBlocks[bottomIndex];
        }
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DestructionZone"))
        {
            if (planetHandler != null)
            {
                planetHandler.TryRemoveBuilding(this);
            }

            Destroy(gameObject);
        }
           
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!spawnedNext)
        {
            if (collision.CompareTag("SpawnZone"))
            {
                if (planetHandler != null)
                {
                    spawnedNext = true;
                    planetHandler.AreaClear();
                }
            }
        }
        
    }


    public void DestroyStructure()
    {
       
        GameManager.Instance.BuildingDestroyed();
        GameObject destroyEffect = Instantiate(GameManager.Instance.destructionParticleEffect, this.transform.position, Quaternion.identity);
        CameraShake.instance.Shake(0.5f, 1f, 1f);
        destroyEffect.transform.parent = null;
        if (planetHandler != null)
        {
            planetHandler.TryRemoveBuilding(this);
        }

        Destroy(this.gameObject);
      
    }

    public void DestroyNoScore()
    {
        GameObject destroyEffect = Instantiate(GameManager.Instance.destructionParticleEffect, this.transform.position, Quaternion.identity);
        destroyEffect.transform.parent = null;
        if (planetHandler != null)
        {
            planetHandler.TryRemoveBuilding(this);
        }

        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        Debug.Log("Building Pressed");
        if (GameManager.Instance.isDebug && ScoreManager.Instance.destroyBuildingsAbility)
        {
            DestroyStructure();
        }
    }
}
