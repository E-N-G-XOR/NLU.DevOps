﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace NLU.DevOps.CommandLine.Tests.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using CommandLine.Test;
    using FluentAssertions;
    using global::CommandLine;
    using NUnit.Framework;

    [TestFixture]
    internal class TestOptionsTests
    {
        private List<string> options;

        [SetUp]
        public void SetUp()
        {
            this.options = new List<string>();
            this.options.Add("-s");
            this.options.Add("luis");
        }

        [Test]
        public void SettingForTestOptionsWhenSet()
        {
            this.options.Add("-u");
            this.options.Add("utterances");
            this.options.Add("-e");
            this.options.Add("servicesettings");
            this.options.Add("--speech");
            this.options.Add("-d");
            this.options.Add("speechdirectory");
            this.options.Add("-t");
            this.options.Add("transcriptionfile");
            this.options.Add("-o");
            this.options.Add("outputpath");
            var args = this.options.ToArray();
            var parser = Parser.Default.ParseArguments<TestOptions>(args)
                .WithParsed(o =>
                {
                    o.UtterancesPath.Should().Be("utterances");
                    o.SettingsPath.Should().Be("servicesettings");
                    o.Speech.Should().BeTrue();
                    o.SpeechFilesDirectory.Should().Be("speechdirectory");
                    o.TranscriptionsFile.Should().Be("transcriptionfile");
                    o.OutputPath.Should().Be("outputpath");
                })
                .WithNotParsed(o => Assert.Fail("Could not Parse Options"));
        }

        [Test]
        public void VerifyUtterancesIsRequired()
        {
            var args = this.options.ToArray();
            var parser = Parser.Default.ParseArguments<TestOptions>(args);
            parser.Tag.Should().Be(ParserResultType.NotParsed);
            var error = parser.As<NotParsed<TestOptions>>().Errors.First();
            error.As<MissingRequiredOptionError>().NameInfo.LongName.Should().Be("utterances");
        }

        [Test]
        public void SettingForTestOptionsWhenNotSet()
        {
            this.options.Add("-u");
            this.options.Add("utterances");
            var args = this.options.ToArray();
            var parser = Parser.Default.ParseArguments<TestOptions>(args)
                .WithParsed(o =>
                {
                    o.UtterancesPath.Should().Be("utterances");
                    o.SettingsPath.Should().Be(null);
                    o.Speech.Should().BeFalse();
                    o.SpeechFilesDirectory.Should().Be(null);
                    o.TranscriptionsFile.Should().Be(null);
                    o.OutputPath.Should().Be(null);
                })
                .WithNotParsed(o => Assert.Fail("Could not Parse Options"));
        }
    }
}
