open System
open System.Net
open System.Windows.Forms


[<EntryPoint>]
[<STAThread>]
let main argv =
    let createBrowser (url : string) =
        let webClient = new WebClient()
        let document = webClient.DownloadString(Uri url)
        let browser = new WebBrowser(ScriptErrorsSuppressed = true,
                                     Dock = DockStyle.Fill,
                                     DocumentText = document)
        browser

    let createForm (url : string) =
        let browser = createBrowser url
        let form = new Form(Text = "Hello from F#!")
        form.Controls.Add browser
        form

    let form = createForm "http://fsharp.org"
    Application.Run(form)
    0
