module ShadowLengthCalculator.Program
open System

type Segment = int * int

let parseSegment (input: string) : Option<Segment> =
    match input.Split(' ') with
    | [| start; end' |] ->
        match Int32.TryParse(start), Int32.TryParse(end') with
        | (true, parsedStart), (true, parsedEnd) ->
            Some (parsedStart, parsedEnd)
        | _ ->
            None
    | _ ->
        None

let readSegments () : Segment list =
    let rec loop (segments: Segment list) =
        let input = Console.ReadLine()
        if String.IsNullOrWhiteSpace(input) then
            segments
        else
            match parseSegment input with
            | Some segment ->
                loop (segment :: segments)
            | None ->
                printfn "Invalid input format"
                loop segments
    loop []

let combineSegments (segments : Segment list) =
    segments
    |> List.sortBy fst
    |> List.fold (fun combined (start, end') ->
        match combined with
        | (prevStart, prevEnd) :: rest ->
            if start <= prevEnd then
                (prevStart, max end' prevEnd) :: rest
            else
                (start, end') :: combined
        | _ ->
            [(start, end')]
    ) []

let calculateTotalLength (segments : Segment list) =
    segments
    |> List.map (fun (start, end') -> end' - start)
    |> List.sum

let main() =
    printfn "Enter segments (start and end) separated by a space:"
    let segments = readSegments( )
    let combinedSegments = combineSegments segments
    let totalLength = calculateTotalLength combinedSegments
    printfn $"Combined segments: %A{combinedSegments}"
    printfn $"Total length: %d{totalLength}"
