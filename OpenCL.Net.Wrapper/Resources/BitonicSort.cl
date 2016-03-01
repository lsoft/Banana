typedef struct
{
    uint AIndex;
    uint BIndex;
    float Distance;
} SortItem;

__inline ulong GetKey(SortItem d)
{
    ulong a = d.AIndex;

    return
        (a << 32) + d.BIndex;
}

__kernel void BitonicSortKernel(
    __global SortItem * theArray,
    const uint stage, 
    const uint passOfStage,
    const uint direction
    )
{
    uint sortIncreasing = direction;
    ulong threadId = get_global_id(0);
     
    ulong pairDistance = 1 << (stage - passOfStage);
    ulong blockWidth   = 2 * pairDistance;
 
    ulong leftId = (threadId % pairDistance) 
                   + (threadId / pairDistance) * blockWidth;
 
    ulong rightId = leftId + pairDistance;
     
    SortItem leftElement = theArray[leftId];
    SortItem rightElement = theArray[rightId];
     
    ulong sameDirectionBlockWidth = 1 << stage;
     
    if((threadId/sameDirectionBlockWidth) % 2 == 1)
    {
        sortIncreasing = 1 - sortIncreasing;
    }
 
    SortItem greater;
    SortItem lesser;
    if(GetKey(leftElement) > GetKey(rightElement))
    {
        greater = leftElement;
        lesser  = rightElement;
    }
    else
    {
        greater = rightElement;
        lesser  = leftElement;
    }
     
    if(sortIncreasing)
    {
        theArray[leftId]  = lesser;
        theArray[rightId] = greater;
    }
    else
    {
        theArray[leftId]  = greater;
        theArray[rightId] = lesser;
    }
}