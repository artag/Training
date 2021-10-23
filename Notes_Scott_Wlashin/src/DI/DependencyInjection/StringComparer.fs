module StringComparer
open System
open Domain

type StringComparisons(logger:ILogger, comparison:StringComparison) =
    member __.CompareTwoStrings str1 str2 =
        logger.Debug "compareTwoStrings: Starting"

        let compareResult = String.Compare(str1, str2, comparison)
        let result =
            if compareResult > 0 then
                Bigger
            else if compareResult < 0 then
                Smaller
            else
                Equal
        logger.Info $"compareTwoStrings: result=%A{result}"
        logger.Debug $"compareTwoStrings: Finished"
        result