using PhysHelper.Parsers;

namespace PhysHelper.Tests.Parsers;

public class BaseParserHandlerTests
{
    [Test]
    public void MockParsersAreCreated_MustCorrectlyParseTheObject()
    {
        // Arrange
        var query = new MockParserQueryClass() { Parser1String = "first", Parser2String = "second", Parser3String = "third" };
        var results = new List<string>();
        var parser = new MockParser1();

        parser
            .SetNext(new MockParser2())
            .SetNext(new MockParser3());

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(3).Items);
            Assert.That(results[0], Is.EqualTo("first"));
            Assert.That(results[1], Is.EqualTo("second"));
            Assert.That(results[2], Is.EqualTo("third"));
        });
    }

    [Test]
    public void MockParsersAreCreatedAndHaveDifferentOrder_MustCorrectlyParseTheObjectWithTheCorrectOrder()
    {
        // Arrange
        var query = new MockParserQueryClass() { Parser1String = "first", Parser2String = "second", Parser3String = "third" };
        var results = new List<string>();
        var parser = new MockParser1();

        // Act
        parser
            .SetNext(new MockParser3())
            .SetNext(new MockParser2());

        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(3).Items);
            Assert.That(results[0], Is.EqualTo("first"));
            Assert.That(results[1], Is.EqualTo("third"));
            Assert.That(results[2], Is.EqualTo("second"));
        });
    }

    private class MockParserQueryClass
    {
        public required string Parser1String { get; set; }
        public required string Parser2String { get; set; }
        public required string Parser3String { get; set; }
    }

    private class MockParser1 : BaseParserHandler<List<string>, MockParserQueryClass>
    {
        protected override void Handle(List<string> parsedObj, MockParserQueryClass query)
        {
            parsedObj.Add(query.Parser1String);
        }
    }

    private class MockParser2 : BaseParserHandler<List<string>, MockParserQueryClass>
    {
        protected override void Handle(List<string> parsedObj, MockParserQueryClass query)
        {
            parsedObj.Add(query.Parser2String);
        }
    }

    private class MockParser3 : BaseParserHandler<List<string>, MockParserQueryClass>
    {
        protected override void Handle(List<string> parsedObj, MockParserQueryClass query)
        {
            parsedObj.Add(query.Parser3String);
        }
    }
}