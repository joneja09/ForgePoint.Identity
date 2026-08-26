#!/usr/bin/env python3
"""Tests for the IdentityServer4 → ForgePoint.Identity rewrite engine."""

from __future__ import annotations

import unittest

from rewrite import rewrite_namespaces, rewrite_packages


class NamespaceTests(unittest.TestCase):
    def test_using_and_namespace(self) -> None:
        source = (
            "using IdentityServer4;\n"
            "using IdentityServer4.Models;\n"
            "using IdentityServer4.Stores;\n"
            "\n"
            "namespace IdentityServer4.Hosting\n"
            "{\n"
            "    public class Startup\n"
            "    {\n"
            "        public void ConfigureServices(IServiceCollection services)\n"
            "        {\n"
            "            services.AddIdentityServer();\n"
            "        }\n"
            "    }\n"
            "}\n"
        )
        updated = rewrite_namespaces(source)
        self.assertIn("using ForgePoint.Identity;", updated)
        self.assertIn("using ForgePoint.Identity.Models;", updated)
        self.assertIn("using ForgePoint.Identity.Stores;", updated)
        self.assertIn("namespace ForgePoint.Identity.Hosting", updated)
        self.assertIn("services.AddIdentityServer();", updated)
        self.assertNotIn("IdentityServer4", updated)

    def test_file_scoped_namespace_and_crlf(self) -> None:
        source = "using IdentityServer4;\r\nnamespace IdentityServer4;\r\n"
        updated = rewrite_namespaces(source)
        self.assertEqual("using ForgePoint.Identity;\r\nnamespace ForgePoint.Identity;\r\n", updated)

    def test_global_using(self) -> None:
        source = "global using IdentityServer4;\nglobal using IdentityServer4.Models;\n"
        updated = rewrite_namespaces(source)
        self.assertEqual(
            "global using ForgePoint.Identity;\nglobal using ForgePoint.Identity.Models;\n",
            updated,
        )

    def test_global_alias(self) -> None:
        source = "typeof(global::IdentityServer4.Models.Client)"
        self.assertEqual(
            "typeof(global::ForgePoint.Identity.Models.Client)",
            rewrite_namespaces(source),
        )

    def test_ef_snapshot_clr_names(self) -> None:
        source = 'modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.Client", b =>'
        updated = rewrite_namespaces(source)
        self.assertIn("ForgePoint.Identity.EntityFramework.Entities.Client", updated)

    def test_friend_assemblies_are_preserved(self) -> None:
        source = (
            '[assembly: InternalsVisibleTo("IdentityServer4.EntityFramework.UnitTests, PublicKey = ABC")]\n'
            '[assembly: InternalsVisibleTo("IdentityServer4.EntityFramework.IntegrationTests")]\n'
            "using IdentityServer4.EntityFramework;\n"
        )
        updated = rewrite_namespaces(source)
        self.assertIn('InternalsVisibleTo("IdentityServer4.EntityFramework.UnitTests, PublicKey = ABC")', updated)
        self.assertIn('InternalsVisibleTo("IdentityServer4.EntityFramework.IntegrationTests")', updated)
        self.assertIn("using ForgePoint.Identity.EntityFramework;", updated)

    def test_type_names_are_not_renamed(self) -> None:
        source = (
            "builder.UseIdentityServer();\n"
            "options.IdentityServerName = IdentityServerConstants.LocalApi.ScopeName;\n"
            "services.AddHealthChecks().AddIdentityServer();\n"
        )
        self.assertEqual(source, rewrite_namespaces(source))


class PackageTests(unittest.TestCase):
    def test_package_reference_longest_first(self) -> None:
        source = """
<Project>
  <ItemGroup>
    <PackageReference Include="IdentityServer4.EntityFramework.Storage" Version="4.1.2" />
    <PackageReference Include="IdentityServer4.EntityFramework" Version="4.1.2" />
    <PackageReference Include="IdentityServer4.AspNetIdentity" Version="4.1.2" />
    <PackageReference Include="IdentityServer4.Storage" Version="4.1.2" />
    <PackageReference Include="IdentityServer4" Version="4.1.2" />
    <PackageVersion Include="IdentityServer4" Version="4.1.2" />
    <PackageReference Update="IdentityServer4.Storage" Version="4.1.2" />
  </ItemGroup>
  <PropertyGroup>
    <PackageId>IdentityServer4</PackageId>
    <AssemblyName>IdentityServer4.EntityFramework.UnitTests</AssemblyName>
    <RootNamespace>IdentityServer4</RootNamespace>
    <PackageTags>Identity;IdentityServer4</PackageTags>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\\src\\IdentityServer4.csproj" />
  </ItemGroup>
</Project>
"""
        updated = rewrite_packages(source)
        self.assertIn('Include="ForgePoint.Identity.EntityFramework.Storage"', updated)
        self.assertIn('Include="ForgePoint.Identity.EntityFramework"', updated)
        self.assertIn('Include="ForgePoint.Identity.AspNetIdentity"', updated)
        self.assertIn('Include="ForgePoint.Identity.Storage"', updated)
        self.assertIn('Include="ForgePoint.Identity"', updated)
        self.assertIn("<PackageId>ForgePoint.Identity</PackageId>", updated)
        self.assertIn('Update="ForgePoint.Identity.Storage"', updated)
        self.assertIn("<AssemblyName>IdentityServer4.EntityFramework.UnitTests</AssemblyName>", updated)
        self.assertIn("<RootNamespace>IdentityServer4</RootNamespace>", updated)
        self.assertIn("<PackageTags>Identity;IdentityServer4</PackageTags>", updated)
        self.assertIn('Include="..\\src\\IdentityServer4.csproj"', updated)
        self.assertNotIn('Include="IdentityServer4"', updated)
        self.assertNotIn('Include="IdentityServer4.', updated)


if __name__ == "__main__":
    unittest.main()
