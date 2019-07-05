module ForestSim.Map

type Terrain = Grass | Rock | Sand
type Map = Terrain list list

let generateMap nTileEdge : Map =
    let rand = System.Random ()
    [ for i in 1 .. nTileEdge ->
        [ for j in 1 .. nTileEdge ->
            match rand.Next() % 8 with
            | 0 -> Sand
            | _ -> Grass
        ]
    ]

let tileX = 40
let tileY = 20

let mapToScreen x y =
    let x2 = (x - y) * tileX / 2 - tileY / 2
    let y2 = (x + y) * tileY / 2
    (x2, y2)