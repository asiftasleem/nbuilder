  1. The project has two outputs - a CLR version and a Silverlight version. All code must compile for both.
  1. Every patch must have unit tests and those tests must provide 100% coverage.
  1. Tests are divided into three categories:
    * Unit - the class under test is completely isolated by use of stubs or mocks.
    * Integration - classes tested together.
    * Functional - 'real life' tests and documentation.
  1. Every new class must have a 'unit' test fixture at least.
  1. Add integration tests when necessary to do so.
  1. For new features or changes to existing features use the functional tests project for high level real world tests and to serve as simple documentation for users.
  1. Any new tests must follow this naming convention:
> > [MethodName\_Scenario\_Expectation()](http://weblogs.asp.net/rosherove/archive/2005/04/03/TestNamingStandards.aspx)
  1. Every class must have an interface and must be injected through constructor arguments.
  1. Every class must have a single responsibility.
  1. Every new test must be in Arrange Act Assert form. If touching an existing test in record/replay, convert it to AAA syntax unless it is too time consuming.
  1. The "Foo Bar" convention is not permitted anywhere
  1. American English spellings should be used not British English