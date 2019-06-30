// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type ForestSim () as game =
    inherit Game ()
    let gfx = new GraphicsDeviceManager (game)
    do  gfx.IsFullScreen <- false
        gfx.PreferredBackBufferWidth <- 1600
        gfx.PreferredBackBufferHeight <- 900
        game.Content.RootDirectory <- "Content"
        ()

    override ForestSim.Initialize () =
        ()

    override ForestSim.LoadContent () =
        ()

    override ForestSim.Update (g:GameTime) =
        ()

    override ForestSim.Draw (g:GameTime) =
        game.GraphicsDevice.Clear (Color.DarkSlateBlue)

[<STAThread>]
[<EntryPoint>]
let main argv = 
    (new ForestSim()).Run()
    0 // Exit code
