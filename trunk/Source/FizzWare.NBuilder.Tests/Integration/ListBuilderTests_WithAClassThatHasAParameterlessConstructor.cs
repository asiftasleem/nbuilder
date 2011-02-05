﻿using System.Linq;
using FizzWare.NBuilder.PropertyNaming;
using FizzWare.NBuilder.Tests.TestClasses;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;

namespace FizzWare.NBuilder.Tests.Integration
{
    [TestFixture]
    public class ListBuilderTests_WithAClassThatHasAParameterlessConstructor
    {
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
        }

        [Test]
        public void ShouldBeAbleToCreateAList()
        {
            var list =
                Builder<MyClass>.CreateListOfSize(10).Build();

            Assert.That(list.Count, Is.EqualTo(10));
        }

        [Test]
        public void PropertiesShouldBeSetSequentially()
        {
            var list = Builder<MyClass>.CreateListOfSize(10).Build();

            Assert.That(list[0].StringOne, Is.EqualTo("StringOne1"));
            Assert.That(list[9].StringOne, Is.EqualTo("StringOne10"));
			Assert.That(list[0].EnumProperty, Is.EqualTo(MyEnum.EnumValue1));
			Assert.That(list[9].EnumProperty, Is.EqualTo(MyEnum.EnumValue5));
        }

        [Test]
        public void ShouldBeAbleToUseTheFirst()
        {
            var specialTitle = "SpecialTitle";

            var list =
                Builder<MyClass>.CreateListOfSize(10).TheFirst(5).With(x => x.StringOne = specialTitle).Build();

            // I want the asserts here to serve as documentation
            // so it's obvious how it works for anyone glancing at this test
            Assert.That(list[0].StringOne, Is.EqualTo(specialTitle));
            Assert.That(list[1].StringOne, Is.EqualTo(specialTitle));
            Assert.That(list[2].StringOne, Is.EqualTo(specialTitle));
            Assert.That(list[3].StringOne, Is.EqualTo(specialTitle));
            Assert.That(list[4].StringOne, Is.EqualTo(specialTitle));
            Assert.That(list[5].StringOne, Is.EqualTo("StringOne6"));
            Assert.That(list[6].StringOne, Is.EqualTo("StringOne7"));
            Assert.That(list[7].StringOne, Is.EqualTo("StringOne8"));
            Assert.That(list[8].StringOne, Is.EqualTo("StringOne9"));
            Assert.That(list[9].StringOne, Is.EqualTo("StringOne10"));
        }

        [Test]
        public void ShouldBeAbleToUseTheLast()
        {
            var specialTitle = "SpecialTitle";

            var list =
                Builder<MyClass>.CreateListOfSize(10).TheLast(5).With(x => x.StringOne = specialTitle).Build();

            Assert.That(list[0].StringOne, Is.EqualTo("StringOne1"));
            Assert.That(list[1].StringOne, Is.EqualTo("StringOne2"));
            Assert.That(list[2].StringOne, Is.EqualTo("StringOne3"));
            Assert.That(list[3].StringOne, Is.EqualTo("StringOne4"));
            Assert.That(list[4].StringOne, Is.EqualTo("StringOne5"));
            Assert.That(list[5].StringOne, Is.EqualTo(specialTitle));
            Assert.That(list[6].StringOne, Is.EqualTo(specialTitle));
            Assert.That(list[7].StringOne, Is.EqualTo(specialTitle));
            Assert.That(list[8].StringOne, Is.EqualTo(specialTitle));
            Assert.That(list[9].StringOne, Is.EqualTo(specialTitle));   
        }

        [Test]
        public void ShouldBeAbleToUseMultipleTheFirsts()
        {
            var title = "FirstTitle";
            var overwrittenTitle = "OverwrittenTitle";

            var list =
                Builder<MyClass>.CreateListOfSize(10)
                                .TheFirst(5)
                                    .With(x => x.StringOne = title)
                                .TheFirst(5)
                                    .With(x => x.StringOne = overwrittenTitle)
                                .Build();

            Assert.That(list.Count, Is.EqualTo(10));
            Assert.That(list[0].StringOne, Is.EqualTo(overwrittenTitle));
            Assert.That(list[4].StringOne, Is.EqualTo(overwrittenTitle));
        }

