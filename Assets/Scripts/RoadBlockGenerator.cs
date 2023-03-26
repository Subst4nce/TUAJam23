using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockGenerator : MonoBehaviour
{
    public static RoadBlockGenerator Instance { get; private set; }

    public RoadBlock[] availableBlocks;

    public int blocksToGenAhead = 3;
    public int blocksToLeaveBehind = 3;

    int blocksCrossed = 0;

    public List<RoadBlock> blocks;


    private void Awake()
    {
        if (blocksToGenAhead > blocksToLeaveBehind)
            blocksToLeaveBehind = blocksToGenAhead;

        Instance = this;
        blocks = new List<RoadBlock>();
    }

    private void Start()
    {
        for (int i = 0; i < blocksToGenAhead; i++)
        {
            GenerateBlockAhead();
        }
    }

    public void BlockCrossed(RoadBlock block)
    {
        GenerateBlockAhead();

        blocksCrossed++;
        if (blocksCrossed >= blocksToLeaveBehind)
        {
            //Start leaving deleting after leaving behind X blocks
            DeleteRoadBlock(blocks[0]);
            blocks.RemoveAt(0);
        }
    }

    private RoadBlock GenerateBlockAhead()
    {
        RoadBlock block;
        if (blocks.Count == 0)
        {
            block = GenerateRoadBlock(Vector3.zero);
        }
        else
        {
            block = GenerateRoadBlock(blocks[blocks.Count - 1].endConnector.position);
        }
        blocks.Add(block);
        return block;
    }
    private RoadBlock GenerateRoadBlock(Vector3 pos)
    {

        var rndIndex = Random.Range(0, availableBlocks.Length);
        var block = GameObject.Instantiate(availableBlocks[rndIndex], transform);

        var posOffset = block.transform.position - block.startConnector.position;
        block.transform.position = pos + posOffset;

        return block;
    }

    private void DeleteRoadBlock(RoadBlock block)
    {
        GameObject.Destroy(block.gameObject);
    }
}
