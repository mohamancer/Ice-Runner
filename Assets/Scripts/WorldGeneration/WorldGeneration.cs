using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    // Gameplay
    private float chunkSpawnZ;
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();

    // Configurable fields
    [SerializeField] private int firstChunkPosition = 5;
    [SerializeField] private int chunkOnScreen = 5;
    [SerializeField] private float despawnDistance = 5.0f;

    [SerializeField] private List<GameObject> chunkPrefab;
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        ResetWorld();   
    }

    private void Start()
    {
        // Check if we have an empty chunkPrefab list 
        if (chunkPrefab.Count == 0)
        {
            Debug.Log("Error");
            return;
        }
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("Assign it automatically to main");
        }

        //try to assign the cameraTransform
    }
    public void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z;
        Chunk lastChunk = activeChunks.Peek();

        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLength + despawnDistance) {
            SpawnNewChunk();
            DeleteLastChunk();
        }
    }

    private void SpawnNewChunk() 
    {
        // Get a random index from the prefabs
        int randomIndex = Random.Range(0, chunkPrefab.Count);
        // if it's within the pool reuse it

        Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == 
                                            (chunkPrefab[randomIndex].name + "(Clone)"));
        // else create it
        if (!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }
        // place it in the pool and show it
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += chunk.chunkLength;

        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();

    }

    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunks.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }

    public void ResetWorld()
    {
        // Reset chunkSpawnZ and spawn chunkOnScreen chunks
        chunkSpawnZ = firstChunkPosition;

        for (int i = activeChunks.Count; i!= 0; i--)
            DeleteLastChunk();

        for (int i = 0; i < chunkOnScreen; i++)
            SpawnNewChunk();
        

    }

}
