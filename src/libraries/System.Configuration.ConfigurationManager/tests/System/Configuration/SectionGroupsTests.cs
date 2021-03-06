// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Configuration;
using System.IO;
using Xunit;

namespace System.ConfigurationTests
{
    public class SectionGroupsTests
    {
        [Fact]
        public void RootSectionGroupNotNull()
        {
            using (var temp = new TempConfig(TestData.EmptyConfig))
            {
                var config = ConfigurationManager.OpenExeConfiguration(temp.ExePath);
                Assert.NotNull(config.RootSectionGroup);
            }
        }

        public static string EmptySectionGroupConfiguration =
@"<?xml version='1.0' encoding='utf-8' ?>
<configuration>
    <configSections>
        <sectionGroup name='emptySectionGroup'>
        </sectionGroup>
    </configSections>
</configuration>";

        [Fact]
        public void EmptySectionGroup()
        {
            using (var temp = new TempConfig(EmptySectionGroupConfiguration))
            {
                var config = ConfigurationManager.OpenExeConfiguration(temp.ExePath);
                ConfigurationSectionGroup sectionGroup = config.GetSectionGroup("emptySectionGroup");
                Assert.NotNull(sectionGroup);
                Assert.Empty(sectionGroup.Sections);
                Assert.Empty(sectionGroup.SectionGroups);
            }
        }

        public static string SimpleSectionGroupConfiguration =
@"<?xml version='1.0' encoding='utf-8' ?>
<configuration>
    <configSections>
        <sectionGroup name='simpleSectionGroup'>
            <section name='fooSection' type='System.Configuration.NameValueSectionHandler, System' />
        </sectionGroup>
    </configSections>
</configuration>";

        [Fact]
        [ActiveIssue("https://github.com/dotnet/runtime/issues/21527", TargetFrameworkMonikers.NetFramework)]
        public void SimpleSectionGroup()
        {
            using (var temp = new TempConfig(SimpleSectionGroupConfiguration))
            {
                var config = ConfigurationManager.OpenExeConfiguration(temp.ExePath);
                ConfigurationSectionGroup sectionGroup = config.GetSectionGroup("simpleSectionGroup");
                Assert.NotNull(sectionGroup);
                Assert.Equal(1, sectionGroup.Sections.Count);
                Assert.Equal("fooSection", sectionGroup.Sections[0].SectionInformation.Name);
                Assert.Equal("System.Configuration.NameValueSectionHandler, System", sectionGroup.Sections[0].SectionInformation.Type);
            }
        }
    }
}
