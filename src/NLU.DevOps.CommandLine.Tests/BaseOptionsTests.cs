﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace NLU.DevOps.CommandLine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandLine.Train;
    using FluentAssertions;
    using global::CommandLine;
    using NUnit.Framework;

    [TestFixture]
    internal static class BaseOptionsTests
    {
        [Test]
        public static void VerifyServiceMissingInBaseOptionsRaisesError()
        {
            var args = Array.Empty<string>();
            var parser = Parser.Default.ParseArguments<TrainOptions>(args);
            parser.Tag.Should().Be(ParserResultType.NotParsed);
            var notParsed = (NotParsed<TrainOptions>)parser;
            var error = notParsed.Errors.First();
            error.Should().BeOfType<MissingRequiredOptionError>();
            var missingOptionError = (MissingRequiredOptionError)error;
            missingOptionError.NameInfo.LongName.Should().Be("service");
        }

        [Test]
        public static void VerifyServiceExistsInBaseDoesNotRaisesError()
        {
            var options = new List<string>();
            options.Add("-s");
            options.Add("luis");
            var args = options.ToArray();
            var parser = Parser.Default.ParseArguments<TrainOptions>(args).WithParsed<TrainOptions>(o =>
            {
                o.Service.Should().Be("luis");
            }).WithNotParsed<TrainOptions>(o => Assert.Fail("Could not Parse Options"));
        }

        [Test]
        public static void VerifyAllSettingsCanBeSet()
        {
            var options = new List<string>();
            options.Add("-s");
            options.Add("service");
            options.Add("-q");
            options.Add("true");
            options.Add("-v");
            options.Add("true");
            options.Add("-i");
            options.Add("includepath");
            var args = options.ToArray();
            var parser = Parser.Default.ParseArguments<TrainOptions>(args).WithParsed<TrainOptions>(o =>
            {
                o.Service.Should().Be("service");
                o.Quiet.Should().BeTrue();
                o.Verbose.Should().BeTrue();
                o.IncludePath.Should().Be("includepath");
            }).WithNotParsed<TrainOptions>(o => Assert.Fail("Could not Parse Options"));
        }
    }
}
