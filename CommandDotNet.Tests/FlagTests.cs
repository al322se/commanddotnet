﻿using System.IO;
using CommandDotNet.Attributes;
using CommandDotNet.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace CommandDotNet.Tests
{
    public class FlagTests : TestBase
    {
        public FlagTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void TestValidFlagsWithDefaultSettings()
        {
            TestCaseRunner<ValidFlagsApplication> testCaseRunner = new TestCaseRunner<ValidFlagsApplication>(TestOutputHelper, new AppSettings
            {
                MethodArgumentMode = ArgumentMode.Option
            });
            testCaseRunner.Run("TestCases/FlagTests.TestValidFlagsWithDefaultSettings.Input.json",
                "TestCases/FlagTests.TestValidFlagsWithDefaultSettings.Output.json");
        }

        [Fact]
        public void TestValidFlagsWithExplicitBooleanMode()
        {
            TestCaseRunner<FlagAppForExplicitBooleanTest> testCaseRunner = 
                new TestCaseRunner<FlagAppForExplicitBooleanTest>(TestOutputHelper, new AppSettings
                {
                    BooleanMode = BooleanMode.Explicit,
                    MethodArgumentMode = ArgumentMode.Option
                });
            testCaseRunner.Run("TestCases/FlagTests.TestValidFlagsWithExplicitBooleanMode.Input.json",
                "TestCases/FlagTests.TestValidFlagsWithExplicitBooleanMode.Output.json");
        }
        
        [Fact]
        public void TestInvalidFlags()
        {
            AppRunner<InvalidFlagApplication> appRunner = new AppRunner<InvalidFlagApplication>(new AppSettings
            {
                MethodArgumentMode = ArgumentMode.Option
            });
            int exitCode = appRunner.Run(new[] {"CommandWithInvalidFlag"});
            exitCode.Should().Be(1);
        }
    }

    public class ValidFlagsApplication
    {
        public void CommandWithFlagTrueOverridden([Option(BooleanMode = BooleanMode.Explicit)]bool flag)
        {
            string output = new
            {
                flag
            }.ToJson();
            
            File.WriteAllText("TestCases/FlagTests.TestValidFlagsWithDefaultSettings.Output.json",
                output);
        }
        
        public void CommandWithFlagFalseOverridden([Option(BooleanMode = BooleanMode.Implicit)]bool flag)
        {
            string output = new
            {
                flag
            }.ToJson();
            
            File.WriteAllText("TestCases/FlagTests.TestValidFlagsWithDefaultSettings.Output.json",
                output);
        }

        public void CommandWithoutFlagOverride(bool flag)
        {
            string output = new
            {
                flag
            }.ToJson();
            
            File.WriteAllText("TestCases/FlagTests.TestValidFlagsWithDefaultSettings.Output.json",
                output);
        }

        public void CommandWithTwo1LetterFlags(bool a, bool b)
        {
            string output = new {a, b}.ToJson();
            File.WriteAllText("TestCases/FlagTests.TestValidFlagsWithDefaultSettings.Output.json", output);
        }
    }

    public class FlagAppForExplicitBooleanTest
    {
        public void CommandWithoutFlagOverride(bool flag)
        {
            string output = new
            {
                flag
            }.ToJson();
            
            File.WriteAllText("TestCases/FlagTests.TestValidFlagsWithExplicitBooleanMode.Output.json",
                output);
        }
    }

    public class InvalidFlagApplication
    {
        public void CommandWithInvalidFlag([Option(BooleanMode = BooleanMode.Implicit)]string flag)
        {
        }
    }
}