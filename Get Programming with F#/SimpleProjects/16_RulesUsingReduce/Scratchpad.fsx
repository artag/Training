open System

#load "Rules.fs"

let rules enableLog : Rules.Rule list =
    [ Rules.mustBe3Words enableLog
      Rules.maxLengthIs30Characters enableLog
      Rules.allLettersMustBeCaps enableLog
      Rules.textWithoutNumbers enableLog ]

let rulesWithLog = rules true
let rulesWithoutLog = rules false

let buildValidator (rules : Rules.Rule list) =
    rules
    |> Seq.reduce(fun firstRule secondRule ->
        fun word ->
            let passed, error = firstRule word
            if passed then
                let passed, error = secondRule word
                if passed then true, ""
                else false, error
            else false, error)

let validate = buildValidator rulesWithLog

"HELLO FRoM F#!!!" |> validate

"HELLO FR0M F#!1!!" |> validate

"HELLO FROM F#!!!" |> validate