// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Tile = Grass | Rock | Sand

type GameState = {
    Foo: int
    Grid: Tile array array
}

type ForestSim () as game =
    inherit Game ()
    let rand = System.Random ()

    // Controls the window size
    let gfx = new GraphicsDeviceManager (game)

    // Immutable contents, mutable reference cell
    let latestGameState = ref {
        Foo = 0
        Grid = [| for i in 1 .. 10 ->
                   [| for j in 1 .. 10 ->
                       Grass
                   |]
               |]
    }

    // Execute game logic on a background thread
    let gameLoop =
        MailboxProcessor.Start (fun inbox -> async {
            while true do
                let! msg = inbox.Receive ()
                let prevState = !latestGameState
                latestGameState := {
                    Foo = rand.Next ()
                    Grid = prevState.Grid
                }
        })

    do // Post-initialization constructor logic
        gfx.IsFullScreen <- false
        gfx.PreferredBackBufferWidth <- 1600
        gfx.PreferredBackBufferHeight <- 900
        game.Content.RootDirectory <- "Content"
        ()

    // Implement MonoGame interface
    override ForestSim.Initialize () = ()
    override ForestSim.LoadContent () = ()
    override ForestSim.Update (gameTime:GameTime) = gameLoop.Post (gameTime)
    override ForestSim.Draw (gameTime:GameTime) =
        let gameState = !latestGameState
        if 0 = gameState.Foo % 2
        then game.GraphicsDevice.Clear (Color.DarkSlateBlue)
        else game.GraphicsDevice.Clear (Color.CornflowerBlue)
        ()

[<STAThread>]
[<EntryPoint>]
let main argv = 
    (new ForestSim ()).Run()
    0 // Exit code
