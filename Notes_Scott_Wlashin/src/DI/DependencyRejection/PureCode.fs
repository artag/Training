module PureCode

type ComparisonResult =
    | Bigger
    | Smaller
    | Equal

let compareTwoStrings str1 str2 =
    if str1 > str2 then
        Bigger
    else if str1 < str2 then
        Smaller
    else
        Equal