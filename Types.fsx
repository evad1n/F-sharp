module Records =
    type Person = { First: string; Last: string }

    let ammon = { First = "Ammon"; Last = "Taylor" }
    let mark = { First = "Mark"; Last = "Abraham" }
    let people = [ ammon; mark ]

    let printPeople =
        for p in people do
            printfn "%A" p
