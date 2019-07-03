module ForestSim.Map

type Terrain = Grass | Rock | Sand
type Obstacle = None | Tree | Stump | LumberCamp
type Tile = Tile of Terrain * Obstacle
type Chunk = Tile array array
type Map = Chunk array array

let generateMap nChunkEdge nTileEdge : Map =
    let rand = System.Random ()
    [| for i in 1 .. nChunkEdge ->
        [| for j in 1 .. nChunkEdge ->
            [| for k in 1 .. nTileEdge ->
                [| for l in 1 .. nTileEdge ->
                    match rand.Next() % 8 with
                    | 0 -> Tile (Grass, Tree)
                    | _ -> Tile (Grass, None)
                |]
            |]
        |]
    |]