        [Test]
        public void ShouldBeAbleToUseAndTheNext()
        {
            var titleone = "TitleOne";
            var titletwo = "TitleTwo";

            var productList =
                Builder<MyClass>
                .CreateListOfSize(4)
                    .TheFirst(2)
                        .With(x => x.StringOne = titleone)
                    .TheNext(2)
                        .With(x => x.StringOne = titletwo)
                    .Build();

            Assert.That(productList[0].StringOne, Is.EqualTo(titleone));
            Assert.That(productList[1].StringOne, Is.EqualTo(titleone));
            Assert.That(productList[2].StringOne, Is.EqualTo(titletwo));
            Assert.That(productList[3].StringOne, Is.EqualTo(titletwo));
        }

        [Test]
        public void ShouldBeAbleToUseAndThePrevious()
        {
            var titleone = "TitleOne";
            var titletwo = "TitleTwo";

            var productList =
                Builder<MyClass>
                .CreateListOfSize(4)
                    .TheLast(2)
                        .With(x => x.StringOne = titletwo)
                    .ThePrevious(2)
                        .With(x => x.StringOne = titleone)
                    .Build();

            Assert.That(productList[0].StringOne, Is.EqualTo(titleone));
            Assert.That(productList[1].StringOne, Is.EqualTo(titleone));
            Assert.That(productList[2].StringOne, Is.EqualTo(titletwo));
            Assert.That(productList[3].StringOne, Is.EqualTo(titletwo));
        }

