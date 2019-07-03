module ForestSim.Entity

type Resource = None | Wood of int
type Pop = Worker
type Entity = {
    popType: Pop
    carrying: Resource

    // Location
    chunkX: int
    chunkY: int
    tileX: int
    tileY: int
    subtileX: float
    subtileY: float
}