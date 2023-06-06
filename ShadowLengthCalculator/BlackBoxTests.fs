module ShadowLengthCalculator.BlackBoxTests

open System
open System.IO
open NUnit.Framework
open Program

[<TestFixture>]
type BlackBoxTests() =
    [<Test>]
    member this.``Black box: No segments provided``() =
        let mockInput = new StringReader("")
        Console.SetIn mockInput

        let segments = readSegments()
        let combinedSegments = combineSegments segments
        let totalLength = calculateTotalLength combinedSegments

        Assert.IsEmpty(segments)
        Assert.IsEmpty(combinedSegments)
        Assert.Zero(totalLength)

    [<Test>]
    member this.``Black box: One segment provided``() =
        let mockInput = new StringReader("1 5")
        Console.SetIn mockInput

        let segments = readSegments()
        let combinedSegments = combineSegments segments
        let totalLength = calculateTotalLength combinedSegments

        CollectionAssert.AreEqual([(1, 5)], segments)
        CollectionAssert.AreEqual([(1, 5)], combinedSegments)
        Assert.AreEqual(4, totalLength)

    [<Test>]
    member this.``Black box: Multiple non-overlapping segments``() =
        let mockInput = new StringReader("1 5\n6 10\n11 15")
        Console.SetIn mockInput

        let segments = readSegments()
        let combinedSegments = combineSegments segments
        let totalLength = calculateTotalLength combinedSegments

        CollectionAssert.AreEqual([(11, 15); (6, 10); (1, 5)], segments)
        CollectionAssert.AreEqual([(11, 15); (6, 10); (1, 5)], combinedSegments)
        Assert.AreEqual(12, totalLength)

    [<Test>]
    member this.``Black box: Multiple overlapping segments``() =
        let mockInput = new StringReader("1 5\n4 8\n7 10")
        Console.SetIn mockInput

        let segments = readSegments()
        let combinedSegments = combineSegments segments
        let totalLength = calculateTotalLength combinedSegments

        CollectionAssert.AreEqual([(7, 10); (4, 8); (1, 5)], segments)
        CollectionAssert.AreEqual([(1, 10)], combinedSegments)
        Assert.AreEqual(9, totalLength)

    [<Test>]
    member this.``Black box: Segments in reverse order``() =
        let mockInput = new StringReader("11 15\n6 10\n1 5")
        Console.SetIn mockInput

        let segments = readSegments()
        let combinedSegments = combineSegments segments
        let totalLength = calculateTotalLength combinedSegments

        CollectionAssert.AreEqual([(1, 5); (6, 10); (11, 15)], segments)
        CollectionAssert.AreEqual([(11, 15); (6, 10); (1, 5)], combinedSegments)
        Assert.AreEqual(12, totalLength)

    [<Test>]
    member this.``Black box: Invalid input format``() =
        let mockInput = new StringReader("1 5\ninvalid\n7 10")
        Console.SetIn mockInput

        let segments = readSegments()
        let combinedSegments = combineSegments segments
        let totalLength = calculateTotalLength combinedSegments

        CollectionAssert.AreEqual([(7, 10); (1, 5)], segments)
        CollectionAssert.AreEqual([(7, 10); (1, 5)], combinedSegments)
        Assert.AreEqual(7, totalLength)
