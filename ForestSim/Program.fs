// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open ForestSim.Entity
open ForestSim.Map

type GameState = {
    map: Map
    entities: Entity list
}

type ForestSim () as game =
    inherit Game ()
    let rand = System.Random ()

    // Controls the window size
    let gfx = new GraphicsDeviceManager(game)

    // Immutable contents, mutable reference cell
    let latestGameState = ref {
        map = []
        entities = []
    }

    // Execute game logic on a background thread
    let gameLoop =
        // We don't want to waste time processing old turns, so take
        // all pending messages and sum the total elapsed time.
        let consumeAllMail (inbox:MailboxProcessor<GameTime>) = async {
            let! gameTime = inbox.Receive ()
            let mutable elapsed = gameTime.ElapsedGameTime
            while inbox.CurrentQueueLength > 0 do
                let! gameTime2 = inbox.Receive ()
                elapsed <- elapsed + gameTime2.ElapsedGameTime
            return elapsed
        }

        // Main game logic loop
        MailboxProcessor.Start (fun inbox -> async {
            while true do
                let! timeElapsed = consumeAllMail inbox
                let prevState = !latestGameState
                latestGameState := {
                    map = prevState.map
                    entities = prevState.entities
                }
        })

    // Post-initialization constructor logic
    do  gfx.IsFullScreen <- false
        gfx.PreferredBackBufferWidth <- 1600
        gfx.PreferredBackBufferHeight <- 900
        game.Content.RootDirectory <- "."

    member game.tileDisplayLayer =
        new RenderTarget2D(game.GraphicsDevice,
                           gfx.PreferredBackBufferWidth,
                           gfx.PreferredBackBufferHeight)

    // Initialize the non-graphic game content
    override ForestSim.Initialize () =
        latestGameState := {
            map = generateMap 32
            entities = []
        }
        base.Initialize()

    // Initialize the graphics
    override ForestSim.LoadContent () =
        base.LoadContent()

    // Execute one tick of game logic
    override ForestSim.Update gameTime =
        gameLoop.Post (gameTime)

    // Draw one frame
    override ForestSim.Draw (gameTime:GameTime) =
        let gameState = !latestGameState
        let grass = game.Content.Load<Texture2D>("content/art/tiles/grass_1")
        let sand = game.Content.Load<Texture2D>("content/art/tiles/sand_1")
        game.GraphicsDevice.Clear(Color.DarkSlateBlue)
        let spriteBatch = new SpriteBatch(game.GraphicsDevice)
        spriteBatch.Begin()
        for x in 0 .. gameState.map.Length - 1 do
            for y in 0 .. gameState.map.Head.Length - 1 do
                let pixelX, pixelY = mapToScreen x y
                let texture = match (gameState.map.Item x).Item y with
                              | Grass -> grass
                              | _ -> sand
                spriteBatch.Draw(texture, new Vector2(float32(pixelX), float32(pixelY)), Color.White);
        spriteBatch.End()
        base.Draw(gameTime)

[<STAThread>]
[<EntryPoint>]
let main argv = 
    (new ForestSim ()).Run()
    0 // Exit code