        [Test]
        public void ShouldBeAbleToUseDo()
        {
            var myOtherClass = Builder<SimpleClass>.CreateNew().Build();

            var objects = Builder<MyClass>.CreateListOfSize(5).All().Do(x => x.Add(myOtherClass)).Build();

            for (int i = 0; i < objects.Count; i++)
            {
                Assert.That(objects[i].SimpleClasses.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void ShouldBeAbleToUseAndTheNextAfterASectionDeclaration()
        {
            var objects = Builder<MyClass>
                .CreateListOfSize(10)
                .Section(0, 4)
                    .With(x => x.Int = 1)
                .TheNext(3)
                    .With(x => x.Int = 2)
                .Build();

            Assert.That(objects[0].Int, Is.EqualTo(1));
            Assert.That(objects[1].Int, Is.EqualTo(1));
            Assert.That(objects[2].Int, Is.EqualTo(1));
            Assert.That(objects[3].Int, Is.EqualTo(1));
            Assert.That(objects[4].Int, Is.EqualTo(1));
            Assert.That(objects[5].Int, Is.EqualTo(2));
            Assert.That(objects[6].Int, Is.EqualTo(2));
            Assert.That(objects[7].Int, Is.EqualTo(2));
        }

		[TestCase(10,5)]
		[TestCase(10,1)]
        [TestCase(5, 5)]
        [TestCase(1, 1)]
		public void ShouldBeAbleToUseRandom(int listSize, int randomItems)
		{
			var objects = Builder<MyClass>
				.CreateListOfSize(listSize)
				.Random(randomItems)
					.With(x => x.StringOne = "TestRandom")
				.Build();

			int numObjectsWithRandomValue = objects.Where(x => x.StringOne.Equals("TestRandom")).Count();

			Assert.AreEqual(randomItems, numObjectsWithRandomValue);
		}

        [Test]
        public void ShouldBeAbleToDisableAutoPropertyNaming()
        {
            try
            {
                BuilderSetup.AutoNameProperties = false;

                var list = Builder<MyClass>.CreateListOfSize(10).Build();

                Assert.That(list[0].Int, Is.EqualTo(0));
                Assert.That(list[9].Int, Is.EqualTo(0));

                Assert.That(list[0].StringOne, Is.Null);
                Assert.That(list[9].StringOne, Is.Null);
            }
            finally
            {
                BuilderSetup.AutoNameProperties = true;
            }
        }

        [Test]
        public void ShouldBeAbleToSpecifyADefaultCustomPropertyNamer()
        {
            try
            {
                BuilderSetup.SetDefaultPropertyNamer(new MockPropertyNamerTests());
                Builder<MyClass>.CreateListOfSize(10).Build();
                Assert.That(MockPropertyNamerTests.SetValuesOfAllInCallCount, Is.EqualTo(1));
            }
            finally
            {
                BuilderSetup.ResetToDefaults();
            }
        }

        [Test]
        public void ShouldBeAbleToSpecifyACustomPropertyNamerForASpecificType()
        {
            try
            {
                IPropertyNamer propertyNamer = mocks.DynamicMock<IPropertyNamer>();

                BuilderSetup.SetPropertyNamerFor<MyClass>(propertyNamer);

                using (mocks.Record())
                {
                    propertyNamer.Expect(x => x.SetValuesOfAllIn(Arg<IList<MyClass>>.Is.TypeOf)).Repeat.Once();
                }

                using (mocks.Playback())
                {
                    Builder<MyClass>.CreateListOfSize(10).Build();
                    Builder<SimpleClass>.CreateListOfSize(10).Build();
                }

                mocks.VerifyAll();
            }
            finally
            {
                BuilderSetup.ResetToDefaults();
            }
        }

        [Test]
        public void ShouldBeAbleToDisableAutomaticPropertyNaming()
        {
            try
            {
                BuilderSetup.AutoNameProperties = false;
                var list = Builder<MyClass>.CreateListOfSize(10).Build();

                Assert.That(list[0].Int, Is.EqualTo(0));
                Assert.That(list[9].Int, Is.EqualTo(0));
            }
            finally
            {
                BuilderSetup.ResetToDefaults();
            }
        }

        [Test]
        public void ShouldBeAbleToDisableAutomaticPropertyNamingForASpecificFieldOfASpecificType()
        {
            try
            {
                BuilderSetup.DisablePropertyNamingFor<MyClass, int>(x => x.Int);

                var list = Builder<MyClass>.CreateListOfSize(10).Build();

                Assert.That(list[0].Int, Is.EqualTo(0));
                Assert.That(list[0].Long, Is.EqualTo(1));

                Assert.That(list[9].Int, Is.EqualTo(0));
                Assert.That(list[9].Long, Is.EqualTo(10));
            }
            finally
            {
                BuilderSetup.ResetToDefaults();
            }
        }

        [Test] 
        public void should_be_able_to_disable_property_naming_for_an_inherited_property()
        {
            try
            {
                BuilderSetup.DisablePropertyNamingFor<MyConcreteClass, int>(x => x.PropertyInAbstractClass);

                var list = Builder<MyConcreteClass>.CreateListOfSize(10).Build();

                Assert.That(list[0].PropertyInAbstractClass, Is.EqualTo(0));
                Assert.That(list[0].PropertyInInheritedClass, Is.EqualTo(1));

                Assert.That(list[9].PropertyInAbstractClass, Is.EqualTo(0));
                Assert.That(list[9].PropertyInInheritedClass, Is.EqualTo(10));
            }
            finally
            {
                BuilderSetup.ResetToDefaults();
            }
        }

        [Test]
        public void Random_test()
        {
            var items = Builder<MyClass>.CreateListOfSize(40)
                .Random(10)
                    .With(x => x.EnumProperty = MyEnum.EnumValue1)
                .Random(10)
                    .With(x => x.EnumProperty = MyEnum.EnumValue2)
                .Random(10)
                    .With(x => x.EnumProperty = MyEnum.EnumValue3)
                .Random(10)
                    .With(x => x.EnumProperty = MyEnum.EnumValue4)
                .Build();

            Assert.That(items.Count(), Is.EqualTo(40));
            Assert.That(items.Count(x => x.EnumProperty == MyEnum.EnumValue1), Is.EqualTo(10));
            Assert.That(items.Count(x => x.EnumProperty == MyEnum.EnumValue2), Is.EqualTo(10));
            Assert.That(items.Count(x => x.EnumProperty == MyEnum.EnumValue3), Is.EqualTo(10));
            Assert.That(items.Count(x => x.EnumProperty == MyEnum.EnumValue4), Is.EqualTo(10));
        }
    }
}