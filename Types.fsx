module Records =
    type Student =
        { First: string
          Last: string
          mutable Grade: int }

        static member Good = { First = ""; Last = ""; Grade = 100 }
        static member Bad = { First = ""; Last = ""; Grade = 0 }

        member this.PrintTable() =
            printfn "| %-20s | %-20s | %-8i |" this.First this.Last this.Grade

        static member TableDivider =
            [ for i in 0 .. 57 do
                match i with
                | 0
                | 23
                | 46
                | 57 -> yield "+"
                | _ -> yield "-" ]
            |> String.concat ""
            |> printfn "%s"

    let goodStudent = Student.Good
    let badStudent = { Student.Bad with Grade = 0 }

    let ammon =
        { First = "Ammon"
          Last = "Taylor"
          Grade = 100 }

    let mark =
        { First = "Mark"
          Last = "Abraham"
          Grade = 59 }

    // <- assignment operator
    mark.Grade <- mark.Grade + 10

    let hunter =
        { badStudent with
              First = "Hunter"
              Last = "Nielson" }

    let will =
        { Student.Good with
              First = "Will"
              Last = "Dickinson" }

    let students = [ ammon; mark; hunter; will ]

    let printStudents (students: Student list) =
        Student.TableDivider
        printfn "| %-20s | %-20s | %-8s |" "First" "Last" "Grade"
        Student.TableDivider
        students |> List.iter (fun s -> s.PrintTable())
        Student.TableDivider

    let goodStudents (student: Student): bool =
        match student with
        | { Grade = g } when g >= 80 -> true
        | _ -> false

    let filterStudents (students: Student list) (f: Student -> bool): Student list = students |> List.filter f

module Ops =
    // Creates active pattern match
    let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd

    let TestNumber input =
        match input with
        | Even -> printfn "%d is even" input
        | Odd -> printfn "%d is odd" input

    // let onlyEvens (xs: int list): int list = xs |> List.filter (fun x -> Even)

    TestNumber 3
