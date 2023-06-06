module ShadowLengthCalculator.WhiteBoxTests

open System
open System.IO
open NUnit.Framework
open Program

[<TestFixture>]
type WhiteBoxTests() =
    [<Test>]
    member this.``parseSegment should return Some segment for valid input``() =
        let input = "1 5"
        let expectedSegment = Some (1, 5)
        let actualSegment = parseSegment input
        Assert.AreEqual(expectedSegment, actualSegment)

    [<Test>]
    member this.``parseSegment should return None for invalid input``() =
        let input = "1 invalid"
        let expectedSegment = None
        let actualSegment = parseSegment input
        Assert.AreEqual(expectedSegment, actualSegment)

    [<Test>]
    member this.``combineSegments should combine overlapping segments``() =
        let segments = [(1, 5); (4, 6); (7, 10)]
        let expectedCombinedSegments = [(7, 10); (1, 6)]
        let actualCombinedSegments = combineSegments segments
        CollectionAssert.AreEqual(expectedCombinedSegments, actualCombinedSegments)
    [<Test>]
    member this.``combineSegments should handle non-overlapping segments``() =
        let segments = [(1, 5); (6, 8); (9, 12)]
        let expectedCombinedSegments = [(9, 12); (6, 8); (1, 5)]
        let actualCombinedSegments = combineSegments segments
        CollectionAssert.AreEqual(expectedCombinedSegments, actualCombinedSegments)

    [<Test>]
    member this.``calculateTotalLength should return the sum of segment lengths``() =
        let segments = [(1, 5); (4, 6); (7, 10)]
        let expectedTotalLength = 8
        let combinedSegments = combineSegments segments
        let actualTotalLength = calculateTotalLength combinedSegments
        Assert.AreEqual(expectedTotalLength, actualTotalLength)
        
    [<Test>]
    member this.``readSegments should return empty list when no input is provided``() =
        let mockInput = new StringReader("")
        Console.SetIn mockInput

        let segments = readSegments()

        Assert.IsEmpty(segments)

    [<Test>]
    member this.``readSegments should skip invalid input and continue``() =
        let mockInput = new StringReader("1 5\ninvalid\n4 6\ninvalid\n7 10\n")
        Console.SetIn mockInput

        let segments = readSegments()

        CollectionAssert.AreEqual([(7, 10); (4, 6); (1, 5)], segments)

    [<Test>]
    member this.``calculateTotalLength should return 0 for an empty list``() =
        let segments = []

        let totalLength = calculateTotalLength segments

        Assert.Zero(totalLength)

    [<Test>]
    member this.``calculateTotalLength should return correct total length for multiple segments``() =
        let segments = [(1, 5); (4, 6); (7, 10)]

        let combinedSegments = combineSegments segments
        let actualTotalLength = calculateTotalLength combinedSegments
        Assert.AreEqual(8, actualTotalLength)

    [<Test>]
    member this.``main should print the correct output``() =
        let mockInput = new StringReader("1 5\n4 6\n7 10\n")
        Console.SetIn mockInput

        let writer = new StringWriter()
        Console.SetOut writer

        main()

        let expectedOutput = "Enter segments (start and end) separated by a space:\r\nCombined segments: [(7, 10); (1, 6)]\r\nTotal length: 8"
        let output = writer.ToString().Trim()

        Assert.AreEqual(expectedOutput, output)
