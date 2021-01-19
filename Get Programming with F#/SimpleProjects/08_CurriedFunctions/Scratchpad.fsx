open System
open System.IO

// 1
let writeToFile (date:DateTime) filename text =
    let path = sprintf "E:\%s-%s.txt" (date.ToString "yyMMdd") filename
    File.WriteAllText(path, text)

let writeToToday = writeToFile DateTime.UtcNow.Date
let writeToTomorrow = writeToFile (DateTime.UtcNow.Date.AddDays 1.)
let writeToTodayHelloWorld = writeToToday "hello-world"

writeToFile DateTime.Now "zero-file" "Simple Text"
writeToToday "first-file" "The quick brown fox jumped over the lazy dog"
writeToTomorrow "second-file" "The quick brown fox jumped over the lazy dog"
writeToTodayHelloWorld "The quick brown fox jumped over the lazy dog"

// 2
let buildDt year month day = DateTime(year, month, day)
buildDt 2020 12 5

let buildDtThisYear = buildDt DateTime.UtcNow.Year
buildDtThisYear 01 05

let buildDtThisYearThisMonth = buildDtThisYear DateTime.UtcNow.Month
buildDtThisYearThisMonth 15
